using TestApplication.API.Factory;
using TestApplication.API.Repository.UserMaster;
using TestApplication.API.Repository.Investor;

namespace TestApplication.API.Container
{
    public static class CustomConatiner
    {
        public static void AddCustomContainer(IServiceCollection services, IConfiguration configuration)
        {
            IConnectionFactory connectionFactory = new ConnectionFactory(configuration.GetConnectionString("DBconnection"));
            services.AddSingleton<IConnectionFactory>(connectionFactory);

            // Repository should be Scoped
            services.AddScoped<IUserMaster,UserMasterRepo>();
            services.AddScoped<IInvestorRepository, InvestorRepository>();
        }
    }
}
