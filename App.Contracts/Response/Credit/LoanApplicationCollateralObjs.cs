using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using static Banking.Contracts.Response.Credit.CollateralTypeObjs;

namespace Banking.Contracts.Response.Credit
{
    public class LoanApplicationCollateralObjs
    {
        public class LoanApplicationCollateralObj : GeneralEntity
        {
            public int LoanApplicationCollateralId { get; set; }

            public int? CollateralCustomerId { get; set; }
            public int? CollateralCustomerConsumptionId { get; set; }
            public string LoanApplicationRefNo { get; set; }
            public string CollateralTypeName { get; set; }
            public string CollateralCode { get; set; }

            public int? LoanApplicationId { get; set; }
            public decimal ActualCollateralValue { get; set; }
            public decimal CollateralValue { get; set; }
        }

        public class AddUpdateLoanApplicationCollateralObj
        {
            public int LoanApplicationCollateralId { get; set; }

            public int? CollateralCustomerId { get; set; }

            public int? LoanApplicationId { get; set; }
            public decimal ActualCollateralValue { get; set; }
        }

        public class LoanApplicationCollateralRegRespObj
        {
            public int LoanApplicationCollateralId { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class LoanApplicationCollateralRespObj
        {
            public IEnumerable<LoanApplicationCollateralObj> LoanApplicationCollaterals { get; set; }
            public IEnumerable<CollateralManagementObj> CollateralManagement { get; set; }
            public byte[] export { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class LoanApplicationCollateralSearchObj
        {
            public int LoanApplicationCollateralId { get; set; }
            public string SearchWord { get; set; }
        }

        public class DeleteLoanApplicationCollateralCommand : IRequest<DeleteRespObj>
        {
            public List<int> LoanApplicationCollateralIds { get; set; }
            public List<int> collateralCustomerConsumptionIds { get; set; }
        }

        public class DeleteRespObj
        {
            public bool Deleted { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class LoanApplicationCollateralDocumentObj : GeneralEntity
        {
            public int LoanApplicationCollateralDocumentId { get; set; }

            public int? LoanApplicationId { get; set; }

            public int? CollateralTypeId { get; set; }

            public byte[] Document { get; set; }

            public string DocumentName { get; set; }

            public int? CollateralCustomerId { get; set; }
        }
    }
}
