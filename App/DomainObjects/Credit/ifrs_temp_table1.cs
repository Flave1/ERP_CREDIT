using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Credit
{
    public class ifrs_temp_table1
    {
        [Key]
        public int id4 { get; set; }

        [StringLength(250)]
        public string CustomerName4 { get; set; }

        [StringLength(250)]
        public string LoanReferenceNumber4 { get; set; }

        [StringLength(250)]
        public string ProductCode4 { get; set; }
    }
}
