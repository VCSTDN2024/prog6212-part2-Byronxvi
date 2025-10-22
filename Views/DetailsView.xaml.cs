using System.Diagnostics;
using System.IO;
using System.Windows;

namespace CMCS_WPF.Views
{
    public partial class DetailsView : Window
    {
        private readonly Models.Claim _claim;
        public DetailsView(Models.Claim claim)
        {
            InitializeComponent();
            _claim = claim;
            DataContext = claim;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_claim.StoredFilePath) || !File.Exists(_claim.StoredFilePath))
            {
                MessageBox.Show("No file attached or file missing.", "File not found", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                var p = new ProcessStartInfo(_claim.StoredFilePath) { UseShellExecute = true };
                Process.Start(p);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Unable to open file: " + ex.Message);
            }
        }
    }
}

