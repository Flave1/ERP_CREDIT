namespace GODP.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_accountopening
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public deposit_accountopening()
        {
            deposit_customercontactpersons = new HashSet<deposit_customercontactpersons>();
            deposit_customerdirectors = new HashSet<deposit_customerdirectors>();
            deposit_customeridentification = new HashSet<deposit_customeridentification>();
            deposit_customerkyc = new HashSet<deposit_customerkyc>();
            deposit_customernextofkin = new HashSet<deposit_customernextofkin>();
            deposit_customersignatory = new HashSet<deposit_customersignatory>();
            deposit_customersignature = new HashSet<deposit_customersignature>();
        }

        [Key]
        public int CustomerId { get; set; }

        public int CustomerTypeId { get; set; }

        public int AccountTypeId { get; set; }

        public int? AccountCategoryId { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        public int? Title { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Othername { get; set; }

        public int? MaritalStatusId { get; set; }

        public int? GenderId { get; set; }

        public int? BirthCountryId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [StringLength(50)]
        public string MotherMaidenName { get; set; }

        public int? RelationshipOfficerId { get; set; }

        [StringLength(500)]
        public string TaxIDNumber { get; set; }

        [StringLength(500)]
        public string BVN { get; set; }

        public int? Nationality { get; set; }

        [StringLength(50)]
        public string ResidentPermitNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PermitIssueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PermitExpiryDate { get; set; }

        [StringLength(50)]
        public string SocialSecurityNumber { get; set; }

        public int? StateOfOrigin { get; set; }

        public int? LocalGovernment { get; set; }

        public bool? ResidentOfCountry { get; set; }

        [StringLength(50)]
        public string Address1 { get; set; }

        [StringLength(50)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public int? StateId { get; set; }

        public int? CountryId { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string MailingAddress { get; set; }

        [StringLength(50)]
        public string MobileNumber { get; set; }

        public bool? InternetBanking { get; set; }

        public bool? EmailStatement { get; set; }

        public bool? Card { get; set; }

        public bool? SmsAlert { get; set; }

        public bool? EmailAlert { get; set; }

        public bool? Token { get; set; }

        public int? EmploymentType { get; set; }

        [StringLength(50)]
        public string EmployerName { get; set; }

        [StringLength(50)]
        public string EmployerAddress { get; set; }

        [StringLength(50)]
        public string EmployerState { get; set; }

        [StringLength(50)]
        public string Occupation { get; set; }

        [StringLength(50)]
        public string BusinessName { get; set; }

        [StringLength(50)]
        public string BusinessAddress { get; set; }

        [StringLength(50)]
        public string BusinessState { get; set; }

        [StringLength(50)]
        public string JobTitle { get; set; }

        [StringLength(50)]
        public string Other { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeclarationDate { get; set; }

        public bool? DeclarationCompleted { get; set; }

        public byte[] SignatureUpload { get; set; }

        public int? SoleSignatory { get; set; }

        public int? MaxNoOfSignatory { get; set; }

        [StringLength(50)]
        public string RegistrationNumber { get; set; }

        [StringLength(50)]
        public string Industry { get; set; }

        [StringLength(50)]
        public string Jurisdiction { get; set; }

        [StringLength(50)]
        public string Website { get; set; }

        [StringLength(50)]
        public string NatureOfBusiness { get; set; }

        [StringLength(50)]
        public string AnnualRevenue { get; set; }

        public bool? IsStockExchange { get; set; }

        [StringLength(50)]
        public string Stock { get; set; }

        [StringLength(50)]
        public string RegisteredAddress { get; set; }

        [StringLength(50)]
        public string ScumlNumber { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<deposit_customercontactpersons> deposit_customercontactpersons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<deposit_customerdirectors> deposit_customerdirectors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<deposit_customeridentification> deposit_customeridentification { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<deposit_customerkyc> deposit_customerkyc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<deposit_customernextofkin> deposit_customernextofkin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<deposit_customersignatory> deposit_customersignatory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<deposit_customersignature> deposit_customersignature { get; set; }
    }
}
