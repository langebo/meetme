using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Commands;
using MeetMe.Application.Exceptions;
using MeetMe.Authentication.Interfaces;
using MeetMe.Domain.Contexts;
using MeetMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Handlers
{
    public class CreateMeetingHandler : IRequestHandler<CreateMeetingCommand, Meeting>
    {
        private readonly MeetingsContext db;
        private readonly IAuthenticationService auth;

        public CreateMeetingHandler(MeetingsContext db, IAuthenticationService auth)
        {
            this.db = db;
            this.auth = auth;
        }

        public async Task<Meeting> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
        {
            var userOidc = auth.GetUserIdentifier();
            var user = await db.Users
                .SingleOrDefaultAsync(u => u.OidcIdentifier == userOidc, cancellationToken);

            if (user == null)
                throw new NotFoundException($"Unable to find user with {userOidc}");

            var invited = await db.Users
                .Where(u => request.InvitedUserIds.Contains(u.Id))
                .ToListAsync(cancellationToken);

            invited.Add(user);

            var result = await db.Meetings.AddAsync(new Meeting
            {
                Creator = user,
                Title = request.Title,
                Invitations = invited.Select(i => new Invitation {User = i}).ToList(),
                Proposals = request.Proposals.Select(p => new Proposal {Time = p}).ToList(),
            }, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}
