using Banking.Contracts.Command;
using Banking.Contracts.V1;
using Banking.Handlers.Auths;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Controllers.V1
{
    [ERPAuthorize]
    public class BankClosureController : Controller
    {
        private readonly IMediator _mediator;
        public BankClosureController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost(ApiRoutes.BankClosure.ADD_UPDATE_BANK_CLOSURE)]
        public async Task<IActionResult> AddUpdateBankClosure([FromBody] AddUpdateBankClosureCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }


        [HttpPost(ApiRoutes.BankClosure.ADD_UPDATE_BANK_CLOSURE_SETUP)]
        public async Task<IActionResult> AddUpdateBankClosureSetup([FromBody] AddUpdateBankClosureSetupCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost(ApiRoutes.BankClosure.DELETE_BANK_CLOSURE)]
        public async Task<IActionResult> DeleteBankClosure([FromBody] DeleteBankClosureCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }


        [HttpPost(ApiRoutes.BankClosure.DELETE_BANK_CLOSURE_SETUP)]
        public async Task<IActionResult> DeleteBankClosureSetup([FromBody] DeleteBankClosureSetupCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
