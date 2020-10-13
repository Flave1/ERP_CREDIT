using AutoMapper;
using Banking.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Banking.DomainObjects.Auth;
using Banking.Repository.Interface.Deposit;
using Banking.Repository.Implement.Deposit;
using Banking.Repository.Interface.Credit;
using Banking.Repository.Implement.Credit;
using Banking.Repository.Implement.Credit.Collateral;
using Banking.Requests;
using Banking.Repository.Interface.InvestorFund;
using Banking.Repository.Implement.InvestorFund;
using Banking.Repository.Implement;

namespace Banking.Installers
{
    public class DbInstaller : IInstaller
    { 
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                   options.UseSqlServer(
                       configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped<IDepositAccountypeService, DepositAccountypeService>();
            services.AddScoped<IBusinessCategoryService, BusinessCategoryService>();
            services.AddScoped<ICashierTellerService, CashierTellerService>();
            services.AddScoped<IDepositCategoryService, DepositCategoryService>();
            services.AddScoped<ITransactionChargeService, TransactionChargeService>();
            services.AddScoped<ITransactionTaxService, TransactionTaxService>();
            services.AddScoped<IAccountSetupService, AccountSetupService>();
            services.AddScoped<IAccountOpeningService, AccountOpeningService>();
            services.AddScoped<IChangeOfRatesService, ChangeOfRatesService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IWithdrawalService, WithdrawalService>();
            services.AddScoped<ITransferService, TransferService>();

            services.AddScoped<ICreditClassification, CreditClassificationRepository>();
            services.AddScoped<ILoanStaging, LoanStagingRepository>();
            services.AddScoped<IFeeRepository, FeeRepository>();
            services.AddScoped<IProduct, ProductRepository>();
            services.AddScoped<ICollateralRepository, CollateralRepository>();
            services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
            services.AddScoped<ILoanScheduleRepository, LoanScheduleRepository>();
            services.AddScoped<ILoanApplicationCollateralDocumentRespository, LoanApplicationCollateralDocumentRespository>();
            services.AddScoped<ICollateralCustomerConsumptionRepository, CollateralCustomerConsumptionRepository>();
            services.AddScoped<IAllowableCollateralRepository, AllowableCollateralRepository>();
            services.AddScoped<ILoanCustomerRepository, LoanCustomerRepository>();
            services.AddScoped<ICreditRiskScoreCardRepository, CreditRiskScoreCardRepository>();
            services.AddScoped<IIdentityServerRequest, IdentityServerRequest>();
            services.AddScoped<IExposureParameter, ExposureRepository>();
            services.AddScoped<ICreditAppraisalRepository, CreditAppraisalRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<ILoanCustomerFSRepository, LoanCustomerFSRepository>();
            services.AddScoped<ILoanManagementRepository, LoanManagementRepository>();
            services.AddScoped<IIFRSRepository, IFRSRepository>();
            services.AddScoped<ILoanOperationsRepository, LoanOperationsRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IFlutterWaveRequest, FlutterWaveRequest>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IInvestorFundService, InvestorFundService>();

            services.AddTransient<ICommonService, CommonService>();
            services.AddDefaultIdentity<ApplicationUser>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = true;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddAutoMapper(typeof(Startup)); 
            services.AddMediatR(typeof(Startup));
            services.AddMvc();  

            

        }
    }
}
