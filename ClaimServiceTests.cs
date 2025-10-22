using CMCS_WPF.Models;
using CMCS_WPF.Services;
using System.IO;
using Xunit;

namespace CMCS_WPF.Tests
{
    public class ClaimServiceTests
    {
        private ClaimService GetClaimService()
        {
            var logger = new FileLogger(Path.Combine(Directory.GetCurrentDirectory(), "Logs"));
            var storage = new StorageService(Path.Combine(Directory.GetCurrentDirectory(), "TestData"));
            return new ClaimService(logger, storage);
        }

        [Fact]
        public void AddClaim_Should_Add_Claim_To_Collection()
        {
            
            var service = GetClaimService();
            var claim = new Claim
            {
                LecturerName = "John Doe",
                HoursWorked = 10,
                HourlyRate = 50
            };

            
            service.AddClaim(claim);

            
            Assert.Contains(claim, service.Claims);
            Assert.Equal(ClaimStatus.Pending, claim.Status);
        }

        [Fact]
        public void ApproveClaim_Should_Update_Status_To_CoordinatorApproved()
        {
            
            var service = GetClaimService();
            var claim = new Claim { LecturerName = "Jane", HoursWorked = 5, HourlyRate = 60 };
            service.AddClaim(claim);

            
            service.Approve(claim.Id, "Coordinator");

            
            Assert.Equal(ClaimStatus.CoordinatorApproved, claim.Status);
        }

        [Fact]
        public void RejectClaim_Should_Update_Status_To_Rejected()
        {
            
            var service = GetClaimService();
            var claim = new Claim { LecturerName = "Jane", HoursWorked = 5, HourlyRate = 60 };
            service.AddClaim(claim);

            
            service.Reject(claim.Id, "Coordinator", "Invalid");

            
            Assert.Equal(ClaimStatus.Rejected, claim.Status);
            Assert.Equal("Invalid", claim.History[^1].Comment);
        }
    }
}
