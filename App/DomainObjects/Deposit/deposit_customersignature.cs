namespace GODP.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_customersignature
    {
        [Key]
        public int SignatureId { get; set; }

        public int CustomerId { get; set; }

        public int SignatoryId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public byte[] SignatureImg { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual deposit_accountopening deposit_accountopening { get; set; }

        public virtual deposit_customersignatory deposit_customersignatory { get; set; }
    }
}
