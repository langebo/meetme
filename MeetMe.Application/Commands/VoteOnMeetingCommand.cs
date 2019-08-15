using System;
using System.Collections.Generic;
using MediatR;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Commands
{
    public class VoteOnMeetingCommand : IRequest<Meeting>
    {
        public string Username { get; set; }
        public Guid MeetingId { get; set; }
        public List<Guid> ProposalIds { get; set; }
    }
}