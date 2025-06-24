namespace InsurenceManagementSystemWebApi.Applications.DTOs
{
    public class ClaimStatusResponseDtoForAdmin
    {
        public int ClaimId { get; set; }
        public required string PolicyName { get; set; }
        public decimal ClaimAmount { get; set; }
        public required string Status { get; set; }
        public required string CustomerName { get; set; }
        public string? AgentName { get; set; }
    }
}
