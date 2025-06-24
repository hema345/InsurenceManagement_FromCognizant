namespace InsurenceManagementSystemWebApi.Applications.DTOs
{
    public class ApprovePolicyRequestDto
    {

        public int CustomerId { get; set; }
        public int AvailablePolicyId { get; set; }
        public int AgentId { get; set; }

    }
}
