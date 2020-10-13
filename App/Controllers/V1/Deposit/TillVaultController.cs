//using Banking.Contracts.Command;
//using Banking.Contracts.V1;
//using MediatR;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Banking.Controllers.V1.Deposit
//{
//    [Authorize]
//    public class TillVaultController : Controller
//    {
//        private readonly IMediator _mediator;
//        public TillVaultController(IMediator mediator)
//        {
//            _mediator = mediator;
//        }


//        [HttpPost(ApiRoutes.TillVaultEndpoints.ADD_UPDATE_TILL_VAULT)]
//        public async Task<IActionResult> AddUpdateTillVault([FromBody] AddUpdateTillVaultCommand command)
//        {
//            var response = await _mediator.Send(command);
//            if (!response.Status.IsSuccessful)
//                return BadRequest(response);
//            return Ok(response);
//        }


//        [HttpPost(ApiRoutes.TillVaultEndpoints.ADD_UPDATE_TILL_VAULT_SETUP)]
//        public async Task<IActionResult> AddUpdateTillVaultSetup([FromBody] AddUpdateBankClosureSetupCommand command)
//        {
//            var response = await _mediator.Send(command);
//            if (!response.Status.IsSuccessful)
//                return BadRequest(response);
//            return Ok(response);
//        }

//        [HttpPost(ApiRoutes.TillVaultEndpoints.DELETE_TILL_VAULT)]
//        public async Task<IActionResult> DeleteTillVault([FromBody] DeleteTillVaultCommand command)
//        {
//            var response = await _mediator.Send(command);
//            if (!response.Status.IsSuccessful)
//                return BadRequest(response);
//            return Ok(response);
//        }


//        [HttpPost(ApiRoutes.TillVaultEndpoints.DELETE_TILL_VAULT_SETUP)]
//        public async Task<IActionResult> DeleteTillVaultSetup([FromBody] DeleteTillVaultSetupCommand command)
//        {
//            var response = await _mediator.Send(command);
//            if (!response.Status.IsSuccessful)
//                return BadRequest(response);
//            return Ok(response);
//        }
//    }
//}
