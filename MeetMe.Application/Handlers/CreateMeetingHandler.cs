using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Commands;
using MeetMe.Domain.Contexts;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Handlers
{
    public class CreateMeetingHandler : IRequestHandler<CreateMeetingCommand, Meeting>
    {
        private readonly MeetingsContext db;

        public CreateMeetingHandler(MeetingsContext db)
        {
            this.db = db;
        }

        public async Task<Meeting> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
        {
            var meeting = await db.Meetings.AddAsync(new Meeting
            {
                Title = request.Title,
                Proposals = request.Proposals.Select(p => new Proposal {Time = p}).ToList(),
            });

            await db.SaveChangesAsync(cancellationToken);
            return meeting.Entity;
        }
    }
}