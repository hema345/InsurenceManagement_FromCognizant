namespace InsurenceManagementSystemWebApi.Applications.DTOs
{
    public class AgentProfileResponseDto 
    {
        public required string Name { get; set; } 
        public required string ContactInfo { get; set; }
        public required string Username { get; set; }
        public int AgentId { get; set; }
    }
}
