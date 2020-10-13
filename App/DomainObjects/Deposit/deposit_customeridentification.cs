namespace GODP.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_customeridentification
    {
        [Key]
        public int CustomerIdentityId { get; set; }

        public int CustomerId { get; set; }

        public int MeansOfID { get; set; }

        [StringLength(50)]
        public string IDNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateIssued { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual deposit_accountopening deposit_accountopening { get; set; }
    }
}
