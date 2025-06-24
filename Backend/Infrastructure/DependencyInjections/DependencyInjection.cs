namespace InsurenceManagementSystemWebApi.Infrastructure.DependencyInjections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureservices(this IServiceCollection services, IConfiguration configuration)
        {

            

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IAgentRepository, AgentRepository>();
            services.AddScoped<IAvailablePolicyRepository, AvailablePolicyRepository>();
            services.AddScoped<IPolicyRequestRepository, PolicyRequestRepository>();
            services.AddScoped<IClaimRepository, ClaimRepository>();
            services.AddScoped<IPolicyRepository, PolicyRepository>();


            services.AddScoped<IAgentRepository, AgentRepository>();
            services.AddScoped<IPolicyRepository, PolicyRepository>();
            services.AddScoped<IClaimRepository, ClaimRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();


            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IAgentRepository, AgentRepository>();



            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IPolicyRepository, PolicyRepository>();
            services.AddScoped<IPolicyRequestRepository, PolicyRequestRepository>();
            services.AddScoped<IClaimRepository, ClaimRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();

            return services;
        }
    }
}
