using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_allowable_collateraltype
    {
        [Key]
        public int AllowableCollateralId { get; set; }

        public int ProductId { get; set; }

        public int CollateralTypeId { get; set; }
    }
}
