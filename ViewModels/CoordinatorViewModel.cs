using CMCS_WPF.Services;
using System.Collections.ObjectModel;
using System.Linq;
using CMCS_WPF.Models;
using System.Windows;

namespace CMCS_WPF.ViewModels
{
    public class CoordinatorViewModel : ObservableObject
    {
        private readonly ClaimService _service;
        public ObservableCollection<Claim> Claims => _service.Claims;

        public RelayCommand ApproveCommand { get; }
        public RelayCommand RejectCommand { get; }

        public CoordinatorViewModel(ClaimService service)
        {
            _service = service;
            ApproveCommand = new RelayCommand(param => Approve(param));
            RejectCommand = new RelayCommand(param => Reject(param));
        }

        private void Approve(object? param)
        {
            if (param == null) return;
            if (!int.TryParse(param.ToString(), out int id)) return;
            try
            {
                _service.Approve(id, "Coordinator");
                MessageBox.Show($"Claim {id} approved (Coordinator).", "Approved", MessageBoxButton.OK, MessageBoxImage.Information);
                OnPropertyChanged(nameof(Claims));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error approving claim: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Reject(object? param)
        {
            if (param == null) return;
            if (!int.TryParse(param.ToString(), out int id)) return;
            try
            {
                _service.Reject(id, "Coordinator", "Rejected by coordinator");
                MessageBox.Show($"Claim {id} rejected.", "Rejected", MessageBoxButton.OK, MessageBoxImage.Warning);
                OnPropertyChanged(nameof(Claims));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error rejecting claim: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
