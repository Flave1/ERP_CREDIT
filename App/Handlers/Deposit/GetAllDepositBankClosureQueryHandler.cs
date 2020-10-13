using Banking.Contracts.Queries;
using Banking.Contracts.Response;
using Banking.Contracts.Response.Deposit;
using Banking.Data;
using Banking.Repository.Interface.Deposit;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Handlers.Deposit
{
    public class GetAllDepositBankClosureQueryHandler : IRequestHandler<GetAllDepositBankClosureQuery, Deposit_BankClosureRespObj>
    {
        private readonly IDepositBankClosure _repo;
        private readonly DataContext _dataContext;
        public GetAllDepositBankClosureQueryHandler(IDepositBankClosure depositBankClosure, DataContext dataContext)
        {
            _repo = depositBankClosure;
            _dataContext = dataContext;
        }
        public async Task<Deposit_BankClosureRespObj> Handle(GetAllDepositBankClosureQuery request, CancellationToken cancellationToken)
        {
            
            var branchList = await _dataContext.deposit_bankclosure.Where(x => x.Deleted == false).ToListAsync();

            var list = await _repo.GetAllDepositBankClosureAsync();

            return new Deposit_BankClosureRespObj
            {
                BankClosures = list.Select(x => new Deposit_bankClosureObj { 
                    AccountBalance = x.AccountBalance,
                    AccountName = x.AccountName,
                    AccountNumber = x.AccountNumber,
                    Active = x.Active,
                    ApproverComment = x.ApproverComment,
                    ApproverName = x.ApproverName,
                    BankClosureId = x.BankClosureId,
                    Charges = x.Charges,
                    Beneficiary = x.Beneficiary,
                    ClosingDate = x.ClosingDate,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    Currency = x.Currency,
                    Deleted = x.Deleted,
                    FinalSettlement =x.FinalSettlement,
                    ModeOfSettlement = x.ModeOfSettlement,
                    Reason = x.Reason,
                    Status = x.Status,
                    Structure = x.Structure,
                    SubStructure = x.SubStructure,
                    TransferAccount = x.TransferAccount,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedOn  =x.UpdatedOn,
                    BranchName = branchList.FirstOrDefault(d => d.BankClosureId == x.BankClosureId).AccountName


                }).ToList(),
                Status = new APIResponseStatus
                {
                    IsSuccessful = true,
                    Message = new APIResponseMessage
                    { FriendlyMessage = list.Count() > 0 ? null : "Search Complete! No Record Found", }
                }
            };
        }
    }
}
//_mapper.Map<List<Deposit_bankClosureObj>>(list),