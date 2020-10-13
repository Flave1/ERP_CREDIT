namespace GODP.Entities.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_customerkycdocumentupload
    {
        [Key]
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

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual deposit_customerkyc deposit_customerkyc { get; set; }
    }
}
