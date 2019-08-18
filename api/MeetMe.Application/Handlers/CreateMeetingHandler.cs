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
            var invitations = await db.Users
                .Where(u => request.InvitedUserIds.Contains(u.Id))
                .ToListAsync(cancellationToken);

            if (invitations.Count != request.InvitedUserIds.Count)
            {
                request.InvitedUserIds.RemoveAll(u => invitations.Select(i => i.Id).Contains(u));
                var missingUserIdsString = string.Join(", ", request.InvitedUserIds.Select(i => $"{i}"));
                throw new NotFoundException($"Invited user(s) do(es) not exist: {missingUserIdsString}");
            }

            var oidcId = auth.GetUserIdentifier();
            var creator = await db.Users.SingleOrDefaultAsync(u => u.OidcIdentifier == oidcId, cancellationToken);

            if (creator == null)
                throw new NotFoundException($"User {oidcId} does not exist");

            var meeting = await db.Meetings.AddAsync(new Meeting
            {
                Title = request.Title,
                Creator = creator,
                Proposals = request.Proposals.Select(p => new Proposal { Time = p }).ToList(),
                Invitations = invitations.Select(i => new Invitation { User = i }).ToList()
            }, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);
            return meeting.Entity;
        }
    }
}