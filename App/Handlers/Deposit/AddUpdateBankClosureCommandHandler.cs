using Banking.AuthHandler.Interface;
using Banking.Contracts.Command;
using Banking.Contracts.Response;
using Banking.Contracts.Response.Deposit;
using Banking.DomainObjects;
using Banking.DomainObjects.Auth; 
using Banking.Repository.Interface.Deposit;
using Banking.Requests;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Handlers.Deposit
{
    public class AddUpdateBankClosureCommandHandler : IRequestHandler<AddUpdateBankClosureCommand, Deposit_bankClosureRegRespObj>
    {
        private readonly IDepositBankClosure _repo;
        private readonly ILoggerService _logger;
        private readonly IIdentityService _identityService;
        private readonly IIdentityServerRequest _serverRequest;
        public AddUpdateBankClosureCommandHandler(
            ILoggerService loggerService, 
            IIdentityService identityService, 
            IDepositBankClosure depositBankClosure,
            IIdentityServerRequest serverRequest)
        {
            _repo = depositBankClosure;
            _logger = loggerService;
            _serverRequest = serverRequest;
            _identityService = identityService;
        }
        public async Task<Deposit_bankClosureRegRespObj> Handle(AddUpdateBankClosureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                deposit_bankclosure currentItem = null;
                if (request.BankClosureId > 0)
                {
                     currentItem = _repo.GetDepositBankClosure(request.BankClosureId);
                    if(currentItem != null)
                    {
                        if(request.AccountName.Trim().ToLower() == currentItem.AccountName.Trim().ToLower())
                        {
                            return new Deposit_bankClosureRegRespObj
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

                var bnkClosure = new deposit_bankclosure
                {
                    BankClosureId = request.BankClosureId,
                    Structure = request.Structure,
                    SubStructure = request.SubStructure,
                    AccountName = request.AccountName,
                    AccountNumber = request.AccountNumber,
                    Status = request.Status,
                    AccountBalance = request.AccountBalance,
                    Currency = request.Currency,
                    ClosingDate = request.ClosingDate,
                    Reason = request.Reason,
                    Charges = request.Charges,
                    FinalSettlement = request.FinalSettlement,
                    Beneficiary = request.Beneficiary,
                    ModeOfSettlement = request.ModeOfSettlement,
                    TransferAccount = request.TransferAccount,
                    ApproverName = request.ApproverName,
                    ApproverComment = request.ApproverComment,
                    Active = true,
                    Deleted = false,
                    CreatedBy = user.UserName,
                    CreatedOn = DateTime.Now,
                };
                if(request.BankClosureId > 0)
                {

                    bnkClosure.UpdatedBy = user.UserName;
                    bnkClosure.UpdatedOn = DateTime.Now;
                    bnkClosure.CreatedBy = currentItem.CreatedBy;
                    bnkClosure.CreatedOn = currentItem.CreatedOn;

                }
               var isDone = _repo.AddUpdateDepositBankClosure(bnkClosure);
                var actionTaken = request.BankClosureId > 0 ? "updated" : "added";
                if (isDone) 
                {
                    return new Deposit_bankClosureRegRespObj
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
                return new Deposit_bankClosureRegRespObj
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
                _logger.Error($"ErrorID : AddUpdateBankClosureCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new Deposit_bankClosureRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : AddUpdateBankClosureCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
    }
}
