using System;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Commands;
using MeetMe.Application.Queries;
using MeetMe.Domain.Models;
using Microsoft.AspNetCore.SignalR;

namespace MeetMe.Service.Hubs
{
    public class MeetingsHub : Hub
    {
        private readonly IMediator mediator;

        public MeetingsHub(IMediator mediator) => this.mediator = mediator;

        public async Task<Meeting> GetMeeting(Guid id) =>
            await mediator.Send(new GetMeetingQuery { Id = id });

        public async Task<Meeting> CreateMeeting(CreateMeetingCommand cmd) =>
            await mediator.Send(cmd);

        public async Task<Meeting> Vote(VoteOnMeetingCommand cmd) => 
            await mediator.Send(cmd);
    }
}