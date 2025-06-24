namespace InsurenceManagementSystemWebApi.Applications.DTOs
{
    public class PolicyRequestStatusResponseDto 
    { 
        public int RequestId { get; set; }
        public required string AvailablePolicyName { get; set; }
        public required string CustomerName { get; set; }
       

        
        public required string Status { get; set; } 
        public DateTime RequestedOn { get; set; }
    }
}
