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
    public class AddInvitationHandler : IRequestHandler<AddInvitationCommand, Meeting>
    {
        private readonly MeetingsContext db;
        private readonly IAuthenticationService auth;
        public AddInvitationHandler(MeetingsContext db, IAuthenticationService auth)
        {
            this.db = db;
            this.auth = auth;
        }
        public async Task<Meeting> Handle(AddInvitationCommand request, CancellationToken cancellationToken)
        {
            var meeting = await db.Meetings
                .Include(m => m.Creator)
                .Include(m => m.Invitations)
                .ThenInclude(i => i.User)
                .Include(m => m.Proposals)
                .ThenInclude(p => p.Votes)
                .ThenInclude(v => v.User)
                .SingleOrDefaultAsync(m => m.Id == request.MeetingId, cancellationToken);
            
            if (meeting == null)
                throw new NotFoundException($"Meeting {request.MeetingId} does not exist");
            var oidcId = auth.GetUserIdentifier();
            if (meeting.Creator.OidcIdentifier != oidcId)
                throw new ForbiddenException($"User {oidcId} is not allowed to access meeting {request.MeetingId}");

            var user = await db.Users
                .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new NotFoundException($"User {request.UserId} does not exist");

            meeting.Invitations.Add(new Invitation { User = user });       
            var result = db.Meetings.Attach(meeting);
            await db.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}