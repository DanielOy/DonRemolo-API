using API.Helpers;
using Core.Entities.Externals;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    var errors = ActionContext.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiErrorResponse(HttpStatusCode.BadRequest, errors);
                    return new BadRequestObjectResult(errorResponse);
                };
            });

            var facebookSettings = new FacebookAuthSettings();
            configuration.Bind(nameof(FacebookAuthSettings), facebookSettings);
            services.AddSingleton(facebookSettings);

            var emailSettings = new EmailSettings();
            configuration.Bind(nameof(EmailSettings), emailSettings);
            services.AddSingleton(emailSettings);

            services.AddHttpClient();
            services.AddSingleton<IFacebookAuthService, FacebookAuthService>();

            return services;
        }
    }
}
