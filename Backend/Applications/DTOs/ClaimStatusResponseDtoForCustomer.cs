    namespace InsurenceManagementSystemWebApi.Applications.DTOs
{
    public class ClaimStatusResponseDtoForCustomer 
    { 
        public int ClaimId { get; set; } 
        public required string PolicyName { get; set; } 
        public decimal ClaimAmount { get; set; }
        public required string Status { get; set; }
        public DateTime FiledDate { get; internal set; }
    }
}
