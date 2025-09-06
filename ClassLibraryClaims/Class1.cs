using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System;
using System.Collections.Generic;
using System.Linq;

namespace ClaimsApp.Core.Models
{
    /// Represents a claim submitted by a lecturer.
    public class Claim
    {
        public int Id { get; set; }                // Unique identifier for each claim
        public string LecturerId { get; set; }     // User ID of the lecturer who submitted it
        public decimal HourlyRate { get; set; }   
        public int HoursWorked { get; set; }      
        public string Notes { get; set; }          // Optional notes from lecturer
        public string Status { get; set; } = "Pending"; // Default status is Pending

        // Calculated property → no need to store in DB, always calculated when needed
        public decimal Total => HourlyRate * HoursWorked;
    }

    /// Represents a Lecturer in the system with personal details.
    public class Lecturer
    {
        public string Id { get; set; }             // Unique user ID (same as login Id)
        public string FullName { get; set; }       
        public string Email { get; set; }          // Email for login/communication
        public string BankAccount { get; set; }    // For payroll processing
    }
}

namespace ClaimsApp.Core.Services
{
    using ClaimsApp.Core.Models;

    /// Service class for handling claims logic.
    /// This is where we keep business rules for claims.
   
    public class ClaimService
    {
        // This is just a temporary in-memory store. Later this can be replaced with a database.
        private readonly List<Claim> _claims = new List<Claim>();

        /// Lecturer submits a new claim → added to the list.
       
        public void SubmitClaim(Claim claim)
        {
            claim.Id = _claims.Count + 1; // Simple auto-increment
            _claims.Add(claim);
        }

        /// Get all claims submitted by a specific lecturer.
        public IEnumerable<Claim> GetClaimsForLecturer(string lecturerId)
        {
            return _claims.Where(c => c.LecturerId == lecturerId);
        }

        /// Get all claims that are still pending (not approved or rejected).
        public IEnumerable<Claim> GetPendingClaims()
        {
            return _claims.Where(c => c.Status == "Pending");
        }

        /// Approve a claim by Id (Manager function).
        public void ApproveClaim(int claimId)
        {
            var claim = _claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
                claim.Status = "Approved";
        }

        /// Reject a claim by Id (Manager function).
        public void RejectClaim(int claimId)
        {
            var claim = _claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
                claim.Status = "Rejected";
        }
    }

    /// Service class for HR payroll calculations.
    public class PayrollService
    {
        /// Calculate the total payroll from all approved claims.
        public decimal CalculatePayroll(IEnumerable<Claim> approvedClaims)
        {
            return approvedClaims.Sum(c => c.Total);
        }
    }
}
