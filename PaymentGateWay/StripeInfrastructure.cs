using PaymentGateWay.Application;
using PaymentGateWay.Contracts;
using Stripe;

namespace PaymentGateWay
{
    public static class StripeInfrastructure
    {
        public static IServiceCollection AddStripeInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            StripeConfiguration.ApiKey = configuration.GetValue<string>("StripeSettings:SecretKey");

            return services
                .AddScoped<CustomerService>()
                .AddScoped<ChargeService>()
                .AddScoped<TokenService>()
                .AddScoped<CardService>()
                .AddScoped<IStripeAppService, StripeAppService>();
        }
    }

}
