using Banking.Contracts.GeneralExtension;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [StringLength(550)]
        public string Address { get; set; }
        public int CustomerTypeId { get; set; }
        public int ApprovalStatusId { get; set; }
        [StringLength(256)]
        public string SecurityAnswered { get; set; } 
        public bool IsItQuestionTime { get; set; }
        public DateTime EnableAtThisTime { get; set; } 
        public int QuestionId { get; set; }
    }



    public class OTPTracker : GeneralEntity
    {
        [Key]
        public int OTPId { get; set; }
        public string OTP { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }

   
}
