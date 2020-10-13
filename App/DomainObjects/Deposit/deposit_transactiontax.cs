namespace GODP.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_transactiontax
    {

        [Key]
        public int TransactionTaxId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string FixedOrPercentage { get; set; }

        public decimal? Amount_Percentage { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual ICollection<deposit_accountsetup> deposit_accountsetup { get; set; }
        //public virtual ICollection<deposit_selectedTransactiontax> deposit_selectedTransactiontax { get; set; }
    }
}
