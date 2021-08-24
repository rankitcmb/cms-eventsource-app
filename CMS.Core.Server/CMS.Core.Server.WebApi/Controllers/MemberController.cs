using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMS.Core.Server.Domain.Commands;
using CMS.Core.Server.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Core.Server.WebApi.Controllers
{
    [Route("api/members")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MemberController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> InsertMember(CreateMemberRequestDto dto,CancellationToken cancellationToken = default)
        {
            if (dto == null)
                return BadRequest();
            
            var command = new CreateMember(Guid.NewGuid(),
                dto.FirstName,
                dto.LastName,
                dto.Dbo,
                dto.Email,
                dto.Mobile,
                dto.Gender,
                dto.JoinedAt);

            await _mediator.Publish(command, cancellationToken);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMember(UpdateMemberRequestDto dto,
            CancellationToken cancellationToken = default)
        {
            if (dto == null)
                return BadRequest();

            var notification = new UpdateMember(dto.Guid,dto.FirstName,dto.LastName,dto.Dbo,dto.Email,dto.Mobile,dto.Gender);
            await _mediator.Publish(notification, cancellationToken);
            return Ok();
        }

    }
}
