using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class ProductObjs
    {
        #region Product

        public class ProductObj : GeneralEntity
        {

            public ProductObj()
            {
                Productfee = new List<ProductFeeObj>();
            }
            public int ProductId { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string ProductTypeName { get; set; }
            public int PaymentType { get; set; }
            public string PaymentTypeName { get; set; }
            public double? EarlyTerminationCharge { get; set; }
            public double? LateTerminationCharge { get; set; }
            public double? LowRiskDefinition { get; set; }
            public double Rate { get; set; }
            public int? Period { get; set; }
            public int? InterestType { get; set; }
            public int? CleanUpCircle { get; set; }
            public decimal? WeightedMaxScore { get; set; }
            public int? Defaultvalue { get; set; }
            public int? DefaultRange { get; set; }
            public decimal? Significant2 { get; set; }
            public decimal? Significant3 { get; set; }
            public int? ProductTypeId { get; set; }
            public int? PrincipalGL { get; set; }
            public int? InterestIncomeExpenseGL { get; set; }
            public int? FeeIncomeGL { get; set; }
            public int? InterestReceivablePayableGL { get; set; }
            public int? FrequencyTypeId { get; set; }
            public string FrequencyTypeName { get; set; }
            public int? ScheduleTypeId { get; set; }
            public string ScheduleTypeName { get; set; }
            public string ProductPrincipalGL { get; set; }
            public string InterestIncomeGL { get; set; }
            public string InterestReceivableGL { get; set; }
            public string ChargeFeeGL { get; set; }
            public decimal? CollateralPercentage { get; set; }
            public decimal? PoratedInterest { get; set; }
            public decimal? ProductLimit { get; set; }
            public int? Tenor { get; set; }
            public int[] AllowableCollaterals { get; set; }
            public List<ProductFeeObj> Productfee { get; set; }
        }
        public class AddUpdateProductObj
        {
            public int ProductId { get; set; }

            public string ProductCode { get; set; }

            public string ProductName { get; set; }

            public int PaymentType { get; set; }

            public bool CollateralRequired { get; set; }

            public bool QuotedInstrument { get; set; }

            public double Rate { get; set; }

            public bool LateRepayment { get; set; }

            public int? Period { get; set; }

            public int? CleanUpCircle { get; set; }

            public decimal? WeightedMaxScore { get; set; }

            public int? Default { get; set; }

            public int? DefaultRange { get; set; }

            public decimal? Significant2 { get; set; }

            public decimal? Significant3 { get; set; }

            public int? ProductTypeId { get; set; }

            public int? PrincipalGL { get; set; }

            public int? InterestIncomeExpenseGL { get; set; }

            public int? InterestReceivablePayableGL { get; set; }

            public int? FrequencyTypeId { get; set; }

            public int? TenorInDays { get; set; }

            public int? ScheduleTypeId { get; set; }

            public decimal? CollateralPercentage { get; set; }

            public decimal? ProductLimit { get; set; }

            public double? LateTerminationCharge { get; set; }

            public double? EarlyTerminationCharge { get; set; }

            public double? LowRiskDefinition { get; set; }

            public int? FeeIncomeGL { get; set; }

            public int? InterestType { get; set; }
            public int[] AllowableCollaterals { get; set; }
        }

        public class ProductRegRespObj
        {
            public ProductObj Products { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class ProductRespObj
        {
            public IEnumerable<ProductObj> Products { get; set; }
            public byte[] export { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class ProductSearchObj
        {
            public int ProductId { get; set; }
            public string SearchWord { get; set; }
        }

        public class DeleteProductCommand
        {
            public List<int> ProductIds { get; set; }
        }

        //public class DeleteRespObj
        //{
        //    public bool Deleted { get; set; }
        //    public APIResponseStatus Status { get; set; }
        //}
        #endregion

        #region Product Type
        public class ProductTypeObj
        {
            public int ProductTypeId { get; set; }

            public string ProductTypeName { get; set; }

            public bool? Active { get; set; }

            public bool? Deleted { get; set; }

            public string CreatedBy { get; set; }

            public DateTime? CreatedOn { get; set; }
            public string UpdatedBy { get; set; }

            public DateTime? UpdatedOn { get; set; }
        }
        public class AddUpdateProductTypeObj
        {
            public int ProductTypeId { get; set; }

            public string ProductTypeName { get; set; }
        }

        public class ProductTypeRegRespObj
        {
            public int ProductTypeId { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class ProductTypeRespObj
        {
            public List<ProductTypeObj> ProductType { get; set; }
            public byte[] export { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class ProductTypeSearchObj
        {
            public int ProductTypeId { get; set; }
            public string SearchWord { get; set; }
        }

        public class DeleteProductTypeCommand 
        {
            public List<int> Ids { get; set; }
        }
        #endregion

        #region ProductFee
        public class ProductFeeObj
        {
            public int ProductFeeId { get; set; }
            public int FeeId { get; set; }
            public string ProductFeeName { get; set; }
            public int ProductPaymentType { get; set; }
            public int ProductFeeType { get; set; }
            public double ProductAmount { get; set; }
            public int ProductId { get; set; }
            public string ProductPaymentTypeName { get; set; }
            public string ProductName { get; set; }
            public string FeeName { get; set; }
            public string ProductFeeTypeName { get; set; }
            public string FeeTypeName { get; set; }
            public bool Status { get; set; }
            public bool IsIntegral { get; set; }
            public decimal RateValue { get; set; }
        }
        public class AddUpdateProductFeeObj
        {
            public int ProductFeeId { get; set; }

            public int FeeId { get; set; }

            public int ProductPaymentType { get; set; }

            public int ProductFeeType { get; set; }

            public double ProductAmount { get; set; }

            public int ProductId { get; set; }
        }

        public class ProductFeeRegRespObj
        {
            public int ProductFeeId { get; set; }
            public APIResponseStatus Status { get; set; }
        }
        public class ProductFeeRespObj
        {
            public List<ProductFeeObj> ProductFee { get; set; }
            public byte[] export { get; set; }
            public APIResponseStatus Status { get; set; }
        }

        public class ProductFeeSearchObj
        {
            public int ProductFeeId { get; set; }
            public int ProductId { get; set; }
            public int LoanApplicationId { get; set; }
            public string SearchWord { get; set; }
        }

        public class DeleteProductFeeCommand 
        {
            public List<int> Ids { get; set; }
        }
        #endregion
    }
}
