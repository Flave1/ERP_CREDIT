using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class ifrs_var_coeff_comp_lgd
    {
        public int Id { get; set; }

        public double? Intercept { get; set; }

        public double? coeff1 { get; set; }

        public double? coeff2 { get; set; }

        public double? coeff3 { get; set; }

        public double? coeff4 { get; set; }

        public double? coeff5 { get; set; }

        public double? coeff6 { get; set; }

        public double? coeff7 { get; set; }

        public double? Macro_Var1 { get; set; }

        public double? Macro_var2 { get; set; }

        public double? Macro_var3 { get; set; }

        public double? Macro_var4 { get; set; }

        public double? Macro_var5 { get; set; }

        public double? Macro_var6 { get; set; }

        public double? Macro_var7 { get; set; }

        public double? LGD_Computed { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        [StringLength(50)]
        public string LoanReferenceNumber { get; set; }

        public int? Year { get; set; }

        [StringLength(50)]
        public string CompanyCode { get; set; }

        public DateTime? RunDate { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string Createdby { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
