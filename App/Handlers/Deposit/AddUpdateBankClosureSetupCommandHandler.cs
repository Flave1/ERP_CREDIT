using Banking.AuthHandler.Interface;
using Banking.Contracts.Command;
using Banking.Contracts.Response;
using Banking.Contracts.Response.Deposit; 
using Banking.Repository.Interface.Deposit;
using Banking.Requests;
using GODP.Entities.Models;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Handlers.Deposit
{
    public class AddUpdateBankClosureSetupCommandHandler : IRequestHandler<AddUpdateBankClosureSetupCommand, Deposit_bankClosureSetupRegRespObj>
    {
        private readonly IDepositBankClosure _repo;
        private readonly ILoggerService _logger;
        private readonly IIdentityService _identityService;
        private readonly IIdentityServerRequest _serverRequest;

        public AddUpdateBankClosureSetupCommandHandler(
            ILoggerService loggerService, 
            IIdentityService identityService, 
            IDepositBankClosure depositBankClosure,
            IIdentityServerRequest serverRequest)
        {
            _repo = depositBankClosure;
            _serverRequest = serverRequest;
            _logger = loggerService;
            _identityService = identityService;
        }

        public async Task<Deposit_bankClosureSetupRegRespObj> Handle(AddUpdateBankClosureSetupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                deposit_bankclosuresetup currentItem = null;
                if (request.BankClosureSetupId > 0)
                {
                    currentItem = _repo.GetBankClosureSetup(request.BankClosureSetupId);
                    if (currentItem != null)
                    {
                        if (request.ChargeType.Trim().ToLower() == currentItem.ChargeType.Trim().ToLower())
                        {
                            return new Deposit_bankClosureSetupRegRespObj
                            {
                                Status = new APIResponseStatus
                                {
                                    IsSuccessful = false,
                                    Message = new APIResponseMessage
                                    {
                                        FriendlyMessage = "This item already exists",
                                    }
                                }
                            };
                        }
                    }
                }



                var user = await _serverRequest.UserDataAsync();

                var bankClosureSetup = new deposit_bankclosuresetup
                {
                    BankClosureSetupId = request.BankClosureSetupId,
                    Structure = request.Structure,
                    ProductId = request.ProductId,
                    ClosureChargeApplicable = request.ClosureChargeApplicable,
                    SettlementBalance = request.SettlementBalance,
                    PresetChart = request.PresetChart,
                    Active = true,
                    Deleted = false,
                    CreatedBy = user.UserName,
                    CreatedOn = DateTime.Now,
                };
                if (request.BankClosureSetupId > 0)
                {
                    bankClosureSetup.UpdatedBy = user.UserName;
                    bankClosureSetup.UpdatedOn = DateTime.Now;
                    bankClosureSetup.CreatedBy = currentItem.CreatedBy;
                    bankClosureSetup.CreatedOn = currentItem.CreatedOn;
                }
                var isDone = _repo.AddUpdateBankClosureSetup(bankClosureSetup);
                var actionTaken = request.BankClosureSetupId > 0 ? "updated" : "added";
                if (isDone)
                {
                    return new Deposit_bankClosureSetupRegRespObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = $"Successfully {actionTaken}",
                            }
                        }
                    };
                }
                return new Deposit_bankClosureSetupRegRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Could not process request",
                        }
                    }
                };
            }
            catch (Exception ex)
            {

                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : AddUpdateBankClosureSetupCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new Deposit_bankClosureSetupRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : AddUpdateBankClosureSetupCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion;
            }
        }
    }
}
