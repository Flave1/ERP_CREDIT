using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_loancustomerdirector
    {
        [Key]
        public int CustomerDirectorId { get; set; }

        public int DirectorTypeId { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Position { get; set; }

        [Required]
        [StringLength(550)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string PhoneNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        [Column(TypeName = "image")]
        public byte[] Signature { get; set; }

        public bool PoliticallyPosition { get; set; }

        public decimal? PercentageShare { get; set; }

        public bool RelativePoliticallyPosition { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual credit_directortype credit_directortype { get; set; }

        public virtual credit_loancustomer credit_loancustomer { get; set; }
    }
}
