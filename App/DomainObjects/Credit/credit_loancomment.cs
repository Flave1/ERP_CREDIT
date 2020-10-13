﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loancomment
    {
        [Key]
        public int LoanCommentId { get; set; }

        public DateTime? Date { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }

        [StringLength(500)]
        public string NextStep { get; set; }

        public int LoanId { get; set; }

        public int LoanScheduleId { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
