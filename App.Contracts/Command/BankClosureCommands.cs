using Banking.Contracts.Response.Deposit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Command
{
    public class AddUpdateBankClosureCommand : IRequest<Deposit_bankClosureRegRespObj>
    {
        public int BankClosureId { get; set; }
        public int? Structure { get; set; }

        public int? SubStructure { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }

        public bool? Status { get; set; }
        public string AccountBalance { get; set; }

        public int? Currency { get; set; }

        public DateTime? ClosingDate { get; set; }

        public string Reason { get; set; }

        public decimal? Charges { get; set; }

        public string FinalSettlement { get; set; }

        public string Beneficiary { get; set; }

        public bool? ModeOfSettlement { get; set; }

        public string TransferAccount { get; set; } 
        public string ApproverName { get; set; } 
        public string ApproverComment { get; set; }
    }
    public class DeleteBankClosureCommand : IRequest<DeleteRespObj>
    {
        public List<int> BankClosureIds { get; set; }
    }
}
