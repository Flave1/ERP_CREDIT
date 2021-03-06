﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_frequencytype
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FrequencyTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Mode { get; set; }

        public double Value { get; set; }

        public int? Days { get; set; }

        public bool? IsVisible { get; set; }

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
