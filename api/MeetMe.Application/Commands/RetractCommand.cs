using System;
using MediatR;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Commands
{
    public class RetractCommand : IRequest<Meeting>
    {
        public Guid MeetingId { get; set; }
        public Guid ProposalId { get; set; }
    }
}
