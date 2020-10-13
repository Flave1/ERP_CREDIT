using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class CustomerSignatoryObj
    {
        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public deposit_customersignatory()
    {
        deposit_customersignature = new HashSet<deposit_customersignature>();
    }*/

        public int SignatoryId { get; set; }

        public int CustomerId { get; set; }

        public int? TitleId { get; set; }

        public int? GenderId { get; set; }

        public int? MaritalStatusId { get; set; }

        public string Surname { get; set; }

        public string Firstname { get; set; }

        public string Othername { get; set; }

        public string ClassofSignatory { get; set; }

        public string IdentificationType { get; set; }

        public string IdentificationNumber { get; set; }

        public string Telephone { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public byte[] SignatureUpload { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? DoB { get; set; }

        public string PlaceOfBirth { get; set; }

        public string MaidenName { get; set; }

        public string NextofKin { get; set; }

        public int? LGA { get; set; }

        public int? StateOfOrigin { get; set; }

        public string TaxIDNumber { get; set; }

        public string MeansOfID { get; set; }

        public DateTime? IDExpiryDate { get; set; }

        public DateTime? IDIssueDate { get; set; }

        public string Occupation { get; set; }

        public string JobTitle { get; set; }

        public string Position { get; set; }

        public int? Nationality { get; set; }

        public string ResidentPermit { get; set; }

        public DateTime? PermitIssueDate { get; set; }

        public DateTime? PermitExpiryDate { get; set; }

        public string SocialSecurityNumber { get; set; }

        public string Address1 { get; set; }

        public string City1 { get; set; }

        public string State1 { get; set; }

        public string Country1 { get; set; }

        public string Address2 { get; set; }

        public string City2 { get; set; }

        public string State2 { get; set; }

        public string Country2 { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        /*public virtual deposit_accountopening deposit_accountopening { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<deposit_customersignature> deposit_customersignature { get; set; }*/
    }

    public class AddUpdateCustomerSignatoryObj
    {
        public int SignatoryId { get; set; }

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
        public string ClassofSignatory { get; set; }

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
    }

    public class CustomerSignatoryRegRespObj
    {
        public int SignatoryId { get; set; }

        public APIResponseStatus Status { get; set; }
    }

    public class CustomerSignatoryRespObj
    {
        public List<CustomerSignatoryObj> CustomerSignatories { get; set; }

        public APIResponseStatus Status { get; set; }
    }
}

