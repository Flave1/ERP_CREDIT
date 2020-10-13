using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Auth
{
    public class ConfirmEmailCode
    {
        [Key]
        public int ConfirmEmailCodeId { get; set; }
        public string UserId { get; set; }
        public string ConfirnamationTokenCode { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
