﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.DomainObjects.InvestorFund
{
    public class inf_collection_website
    {
        [Key]
        public int WebsiteCollectionOperationId { get; set; }

        public int InvestorFundId { get; set; }

        public int? InvestorFundCustomerId { get; set; }

        public int? ProductId { get; set; }

        public int? ProposedTenor { get; set; }

        public decimal? ProposedRate { get; set; }

        public int? FrequencyId { get; set; }

        public int? Period { get; set; }

        public decimal? ProposedAmount { get; set; }

        public int? CurrencyId { get; set; }

        public DateTime? EffectiveDate { get; set; }

        [StringLength(50)]
        public string InvestmentPurpose { get; set; }

        public DateTime? CollectionDate { get; set; }

        public decimal? AmountPayable { get; set; }

        public int? DrProductPrincipal { get; set; }

        public int? CrReceiverPrincipalGL { get; set; }

        public int? ApprovalStatus { get; set; }

        [StringLength(500)]
        public string PaymentAccount { get; set; }

        [StringLength(500)]
        public string Account { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        [StringLength(50)]
        public string Updatedby { get; set; }

        [StringLength(50)]
        public string Createdby { get; set; }
        public DateTime? Updatedon { get; set; }
        public DateTime? Createdon { get; set; }
    }
}
