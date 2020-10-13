using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Deposit
{
    public class KyCustomerDocUploadObj
    {
        public int DocumentId { get; set; }

        public int CustomerId { get; set; }

        public int? KycId { get; set; }

        public string DocumentName { get; set; }

        public byte[] DocumentUpload { get; set; }

        public string PhysicalLocation { get; set; }

        public string FileExtension { get; set; }

        public int? DocumentType { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //public virtual deposit_customerkyc deposit_customerkyc { get; set; }
    }

    public class AddUpdateKyCustomerDocUploadObj
    {
        public int DocumentId { get; set; }

        public int CustomerId { get; set; }

        public int? KycId { get; set; }

        [StringLength(50)]
        public string DocumentName { get; set; }

        public byte[] DocumentUpload { get; set; }

        [StringLength(50)]
        public string PhysicalLocation { get; set; }

        [StringLength(50)]
        public string FileExtension { get; set; }

        public int? DocumentType { get; set; }
    }

    public class KyCustomerDocUploadRegRespObj
    {
        public int DocumentId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class KyCustomerDocUploadRespObj
    {
        public List<KyCustomerDocUploadObj> KyCustomerDocUploads { get; set; }

        public APIResponseStatus Status { get; set; }
    }
}

