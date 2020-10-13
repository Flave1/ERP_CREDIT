using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class credit_creditriskcategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CreditRiskCategoryId { get; set; }

        [Required]
        [StringLength(255)]
        public string CreditRiskCategoryName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public bool UseInOrigination { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
