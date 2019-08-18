using System;
using System.Collections.Generic;
using MediatR;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Queries
{
    public class GetMeetingsQuery : IRequest<List<Meeting>>
    {
        public Guid UserId { get; set; }
    }
}