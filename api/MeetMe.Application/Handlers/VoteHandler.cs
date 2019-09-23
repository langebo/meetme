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
    public class VoteHandler : IRequestHandler<VoteCommand, Meeting>
    {
        private readonly MeetingsContext db;
        private readonly IAuthenticationService auth;

        public VoteHandler(MeetingsContext db, IAuthenticationService auth)
        {
            this.db = db;
            this.auth = auth;
        }

        public async Task<Meeting> Handle(VoteCommand request, CancellationToken cancellationToken)
        {
            var userOidc = auth.GetUserIdentifier();
            var user = await db.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.OidcIdentifier == userOidc, cancellationToken);

            if (user == null)
                throw new NotFoundException($"Unable to find user {userOidc}");

            var meeting = await db.Meetings
                .Include(m => m.Creator)
                .Include(m => m.Proposals)
                .Include(m => m.Invitations)
                .ThenInclude(i => i.User)
                .Include(m => m.Invitations)
                .ThenInclude(i => i.Votes)
                .FirstOrDefaultAsync(m => m.Id == request.MeetingId, cancellationToken);

            if (meeting == null)
                throw new NotFoundException($"Unable to find meeting {request.MeetingId}");

            if (meeting.Invitations.All(i => i.User.Id != user.Id))
                throw new ForbiddenException($"User {user.OidcIdentifier} is not eligible to vote on meeting {request.MeetingId}");

            if (meeting.Proposals.All(p => p.Id != request.ProposalId))
                throw new NotFoundException($"Unable to find proposal {request.ProposalId} on meeting {request.MeetingId}");

            meeting.Invitations.Single(i => i.User.Id == user.Id).Votes.Add(new Vote {ProposalId = request.ProposalId});

            var result = db.Meetings.Attach(meeting);
            await db.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }
    }
}
