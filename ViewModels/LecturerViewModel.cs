using CMCS_WPF.Models;
using CMCS_WPF.Services;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace CMCS_WPF.ViewModels
{
    public class LecturerViewModel : ObservableObject
    {
        private readonly ClaimService _claimService;
        private readonly FileLogger _logger;
        private readonly string[] _allowed = new[] { ".pdf", ".docx", ".xlsx" };
        private const long MaxFileBytes = 5 * 1024 * 1024; 

        public LecturerViewModel(ClaimService claimService, FileLogger logger)
        {
            _claimService = claimService;
            _logger = logger;
            NewClaim = new Claim();
            SubmitCommand = new RelayCommand(async _ => await SubmitAsync(), _ => CanSubmit());
            UploadCommand = new RelayCommand(_ => UploadFile());
        }

        public Claim NewClaim { get; set; }
        public RelayCommand SubmitCommand { get; }
        public RelayCommand UploadCommand { get; }

        private bool CanSubmit()
        {
            return !string.IsNullOrWhiteSpace(NewClaim.LecturerName)
                && NewClaim.HoursWorked > 0
                && NewClaim.HourlyRate > 0;
        }

        private async Task SubmitAsync()
        {
            try
            {
                var added = _claimService.AddClaim(NewClaim);
                MessageBox.Show($"Claim submitted (ID: {added.Id})", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                NewClaim = new Claim(); 
                OnPropertyChanged(nameof(NewClaim));
            }
            catch (Exception ex)
            {
                _logger.Log("ERROR", "Submit failed", ex);
                MessageBox.Show("An error occurred while submitting the claim. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UploadFile()
        {
            var dlg = new OpenFileDialog { Filter = "Documents|*.pdf;*.docx;*.xlsx" };
            if (dlg.ShowDialog() != true) return;

            try
            {
                var ext = Path.GetExtension(dlg.FileName).ToLowerInvariant();
                if (Array.IndexOf(_allowed, ext) < 0)
                {
                    MessageBox.Show("Invalid file type. Allowed: .pdf, .docx, .xlsx", "Invalid File", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var fi = new FileInfo(dlg.FileName);
                if (fi.Length > MaxFileBytes)
                {
                    MessageBox.Show("File too large. Max 5 MB allowed.", "Invalid File", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var uploads = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
                Directory.CreateDirectory(uploads);
                var unique = Guid.NewGuid().ToString() + ext;
                var full = Path.Combine(uploads, unique);
                File.Copy(dlg.FileName, full);

                NewClaim.OriginalFileName = Path.GetFileName(dlg.FileName);
                NewClaim.StoredFilePath = full;
                MessageBox.Show($"Uploaded: {NewClaim.OriginalFileName}", "Upload", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logger.Log("ERROR", "Upload failed", ex);
                MessageBox.Show("Error uploading file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
