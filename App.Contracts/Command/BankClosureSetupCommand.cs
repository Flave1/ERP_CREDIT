using Banking.Contracts.Response.Deposit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Command
{
    public class AddUpdateBankClosureSetupCommand : IRequest<Deposit_bankClosureSetupRegRespObj>
    {
        public int BankClosureSetupId { get; set; }

        public int? Structure { get; set; }

        public int? ProductId { get; set; }

        public bool? ClosureChargeApplicable { get; set; }

        public string Charge { get; set; }

        public decimal? Amount { get; set; }

        public string ChargeType { get; set; }

        public bool? SettlementBalance { get; set; }

        public bool? PresetChart { get; set; }
    }
    public class DeleteBankClosureSetupCommand : IRequest<DeleteRespObj>
    {
        public List<int> BankClosureSetupIds { get; set; }
    }
}
