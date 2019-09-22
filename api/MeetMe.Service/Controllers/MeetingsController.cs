using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Commands;
using MeetMe.Application.Queries;
using MeetMe.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetMe.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class MeetingsController : ControllerBase
    {
        private readonly IMediator mediator;

        public MeetingsController(IMediator mediator) => this.mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<List<Meeting>>> GetMeetings() =>
            await mediator.Send(new GetMeetingsQuery());

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Meeting>> GetMeeting(Guid id) =>
            Ok(await mediator.Send(new GetMeetingQuery {Id = id}));

        [HttpPost]
        public async Task<ActionResult<Meeting>> CreateMeeting([FromBody] CreateMeetingCommand cmd) =>
            Ok(await mediator.Send(cmd));

        [HttpPost("{id:Guid}/invite")]
        public async Task<ActionResult<Meeting>> Invite(Guid id, [FromBody] InviteCommand cmd)
        {
            if (id != cmd.MeetingId)
                return BadRequest("Mismatching MeetingId between body and query parameters");

            return Ok(await mediator.Send(cmd));
        }

        [HttpPost("{id:Guid}/eject")]
        public async Task<ActionResult<Meeting>> Eject(Guid id, [FromBody] EjectCommand cmd)
        {
            if (id != cmd.MeetingId)
                return BadRequest("Mismatching MeetingId between body and query parameters");

            return Ok(await mediator.Send(cmd));
        }

        [HttpPost("{id:Guid}/vote")]
        public async Task<ActionResult<Meeting>> Vote(Guid id, [FromBody] VoteCommand cmd)
        {
            if (id != cmd.MeetingId)
                return BadRequest("Mismatching MeetingId between body and query parameters");

            return Ok(await mediator.Send(cmd));
        }

        [HttpPost("{id:Guid}/retract")]
        public async Task<ActionResult<Meeting>> Retract(Guid id, [FromBody] RetractCommand cmd)
        {
            if (id != cmd.MeetingId)
                return BadRequest("Mismatching MeetingId between body and query parameters");

            return Ok(await mediator.Send(cmd));
        }
    }
}