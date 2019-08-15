using System;
using MediatR;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Queries
{
    public class GetMeetingQuery : IRequest<Meeting>
    {
        public Guid Id { get; set; }
    }
}