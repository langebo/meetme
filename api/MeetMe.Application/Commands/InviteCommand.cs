using System;
using MediatR;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Commands
{
    public class InviteCommand : IRequest<Meeting>
    {
        public Guid MeetingId { get; set; }
        public Guid UserId { get; set; }
    }
}