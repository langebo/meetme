using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Queries;
using MeetMe.Domain.Contexts;
using MeetMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Application.Handlers
{
    public class GetMeetingHandler : IRequestHandler<GetMeetingQuery, Meeting>
    {
        private readonly MeetingsContext db;

        public GetMeetingHandler(MeetingsContext db)
        {
            this.db = db;
        }

        public async Task<Meeting> Handle(GetMeetingQuery request, CancellationToken cancellationToken)
        {
            return await db.Meetings
                .Include(m => m.Proposals)
                .Include(m => m.Votes)
                .ThenInclude(v => v.Proposal)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id);
        }
    }
}