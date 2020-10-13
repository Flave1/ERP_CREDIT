using System;
using System.Collections.Generic;
using System.Text;
using Banking.DomainObjects;
using Banking.DomainObjects.Auth;
using Banking.DomainObjects.Credit;
using Banking.DomainObjects.Finance;
using Banking.DomainObjects.InvestorFund;
using GODP.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Banking.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext()
        {

        }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }
         
        public DbSet<OTPTracker> OTPTracker { get; set; }
        public DbSet<ConfirmEmailCode> ConfirmEmailCode { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<deposit_accountopening> deposit_accountopening { get; set; }
        public DbSet<deposit_accountreactivation> deposit_accountreactivation { get; set; }
        public DbSet<deposit_accountreactivationsetup> deposit_accountreactivationsetup { get; set; }
        public DbSet<deposit_accountsetup> deposit_accountsetup { get; set; }
        public DbSet<deposit_accountype> deposit_accountype { get; set; }
        public DbSet<deposit_bankclosure> deposit_bankclosure{ get; set; }
        public DbSet<deposit_bankclosuresetup> deposit_bankclosuresetup { get; set; }
        public DbSet<deposit_businesscategory> deposit_businesscategory { get; set; }
        public DbSet<deposit_cashiertellerform> deposit_cashiertellerform { get; set; }
        public DbSet<deposit_cashiertellersetup> deposit_cashiertellersetup { get; set; }
        public DbSet<deposit_category> deposit_category { get; set; }
        public DbSet<deposit_changeofrates> deposit_changeofrates { get; set; }
        public DbSet<deposit_changeofratesetup> deposit_changeofratesetup { get; set; }
        public DbSet<deposit_customercontactpersons> deposit_customercontactpersons { get; set; }
        public DbSet<deposit_customerdirectors> deposit_customerdirectors { get; set; }
        public DbSet<deposit_customeridentification> deposit_customeridentification { get; set; }
        public DbSet<deposit_customerkyc> deposit_customerkyc { get; set; }
        public DbSet<deposit_customerkycdocumentupload> deposit_customerkycdocumentupload { get; set; }
        public DbSet<deposit_customernextofkin> deposit_customernextofkin { get; set; }
        public DbSet<deposit_customersignatory> deposit_customersignatory { get; set; }
        public DbSet<deposit_customersignature> deposit_customersignature { get; set; }
        public DbSet<deposit_depositform> deposit_depositform { get; set; }
        public DbSet<deposit_selectedTransactioncharge> deposit_selectedTransactioncharge { get; set; }
        public DbSet<deposit_selectedTransactiontax> deposit_selectedTransactiontax { get; set; }
        public DbSet<deposit_tillvaultform> deposit_tillvaultform { get; set; }
        public DbSet<deposit_tillvaultopeningclose> deposit_tillvaultopeningclose { get; set; }
        public DbSet<deposit_tillvaultsetup> deposit_tillvaultsetup { get; set; }
        public DbSet<deposit_tillvaultsummary> deposit_tillvaultsummary { get; set; }
        public DbSet<deposit_transactioncharge> deposit_transactioncharge { get; set; }
        public DbSet<deposit_transactioncorrectionform> deposit_transactioncorrectionform { get; set; }
        public DbSet<deposit_transactioncorrectionsetup> deposit_transactioncorrectionsetup { get; set; }
        public DbSet<deposit_transactiontax> deposit_transactiontax { get; set; }
        public DbSet<deposit_transferform> deposit_transferform { get; set; }
        public DbSet<deposit_transfersetup> deposit_transfersetup { get; set; }
        public DbSet<deposit_withdrawalform> deposit_withdrawalform { get; set; }
        public DbSet<deposit_withdrawalsetup> deposit_withdrawalsetup { get; set; }

        public virtual DbSet<credit_casa> credit_casa { get; set; }
        public virtual DbSet<credit_casa_lien> credit_casa_lien { get; set; }
        public virtual DbSet<credit_collateralcustomer> credit_collateralcustomer { get; set; }
        public virtual DbSet<credit_allowable_collateraltype> credit_allowable_collateraltype { get; set; }
        public virtual DbSet<credit_collateralcustomerconsumption> credit_collateralcustomerconsumption { get; set; }
        public virtual DbSet<credit_collateraltype> credit_collateraltype { get; set; }
        public virtual DbSet<credit_corporateapplicationscorecard> credit_corporateapplicationscorecard { get; set; }
        public virtual DbSet<credit_corporateapplicationscorecarddetails> credit_corporateapplicationscorecarddetails { get; set; }
        public virtual DbSet<credit_creditbureau> credit_creditbureau { get; set; }
        public virtual DbSet<credit_creditclassification> credit_creditclassification { get; set; }
        public virtual DbSet<credit_creditrating> credit_creditrating { get; set; }
        public virtual DbSet<credit_creditratingpd> credit_creditratingpd { get; set; }
        public virtual DbSet<credit_creditriskattribute> credit_creditriskattribute { get; set; }
        public virtual DbSet<credit_creditriskcategory> credit_creditriskcategory { get; set; }
        public virtual DbSet<credit_creditriskscorecard> credit_creditriskscorecard { get; set; }
        public virtual DbSet<credit_customerbankdetails> credit_customerbankdetails { get; set; }
        public virtual DbSet<credit_customeridentitydetails> credit_customeridentitydetails { get; set; }
        public virtual DbSet<credit_customerloanlgd_history_final> credit_customerloanlgd_history_final { get; set; }
        public virtual DbSet<credit_customerloanpd> credit_customerloanpd { get; set; }
        public virtual DbSet<credit_customerloanpd_application> credit_customerloanpd_application { get; set; }
        public virtual DbSet<credit_customerloanpd_history> credit_customerloanpd_history { get; set; }
        public virtual DbSet<credit_customerloanpd_history_final> credit_customerloanpd_history_final { get; set; }
        public virtual DbSet<credit_customerloanscorecard> credit_customerloanscorecard { get; set; }
        public virtual DbSet<credit_customernextofkin> credit_customernextofkin { get; set; }
        public virtual DbSet<credit_daily_accural> credit_daily_accural { get; set; }
        public virtual DbSet<credit_daycountconvention> credit_daycountconvention { get; set; }
        public virtual DbSet<credit_directorshareholder> credit_directorshareholder { get; set; }
        public virtual DbSet<credit_directortype> credit_directortype { get; set; }
        public virtual DbSet<credit_documenttype> credit_documenttype { get; set; }
        public virtual DbSet<credit_exposureparament> credit_exposureparament { get; set; }
        public virtual DbSet<credit_fee> credit_fee { get; set; }
        public virtual DbSet<credit_fee_charge> credit_fee_charge { get; set; }
        public virtual DbSet<credit_frequencytype> credit_frequencytype { get; set; }
        public virtual DbSet<credit_impairment> credit_impairment { get; set; }
        public virtual DbSet<credit_impairment_final> credit_impairment_final { get; set; }
        public virtual DbSet<credit_individualapplicationscorecard> credit_individualapplicationscorecard { get; set; }
        public virtual DbSet<credit_individualapplicationscorecard_history> credit_individualapplicationscorecard_history { get; set; }
        public virtual DbSet<credit_individualapplicationscorecarddetails> credit_individualapplicationscorecarddetails { get; set; }
        public virtual DbSet<credit_individualapplicationscorecarddetails_history> credit_individualapplicationscorecarddetails_history { get; set; }
        public virtual DbSet<credit_lgd_historyinformation> credit_lgd_historyinformation { get; set; }
        public virtual DbSet<credit_lgd_historyinformationdetails> credit_lgd_historyinformationdetails { get; set; }
        public virtual DbSet<credit_loan> credit_loan { get; set; }
        public virtual DbSet<credit_loan_archive> credit_loan_archive { get; set; }
        public virtual DbSet<credit_loan_past_due> credit_loan_past_due { get; set; }
        public virtual DbSet<credit_loan_repayment> credit_loan_repayment { get; set; }
        public virtual DbSet<credit_loan_review_operation> credit_loan_review_operation { get; set; }
        public virtual DbSet<credit_loan_temp> credit_loan_temp { get; set; }
        public virtual DbSet<credit_loanapplication> credit_loanapplication { get; set; }
        public virtual DbSet<credit_loanapplication_website> credit_loanapplication_website { get; set; }
        public virtual DbSet<credit_loanapplicationcollateral> credit_loanapplicationcollateral { get; set; }
        public virtual DbSet<credit_loanapplicationcollateraldocument> credit_loanapplicationcollateraldocument { get; set; }
        public virtual DbSet<credit_loanapplicationrecommendationlog> credit_loanapplicationrecommendationlog { get; set; }
        public virtual DbSet<credit_loancomment> credit_loancomment { get; set; }
        public virtual DbSet<credit_loancreditbureau> credit_loancreditbureau { get; set; }
        public virtual DbSet<credit_loancustomer> credit_loancustomer { get; set; }
        public virtual DbSet<credit_loancustomerdirector> credit_loancustomerdirector { get; set; }
        public virtual DbSet<credit_loancustomerdocument> credit_loancustomerdocument { get; set; }
        public virtual DbSet<credit_loancustomerfscaption> credit_loancustomerfscaption { get; set; }
        public virtual DbSet<credit_loancustomerfscaptiondetail> credit_loancustomerfscaptiondetail { get; set; }
        public virtual DbSet<credit_loancustomerfscaptiongroup> credit_loancustomerfscaptiongroup { get; set; }
        public virtual DbSet<credit_loancustomerfsratiodivisortype> credit_loancustomerfsratiodivisortype { get; set; }
        public virtual DbSet<credit_loancustomerfsratiovaluetype> credit_loancustomerfsratiovaluetype { get; set; }
        public virtual DbSet<credit_loancustomerratiodetail> credit_loancustomerratiodetail { get; set; }
        public virtual DbSet<credit_loandecision> credit_loandecision { get; set; }
        public virtual DbSet<credit_loanoperation> credit_loanoperation { get; set; }
        public virtual DbSet<credit_loanreviewapplication> credit_loanreviewapplication { get; set; }
        public virtual DbSet<credit_loanreviewapplicationlog> credit_loanreviewapplicationlog { get; set; }
        public virtual DbSet<credit_loanreviewofferletter> credit_loanreviewofferletter { get; set; }
        public virtual DbSet<credit_loanreviewoperation> credit_loanreviewoperation { get; set; }
        public virtual DbSet<credit_loanreviewoperationirregularinput> credit_loanreviewoperationirregularinput { get; set; }
        public virtual DbSet<credit_loanschedulecategory> credit_loanschedulecategory { get; set; }
        public virtual DbSet<credit_loanscheduledaily> credit_loanscheduledaily { get; set; }
        public virtual DbSet<credit_loanscheduledailyarchive> credit_loanscheduledailyarchive { get; set; }
        public virtual DbSet<credit_loanscheduleirrigular> credit_loanscheduleirrigular { get; set; }
        public virtual DbSet<credit_loanscheduleperiodic> credit_loanscheduleperiodic { get; set; }
        public virtual DbSet<credit_loanscheduleperiodicarchive> credit_loanscheduleperiodicarchive { get; set; }
        public virtual DbSet<credit_loanscheduletype> credit_loanscheduletype { get; set; }
        public virtual DbSet<credit_loanstaging> credit_loanstaging { get; set; }
        public virtual DbSet<credit_loantable> credit_loantable { get; set; }
        public virtual DbSet<credit_loantransactiontype> credit_loantransactiontype { get; set; }
        public virtual DbSet<credit_offerletter> credit_offerletter { get; set; }
        public virtual DbSet<credit_product> credit_product { get; set; }
        public virtual DbSet<credit_productfee> credit_productfee { get; set; }
        public virtual DbSet<credit_productfeestatus> credit_productfeestatus { get; set; }
        public virtual DbSet<credit_producthistoricalpd> credit_producthistoricalpd { get; set; }
        public virtual DbSet<credit_producttype> credit_producttype { get; set; }
        public virtual DbSet<credit_repaymenttype> credit_repaymenttype { get; set; }
        public virtual DbSet<credit_systemattribute> credit_systemattribute { get; set; }
        public virtual DbSet<credit_temp_loanscheduledaily> credit_temp_loanscheduledaily { get; set; }
        public virtual DbSet<credit_temp_loanscheduleirrigular> credit_temp_loanscheduleirrigular { get; set; }
        public virtual DbSet<credit_temp_loanscheduleperiodic> credit_temp_loanscheduleperiodic { get; set; }
        public virtual DbSet<credit_weightedriskscore> credit_weightedriskscore { get; set; }
        public virtual DbSet<cor_allowable_collateraltype> cor_allowable_collateraltype { get; set; }
        public virtual DbSet<tmp_loanapplicationscheduleperiodic> tmp_loanapplicationscheduleperiodic { get; set; }
        public virtual DbSet<credit_loan_cheque> credit_loan_cheque { get; set; }
        public virtual DbSet<credit_loan_cheque_list> credit_loan_cheque_list { get; set; }
        public virtual DbSet<credit_customercarddetails> credit_customercarddetails { get; set; }

        public virtual DbSet<ifrs_setup_data> ifrs_setup_data { get; set; }
        public virtual DbSet<ifrs_macroeconomic_variables_year> ifrs_macroeconomic_variables_year { get; set; }
        public virtual DbSet<ifrs_historical_product_pd> ifrs_historical_product_pd { get; set; }
        public virtual DbSet<ifrs_scenario_setup> ifrs_scenario_setup { get; set; }
        public virtual DbSet<ifrs_temp_table1> ifrs_temp_table1 { get; set; }
        public virtual DbSet<ifrs_computed_forcasted_pd_lgd> ifrs_computed_forcasted_pd_lgd { get; set; }
        public virtual DbSet<ifrs_forecasted_lgd> ifrs_forecasted_lgd { get; set; }
        public virtual DbSet<Ifrs_forecasted_macroeconimcs_mapping> Ifrs_forecasted_macroeconimcs_mapping { get; set; }
        public virtual DbSet<ifrs_forecasted_pd> ifrs_forecasted_pd { get; set; }
        public virtual DbSet<ifrs_historical_macroeconimcs_mapping> ifrs_historical_macroeconimcs_mapping { get; set; }
        public virtual DbSet<ifrs_macroeconomic_variables> ifrs_macroeconomic_variables { get; set; }
        public virtual DbSet<ifrs_pit_formula> ifrs_pit_formula { get; set; }
        public virtual DbSet<ifrs_product_regressed_lgd> ifrs_product_regressed_lgd { get; set; }
        public virtual DbSet<ifrs_product_regressed_pd> ifrs_product_regressed_pd { get; set; }
        public virtual DbSet<ifrs_regress_macro_variable> ifrs_regress_macro_variable { get; set; }
        public virtual DbSet<ifrs_regress_macro_variable_lgd> ifrs_regress_macro_variable_lgd { get; set; }
        public virtual DbSet<ifrs_var_coeff_comp_lgd> ifrs_var_coeff_comp_lgd { get; set; }
        public virtual DbSet<ifrs_var_coeff_comp_pd> ifrs_var_coeff_comp_pd { get; set; }
        public virtual DbSet<ifrs_xxxxx> ifrs_xxxxx { get; set; }



        public virtual DbSet<inf_product> inf_product { get; set; }
        public virtual DbSet<inf_producttype> inf_producttype { get; set; }
        public virtual DbSet<inf_investorfund> inf_investorfund { get; set; }
        public virtual DbSet<inf_collection> inf_collection { get; set; }
        public virtual DbSet<inf_liquidation> inf_liquidation { get; set; }
        public virtual DbSet<inf_investorfund_website> inf_investorfund_website { get; set; }
        public virtual DbSet<inf_investmentrecommendationlog> inf_investmentrecommendationlog { get; set; }
        public virtual DbSet<inf_customer> inf_customer { get; set; }
        public virtual DbSet<inf_investdailyschedule> inf_investdailyschedule { get; set; }
        public virtual DbSet<inf_investdailyschedule_topup> inf_investdailyschedule_topup { get; set; }
        public virtual DbSet<inf_investmentperiodicschedule> inf_investmentperiodicschedule { get; set; }
        public virtual DbSet<inf_liquidation_website> inf_liquidation_website { get; set; }
        public virtual DbSet<inf_collection_website> inf_collection_website { get; set; }
        public virtual DbSet<inf_collectionrecommendationLog> inf_collectionrecommendationLog { get; set; }
        public virtual DbSet<inf_daily_accural> inf_daily_accural { get; set; }
        public virtual DbSet<inf_liquidationrecommendationLog> inf_liquidationrecommendationlog  { get; set; }
        public virtual DbSet<inf_investorfund_topup_website> inf_investorfund_topup_website { get; set; }
        public virtual DbSet<inf_investorfund_rollover_website> inf_investorfund_rollover_website { get; set; }
        public virtual DbSet<fin_customertransaction> fin_customertransaction { get; set; }
        public DbSet<cor_approvaldetail> cor_approvaldetail { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
