using Banking.Contracts.Response.Credit;
using Banking.DomainObjects.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.Credit
{
    public interface IIFRSRepository
    {
        #region IFRS Setup Data
        bool AddUpdateIFRSSetupData(IFRSSetupDataObj entity);
        IEnumerable<IFRSSetupDataObj> GetAllIFRSSetupData();
        IFRSSetupDataObj GetIFRSSetupData(int setupId);
        bool DeleteIFRSSetupData(int setupId);
        #endregion

        #region IFRS Macro Economic Variable
        bool AddUpdateMacroEconomicVariable(MacroEconomicVariableObj entity);
        IEnumerable<MacroEconomicVariableObj> GetAllMacroEconomicVariable();
        MacroEconomicVariableObj GetMacroEconomicVariable(int macroEconomicVariableId);
        bool DeleteMacroEconomicVariable(int macroEconomicVariableId);
        bool ValidateYearExist(int year);
        bool UploadMacroEconomicViriable(List<byte[]> record, string createdBy);
        byte[] GenerateExportMacroEconomicViriable();
        #endregion

        #region IFRS Score Card History
        IEnumerable<ScoreCardHistoryObj> GetAllScoreCardHistory();
        byte[] GenerateExportScoreCardHistory();
        byte[] GenerateExportScoreCardHistoryByProduct(int productId, int customerTypeId);
        bool UploadScoreCardHistory(List<byte[]> record, string createdBy);
        bool DeleteScoreCardHistory(int applicationScoreCardId);
        bool UpdateScoreCardHistoryByLoanDisbursement(int loanId, string createdBy);
        #endregion

        #region IFRS LGD History
        IEnumerable<LGDHistoryObj> GetAllLoanLGDHistory();
        byte[] GenerateExportLGDHistory();
        bool UploadLGDHistory(List<byte[]> record, string createdBy);
        bool DeleteLGDHistory(int historicalLGDId);
        #endregion

        #region Scenario
        bool UpdateIfrsScenarioSetup(IfrsScenarioSetupObj entity);
        IEnumerable<IfrsScenarioSetupObj> GetAllIfrsScenarioSetup();
        IfrsScenarioSetupObj GetIfrsScenarioSetup(int id);
        bool DeleteIfrsScenarioSetup(int id);
        #endregion

        #region Impairment
        IEnumerable<ImpairmentObj> GetImpairmentFromSP();
        byte[] GenerateExportImpairment();
        bool RunImpairment();
        #endregion
    }
}
