using System.Reflection;
using FluentValidation;
using MediatR;
using MeetMe.Application.Behaviors;
using MeetMe.Application.Queries;
using MeetMe.Authentication.Interfaces;
using MeetMe.Authentication.Services;
using MeetMe.Domain.Contexts;
using MeetMe.Mocks.Extensions;
using MeetMe.Mocks.Services;
using MeetMe.Service.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MeetMe.Service
{
    public class Startup
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            this.config = config;
            this.env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthorization(options =>
            //    options.DefaultPolicy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build());

            services.AddControllers(options =>
                options.Filters.Add(typeof(ExceptionFilter)));

            services.AddDbContext<MeetingsContext>(options =>
                options.UseNpgsql(config.GetConnectionString("MeetMeDb"), 
                    b => b.MigrationsAssembly("MeetMe.Service")));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR(typeof(GetMeetingQuery).GetTypeInfo().Assembly);
            services.Scan(scan => scan
                .FromAssemblyOf<GetMeetingQuery>()
                .AddClasses(c => c.AssignableTo(typeof(AbstractValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            services.AddHttpContextAccessor();

            if(env.IsDevelopment())
                services.AddTransient<IAuthenticationService, TestAuthenticationService>();
            else
                services.AddTransient<IAuthenticationService, AuthenticationService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (env.IsDevelopment())
            {
                app.AddFakeUser();
                app.UseCors(options => options
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            }
            else
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }
            
            app.UseRouting();
            app.UseEndpoints(endpoints =>
                endpoints.MapControllers());
        }
    }
}
