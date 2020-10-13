using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class CollateralTypeObjs
    {
        public class CollateralTypeObj : GeneralEntity
        {
            public int CollateralTypeId { get; set; }

            public string Name { get; set; }

            public string Details { get; set; }

            public bool? RequireInsurancePolicy { get; set; }

            public int? ValuationCycle { get; set; }

            public int? HairCut { get; set; }

            public bool? AllowSharing { get; set; }
        }

        public class AddUpdateCollateralTypeObj
        {
            public int CollateralTypeId { get; set; }

            public string Name { get; set; }

            public string Details { get; set; }

            public bool? RequireInsurancePolicy { get; set; }

            public int? ValuationCycle { get; set; }

            public int? HairCut { get; set; }

            public bool? AllowSharing { get; set; }
        }

        public class CollateralTypeRegRespObj
        {
            public int CollateralTypeId { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class CollateralTypeRespObj
        {
            public IEnumerable<CollateralTypeObj> CollateralTypes { get; set; }
            public byte[] export { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class CollateralTypeSearchObj
        {
            public int CollateralTypeId { get; set; }
            public int LoanApplicationId { get; set; }
            public string SearchWord { get; set; }
        }

        public class DeleteCollateralTypeCommand : IRequest<DeleteRespObj>
        {
            public List<int> CollateralTypeIds { get; set; }
        }

        public class DeleteRespObj
        {
            public bool Deleted { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class AllowableCollateralObj
        {
            public int AllowableCollateralId { get; set; }

            public int ProductId { get; set; }

            public int CollateralTypeId { get; set; }
        }

        public class CollateralManagementObj
        {
            public int? LoanApplicationId { get; set; }

            public decimal? ExpectedCollateralValue { get; set; }

            public decimal? TotalCollateralValue { get; set; }

            public int CustomerId { get; set; }

            public string LoanApplicationRefNo { get; set; }

            public string CustomerName { get; set; }

        }
    }
}
