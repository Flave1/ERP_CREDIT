namespace GODP.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_customerkyc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public deposit_customerkyc()
        {
            deposit_customerkycdocumentupload = new HashSet<deposit_customerkycdocumentupload>();
        }

        [Key]
        public int KycId { get; set; }

        public int CustomerId { get; set; }

        public bool? Financiallydisadvantaged { get; set; }

        [StringLength(500)]
        public string Bankpolicydocuments { get; set; }

        public bool? TieredKycrequirement { get; set; }

        public int? RiskCategoryId { get; set; }

        public bool? PoliticallyExposedPerson { get; set; }

        [StringLength(500)]
        public string Details { get; set; }

        [StringLength(500)]
        public string AddressVisited { get; set; }

        [StringLength(500)]
        public string CommentOnLocation { get; set; }

        [StringLength(50)]
        public string LocationColor { get; set; }

        [StringLength(50)]
        public string LocationDescription { get; set; }

        [StringLength(50)]
        public string NameOfVisitingStaff { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfVisitation { get; set; }

        public bool? UtilityBillSubmitted { get; set; }

        public bool? AccountOpeningCompleted { get; set; }

        public bool? RecentPassportPhoto { get; set; }

        [StringLength(50)]
        public string ConfirmationName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ConfirmationDate { get; set; }

        [StringLength(50)]
        public string DeferralFullName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeferralDate { get; set; }

        [StringLength(50)]
        public string DeferralApproved { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual deposit_accountopening deposit_accountopening { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<deposit_customerkycdocumentupload> deposit_customerkycdocumentupload { get; set; }
    }
}
