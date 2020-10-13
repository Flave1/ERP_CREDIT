using Banking.Contracts.Response.Credit;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Repository.Interface.Credit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.LoanApplicationCollateralObjs;

namespace Banking.Repository.Implement.Credit
{
    public class LoanApplicationCollateralDocumentRespository : ILoanApplicationCollateralDocumentRespository
    {
        private readonly DataContext _dataContext;
        public LoanApplicationCollateralDocumentRespository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task AddOrUpdateLoanApplicationCollateralDocumentAsync(LoanApplicationCollateralDocumentObj loanApplicationCollateralDocument)
        {
            var loanApplicationCollateralDocumentDb = await _dataContext.credit_loanapplicationcollateraldocument.FirstOrDefaultAsync(x => x.CollateralCustomerId == loanApplicationCollateralDocument.CollateralCustomerId && x.Deleted == false);

            if (loanApplicationCollateralDocumentDb != null)
            {
                loanApplicationCollateralDocumentDb.Deleted = true;

                _dataContext.SaveChanges();
            }

            var loanCol = new credit_loanapplicationcollateraldocument
            {
                CollateralCustomerId = loanApplicationCollateralDocument.CollateralCustomerId,
                CollateralTypeId = loanApplicationCollateralDocument.CollateralTypeId,
                DocumentName = loanApplicationCollateralDocument.DocumentName,
                LoanApplicationId = loanApplicationCollateralDocument.LoanApplicationId,
                Document = loanApplicationCollateralDocument.Document,
                Active = true,
                Deleted = false,
                CreatedBy = loanApplicationCollateralDocument.CreatedBy,
                CreatedOn = DateTime.Now,
                UpdatedBy = loanApplicationCollateralDocument.CreatedBy,
                UpdatedOn = DateTime.Now,
            };

            await _dataContext.credit_loanapplicationcollateraldocument.AddAsync(loanCol);

             var res = await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteLoanApplicationCollateralDocumentAsync(int collateralCustomerId)
        {
            var loanApplicationCollateralDocumentsDb = await _dataContext.credit_loanapplicationcollateraldocument.Where(x => x.CollateralCustomerId == collateralCustomerId).ToListAsync();

            if (loanApplicationCollateralDocumentsDb != null)
            {
                foreach (var document in loanApplicationCollateralDocumentsDb)
                {
                    document.Deleted = true;
                }

                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<IList<LoanApplicationCollateralDocumentObj>> GetAllLoanApplicationCollateralDocumentAsync(int loanApplicationId)
        {
            var loanApplicationCollateralDocumentsDb = await _dataContext.credit_loanapplicationcollateraldocument.Where(x => x.CollateralCustomerId == loanApplicationId && x.Deleted != true).ToListAsync();
            var loanApplicationCollateralsViewModel = new List<LoanApplicationCollateralDocumentObj>();

            foreach (var loanDocument in loanApplicationCollateralsViewModel)
            {
                loanApplicationCollateralsViewModel.Add(new LoanApplicationCollateralDocumentObj
                {
                    CollateralTypeId = loanDocument.CollateralTypeId,
                    DocumentName = loanDocument.DocumentName,
                    LoanApplicationId = loanDocument.LoanApplicationId,
                    Document = loanDocument.Document,
                });
            }

            return loanApplicationCollateralsViewModel;
        }

        public async Task<LoanApplicationCollateralDocumentObj> GetLoanApplicationCollateralDocumentByIdAsync(int CollateralCustomerId)
        {
            var loanApplicationCollateralDocumentDb = await _dataContext.credit_loanapplicationcollateraldocument.FirstOrDefaultAsync(x => x.CollateralCustomerId == CollateralCustomerId && x.Deleted != true);

            return new LoanApplicationCollateralDocumentObj
            {
                CollateralTypeId = loanApplicationCollateralDocumentDb.CollateralTypeId,
                DocumentName = loanApplicationCollateralDocumentDb.DocumentName,
                LoanApplicationId = loanApplicationCollateralDocumentDb.LoanApplicationId,
                Document = loanApplicationCollateralDocumentDb.Document,
            };
        }
    }
}
