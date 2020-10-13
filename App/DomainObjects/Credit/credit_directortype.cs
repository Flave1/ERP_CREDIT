using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_directortype
    {
        [Key]
        public int DirectorTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public virtual ICollection<credit_loancustomerdirector> credit_loancustomerdirector { get; set; }
    }
}
