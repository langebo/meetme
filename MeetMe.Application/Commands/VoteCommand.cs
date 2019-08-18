using System;
using MediatR;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Commands
{
    public class VoteCommand : IRequest<Vote>
    {
        public Guid MeetingId { get; set; }
        public Guid ProposalId { get; set; }
        public Guid UserId { get; set; }
    }
}