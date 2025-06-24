namespace InsurenceManagementSystemWebApi.Applications.DTOs
{
    public class ClaimFilingRequestDtoForCustomer
    {
        public int PolicyId { get; set; }
        public int CustomerId { get; set; }
      
        public decimal ClaimAmount { get; set; }
    }
}
