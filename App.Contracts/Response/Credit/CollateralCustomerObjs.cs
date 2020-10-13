using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class CollateralCustomerObjs
    {
        public class CollateralCustomerObj : GeneralEntity
        {
            public int CollateralCustomerId { get; set; }

            public int CustomerId { get; set; }
            public int LoanApplicationId { get; set; }

            public int CollateralTypeId { get; set; }

            public int CurrencyId { get; set; }

            public decimal CollateralValue { get; set; }

            public string Location { get; set; }
            public string CollateralCode { get; set; }
            public string CustomerName { get; set; }
            public string DocumentName { get; set; }
            public string CollateralTypeName { get; set; }
            public string Currency { get; set; }

            public bool? CollateralVerificationStatus { get; set; }
        }

        public class AddUpdateCollateralCustomerObj
        {
            public int CollateralCustomerId { get; set; }

            public int CustomerId { get; set; }

            public int CollateralTypeId { get; set; }

            public int CurrencyId { get; set; }

            public decimal CollateralValue { get; set; }

            public string Location { get; set; }
            public string CollateralCode { get; set; }

            public bool? CollateralVerificationStatus { get; set; }
        }

        public class CollateralCustomerRegRespObj
        {
            public int CollateralCustomerId { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class CollateralCustomerRespObj
        {
            public IEnumerable<CollateralCustomerObj> CollateralCustomers { get; set; }
            public CollateralCustomerConsumptionObj CollateralCustomerConsumption { get; set; }
            public byte[] export { get; set; }
            public decimal amount { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class CollateralCustomerSearchObj
        {
            public int CollateralCustomerId { get; set; }
            public int collateralCustomerConsumptionId { get; set; }
            public int LoanApplicationId { get; set; }
            public int CustomerId { get; set; }
            public bool IncludeNotAllowSharing { get; set; }
            public bool VerificationStatus { get; set; }
            public string SearchWord { get; set; }
        }

        public class DeleteCollateralCustomerCommand : IRequest<DeleteRespObj>
        {
            public List<int> CollateralCustomerIds { get; set; }
        }

        public class DeleteRespObj
        {
            public bool Deleted { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class CollateralCustomerConsumptionObj : GeneralEntity
        {
            public int CollateralCustomerConsumptionId { get; set; }

            public int CollateralCustomerId { get; set; }

            public int? CustomerId { get; set; }

            public decimal CollateralCurrentAmount { get; set; }

            public decimal ActualCollateralValue { get; set; }

            public int LoanApplicationId { get; set; }

            public decimal Amount { get; set; }

            public string CollateralType { get; set; }

            public int ExpectedCollateralValue { get; set; }

            public string CollateralCode { get; set; }
        }
    }
}
