using System;
using System.Collections.ObjectModel;

namespace CMCS_WPF.Models
{
    public enum ClaimStatus
    {
        Pending,
        CoordinatorApproved,
        ManagerApproved,
        Rejected,
        Settled
    }

    public class Claim
    {
        public Claim()
        {
            History = new ObservableCollection<ClaimHistory>();
        }

        public int Id { get; set; }
        public string LecturerName { get; set; } = string.Empty;
        public double HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string? OriginalFileName { get; set; }
        public string? StoredFilePath { get; set; }
        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;
        public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;
        public ObservableCollection<ClaimHistory> History { get; set; }
        public decimal TotalAmount => (decimal)HoursWorked * HourlyRate;
    }
}

