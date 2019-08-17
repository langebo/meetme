using System;
using MediatR;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Commands
{
    public class VoteOnProposalCommand : IRequest<Proposal>
    {
        public Guid ProposalId { get; set; }
        public string Username { get; set; }
    }
}