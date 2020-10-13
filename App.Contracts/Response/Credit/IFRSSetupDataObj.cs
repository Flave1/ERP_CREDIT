using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.Credit
{
    public class IFRSSetupDataObj : GeneralEntity
    {
        public int SetUpId { get; set; }
        public double? Threshold { get; set; }
        public int? Deteroriation_Level { get; set; }
        public int? Classification_Type { get; set; }
        public int? Historical_PD_Year_Count { get; set; }
        public bool? PdBasis { get; set; }
        public int? Ltpdapproach { get; set; }
        public double? Ccf { get; set; }
        public string GroupBased { get; set; }
        public DateTime? RunDate { get; set; }
    }

    public class ScoreCardHistoryObj
    {
        public int ApplicationScoreCardId { get; set; }

        public int? CustomerTypeId { get; set; }

        public decimal LoanAmount { get; set; }

        public string CustomerName { get; set; }

        public string LoanReferenceNumber { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? MaturityDate { get; set; }

        public decimal? Field1 { get; set; }

        public decimal? Field2 { get; set; }

        public decimal? Field3 { get; set; }

        public decimal? Field4 { get; set; }

        public decimal? Field5 { get; set; }

        public decimal? Field6 { get; set; }

        public decimal? Field7 { get; set; }

        public decimal? Field8 { get; set; }

        public decimal? Field9 { get; set; }

        public decimal? Field10 { get; set; }

        public decimal? Field11 { get; set; }

        public decimal? Field12 { get; set; }

        public decimal? Field13 { get; set; }

        public decimal? Field14 { get; set; }

        public decimal? Field15 { get; set; }

        public decimal? Field16 { get; set; }

        public decimal? Field17 { get; set; }

        public decimal? Field18 { get; set; }

        public decimal? Field19 { get; set; }

        public decimal? Field20 { get; set; }

        public decimal? Field21 { get; set; }

        public decimal? Field22 { get; set; }

        public decimal? Field23 { get; set; }

        public decimal? Field24 { get; set; }

        public decimal? Field25 { get; set; }

        public decimal? Field26 { get; set; }

        public decimal? Field27 { get; set; }

        public decimal? Field28 { get; set; }

        public decimal? Field29 { get; set; }

        public decimal? Field30 { get; set; }

    }

    public class MacroEconomicVariableObj : GeneralEntity
    {
        public int MacroEconomicVariableId { get; set; }

        public int? Year { get; set; }

        public double? Gdp { get; set; }

        public double? Unemployement { get; set; }

        public double? Inflation { get; set; }

        public double? Erosion { get; set; }

        public double? ForegnEx { get; set; }

        public double? Others { get; set; }

        public double? Otherfactor { get; set; }
    }

    public class LGDHistoryObj
    {
        public int HistoricalLGDId { get; set; }

        public decimal LoanAmount { get; set; }

        public string CustomerName { get; set; }

        public string LoanReferenceNumber { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? MaturityDate { get; set; }

        public double? Cr { get; set; }

        public double? Eir { get; set; }

        public string Frequency { get; set; }

        public decimal? Field1 { get; set; }

        public decimal? Field2 { get; set; }

        public decimal? Field3 { get; set; }

        public decimal? Field4 { get; set; }

        public decimal? Field5 { get; set; }

        public decimal? Field6 { get; set; }

        public decimal? Field7 { get; set; }

        public decimal? Field8 { get; set; }

        public decimal? Field9 { get; set; }

        public decimal? Field10 { get; set; }

        public decimal? Field11 { get; set; }

        public decimal? Field12 { get; set; }

        public decimal? Field13 { get; set; }

        public decimal? Field14 { get; set; }

        public decimal? Field15 { get; set; }

        public decimal? Field16 { get; set; }

        public decimal? Field17 { get; set; }

        public decimal? Field18 { get; set; }

        public decimal? Field19 { get; set; }

        public decimal? Field20 { get; set; }

        public decimal? Field21 { get; set; }

        public decimal? Field22 { get; set; }

        public decimal? Field23 { get; set; }

        public decimal? Field24 { get; set; }

        public decimal? Field25 { get; set; }

        public decimal? Field26 { get; set; }

        public decimal? Field27 { get; set; }

        public decimal? Field28 { get; set; }

        public decimal? Field29 { get; set; }

        public decimal? Field30 { get; set; }

        public decimal? Field31 { get; set; }

        public decimal? Field32 { get; set; }

        public decimal? Field33 { get; set; }

        public decimal? Field34 { get; set; }

        public decimal? Field35 { get; set; }

        public decimal? Field36 { get; set; }

        public decimal? Field37 { get; set; }

        public decimal? Field38 { get; set; }

        public decimal? Field39 { get; set; }

        public decimal? Field40 { get; set; }

        public decimal? Field41 { get; set; }

        public decimal? Field42 { get; set; }

        public decimal? Field43 { get; set; }

        public decimal? Field44 { get; set; }

        public decimal? Field45 { get; set; }

        public decimal? Field46 { get; set; }

        public decimal? Field47 { get; set; }

        public decimal? Field48 { get; set; }

        public decimal? Field49 { get; set; }

        public decimal? Field50 { get; set; }

        public decimal? Field51 { get; set; }

        public decimal? Field52 { get; set; }

        public decimal? Field53 { get; set; }

        public decimal? Field54 { get; set; }

        public decimal? Field55 { get; set; }

        public decimal? Field56 { get; set; }

        public decimal? Field57 { get; set; }

        public decimal? Field58 { get; set; }

        public decimal? Field59 { get; set; }

        public decimal? Field60 { get; set; }
    }

    public class IfrsScenarioSetupObj : GeneralEntity
    {

        public int ScenarioId { get; set; }

        public string Scenario { get; set; }

        public decimal? Likelihood { get; set; }

        public decimal? Rate { get; set; }
    }

    public class ImpairmentObj
    {
        public int ImpairmentFinalId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string ECLType { get; set; }

        public string Stage { get; set; }

        public string Scenario { get; set; }

        public decimal? Likelihood { get; set; }

        public decimal? Rate { get; set; }

        public decimal PD { get; set; }

        public decimal LGD { get; set; }

        public decimal? EAD { get; set; }

        public decimal? ECL { get; set; }
        public DateTime Date { get; set; }
    }

    public class IFRSRespObj
    {
        public IEnumerable<IFRSSetupDataObj> SetUpData { get; set; }
        public IEnumerable<IfrsScenarioSetupObj> IfrsScenarioSetup { get; set; }
        public IEnumerable<ScoreCardHistoryObj> ScoreCardHistory { get; set; }
        public IEnumerable<MacroEconomicVariableObj> MacroVariables { get; set; }
        public IEnumerable<LGDHistoryObj> LGDHistory { get; set; }
        public IEnumerable<ImpairmentObj> Impairment { get; set; }
        public byte[] Export { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class IFRSSearchObj
    {
        public int ScenarioId { get; set; }
        public int HistoricalLGDId { get; set; }
        public int MacroEconomicVariableId { get; set; }
        public int ApplicationScoreCardId { get; set; }
        public int SetUpId { get; set; }
        public int ProductId { get; set; }
        public int CustomerTypeId { get; set; }
    }
}
