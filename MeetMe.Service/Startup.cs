using System.Reflection;
using FluentValidation;
using MediatR;
using MeetMe.Application.Behaviors;
using MeetMe.Application.Queries;
using MeetMe.Domain.Contexts;
using MeetMe.Service.Filters;
using MeetMe.Service.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeetMe.Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration config) => Configuration = config;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddControllers(o => o.Filters.Add(typeof(ExceptionFilter)));

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
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MeetingsHub>("/meetings");
            });
        }
    }
}
