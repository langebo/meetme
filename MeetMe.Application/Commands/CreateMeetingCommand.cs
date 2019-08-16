using System;
using System.Collections.Generic;
using MediatR;
using MeetMe.Domain.Models;

namespace MeetMe.Application.Commands
{
    public class CreateMeetingCommand : IRequest<Meeting>
    {
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public List<DateTimeOffset> Proposals { get; set; }

        public CreateMeetingCommand()
        {
            Proposals = new List<DateTimeOffset>();
        }
    }
}