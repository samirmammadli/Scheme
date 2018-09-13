using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils.Messaging;
using Scheme.Services;
using Scheme.Services.MailService;
using Scheme.Services.TokenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Scheme.Tools.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void AddMailService(this IServiceCollection services)
        {
            services.AddSingleton<IEmailSender, TLSMailSend>();
        }

        public static void AddMailService(this IServiceCollection services, Action<MailServiceOptions> options)
        {
            MailServiceOptions opt = new MailServiceOptions();
            options.Invoke(opt);
            services.AddSingleton<IEmailSender>(new TLSMailSend(opt));
        }

        public static void AddJWTBasedAuthorisation(this IServiceCollection services)
        {
            services.AddScoped<ITokenAuthProvider, JWTBasedTokenProvider>();
        }
    }
}
