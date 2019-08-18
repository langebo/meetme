using System;
using MediatR;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Commands
{
    public class VoteCommand : IRequest<Proposal>
    {
        public Guid ProposalId { get; set; }
    }
}