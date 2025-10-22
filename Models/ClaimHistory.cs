using System;

namespace CMCS_WPF.Models
{
    public class ClaimHistory
    {
        public int ClaimId { get; set; }
        public ClaimStatus Status { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
    }
}
