using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.InvestorFund
{
    public partial class inf_customer
    {
       
        [Key]
        public int InvestorFundCustormerId { get; set; }

        public int? CustomerTypeId { get; set; }

        public int? TitleId { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime? DateofBirth { get; set; }

        public int? GenderId { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        public int? MaritalStatusId { get; set; }

        public int? CompanyStructureId { get; set; }

        [StringLength(50)]
        public string Industry { get; set; }

        public int? Size { get; set; }

        public DateTime? DateOfIncorporation { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public int? CountryId { get; set; }

        public int? CityId { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(50)]
        public string PostalAddress { get; set; }

        public int? RelationshipOfficerId { get; set; }

        public bool? PoliticallyExposed { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string Updatedby { get; set; }

        [StringLength(50)]
        public string Createdby { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Updatedon { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Createdon { get; set; }

    }
}
