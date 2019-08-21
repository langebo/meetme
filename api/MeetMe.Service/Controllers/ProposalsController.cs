using System;
using System.Threading.Tasks;
using MediatR;
using MeetMe.Application.Commands;
using MeetMe.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetMe.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProposalsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProposalsController(IMediator mediator) => this.mediator = mediator;

        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult<Proposal>> Vote(Guid id, [FromBody] VoteCommand cmd)
        {
            if (id != cmd.ProposalId)
                return BadRequest($"Mismatching ProposalId between body and query parameters");

            return Ok(await mediator.Send(cmd));
        }
    }
}