namespace GODP.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_customerdirectors
    {
        [Key]
        public int DirectorId { get; set; }

        public int CustomerId { get; set; }

        public int? TitleId { get; set; }

        public int? GenderId { get; set; }

        public int? MaritalStatusId { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Othername { get; set; }

        [StringLength(50)]
        public string IdentificationType { get; set; }

        [StringLength(50)]
        public string IdentificationNumber { get; set; }

        [StringLength(50)]
        public string Telephone { get; set; }

        [StringLength(50)]
        public string Mobile { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public byte[] SignatureUpload { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DoB { get; set; }

        [StringLength(50)]
        public string PlaceOfBirth { get; set; }

        [StringLength(50)]
        public string MaidenName { get; set; }

        [StringLength(50)]
        public string NextofKin { get; set; }

        public int? LGA { get; set; }

        public int? StateOfOrigin { get; set; }

        [StringLength(50)]
        public string TaxIDNumber { get; set; }

        public int? BVN { get; set; }

        [Required]
        [StringLength(50)]
        public string MeansOfID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IDExpiryDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IDIssueDate { get; set; }

        [StringLength(50)]
        public string Occupation { get; set; }

        [StringLength(50)]
        public string JobTitle { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        public int? Nationality { get; set; }

        public bool? ResidentOfCountry { get; set; }

        [StringLength(50)]
        public string ResidentPermit { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PermitIssueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PermitExpiryDate { get; set; }

        [StringLength(50)]
        public string SocialSecurityNumber { get; set; }

        [StringLength(50)]
        public string Address1 { get; set; }

        [StringLength(50)]
        public string City1 { get; set; }

        [StringLength(50)]
        public string State1 { get; set; }

        [StringLength(50)]
        public string Country1 { get; set; }

        [StringLength(50)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string City2 { get; set; }

        [StringLength(50)]
        public string State2 { get; set; }

        [StringLength(50)]
        public string Country2 { get; set; }

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
