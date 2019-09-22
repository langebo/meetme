using System.Linq;
using MeetMe.Domain.Contexts;
using MeetMe.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MeetMe.Mocks.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddFakeUser(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetService<MeetingsContext>();

            if (db.Users.Any())
                return app;

            db.Users.AddRange(
                new User
                {
                    Name = "Boris",
                    Email = "boris@incoqnito.io",
                    OidcIdentifier = "boris"
                }, 
                new User
                {
                    Name = "Chris",
                    Email = "chris@incoqnito.io",
                    OidcIdentifier = "chris"
                }, 
                new User
                {
                    Name = "Patrick",
                    Email = "patrick@incoqnito.io",
                    OidcIdentifier = "patrick"
                }, 
                new User
                {
                    Name = "Nils",
                    Email = "nils@incoqnito.io",
                    OidcIdentifier = "nils"
                }, 
                new User
                {
                    Name = "Micha",
                    Email = "michael@incoqnito.io",
                    OidcIdentifier = "michael"
                }, 
                new User
                {
                    Name = "Mohammed",
                    Email = "mo@incoqnito.io",
                    OidcIdentifier = "mohammed"
                });

            db.SaveChanges();

            return app;
        }
    }
}
