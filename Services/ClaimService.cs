using CMCS_WPF.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CMCS_WPF.Services
{
    public class ClaimService
    {
        private readonly FileLogger _logger;
        private readonly StorageService _storage;
        private int _nextId;

        public ObservableCollection<Claim> Claims { get; } = new ObservableCollection<Claim>();

        public ClaimService(FileLogger logger, StorageService storage)
        {
            _logger = logger;
            _storage = storage;
            LoadExisting();
        }

        private void LoadExisting()
        {
            try
            {
                var list = _storage.Load() ?? new System.Collections.Generic.List<Claim>();
                foreach (var c in list)
                {
                    Claims.Add(c);
                }
                _nextId = (Claims.Any() ? Claims.Max(x => x.Id) : 0) + 1;
            }
            catch (Exception ex)
            {
                _logger.Log("ERROR", "Failed to load existing claims", ex);
            }
        }

        private void SaveAll()
        {
            try
            {
                _storage.Save(Claims);
            }
            catch (Exception ex)
            {
                _logger.Log("ERROR", "Failed to save claims", ex);
            }
        }

        public Claim AddClaim(Claim claim)
        {
            claim.Id = _nextId++;
            claim.DateSubmitted = DateTime.UtcNow;
            claim.Status = ClaimStatus.Pending;
            claim.History.Add(new ClaimHistory
            {
                ClaimId = claim.Id,
                Status = claim.Status,
                Role = "Lecturer",
                Comment = "Submitted",
                ChangedAt = DateTime.UtcNow
            });

            Claims.Add(claim);
            SaveAll();
            _logger.Log("INFO", $"Claim {claim.Id} added (Lecturer: {claim.LecturerName})");
            return claim;
        }

        public void Approve(int id, string role)
        {
            var claim = Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) return;
            claim.Status = role == "Manager" ? ClaimStatus.ManagerApproved : ClaimStatus.CoordinatorApproved;
            claim.History.Add(new ClaimHistory
            {
                ClaimId = id,
                Status = claim.Status,
                Role = role,
                Comment = $"{role} approved",
                ChangedAt = DateTime.UtcNow
            });
            SaveAll();
            _logger.Log("INFO", $"Claim {id} approved by {role}");
        }

        public void Reject(int id, string role, string? comment = null)
        {
            var claim = Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) return;
            claim.Status = ClaimStatus.Rejected;
            claim.History.Add(new ClaimHistory
            {
                ClaimId = id,
                Status = claim.Status,
                Role = role,
                Comment = comment ?? $"{role} rejected",
                ChangedAt = DateTime.UtcNow
            });
            SaveAll();
            _logger.Log("INFO", $"Claim {id} rejected by {role}");
        }
    }
}

