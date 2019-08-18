using System;
using MediatR;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Commands
{
    public class RemoveInvitationCommand : IRequest<Meeting>
    {
        public Guid MeetingId { get; set; }
        public Guid InvitationId { get; set; }
    }
}