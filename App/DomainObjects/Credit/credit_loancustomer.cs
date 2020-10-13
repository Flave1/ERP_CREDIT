using Banking.DomainObjects.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loancustomer
    {
        [Key]
        public int CustomerId { get; set; }

        public int CustomerTypeId { get; set; }

        public int? TitleId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string MiddleName { get; set; }

        public int? GenderId { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public byte[] Signature { get; set; }

        [StringLength(50)]
        public string RegistrationSource { get; set; }
        public DateTime? DOB { get; set; }

        [Required]
        [StringLength(550)]
        public string Address { get; set; }

        [StringLength(550)]
        public string PostaAddress { get; set; }

        public int? CityId { get; set; }

        [StringLength(150)]
        public string Occupation { get; set; }

        public int? EmploymentType { get; set; }

        public bool? PoliticallyExposed { get; set; }

        [StringLength(250)]
        public string CompanyName { get; set; }

        [StringLength(250)]
        public string CompanyWebsite { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string PhoneNo { get; set; }

        [StringLength(50)]
        public string RegistrationNo { get; set; }

        public int? CountryId { get; set; }

        [StringLength(250)]
        public string Industry { get; set; }

        [StringLength(250)]
        public string IncorporationCountry { get; set; }

        public decimal? AnnualTurnover { get; set; }

        public decimal? ShareholderFund { get; set; }

        public int ApprovalStatusId { get; set; }

        public int? MaritalStatusId { get; set; }

        [StringLength(50)]
        public string CASAAccountNumber { get; set; }

        public int? RelationshipManagerId { get; set; }

        public int? CompanyStructureId { get; set; }

        public int? Size { get; set; }

        public decimal? ProfileStatus { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string UserIdentity { get; set; }

        public virtual ICollection<credit_collateralcustomer> credit_collateralcustomer { get; set; }
        public virtual ICollection<credit_corporateapplicationscorecard> credit_corporateapplicationscorecard { get; set; }
        public virtual ICollection<credit_corporateapplicationscorecarddetails> credit_corporateapplicationscorecarddetails { get; set; }
        public virtual ICollection<credit_customerbankdetails> credit_customerbankdetails { get; set; }
        public virtual ICollection<credit_customeridentitydetails> credit_customeridentitydetails { get; set; }
        public virtual ICollection<credit_customernextofkin> credit_customernextofkin { get; set; }
        public virtual ICollection<credit_directorshareholder> credit_directorshareholder { get; set; }
        public virtual ICollection<credit_individualapplicationscorecarddetails> credit_individualapplicationscorecarddetails { get; set; }
        public virtual ICollection<credit_loanapplication> credit_loanapplication { get; set; }
        public virtual ICollection<credit_loancustomerdirector> credit_loancustomerdirector { get; set; }
        public virtual ICollection<credit_loancustomerdocument> credit_loancustomerdocument { get; set; }
        public virtual ICollection<credit_loancustomerfscaptiondetail> credit_loancustomerfscaptiondetail { get; set; }
    }
}
