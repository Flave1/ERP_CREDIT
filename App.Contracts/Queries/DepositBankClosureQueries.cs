using Banking.Contracts.Response.Deposit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Queries
{
    public class GetAllDepositBankClosureQuery : IRequest<Deposit_BankClosureRespObj> { }

    public class GetDepositBankClosureQuery : IRequest<Deposit_BankClosureRespObj>
    {
        public GetDepositBankClosureQuery() { }
        public int BankClosureId { get; set; }
        public GetDepositBankClosureQuery(int bankClosureId)
        {
            BankClosureId = bankClosureId;
        }
    }
}
