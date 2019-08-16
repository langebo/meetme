using System;
using MediatR;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Commands
{
    public class VoteOnProposalCommand : IRequest<Meeting>
    {
        public Guid MeetingId { get; set; }
        public Guid ProposalId { get; set; }
        public string Username { get; set; }
    }
}