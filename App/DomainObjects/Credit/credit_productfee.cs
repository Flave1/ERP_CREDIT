using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_productfee
    {
        [Key]
        public int ProductFeeId { get; set; }

        public int FeeId { get; set; }

        public int ProductPaymentType { get; set; }

        public int ProductFeeType { get; set; }

        public double ProductAmount { get; set; }

        public int ProductId { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual credit_fee credit_fee { get; set; }

        public virtual credit_product credit_product { get; set; }

        public virtual credit_repaymenttype credit_repaymenttype { get; set; }
    }
}
