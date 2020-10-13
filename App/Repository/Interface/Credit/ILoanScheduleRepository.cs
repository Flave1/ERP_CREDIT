using GOSLibraries.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.LoanScheduleObjs;
using static Banking.Contracts.Response.Credit.LookUpViewObjs;

namespace Banking.Repository.Interface.Credit
{
    public interface ILoanScheduleRepository
    {
        int CalculateNumberOfInstallments(TenorModeEnum tenorModeId, int frequencyTypeId, int tenor);

        int CalculateNumberOfInstallments(DateTime firstPaymentDate, DateTime maturityDate, FrequencyTypeEnum frequencyType);

        DateTime CalculateFirstPayDate(DateTime effectiveDate, int frequencyTypeId);

        IEnumerable<LookupObj> GetAllLoanScheduleCategory();

        IEnumerable<LookupObj> GetAllLoanScheduleType();

        IEnumerable<LookupObj> GetLoanScheduleTypeByCategory(int categoryId);

        List<LoanPaymentSchedulePeriodicObj> GeneratePeriodicLoanSchedule(LoanPaymentScheduleInputObj loanInput);

        List<LoanPaymentScheduleDailyObj> GenerateDailyLoanSchedule(LoanPaymentScheduleInputObj loanInput);

        IEnumerable<LookupObj> GetAllFrequencyTypes();

        IEnumerable<LookupObj> GetAllDayCount();

        Task<bool> AddLoanSchedule(int loanId, LoanPaymentScheduleInputObj loanInput);

        Task<bool> AddTempLoanSchedule(int loanId, LoanPaymentScheduleInputObj loanInput);

        Task<bool> AddTempLoanApplicationSchedule(int loanApplicationId, LoanPaymentScheduleInputObj loanInput);

        IEnumerable<LoanPaymentSchedulePeriodicObj> GetPeriodicScheduleByLoaanId(int loanId);

        IEnumerable<LoanPaymentSchedulePeriodicObj> GetPeriodicScheduleByLoaanIdDeleted(int loanId);

        Task<bool> AddReviewedLoanSchedule(int loanId, LoanPaymentScheduleInputObj loanInput);

        byte[] GeneratePeriodicLoanScheduleExport(LoanPaymentScheduleInputObj loanInput);
        List<LoanPaymentSchedulePeriodicObj> GenerateNormalAnnuityPeriodicSchedule(LoanPaymentScheduleInputObj loanInput);
    }
}
