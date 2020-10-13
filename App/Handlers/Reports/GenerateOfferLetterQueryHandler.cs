using Banking.Contracts.Command.Reports; 
using Banking.Repository.Interface.Credit;
using Banking.Requests;
using Finance.Contracts.Response.Reports;
using GOSLibraries.GOS_API_Response;
using MediatR; 
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Handlers.Reports
{
    public class GenerateOfferLetterQueryHandler : IRequestHandler<GenerateOfferLetterQuery, OfferLetterDetailObj>
    {
        private readonly ILoanApplicationRepository _repo;
        private readonly ILoanCustomerRepository _loanCustomerRepo;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly IProduct _product;
        private readonly ILoanRepository _loanRepository;
        
        public GenerateOfferLetterQueryHandler(
            ILoanApplicationRepository loanApplicationRepository,
            ILoanCustomerRepository loanCustomerRepository,
            IIdentityServerRequest serverRequest,
            IProduct product,
            ILoanRepository loanRepository)
        {
            _repo = loanApplicationRepository;
            _serverRequest = serverRequest;
            _product = product;
            _loanCustomerRepo = loanCustomerRepository;
            _loanRepository = loanRepository;
        }
        public async Task<OfferLetterDetailObj> Handle(GenerateOfferLetterQuery request, CancellationToken cancellationToken)
        { 
            var _LoanOffer = await _repo.GetLoanapplicationOfferLeterAsync(request.ApplicationReference); 
            OfferLetterDetailObj offerletterResp = new OfferLetterDetailObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
            if (_LoanOffer != null)
            {
                var _Companies = await _serverRequest.GetAllCompanyAsync();
                var _Currency = await _serverRequest.GetCurrencyAsync();
                var _LoanCustomer = await _loanCustomerRepo.GetLoanCustomersAsync();
                var _Product = await _product.GetProductsAsync();

               
                offerletterResp.CompanyName = _Companies.companyStructures.FirstOrDefault(c => c.companyStructureId == _LoanOffer.CompanyId)?.name;
                offerletterResp.ProductName = _Product.FirstOrDefault(c => c.ProductId == _LoanOffer.ApprovedProductId)?.ProductName;
                offerletterResp.CurrencyName = _Currency.commonLookups.FirstOrDefault(c => c.LookupId == _LoanOffer.CurrencyId)?.LookupName;
                offerletterResp.Tenor = _LoanOffer.ApprovedTenor;
                offerletterResp.InterestRate = _LoanOffer.ApprovedRate;
                offerletterResp.LoanAmount = _LoanOffer.ApprovedAmount;
                offerletterResp.ExchangeRate = _LoanOffer.ExchangeRate;
                offerletterResp.CustomerAddress = _LoanCustomer.FirstOrDefault(a => a.CustomerId == _LoanOffer.CustomerId)?.Address;
                offerletterResp.CustomerEmailAddress = _LoanCustomer.FirstOrDefault(d => d.CustomerId == _LoanOffer.CustomerId)?.Email;
                offerletterResp.CustomerPhoneNumber = _LoanCustomer.FirstOrDefault(d => d.CustomerId == _LoanOffer.CustomerId)?.PhoneNo;
                offerletterResp.ApplicationDate = _LoanOffer.ApplicationDate;
                offerletterResp.LoanApplicationId = _LoanOffer.ApplicationRefNumber;
                offerletterResp.RepaymentSchedule = "Not applicable";
                offerletterResp.RepaymentTerms = "Not applicable";
                offerletterResp.Purpose = _LoanOffer.Purpose;
                offerletterResp.CustomerName = $"{_LoanCustomer.FirstOrDefault(d => d.CustomerId == _LoanOffer.CustomerId)?.FirstName} {_LoanCustomer.FirstOrDefault(d => d.CustomerId == _LoanOffer.CustomerId)?.LastName} " ;
            }

            var decision =  _repo.GetOfferletterDecision(_LoanOffer.LoanApplicationId);
            var customerDocs = await _loanCustomerRepo.GetCustomerDocumentsAsync(_LoanOffer.CustomerId);

            var signatureExist = customerDocs.Any(s => s.DocumentTypeId == 1);
            if (decision != null && signatureExist)
            {
                if (decision.ReportStatus.ToLower().Trim() == "accept")
                {
                    offerletterResp.Signature = customerDocs.FirstOrDefault(x =>  x.DocumentTypeId == 1)?.DocumentFile;
                }
            }

            if (offerletterResp != null)
            {
                offerletterResp.Status.IsSuccessful = true;
                return offerletterResp;
            }

            offerletterResp.Status.Message.FriendlyMessage = "Unable to fetch offer letter";
            return offerletterResp;
        }

    }
}
