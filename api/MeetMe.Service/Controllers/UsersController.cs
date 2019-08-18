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
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator) => this.mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers() =>
            Ok(await mediator.Send(new GetUsersQuery()));

        [HttpGet("{me}")]
        public async Task<ActionResult<User>> GetMe() => 
            Ok(await mediator.Send(new GetMeQuery()));
        
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserCommand cmd) => 
            Ok(await mediator.Send(cmd));
    }
}