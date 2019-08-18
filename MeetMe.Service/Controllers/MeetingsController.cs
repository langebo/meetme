using System;
using System.Collections.Generic;
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
            Ok(await mediator.Send(new GetMeetingQuery {Id = id}));

        [HttpGet]
        public async Task<ActionResult<List<Meeting>>> GetMeetings([FromQuery] Guid userId) =>
            await mediator.Send(new GetMeetingsQuery { UserId = userId });

        [HttpPost]
        public async Task<ActionResult<Meeting>> CreateMeeting([FromBody] CreateMeetingCommand cmd) =>
            Ok(await mediator.Send(cmd));

        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult<Meeting>> Vote(Guid id, [FromBody] VoteCommand cmd)
        {
            if (id != cmd.MeetingId)
                return BadRequest("Mismatching MeetingId between body and query parameters");

            return Ok(await mediator.Send(cmd));
        }

        [HttpPost("{id:Guid}/invite")]
        public async Task<ActionResult<Meeting>> Invite(Guid id, [FromBody] AddInvitationCommand cmd)
        {
            if (id != cmd.MeetingId)
                return BadRequest("Mismatching MeetingId between body and query parameters");

            return Ok(await mediator.Send(cmd));
        }

        [HttpPost("{id:Guid}/uninvite")]
        public async Task<ActionResult<Meeting>> Uninvite(Guid id, [FromBody] RemoveInvitationCommand cmd)
        {
            if (id != cmd.MeetingId)
                return BadRequest("Mismatching MeetingId between body and query parameters");

            return Ok(await mediator.Send(cmd));
        }

    }
}