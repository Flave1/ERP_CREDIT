using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class DashboardObj
    {
        public DashboardObj()
        {
            Labels = new string[] { };

            Datasets = new List<Dataset>();
        }

        public string[] Labels { get; set; }

        public List<Dataset> Datasets { get; set; }
    }

    public class Dataset
    {
        public string Label { get; set; }

        public int[] Data { get; set; }

        public bool Fill { get; set; }

        public string BorderColor { get; set; }

        public string BackgroundColor { get; set; }
    }

    public class LoanApplicationDetailObj
    {
        public int TotalLoanApplication { get; set; }
        public int TotalLoanApplicationWebsite { get; set; }

        public int TotalAappraisalLoan { get; set; }

        public int TotalDisbursementLoan { get; set; }

        public int TotalPaymentDue { get; set; }

        public int TotalOverDue { get; set; }
        public List<stagingObj> LoanStagingCount { get; set; }

    }
    public class stagingObj
    {
        public int count { get; set; }
        public string stage { get; set; }
    }

    public class InvestmentApplicationDetailObj
    {
        public int PendingInvestment { get; set; }
        public int PendingCollection { get; set; }
        public int PendingLiquidation { get; set; }
        public int InvestmentAppraisal { get; set; }
        public int LiquidationAppraisal { get; set; }
        public int CollectionAppraisal { get; set; }
        public int TopUpAppraisal { get; set; }
        public int RollOverAppraisal { get; set; }

    }

    public class LoanConcentrationObj
    {
        public DatasetLoanConcentrationObj Datasets { get; set; }

        public string[] Labels { get; set; }

        public decimal? Provisioning { get; set; }
    }

    public class DatasetLoanConcentrationObj
    {
        public decimal?[] Data { get; set; }

        public string[] BackgroundColor { get; set; }

        public string[] HoverBackgroundColor { get; set; }
    }

    public class ProductVolumeObj
    {
        public string ProductName { get; set; }

        public decimal? TotalVolume { get; set; }
    }

    public class CustomerDashboardObj
    {
        public CustomerDashboardObj()
        {
            Labels = new string[] { };

            Datasets = new List<CustomerDatasetObj>();
        }

        public string[] Labels { get; set; }

        public List<CustomerDatasetObj> Datasets { get; set; }
    }

    public class CustomerDatasetObj
    {
        public string Label { get; set; }

        public decimal[] Data { get; set; }
        public string BorderColor { get; set; }

        public string BackgroundColor { get; set; }
    }

    public class DashboardRespObj
    {
        public LoanApplicationDetailObj LoanApplicationDetail { get; set; }
        public DashboardObj PerformanceMetric { get; set; }
        public DashboardObj Par { get; set; }
        public DashboardObj InvestmentChart { get; set; }
        public InvestmentApplicationDetailObj InvestmentApplicationDetail { get; set; }
        public LoanConcentrationObj LoanConcentration { get; set; }
        public LoanConcentrationObj LoanOverDue { get; set; }
        public LoanConcentrationObj InvestmentConcentration { get; set; }
        public ProductVolumeObj ProductVolume { get; set; }
        public CustomerDashboardObj CustomerDashboard { get; set; }
        public CustomerDashboardObj LoanStaging { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class DashboardSearchObj
    {
        public string AccountNumber { get; set; }
    }
}
