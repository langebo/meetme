using System;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Commands;
using MeetMe.Application.Queries;
using MeetMe.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeetMe.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly IMediator mediator;

        public MeetingsController(IMediator mediator) => this.mediator = mediator;

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Meeting>> GetMeeting(Guid id) => 
            Ok(await mediator.Send(new GetMeetingQuery { Id = id }));

        [HttpPost]
        public async Task<ActionResult<Meeting>> CreateMeeting([FromBody] CreateMeetingCommand cmd) => 
            Ok(await mediator.Send(cmd));

        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult<Meeting>> Vote(Guid id, [FromBody] VoteOnMeetingCommand cmd) 
        {
            if(id != cmd.MeetingId)
                return BadRequest("Id mismatch");

            return Ok(await mediator.Send(cmd));
        }
    }
}