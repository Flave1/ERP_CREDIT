using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class TransactionTaxObj
    {
        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public deposit_transactiontax()
    {
        deposit_accountsetup = new HashSet<deposit_accountsetup>();
        deposit_selectedTransactiontax = new HashSet<deposit_selectedTransactiontax>();
    }*/

        public int TransactionTaxId { get; set; }

        public string Name { get; set; }

        public string FixedOrPercentage { get; set; }

        public decimal? Amount_Percentage { get; set; }

        public string Description { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        /*[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<deposit_accountsetup> deposit_accountsetup { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<deposit_selectedTransactiontax> deposit_selectedTransactiontax { get; set; }*/
    }

    public class AddUpdateTransactionTaxObj
    {
        public int TransactionTaxId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string FixedOrPercentage { get; set; }

        [Column("Amount/Percentage")]
        public decimal? Amount_Percentage { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }

    public class TransactionTaxRegRespObj
    {
        public int TransactionTaxId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class TransactionTaxRespObj
    {
        public List<TransactionTaxObj> TransactionTaxes { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

