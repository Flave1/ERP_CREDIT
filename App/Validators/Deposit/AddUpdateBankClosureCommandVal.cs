using Banking.Contracts.Command;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Validators.Deposit
{
    public class AddUpdateBankClosureCommandVal : AbstractValidator<AddUpdateBankClosureCommand>
    {
        public AddUpdateBankClosureCommandVal()
        {
            RuleFor(x => x.AccountNumber).NotEmpty().Length(10, 10);
            RuleFor(x => x.ApproverName).NotEmpty().Length(2, 200);
        }
    }
}
