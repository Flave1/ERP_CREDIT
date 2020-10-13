﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_corporateapplicationscorecard
    {
        [Key]
        public int ApplicationScoreCardId { get; set; }

        public int LoanApplicationId { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public decimal? Field1 { get; set; }

        public decimal? Field2 { get; set; }

        public decimal? Field3 { get; set; }

        public decimal? Field4 { get; set; }

        public decimal? Field5 { get; set; }

        public decimal? Field6 { get; set; }

        public decimal? Field7 { get; set; }

        public decimal? Field8 { get; set; }

        public decimal? Field9 { get; set; }

        public decimal? Field10 { get; set; }

        public decimal? Field11 { get; set; }

        public decimal? Field12 { get; set; }

        public decimal? Field13 { get; set; }

        public decimal? Field14 { get; set; }

        public decimal? Field15 { get; set; }

        public decimal? Field16 { get; set; }

        public decimal? Field17 { get; set; }

        public decimal? Field18 { get; set; }

        public decimal? Field19 { get; set; }

        public decimal? Field20 { get; set; }

        public decimal? Field21 { get; set; }

        public decimal? Field22 { get; set; }

        public decimal? Field23 { get; set; }

        public decimal? Field24 { get; set; }

        public decimal? Field25 { get; set; }

        public decimal? Field26 { get; set; }

        public decimal? Field27 { get; set; }

        public decimal? Field28 { get; set; }

        public decimal? Field29 { get; set; }

        public decimal? Field30 { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual credit_loanapplication credit_loanapplication { get; set; }

        public virtual credit_loancustomer credit_loancustomer { get; set; }

        public virtual credit_product credit_product { get; set; }
    }
}
