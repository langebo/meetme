using System.Reflection;
using FluentValidation;
using MediatR;
using MeetMe.Application.Behaviors;
using MeetMe.Application.Queries;
using MeetMe.Authentication.Interfaces;
using MeetMe.Authentication.Services;
using MeetMe.Domain.Contexts;
using MeetMe.Service.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Okta.AspNetCore;

namespace MeetMe.Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration config) => Configuration = config;

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;

            services.AddAuthentication(OktaDefaults.ApiAuthenticationScheme)
                .AddOktaWebApi(new OktaWebApiOptions { OktaDomain = "https://dev-594008.okta.com" });

            services.AddAuthorization(options =>
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build());

            services.AddControllers(options =>
                options.Filters.Add(typeof(ExceptionFilter)));

            services.AddDbContext<MeetingsContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("MeetMeDb"), 
                    b => b.MigrationsAssembly("MeetMe.Service")));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR(typeof(GetMeetingQuery).GetTypeInfo().Assembly);
            services.Scan(scan => scan
                .FromAssemblyOf<GetMeetingQuery>()
                .AddClasses(c => c.AssignableTo(typeof(AbstractValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            services.AddHttpContextAccessor();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
        }

        public void Configure(IApplicationBuilder app)
        { 
            app.UseCors(options => options
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod());            

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();   

            app.UseEndpoints(endpoints =>
                endpoints.MapControllers());
        }
    }
}
