using System;
using System.Collections.Generic;

namespace Banking.Contracts.V1
{
    public class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class ReportEndpoints
        {
            public const string LOAN_OFFER_LETTER = Base + "/loanapplication/generate/offerletter";
            public const string LOAN_OFFER_LETTER_LMS = Base + "/loanapplication/generate/offerletter/lms";
            public const string LOAN_OFFER_LETTER_INVESTMENT = Base + "/loanapplication/generate/offerletter/investment";
            public const string LOAN_OFFER_LETTER_SCHEDULE = Base + "/loanapplication/generate/offerletter/schedule";
            public const string LOAN_OFFER_LETTER_FEE = Base + "/loanapplication/generate/offerletter/fee";
            public const string LOAN_INDIVIDUAL_CUSTOMER_REPORT = Base + "/loanapplication/loan/individual/customer/report";
            public const string LOAN_CORPORATE_CUSTOMER_REPORT = Base + "/loanapplication/loan/corporate/customer/report";
            public const string LOAN_REPORT = Base + "/loanapplication/loan/report";            
            public const string INVESTMENT_INDIVIDUAL_CUSTOMER_REPORT = Base + "/loanapplication/investment/individual/customer/report";
            public const string INVESTMENT_CORPORATE_CUSTOMER_REPORT = Base + "/loanapplication/investment/corporate/customer/report";
            public const string INVESTMENT_REPORT = Base + "/loanapplication/investment/report";
        }
            public static class Identity
        {
            public const string CUSTOMER_LOGIN = Base + "/customer/identity/login";
            public const string CUSTOMER_OTP_LOGIN = Base + "/customerotp/identity/login";
            public const string ANSWER = Base + "/customer/answer/question";
            public const string GET_ANSWER = Base + "/customer/get/answer/question";
            public const string RECOVER_PASSWORD_BY_EMAIL = Base + "/customer/identity/recoverpassword/byemail";
            public const string NEW_PASS = Base + "/customer/identity/newpassword";
            public const string LOGIN = "/identity/login";
            public const string SECURITY = "/admin/auth/guard/get/all";
            public const string FAILED_LOGIN = "/identity/failed/login";
            public const string Session_LOGIN = "/identity/session/login";
            public const string CUSTOMER_REGISTER = Base + "/customer/identity/register";
            public const string CUSTOMER_REFRESHTOKEN = Base + "/customer/identity/refresh";
            public const string CUSTOMER_CHANGE_PASSWORD = Base + "/customer/identity/changePassword";
            public const string CUSTOMER_CONFIRM_EMAIL = Base + "/customer/identity/confirmEmail";
            public const string CUSTOMER_VERIFY_EMAIL = Base + "/customer/identity/verifyEmail";
            public const string FETCH_USERDETAILS =  "/identity/profile";
            public const string FETCH_CUSTOMER_USERDETAILS = Base + "/customer/identity/profile";
            public const string CUSTOMER_CONFIRM_CODE = Base + "/customer/identity/confirmCode";
            public const string GET_PERMIT_DEATAILS = "/identity/permitDetails";
            public const string SEND_MAIL = "/email/send/emails";
            public const string GET_FLUTTERWAVE_KEYS = "/payment/flutterwave/get/keys";

            
        }


        public static class IdentitySeverWorkflow
        {
            //CALLED THROUGH CODE
            public const string GO_FOR_APPROVAL = "/workflow/goThroughApprovalFromCode";
            public const string GET_ALL_STAFF_AWAITING_APPROVALS = "/workflow/get/all/staffAwaitingApprovalsFromCode";
            public const string STAFF_APPROVAL_REQUEST = "/workflow/staff/approvaltask";
            public const string GET_ALL_STAFF = "/admin/get/all/staff";
            public const string GET_QUESTION = "/admin/get/all/questions";
            public const string ACTIVITES = "/admin/get/all/activities";
            public const string GET_THIS_ROLES = "/admin/get/this/userroles";
        }

        public static class Finance
        {
            public const string PASS_TO_ENTRY = "/financialtransaction/pass/to/entry";
            public const string GET_ALL_SUBGL = "/subgl/get/all";
        }
        public static class Currency
        {
            public const string GET_ALL_CURRENCY = "/common/currencies";
            public const string GET_ALL_GENDER = "/common/genders";
            public const string GET_ALL_MARITAL_STATUS = "/common/maritalStatus";
            public const string GET_ALL_EMPLOYMENT_TYPE = "/common/employerTypes";
            public const string GET_ALL_IDENTITICATION_TYPE = "/common/Identifications";
            public const string GET_CURRENCY_BY_ID = "/common/get/single/currencyById";
        }
        public static class CompanyStructure
        {
            public const string GET_ALL_COMPANY_STRUCTURE = "/company/get/all/companystructures";
            public const string GET_COMPANY_STRUCTURE_BY_ID = "/company/get/single/companystructure/id";
        }
        public static class Workflow
        {
            public const string ADD_WORKFLOW = "/workflow/add/update/workflow";
            public const string GET_WORKFLOW_BY_ID = "/workflow/get/all/workflow/workflowId";
            public const string GET_ALL_OPERATION_TYPES = "/workflow/get/all/operationTypes";
            public const string GET_ALL_OPERATIONS = "/workflow/get/all/operations";
            public const string STAFF_APPROVAL_REQUEST = "/workflow/staff/approvaltask";
            public const string STAFF_CAN_EDIT = "/workflow/staff/canedit";
        }

        #region Credit
        public class Dashboard
        {
            //CreditDashboard
            public const string GET_PERFORMANCE= Base + "/dashboard/performancematrics";
            public const string GET_CUSTOMER_TRANSACTION = Base + "/dashboard/customer/transaction/summary";
            public const string GET_LOANAPPLICATION_DETAILS = Base + "/dashboard/loanapplicationdetails";
            public const string GET_CONCENTRATION_DETAILS = Base + "/dashboard/loanconcentrationdetails";
            public const string GET_LOAN_PAR = Base + "/dashboard/loanpar";
            public const string GET_LOANSTAGING = Base + "/dashboard/loanstaging";
            public const string GET_LOANOVERDUE = Base + "/dashboard/loanoverdue";

            //InvestmentDashboard
            public const string GET_INVESTMENT_APP_DETAILS = Base + "/dashboard/investmentapplicationdetails";
            public const string GET_INVESTMENT_APP_CHARTS = Base + "/dashboard/investmentapplication/chart";
            public const string GET_INVESTMENT_CONCENTRATION = Base + "/dashboard/investment/concentration";
        }

        public class Fee
        {
            public const string ADD_FEE = Base + "/fee/add/update";
            public const string GET_ALL_FEE = Base + "/fee/get/all";
            public const string GET_REPAYMENT_TYPE = Base + "/fee/get/allrepaymenttype";
            public const string GET_ALL_INTEGRAL_FEE = Base + "/fee/get/allintegralfee";
            public const string GET_FEE_BY_ID = Base + "/fee/get/feeById";
            public const string UPLOAD_FEE = Base + "/fee/upload";
            public const string DOWNLOAD_FEE = Base + "/fee/download";
            public const string DELETE_FEE = Base + "/fee/delete";
        }

            public class Ifrs
        {
            public const string ADD_SETUP_DATA = Base + "/ifrs/add/update/setup/data";
            public const string GET_ALL_SETUP_DATA = Base + "/ifrs/get/all/setup/data";
            public const string GET_SETUP_DATA_ID = Base + "/ifrs/get/single/setup/data";
            public const string DELETE_SETUP_DATA = Base + "/ifrs/delete/setup/data";

            public const string ADD_SCENARIO_SETUP = Base + "/ifrs/add/update/scenario/setup";
            public const string GET_ALL_SCENARIO_SETUP = Base + "/ifrs/get/all/scenario/setup";
            public const string GET_SCENARIO_SETUP_ID = Base + "/ifrs/get/single/scenario/setup";
            public const string DELETE_SCENARIO_SETUP = Base + "/ifrs/delete/scenario/setup";

            public const string ADD_MACRO_VARIABLE = Base + "/ifrs/add/update/macro/economic/variable";
            public const string GET_ALL_MACRO_VARIABLE = Base + "/ifrs/get/all/macro/economic/variable";
            public const string GET_MACRO_VARIABLE_ID = Base + "/ifrs/get/single/macro/economic/variable";
            public const string DELETE_MACRO_VARIABLE = Base + "/ifrs/delete/macro/economic/variable";
            public const string UPLOAD_MACRO_VARIABLE = Base + "/ifrs/upload/macro/economic/variable";
            public const string DOWNLOAD_MACRO_VARIABLE = Base + "/ifrs/download/macro/economic/variable";

            public const string GET_ALL_SCORE_CARD = Base + "/ifrs/get/all/score/card/history";
            public const string DELETE_SCORE_CARD = Base + "/ifrs/delete/score/card/history";
            public const string UPLOAD_SCORE_CARD = Base + "/ifrs/upload/score/card/history";
            public const string DOWNLOAD_HISTORICAL_PD = Base + "/ifrs/download/historical/pd";
            public const string DOWNLOAD_SCORE_CARD = Base + "/ifrs/download/score/card/history";

            public const string GET_ALL_LGD = Base + "/ifrs/get/all/lgd/history";
            public const string DELETE_LGD = Base + "/ifrs/delete/lgd/history";
            public const string UPLOAD_LGD = Base + "/ifrs/upload/lgd/history";
            public const string DOWNLOAD_LGD = Base + "/ifrs/download/lgd/history";

            public const string RUN_IMPAIRMENT = Base + "/ifrs/run/impairment";
            public const string GET_IMPAIRMENT = Base + "/ifrs/get/impairment";
            public const string DOWNLOAD_IMPAIRMENT = Base + "/ifrs/get/export/impairment";
        }

        public class LoanStaging
        {
            public const string ADD_LOAN_STAGING = Base + "/loanstaging/add/update";
            public const string GET_ALL_LOAN_STAGING = Base + "/loanstaging/get/all";
            public const string GET_LOAN_STAGING_BY_ID = Base + "/loanstaging/get/loanstagingById";
            public const string UPLOAD_LOAN_STAGING = Base + "/loanstaging/upload";
            public const string DOWNLOAD_LOAN_STAGING = Base + "/loanstaging/download";
            public const string DELETE_LOAN_STAGING = Base + "/loanstaging/delete";
        }

        public class CreditAppraisal
        {
            public const string GO_FOR_APPROVAL = Base + "/creditappraisal/update/loan/approval";
            public const string GET_AWAITING_APPROVAL_LIST = Base + "/creditappraisal/get/all";
            public const string GET_APPROVAL_COMMENTS = Base + "/creditappraisal/get/approval/comments";

            public const string UPDATE_CUSTOMER_TRANSACTION = Base + "/creditappraisal/update/customer/transaction";
            public const string CUSTOMER_TRANSACTION_SEARCH = Base + "/creditappraisal/search/customer/transaction";
            public const string CUSTOMER_TRANSACTION_SEARCH_EXPORT = Base + "/creditappraisal/export/customer/transaction";
            public const string GET_ALL_FLUTTERWAVE_TRANSACTIONS = Base + "/creditappraisal/get/all/flutterwave/transactions";
            public const string VERIFY_FLUTTERWAVE_TRANSACTIONS = Base + "/creditappraisal/verify/flutterwave/transactions";
        }

        public class LoanManagement
        {
            public const string GET_ALL_RUNNING_LOANS = Base + "/lms/get/all/running/loans";
            public const string GET_ALL_RUNNING_LOANS_CUSTOMER_ID = Base + "/lms/get/all/running/loans/customerId";
            public const string GET_ALL_APPLICATION = Base + "/lms/get/all/application";
            public const string GET_LOANS_REVIEW_APPLICATION_ID = Base + "/lms/get/review/loans";
            public const string GET_LOANS_REVIEW_APPLICATION_LOAN_ID = Base + "/lms/get/rewiew/loans/loanId";
            public const string GET_LOANS_REVIEW_APPLICATION = Base + "/lms/get/all/review/application";
            public const string ADD_LOAN_APPLICATION = Base + "/lms/add/application";
            public const string GET_ALL_LOAN_REVIEW_APPROVAL_LIST = Base + "/lms/get/all/loan/review/approval/list";
            public const string GO_FOR_LOAN_REVIEW_APPROVAL = Base + "/lms/add/loan/review/approval";
            public const string LOAN_REVIEW_APPROVAL_SCHEDULE = Base + "/lms/add/loan/review/approval/schedule";
            public const string LOAN_REVIEW_LOG = Base + "/lms/add/loan/review/log";
            public const string GET_LOAN_REVIEW_LOG = Base + "/lms/get/loan/review/log";
            public const string GET_LOAN_REVIEW_OFFER_LETTER = Base + "/lms/get/loan/review/offer/letter";
            public const string OFFER_LETTER_UPLOAD = Base + "/lms/loan/review/offer/letter/upload";
        }

        public class LoanOperation
        {
            public const string ADD_LOAN_OPERATION_APPROVAL = Base + "/loanOperations/add/loan/operation/approval";
            public const string GET_APPROVED_LOAN_REVIEW = Base + "/loanOperations/get/approved/loan/review";
            public const string GET_APPROVED_LOAN_OPERATION_REVIEW = Base + "/loanOperations/get/approved/loan/operation/review";
            public const string GET_RUNNING_lOAN_BY_REFNO = Base + "/loanOperations/get/running/loans/byrefNo";
            public const string GET_SCHEDULE_BY_LOAN_ID = Base + "/loanOperations/get/loanschedule/by/loanId";
            public const string GET_OPERATION_TYPE = Base + "/loanOperations/get/operationtype";
            public const string GET_OPERATION_TYPE_REMEDIAL = Base + "/loanOperations/get/remedial/operationtype";
            public const string GET_APPROVED_LOAN_REVIEW_REMEDIAL = Base + "/loanOperations/get/approved/loanreview/remedial"; 
            public const string GET_OPERATION_TYPE_RESTRUCTURE = Base + "/loanOperations/get/operationtype/resturcture";
            public const string RUN_END_OF_DAY = Base + "/loanOperations/run/end/of/day";
        }

        public class Loan
        {
            public const string ADD_LOAN_BOOKING = Base + "/loan/add/update/loan/booking";
            public const string GET_REVIEWED_LOANS = Base + "/loan/get/reviewed/loans";
            public const string GET_REVIEWED_LOANS_ID = Base + "/loan/get/reviewed/loans/loanId";
            public const string GET_ALL_LOANS = Base + "/loan/get/all/loans";
            public const string GET_LOAN_DETAILED_INFORMATION = Base + "/loan/get/loan/detailed/information/loanId";
            public const string GET_LOAN_MANAGED_INFORMATION = Base + "/loan/get/loan/manage/information/loanId";
            public const string LOAN_BOOKING_APPROVAL = Base + "/loan/update/loan/approval/";
            public const string GET_LOAN_BOOKING_APPROVAL_LIST = Base + "/loan/get/loan/approval/list";
            public const string UPDATE_LOAN_REPAYMENT_BY_FLUTTERWAVE = Base + "/loan/update/repayment";
            public const string UPDATE_LOAN_REPAYMENT_BY_FLUTTERWAVE_ZERO = Base + "/loan/update/repayment/zero";
            public const string GET_LOAN_PAST_DUE_INFORMATION = Base + "/loan/get/loan/past/due/list";
            public const string GET_LOAN_PAYMENT_DUE_INFORMATION = Base + "/loan/get/loan/payment/due/list";
            public const string GET_LOAN_SCHEDULE_INPUT= Base + "/loan/get/loan/schedule/input";

            public const string ADD_LOAN_COMMENT = Base + "/loan/add/update/loan/comment";
            public const string GET_ALL_LOAN_COMMENT = Base + "/loan/get/all/loan/comment";
            public const string GET_LOAN_COMMENT_ID = Base + "/loan/get/single/loan/comment";
            public const string DELETE_LOAN_COMMENT = Base + "/loan/delete/loan/comment";

            public const string ADD_LOAN_DECISION = Base + "/loan/add/update/loan/decision";
            public const string GET_ALL_LOAN_DECISION = Base + "/loan/get/all/loan/decision";
            public const string GET_LOAN_DECISION_ID = Base + "/loan/get/single/loan/decision";
            public const string DELETE_LOAN_DECISION = Base + "/loan/delete/loan/decision";

            public const string ADD_LOAN_CHEQUE = Base + "/loan/add/update/loan/cheque";
            public const string UPDATE_LOAN_CHEQUE_STATUS = Base + "/loan/update/loan/cheque/status";
            public const string DOWNLOAD_LOAN_CHEQUE = Base + "/loan/download/single/loan/cheque";
            public const string UPLOAD_LOAN_CHEQUE = Base + "/loan/upload/single/loan/cheque";
            public const string GET_ALL_LOAN_CHEQUE = Base + "/loan/get/all/loan/cheque";
        }

        public class LoanApplication
        {
            public const string ADD_LOAN_APPLICATION = Base + "/loanapplication/add/update";
            public const string ADD_LOAN_APPLICATION_CUSTOMER = Base + "/loanapplication/add/update/customer";
            public const string GET_ALL_LOAN_APPLICATION = Base + "/loanapplication/get/all";
            public const string GET_ALL_LOAN_APPLICATION_LIST = Base + "/loanapplication/get/all/list";
            public const string GET_ALL_LOAN_APPLICATION_OFFERLETTER = Base + "/loanapplication/get/all/offerletter";
            public const string GET_ALL_LOAN_APPLICATION_OFFERLETTER_REVIEW = Base + "/loanapplication/get/all/offerletter/review";
            public const string GET_ALL_LOAN_APPLICATION_WEBSITE_LIST = Base + "/loanapplication/get/all/website/list";
            public const string GET_ALL_LOAN_APPLICATION_WEBSITE_ID = Base + "/loanapplication/get/website/Id";
            public const string GET_ALL_LOAN_APPLICATION_PRODUCT_ID = Base + "/loanapplication/get/by/pid";
            public const string GET_ALL_LOAN_APPLICATION_REFERENCE = Base + "/loanapplication/get/by/ref/number";
            public const string GET_ALL_LOAN_APPLICATION_CUSTOMER = Base + "/loanapplication/get/by/customer";
            public const string GET_ALL_RUNNING_LOAN_APPLICATION_CUSTOMER = Base + "/loanapplication/get/runningloan/by/customer";
            public const string GET_LOAN_APPLICATION_BY_ID = Base + "/loanapplication/get/Id";
            public const string SUBMIT_FOR_APPRAISAL = Base + "/loanapplication/submit/appraisal";
            public const string LOAN_APPLICATION_OFFERLETTER_UPLOAD = Base + "/loanapplication/offerletter/upload";
            public const string ADD_LOAN_APPLICATION_DECISION = Base + "/loanapplication/add/offerletter/decision";
            public const string GET_LOAN_APPLICATION_DECISION = Base + "/loanapplication/get/offerletter/decision/status";
            public const string LOAN_APPLICATION_RECOMMENDATION = Base + "/loanapplication/add/recommendation";
            public const string GET_LOAN_APPLICATION_RECOMMENDATION = Base + "/loanapplication/get/recommendation";
            public const string DELETE_LOAN_APPLICATION = Base + "/loanapplication/delete";

            public const string GET_LOAN_APPLICATION_EXPOSURE = Base + "/loanapplication/get/all/exposure";
            public const string GET_LOAN_APPLICATION_EXPOSURE_ID = Base + "/loanapplication/get/exposure/Id";
            public const string ADD_LOAN_APPLICATION_EXPOSURE= Base + "/loanapplication/add/update/exposure";
        }

        public class CreditRiskScoreCard
        {
            public const string ADD_CREDITRISK_SCORECARD = Base + "/creditriskscorecard/add/update";
            public const string ADD_CREDITRISK_APPLICATION_SCORECARD = Base + "/creditriskscorecard/add/application/scorecard";
            public const string GET_CREDITRISK_APPLICATION_ATTRIBUTE= Base + "/creditriskscorecard/get/application/attribute";
            public const string GET_CREDITRISK_GROUP_ATTRIBUTE= Base + "/creditriskscorecard/get/group/attribute";
            public const string GET_ALL_CREDITRISK_SCORECARD = Base + "/creditriskscorecard/get/all";
            public const string GET_CREDITRISK_SCORECARD_BY_ID = Base + "/creditriskscorecard/get/Id";
            public const string DELETE_CREDITRISK_SCORECARD = Base + "/creditriskscorecard/delete";

            //Credit Risk Attribute
            public const string ADD_CREDITRISK_ATTRIBUTE = Base + "/creditriskscorecard/add/update/attribute";
            public const string GET_ALL_CREDITRISK_ATTRIBUTE = Base + "/creditriskscorecard/get/all/attribute";
            public const string GET_ALL_SYSTEM_ATTRIBUTE = Base + "/creditriskscorecard/get/system/attribute";
            public const string GET_ALL_MAPPED_RISKED_ATTRIBUTE = Base + "/creditriskscorecard/get/mapped/attribute";
            public const string GET_CREDITRISK_ATTRIBUTE_BY_ID = Base + "/creditriskscorecard/get/Id/attribute";
            public const string UPLOAD_CREDITRISK_ATTRIBUTE = Base + "/creditriskscorecard/upload/attribute";
            public const string DOWNLOAD_CREDITRISK_ATTRIBUTE = Base + "/creditriskscorecard/download/attribute";
            public const string DELETE_CREDITRISK_ATTRIBUTE = Base + "/creditriskscorecard/delete/attribute";

            //Credit Risk Category
            public const string ADD_CREDIT_CATEGORY = Base + "/creditriskscorecard/add/update/category";
            public const string GET_ALL_CREDIT_CATEGORY = Base + "/creditriskscorecard/get/all/category";
            public const string GET_CREDIT_CATEGORY_BY_ID = Base + "/creditriskscorecard/get/Id/category";
            public const string DELETE_CREDIT_CATEGORY = Base + "/creditriskscorecard/delete/category";

            //Credit Risk Rating
            public const string ADD_CREDITRISK_RATING = Base + "/creditriskscorecard/add/update/rating";
            public const string GET_ALL_CREDITRISK_RATING = Base + "/creditriskscorecard/get/all/rating";
            public const string GET_CREDITRISK_RATING_DETAIL = Base + "/creditriskscorecard/get/loanapplicationId/rating";
            public const string GET_CREDITRISK_RATING_BY_ID = Base + "/creditriskscorecard/get/Id/rating";
            public const string UPLOAD_CREDITRISK_RATING = Base + "/creditriskscorecard/upload/rating";
            public const string DOWNLOAD_CREDITRISK_RATING = Base + "/creditriskscorecard/download/rating";
            public const string DELETE_CREDITRISK_RATING = Base + "/creditriskscorecard/delete/rating";

            //Credit Weighted Risk Score
            public const string ADD_CREDIT_WEIGHTED_RISKSCORE = Base + "/creditriskscorecard/add/update/weightedscore";
            public const string GET_ALL_CREDIT_WEIGHTED_RISKSCORE = Base + "/creditriskscorecard/get/all/weightedscore";
            public const string GET_CREDIT_WEIGHTED_RISKSCORE_BY_ID = Base + "/creditriskscorecard/get/Id/weightedscore";
            public const string GET_CREDIT_WEIGHTED_RISKSCORE_BY_CUSTOMERTYPE = Base + "/creditriskscorecard/get/customertype/weightedscore";
            public const string DELETE_CREDIT_WEIGHTED_RISKSCORE = Base + "/creditriskscorecard/delete/weightedscore";

            //Credit Bureau Setup
            public const string ADD_CREDITBUREAU = Base + "/creditriskscorecard/add/update/creditbureau";
            public const string GET_ALL_CREDITBUREAU = Base + "/creditriskscorecard/get/all/creditbureau";
            public const string GET_CREDITBUREAU_BY_ID = Base + "/creditriskscorecard/get/Id/creditbureau";
            public const string UPLOAD_CREDITBUREAU = Base + "/creditriskscorecard/upload/creditbureau";
            public const string DOWNLOAD_CREDITBUREAU = Base + "/creditriskscorecard/download/creditbureau";
            public const string DELETE_CREDITBUREAU = Base + "/creditriskscorecard/delete/creditbureau";

            //Loan Credit Bureau
            public const string ADD_LOAN_CREDITBUREAU = Base + "/creditriskscorecard/add/update/loan/creditbureau";
            public const string GET_ALL_LOAN_CREDITBUREAU = Base + "/creditriskscorecard/get/all/loan/creditbureau";
            public const string GET_LOAN_CREDITBUREAU_BY_ID = Base + "/creditriskscorecard/get/Id/loan/creditbureau";
            public const string GET_APPLICATION_CREDITBUREAU = Base + "/creditriskscorecard/get/application/loan/creditbureau";
            public const string UPLOAD_LOAN_CREDITBUREAU = Base + "/creditriskscorecard/upload/loan/creditbureau";
            public const string DELETE_LOAN_CREDITBUREAU = Base + "/creditriskscorecard/delete/loan/creditbureau";

            //Credit Risk Rating PD
            public const string ADD_CREDITRISK_RATING_PD = Base + "/creditriskscorecard/add/update/rating/pd";
            public const string GET_ALL_CREDITRISK_RATING_PD = Base + "/creditriskscorecard/get/all/rating/pd";
            public const string GET_GROUPED_CREDITRISK_RATING_PD = Base + "/creditriskscorecard/get/grouped/rating/pd";
            public const string GET_CREDITRISK_RATING_PD_BY_ID = Base + "/creditriskscorecard/get/Id/rating/pd";
            public const string UPLOAD_CREDITRISK_RATING_PD = Base + "/creditriskscorecard/upload/rating/pd";
            public const string DOWNLOAD_CREDITRISK_RATING_PD = Base + "/creditriskscorecard/download/rating/pd";
            public const string DELETE_CREDITRISK_RATING_PD = Base + "/creditriskscorecard/delete/rating/pd";
        }

        public class LoanCustomer
        {
            public const string ADD_LOANCUSTOMER = Base + "/loancustomer/add/update";         
            public const string GET_ALL_LOANCUSTOMER = Base + "/loancustomer/get/all";
            public const string GET_LOANCUSTOMER_BY_ID = Base + "/loancustomer/get/loancustomerById";
            public const string GET_ALL_LOANCUSTOMER_LITE = Base + "/loancustomer/get/all/lite";            
            public const string UPLOAD_LOANCUSTOMER = Base + "/loancustomer/upload";
            public const string DOWNLOAD_LOANCUSTOMER = Base + "/loancustomer/download";
            public const string DELETE_LOANCUSTOMER = Base + "/loancustomer/delete";
            public const string ADD_LOANCUSTOMER_BY_CUSTOMER = Base + "/loancustomer/add/update/by/customer";
            public const string ADD_LOANCUSTOMER_BY_CUSTOMER_WEBSITE = Base + "/loancustomer/register/customer/website";
            public const string VERIFY_EMAIL = Base + "/loancustomer/verify/email";
            public const string START_LOAN_CUSTOMER = Base + "/loancustomer/start/loan/customer";
            public const string START_LOAN_CUSTOMER_ID = Base + "/loancustomer/start/loan/customer/Id";
            public const string GET_LOANCUSTOMER_CASA = Base + "/loancustomer/get/loancustomer/casa";
            public const string GET_LOANCUSTOMER_DASHBOARD = Base + "/loancustomer/get/loancustomer/dashboard";
            public const string GET_LOANCUSTOMER_TRANSACTION = Base + "/loancustomer/get/loancustomer/transaction";

            //Loan Customer Director
            public const string ADD_LOANCUSTOMER_DIRECTOR = Base + "/loancustomer/add/update/director";
            public const string GET_ALL_LOANCUSTOMER_DIRECTOR = Base + "/loancustomer/get/all/director";
            public const string GET_LOANCUSTOMER_DIRECTOR_BY_ID = Base + "/loancustomer/get/directorById";
            public const string GET_LOANCUSTOMER_DIRECTOR_BY_CUSTOMER = Base + "/loancustomer/get/director/customer";
            public const string GET_LOANCUSTOMER_DIRECTOR_SIGNATURE = Base + "/loancustomer/get/director/signature";
            public const string DELETE_LOANCUSTOMER_DIRECTOR = Base + "/loancustomer/delete/director";

            //Loan Customer Document
            public const string ADD_LOANCUSTOMER_DOCUMENT = Base + "/loancustomer/add/update/document";
            public const string GET_ALL_LOANCUSTOMER_DOCUMENT = Base + "/loancustomer/get/all/document";
            public const string GET_LOANCUSTOMER_DOCUMENT_BY_ID = Base + "/loancustomer/get/documentById";
            public const string GET_LOANCUSTOMER_DOCUMENT_BY_CUSTOMER = Base + "/loancustomer/get/document/customer";
            public const string UPLOAD_DOCUMENT = Base + "/loancustomer/upload/document";
            public const string DELETE_DOCUMENT = Base + "/loancustomer/delete/document";

            //Loan Customer Identity
            public const string ADD_LOANCUSTOMER_IDENTITY = Base + "/loancustomer/add/update/identity";
            public const string GET_ALL_LOANCUSTOMER_IDENTITY = Base + "/loancustomer/get/all/identity";
            public const string GET_LOANCUSTOMER_IDENTITY_BY_ID = Base + "/loancustomer/get/identityById";
            public const string GET_LOANCUSTOMER_IDENTITY_BY_CUSTOMER = Base + "/loancustomer/get/identity/customer";
            public const string DELETE_IDENTITY = Base + "/loancustomer/delete/identity";

            //Loan Customer NextOfKin
            public const string ADD_LOANCUSTOMER_NEXTOFKIN = Base + "/loancustomer/add/update/nextofkin";
            public const string GET_ALL_LOANCUSTOMER_NEXTOFKIN = Base + "/loancustomer/get/all/nextofkin";
            public const string GET_LOANCUSTOMER_NEXTOFKIN = Base + "/loancustomer/get/nextofkinById";
            public const string GET_LOANCUSTOMER_NEXTOFKIN_BY_CUSTOMER = Base + "/loancustomer/get/nextofkin/customer";
            public const string DELETE_NEXTOFKIN = Base + "/loancustomer/delete/nextofkin";

            //Loan Customer BankDetails
            public const string ADD_LOANCUSTOMER_BANKDETAILS = Base + "/loancustomer/add/update/bankdetails";
            public const string ADD_LOANCUSTOMER_CARDDETAILS = Base + "/loancustomer/add/update/carddetails";
            public const string GET_ALL_LOANCUSTOMER_BANKDETAILS = Base + "/loancustomer/get/all/bankdetails";
            public const string GET_LOANCUSTOMER_BANKDETAILS_BY_ID = Base + "/loancustomer/get/bankdetailsById";
            public const string GET_LOANCUSTOMER_BANKDETAILS_BY_CUSTOMER = Base + "/loancustomer/get/bankdetails/customer";
            public const string GET_LOANCUSTOMER_CARDDETAILS_BY_CUSTOMER = Base + "/loancustomer/get/carddetails/customer";
            public const string DELETE_BANKDETAILS = Base + "/loancustomer/delete/bankdetails";

            //Loan Customer DirectorShareHolder
            public const string ADD_LOANCUSTOMER_DIRECTORSHAREHOLDER = Base + "/loancustomer/add/update/directorshareholder";
            public const string GET_ALL_LOANCUSTOMER_DIRECTORSHAREHOLDER = Base + "/loancustomer/get/all/directorshareholder";
            public const string GET_LOANCUSTOMER_DIRECTORSHAREHOLDER = Base + "/loancustomer/get/directorshareholderById";
            public const string GET_LOANCUSTOMER_DIRECTORSHAREHOLDER_BY_CUSTOMER = Base + "/loancustomer/get/directorshareholder/customer";
            public const string DELETE_DIRECTORSHAREHOLDER = Base + "/loancustomer/delete/directorshareholder";
        }

        public class LoanCustomerFS
        {
            public const string ADD_FS_CAPTION_GROUP = Base + "/loancustomerfs/add/update/fscaption/group";
            public const string GET_ALL_FS_CAPTION_GROUP = Base + "/loancustomerfs/get/all/fscaption/group";
            public const string GET_FS_CAPTION_GROUP_BY_GROUP_ID = Base + "/loancustomerfs/get/fscaption/group/bygroupId";
            public const string GET_FS_CAPTION_GROUP_BY_ID = Base + "/loancustomerfs/get/single/fscaption/group";
            public const string FS_ALL_CAPTION_GROUP = Base + "/loancustomerfs/fscaption/all/caption/group";
            public const string DELETE_FS_CAPTION_GROUP = Base + "/loancustomerfs/delete/fscaption/group";

            public const string ADD_FS_CAPTION = Base + "/loancustomerfs/add/update/fscaption";
            public const string GET_ALL_FS_CAPTION = Base + "/loancustomerfs/get/all/fscaption";
            public const string GET_ALL_FS_CAPTION_BY_ID = Base + "/loancustomerfs/get/fscaptionbyId";
            public const string GET_ALL_FS_CAPTION_BY_GROUP_ID = Base + "/loancustomerfs/get/fscaption/bygroupId";
            public const string GET_FS_CAPTION_UNMAPPED = Base + "/loancustomerfs/get/fscaption/unmapped";
            public const string DELETE_FS_CAPTION = Base + "/loancustomerfs/delete/fscaption";

            public const string ADD_FS_CAPTION_DETAIL = Base + "/loancustomerfs/add/update/fscaption/detail";
            public const string GET_ALL_FS_CAPTION_DETAIL = Base + "/loancustomerfs/get/all/fscaption/detail";
            public const string GET_ALL_FS_CAPTION_DETAIL_BY_ID = Base + "/loancustomerfs/get/fscaption/detailbyId";
            public const string GET_ALL_FS_CAPTION_MAPPED = Base + "/loancustomerfs/get/fscaption/detail/mappeds";
            public const string GET_ALL_FS_CAPTION_DETAIL_MAPPED = Base + "/loancustomerfs/get/fscaption/detail/mapped";
            public const string DELETE_FS_CAPTION_DETAIL = Base + "/loancustomerfs/delete/fscaption/detail";

            public const string ADD_FS_RATIO_DETAIL = Base + "/loancustomerfs/add/update/fsratio/detail";
            public const string GET_ALL_FS_RATIO_DETAIL = Base + "/loancustomerfs/get/all/fsratio/detail";
            public const string GET_ALL_FS_RATIO_DETAIL_BY_ID = Base + "/loancustomerfs/get/single/fsratio";
            public const string GET_ALL_FS_RATIO_CAPTION = Base + "/loancustomerfs/get/fsratio/caption";
            public const string GET_FS_RATIO_DETAIL = Base + "/loancustomerfs/get/fsratio/detail";
            public const string GET_FS_RATIO_CALCULATION = Base + "/loancustomerfs/get/fsratio/calculations";
            public const string GET_FS_RATIO_VALUES = Base + "/loancustomerfs/get/fsratio/values";
            public const string DIVISOR_TYPES = Base + "/loancustomerfs/get/fsratio/divisor/types";
            public const string VALUE_TYPES = Base + "/loancustomerfs/get/fsratio/value/types";
            public const string DELETE_FS_RATIO_DETAIL = Base + "/loancustomerfs/delete/fsratio/detail";
        }
        public class CreditClassification
        {
            public const string ADD_CLASSIFICATION = Base + "/creditclassification/add/update";
            public const string GET_ALL_CLASSIFICATION = Base + "/creditclassification/get/all";
            public const string GET_CLASSIFICATION_BY_ID = Base + "/creditclassification/get/creditclassificationById";
            public const string UPLOAD_CLASSIFICATION = Base + "/creditclassification/upload";
            public const string DOWNLOAD_CLASSIFICATION = Base + "/creditclassification/download";
            public const string DELETE_CLASSIFICATION = Base + "/creditclassification/delete";

            public const string UPDATE_OPERATING_ACCOUNT = Base + "/creditclassification/add/update/operating/account";
            public const string GET_ALL_OPERATING_ACCOUNT = Base + "/creditclassification/get/all/operating/account";
        }

        public class LoanSchedule
        {
            public const string GET_ALL_FREQUENCYTYPE = Base + "/loanschedule/get/all/frequencytype";
            public const string GET_ALL_SCHEDULETYPE = Base + "/loanschedule/get/all/scheduletype";
            public const string GET_ALL_DAY_COUNT = Base + "/loanschedule/get/all/daycount";
            public const string GET_ALL_TEMP_SCHEDULE = Base + "/loanschedule/get/all/temp/schedule";
            public const string DOWNLOAD_PERIODIC_SCHEDULE = Base + "/loanschedule/export/periodic/schedule";
            public const string GET_DELETED_LOAN_SCHEDULE = Base + "/loanschedule/deleted/loan/schedule";
            public const string GET_ALL_PERIODIC_SCHEDULE = Base + "/loanschedule/get/periodic/schedule";
            public const string GET_ALL_SCHEDULE_BY_ID = Base + "/loanschedule/get/schedulebyId";
            public const string GET_ALL_SCHEDULE_BY_LOANID = Base + "/loanschedule/get/schedulebyLoanId";
        }

        public class CreditProducts
        {
            public const string ADD_PRODUCT = Base + "/product/add/update";
            public const string GET_ALL_PRODUCT = Base + "/product/get/all";
            public const string GET_PRODUCT_BY_ID = Base + "/product/get/productById";
            public const string UPLOAD_PRODUCT = Base + "/product/upload";
            public const string DOWNLOAD_PRODUCT = Base + "/product/download";
            public const string DELETE_PRODUCT = Base + "/product/delete";

            public const string ADD_PRODUCT_TYPE = Base + "/product/add/update/producttype";
            public const string GET_PRODUCT_TYPE = Base + "/product/get/all/producttype";
            public const string GET_PRODUCT_TYPE_ID = Base + "/product/get/producttypeById";
            public const string UPLOAD_PRODUCT_TYPE = Base + "/product/upload/producttype";
            public const string DOWNLOAD_PRODUCT_TYPE = Base + "/product/download/producttype";
            public const string DELETE_PRODUCT_TYPE = Base + "/product/delete/producttype";

            public const string ADD_PRODUCT_FEE = Base + "/product/add/update/productfee";
            public const string GET_ALL_PRODUCT_FEE = Base + "/product/get/all/productfee";
            public const string GET_PRODUCT_FEE_BY_ID = Base + "/product/get/productFeeById";
            public const string GET_PRODUCT_FEE_BY_PRODUCT_ID = Base + "/product/get/productFeeByProductId";
            public const string GET_PRODUCT_FEE_BY_LOANAPPLICATION_ID = Base + "/product/get/productFeeByLoanApplicationId";
            public const string UPLOAD_PRODUCT_FEE = Base + "/product/upload/productfee";
            public const string DOWNLOAD_PRODUCT_FEE = Base + "/product/download/productfee";
            public const string DELETE_PRODUCT_FEE = Base + "/product/delete/productfee";
        }

        public class Collateral
        {
            public const string ADD_COLLATERAL_TYPE = Base + "/collateral/add/update/collateraltype";
            public const string GET_ALL_COLLATERAL_TYPE = Base + "/collateral/get/all/collateraltype";
            public const string GET_COLLATERAL_TYPE_BY_ID = Base + "/collateral/get/Id/collateraltype";
            public const string GET_COLLATERAL_TYPE_BY__LOANAPPLICATION_ID = Base + "/collateral/get/allowable/collateraltype";
            public const string UPLOAD_COLLATERAL_TYPE = Base + "/collateral/upload/collateraltype";
            public const string DOWNLOAD_COLLATERAL_TYPE = Base + "/collateral/download/collateraltype";
            public const string DELETE_COLLATERAL_TYPE = Base + "/collateral/delete/collateraltype";

            public const string ADD_COLLATERAL_CUSTOMER = Base + "/collateral/add/collateralcustomer";
            public const string UPDATE_COLLATERAL_CUSTOMER = Base + "/collateral/add/update/collateralcustomerdocument";
            public const string GET_COLLATERAL_CUSTOMER = Base + "/collateral/get/all/collateralcustomer";
            public const string GET_COLLATERAL_CUSTOMER_ID = Base + "/collateral/get/collateralcustomerById";
            public const string UPLOAD_COLLATERAL_CUSTOMER = Base + "/collateral/upload/collateralcustomer";
            public const string DOWNLOAD_COLLATERAL_CUSTOMER = Base + "/collateral/download/collateralcustomer";
            public const string DELETE_COLLATERAL_CUSTOMER = Base + "/collateral/delete/collateralcustomer";
            public const string CUSTOMER_LOAN_COLLATERAL_CONSUMPTION = Base + "/collateral/customer/loan/collateral/consumption";
            public const string REQUIRED_COLLATERAL_VALUE = Base + "/collateral/require/collateral/value";
            public const string CURRENT_CUSTOMER_COLLATERAL_VALUE = Base + "/collateral/current/customer/collateral/value";
            public const string COLLATERAL_SINGLE_CUSTOMER = Base + "/collateral/collateral/single/customer";
            public const string GET_COLLATERAL_CUSTOMER_BY_IDS = Base + "/collateral/get/customer/collateral";

            public const string ADD_LOAN_APPLICATION_COLLATERAL = Base + "/collateral/add/update/loanapplicationccollateral";
            public const string GET_ALL_COLLATERAL_MANAGEMENT = Base + "/collateral/get/collateral/management";
            public const string GET_ALL_LOAN_APPLICATION_COLLATERAL = Base + "/collateral/get/all/loanapplicationccollateral";
            public const string GET_LOAN_APPLICATION_COLLATERAL_BY_LOANAPPLICATION_ID = Base + "/collateral/get/loanapplicationccollateral/loanapplicationId";
            public const string DELETE_LOAN_APPLICATION_COLLATERAL = Base + "/collateral/delete/loanapplicationccollateral";
            public const string DELETE_LOAN_CUSTOMER_COLLATERAL = Base + "/collateral/delete/loancustomercollateral";
        }
        #endregion

        #region Deposit
        public class AccountType
        {
            public const string ADD_UPDATE_ACCOUNT_TYPE = Base + "/acounttype/add/update/accountType";
            public const string GET_ALL_ACCOUNT_TYPE = Base + "/acounttype/get/all/accountType";
            public const string GET_ACCOUNT_TYPE_BY_ID = Base + "/acounttype/get/accountTypeById";
            public const string DELETE_ACCOUNT_TYPE = Base + "/accountType/delete/accountType";
            public const string DOWNLOAD_ACCOUNT_TYPE = Base + "/accountType/download/accountType";
            public const string UPLOAD_ACCOUNT_TYPE = Base + "/accountType/upload/accountType";
        }

        public class AccountSetup
        {
            public const string ADD_UPDATE_ACCOUNTSETUP = Base + "/accountsetup/add/update/accountsetup";
            public const string GET_ALL_ACCOUNTSETUP = Base + "/accountsetup/get/all/accountsetup";
            public const string GET_ACCOUNTSETUP_BY_ID = Base + "/accountsetup/get/accountsetupbyid";
            public const string DELETE_ACCOUNTSETUP = Base + "/accountsetup/delete/accountsetup";
            public const string DOWNLOAD_ACCOUNTSETUP = Base + "/accountsetup/download/accountsetup";
            public const string UPLOAD_ACCOUNTSETUP = Base + "/accountsetup/upload/accountsetup";

            public const string ADD_UPDATE_DEPOSITFORM = Base + "/accountsetup/add/update/depositform";
            public const string GET_ALL_DEPOSITFORM = Base + "/accountsetup/get/all/depositform";
            public const string GET_DEPOSITFORM_BY_ID = Base + "/accountsetup/get/depositformbyid";
            public const string DELETE_DEPOSITFORM = Base + "/accountsetup/delete/depositform";
        }

        public class BankClosure
        {
            public const string ADD_UPDATE_BANK_CLOSURE = Base + "/bankclosure/add/update/bankClosure";
            public const string GET_ALL_BANK_CLOSURE = Base + "/bankclosure/get/all/bankClosures";
            public const string ADD_UPDATE_BANK_CLOSURE_SETUP = Base + "/bankclosure/add/update/bankClosureSetup";
            public const string DELETE_BANK_CLOSURE = Base + "/bankclosure/delete/bankClosure";
            public const string DELETE_BANK_CLOSURE_SETUP = Base + "/bankclosure/delete/bankClosureSetup";
        }

        public class BusinessCategory
        {
            public const string ADD_UPDATE_BUSINESSCATEGORY = Base + "/businesscategory/add/update";
            public const string GET_ALL_BUSINESSCATEGORY = Base + "/businesscategory/get/all";
            public const string GET_BUSINESSCATEGORY_BY_ID = Base + "/businesscategory/get/businesscategoryId";
            public const string DELETE_BUSINESSCATEGORY = Base + "/businesscategory/delete";
            public const string DOWNLOAD_BUSINESSCATEGORY = Base + "/businesscategory/download";
            public const string UPLOAD_BUSINESSCATEGORY = Base + "/businesscategory/upload";
        }

        public class CashierTeller
        {
            public const string ADD_UPDATE_CASHIERTELLERSETUP = Base + "/cashierteller/add/update/cashiertellersetup";
            public const string GET_ALL_CASHIERTELLERSETUP = Base + "/cashierteller/get/all/cashiertellersetup";
            public const string GET_CASHIERTELLERSETUP_BY_ID = Base + "/cashierteller/get/cashiertellersetupid";
            public const string DELETE_CASHIERTELLERSETUP = Base + "/cashierteller/delete/cashiertellersetup";
            public const string DOWNLOAD_CASHIERTELLERSETUP = Base + "/cashierteller/download/cashiertellersetup";
            public const string UPLOAD_CASHIERTELLERSETUP = Base + "/cashierteller/upload/cashiertellersetup";

            public const string ADD_UPDATE_CASHIERTELLERFORM = Base + "/cashierteller/add/update/cashiertellerform";
            public const string GET_ALL_CASHIERTELLERFORM = Base + "/cashierteller/get/all/cashiertellerform";
            public const string GET_CASHIERTELLERFORM_BY_ID = Base + "/cashierteller/get/cashiertellerformid";
            public const string DELETE_CASHIERTELLERFORM = Base + "/cashierteller/delete/cashiertellerform";
        }

        public class ChangeOfRates
        {
            public const string ADD_UPDATE_CHANGEOFRATES_SETUP = Base + "/changeofrates/add/update/changeofratessetup";
            public const string GET_ALL_CHANGEOFRATES_SETUP = Base + "/changeofrates/get/all/changeofratessetup";
            public const string GET_CHANGEOFRATES_SETUP_BY_ID = Base + "/changeofrates/get/changeofratessetupid";
            public const string DELETE_CHANGEOFRATES_SETUP = Base + "/changeofrates/delete/changeofratessetup";
            public const string DOWNLOAD_CHANGEOFRATES_SETUP = Base + "/changeofrates/download/changeofratessetup";
            public const string UPLOAD_CHANGEOFRATES_SETUP = Base + "/changeofrates/upload/changeofratessetup";

            public const string ADD_UPDATE_CHANGEOFRATES = Base + "/changeofrates/add/update/changeofrates";
            public const string GET_ALL_CHANGEOFRATES = Base + "/changeofrates/get/all/changeofrates";
            public const string GET_CHANGEOFRATES_BY_ID = Base + "/changeofrates/get/changeofratesid";
            public const string DELETE_CHANGEOFRATES = Base + "/changeofrates/delete/changeofrates";
        }

        public class ContactPersons
        {
            public const string ADD_UPDATE_CONTACTPERSONS = Base + "/accountopening/add/update/contactpersons";
            public const string GET_ALL_CONTACTPERSONS = Base + "/accountopening/get/all/contactpersons";
            public const string GET_CONTACTPERSONS_BY_ID = Base + "/accountopening/get/contactpersons";
            public const string DELETE_CONTACTPERSONS = Base + "/accountopening/delete/contactpersons";
        }

        public class Customer
        {
            public const string ADD_UPDATE_CUSTOMER = Base + "/accountopening/add/update/customer";
            public const string GET_ALL_CUSTOMER = Base + "/accountopening/get/all/customerlite";
            public const string GET_ALL_CUSTOMER_CASA = Base + "/accountopening/get/casa/list";
            public const string GET_CUSTOMER_BY_ID = Base + "/accountopening/get/customerdetailsbyid";
            public const string DELETE_CUSTOMER = Base + "/accountopening/delete/customer";
            public const string ADD_DEPOSITCUSTOMER = Base + "/accountopening/add/deposit/customer";
            public const string UPDATE_CASA = Base + "/accountopening/update/casa";
        }

        public class DepositCategory
        {
            public const string ADD_UPDATE_DEPOSITCATEGORY = Base + "/depositcategory/add/update/depositcategory";
            public const string GET_ALL_DEPOSITCATEGORY = Base + "/depositcategory/get/all/depositcategory";
            public const string GET_DEPOSITCATEGORY_BY_ID = Base + "/depositcategory/get/depositcategoryid";
            public const string DELETE_DEPOSITCATEGORY = Base + "/depositcategory/delete/depositcategory";
            public const string DOWNLOAD_DEPOSITCATEGORY = Base + "/depositcategory/download/depositcategory";
            public const string UPLOAD_DEPOSITCATEGORY = Base + "/depositcategory/upload/depositcategory";
        }

        public class Directors
        {
            public const string ADD_UPDATE_DIRECTORS = Base + "/accountopening/add/update/directors";
            public const string UPLOAD_DIRECTORS = Base + "/accountopening/upload/directorssignature";
            public const string GET_ALL_DIRECTORS = Base + "/accountopening/get/all/directors";
            public const string GET_DIRECTORS_BY_ID = Base + "/accountopening/get/directors";
            public const string DELETE_DIRECTORS = Base + "/accountopening/delete/directors";
        }

        public class DocumentUpload
        {
            public const string ADD_UPDATE_DOCUMENTUPLOAD = Base + "/accountopening/add/update/documentupload";
            public const string GET_ALL_DOCUMENTUPLOAD = Base + "/accountopening/get/all/documentupload";
            public const string GET_DOCUMENTUPLOAD_BY_ID = Base + "/accountopening/get/documentupload";
            public const string DELETE_DOCUMENTUPLOAD = Base + "/accountopening/delete/documentupload";
        }

        public class IdentityDetails
        {
            public const string ADD_UPDATE_IDENTITYDETAILS = Base + "/accountopening/add/update/identitydetails";
            public const string GET_ALL_IDENTITYDETAILS = Base + "/accountopening/get/all/identitydetails";
            public const string GET_IDENTITYDETAILS_BY_ID = Base + "/accountopening/get/identitydetails";
            public const string DELETE_IDENTITYDETAILS = Base + "/accountopening/delete/identitydetails";
        }

        public class KYCustomer
        {
            public const string ADD_UPDATE_KYCUSTOMER = Base + "/accountopening/add/update/kycustomer";
            public const string GET_ALL_KYCUSTOMER = Base + "/accountopening/get/all/kycustomer";
            public const string GET_KYCUSTOMER_BY_ID = Base + "/accountopening/get/kycustomer";
            public const string DELETE_KYCUSTOMER = Base + "/accountopening/delete/kycustomer";
        }

        public class NextOfKin
        {
            public const string ADD_UPDATE_NEXTOFKIN = Base + "/accountopening/add/update/nextofkin";
            public const string GET_ALL_NEXTOFKIN = Base + "/accountopening/get/all/nextofkin";
            public const string GET_NEXTOFKIN_BY_ID = Base + "/accountopening/get/nextofkin";
            public const string DELETE_NEXTOFKIN = Base + "/accountopening/delete/nextofkin";
        }

        public class SignatureUpload
        {
            public const string ADD_UPDATE_SIGNATUREUPLOAD = Base + "/accountopening/add/update/signatureupload";
            public const string GET_ALL_SIGNATUREUPLOAD = Base + "/accountopening/get/all/signatureupload";
            public const string GET_SIGNATUREUPLOAD_BY_ID = Base + "/accountopening/get/signatureuploadid";
            public const string GET_SIGNATUREUPLOAD_BY_IDS = Base + "/accountopening/get/signatureuploadbyids";
            public const string DELETE_SIGNATUREUPLOAD = Base + "/accountopening/delete/signatureupload";
        }

        public class Signatory
        {
            public const string ADD_UPDATE_SIGNATORY = Base + "/accountopening/add/update/signatory";
            public const string UPLOAD_SIGNATORY = Base + "/accountopening/upload/signatory";
            public const string GET_ALL_SIGNATORY = Base + "/accountopening/get/all/signatory";
            public const string GET_SIGNATORY_BY_ID = Base + "/accountopening/get/signatory";
            public const string DELETE_SIGNATORY = Base + "/accountopening/delete/signatory";
        }


        public class TillVault
        {
            public const string ADD_UPDATE_TILL_VAULT = Base + "/tillvault/add/update/tillVault";
            public const string ADD_UPDATE_TILL_VAULT_SETUP = Base + "/tillvault/add/update/tillVaultSetup";
            public const string DELETE_TILL_VAULT = Base + "/tillvault/delete/tillVault";
            public const string DELETE_TILL_VAULT_SETUP = Base + "/tillvault/delete/tillVaultSetup";
        }

        public class TransactionCharge
        {
            public const string ADD_UPDATE_TRANSACTIONCHARGE = Base + "/transactioncharge/add/update/transactioncharge";
            public const string GET_ALL_TRANSACTIONCHARGE = Base + "/transactioncharge/get/all/transactioncharge";
            public const string GET_TRANSACTIONCHARGE_BY_ID = Base + "/transactioncharge/get/transactionchargebyid";
            public const string DELETE_TRANSACTIONCHARGE = Base + "/transactioncharge/delete/transactioncharge";
            public const string DOWNLOAD_TRANSACTIONCHARGE = Base + "/transactioncharge/download/transactioncharge";
            public const string UPLOAD_TRANSACTIONCHARGE = Base + "/transactioncharge/upload/transactioncharge";
        }

        public class TransactionTax
        {
            public const string ADD_UPDATE_TRANSACTIONTAX = Base + "/transactiontax/add/update/transactiontax";
            public const string GET_ALL_TRANSACTIONTAX = Base + "/transactiontax/get/all/transactiontax";
            public const string GET_TRANSACTIONTAX_BY_ID = Base + "/transactiontax/get/transactiontaxbyid";
            public const string DELETE_TRANSACTIONTAX = Base + "/transactiontax/delete/transactiontax";
            public const string DOWNLOAD_TRANSACTIONTAX = Base + "/transactiontax/download/transactiontax";
            public const string UPLOAD_TRANSACTIONTAX = Base + "/transactiontax/upload/transactiontax";
        }

        public class Transfer
        {
            public const string ADD_UPDATE_TRANSFER_SETUP = Base + "/transfer/add/update/transfersetup";
            public const string GET_ALL_TRANSFER_SETUP = Base + "/transfer/get/all/transfersetup";
            public const string GET_TRANSFER_SETUP_BY_ID = Base + "/transfer/get/transfersetupid";
            public const string DELETE_TRANSFER_SETUP = Base + "/transfer/delete/transfersetup";
            public const string DOWNLOAD_TRANSFER_SETUP = Base + "/transfer/download/transfersetup";
            public const string UPLOAD_TRANSFER_SETUP = Base + "/transfer/upload/transfersetup";

            public const string ADD_UPDATE_TRANSFER = Base + "/transfer/add/update/transfer";
            public const string GET_ALL_TRANSFER = Base + "/transfer/get/all/transfer";
            public const string GET_TRANSFER_BY_ID = Base + "/transfer/get/transferid";
            public const string DELETE_TRANSFER = Base + "/transfer/delete/transfer";
        }

        public class Withdrawal
        {
            public const string ADD_UPDATE_WITHDRAWAL_SETUP = Base + "/withdrawal/add/update/withdrawalsetup";
            public const string GET_ALL_WITHDRAWAL_SETUP = Base + "/withdrawal/get/all/withdrawalsetup";
            public const string GET_WITHDRAWAL_SETUP_BY_ID = Base + "/withdrawal/get/withdrawalsetupid";
            public const string DELETE_WITHDRAWAL_SETUP = Base + "/withdrawal/delete/withdrawalsetup";
            public const string DOWNLOAD_WITHDRAWAL_SETUP = Base + "/withdrawal/download/withdrawalsetup";
            public const string UPLOAD_WITHDRAWAL_SETUP = Base + "/withdrawal/upload/withdrawalsetup";

            public const string ADD_UPDATE_WITHDRAWAL = Base + "/withdrawal/add/update/withdrawal";
            public const string GET_ALL_WITHDRAWAL = Base + "/withdrawal/get/all/withdrawal";
            public const string GET_WITHDRAWAL_BY_ID = Base + "/withdrawal/get/withdrawalid";
            public const string DELETE_WITHDRAWAL = Base + "/withdrawal/delete/withdrawal";
        }

        #endregion

        #region InvestorsFund

        public static class Product
        {
            public const string ADD_UPDATE_PRODUCT = Base + "/investorfundproduct/add/update/product";
            public const string GET_ALL_PRODUCT = Base + "/investorfundproduct/get/all/product";
            public const string GET_PRODUCT_BY_ID = Base + "/investorfundproduct/get/productbyid";
            public const string DELETE_PRODUCT = Base + "/investorfundproduct/delete/product";
            public const string DOWNLOAD_PRODUCT = Base + "/investorfundproduct/download/product";
            public const string UPLOAD_PRODUCT = Base + "/investorfundproduct/upload/product";
        }

        public static class ProductType
        {
            public const string ADD_UPDATE_PRODUCTTYPE = Base + "/investorfundproduct/add/update/producttype";
            public const string GET_ALL_PRODUCTTYPE = Base + "/investorfundproduct/get/all/producttype";
            public const string GET_PRODUCTTYPE_BY_ID = Base + "/investorfundproduct/get/producttypebyid";
            public const string DELETE_PRODUCTTYPE = Base + "/investorfundproduct/delete/producttype";
            public const string DOWNLOAD_PRODUCTTYPE = Base + "/investorfundproduct/download/producttype";
            public const string UPLOAD_PRODUCTTYPE = Base + "/investorfundproduct/upload/producttype";
        }

        public static class InvestorFund
        {
            public const string ADD_UPDATE_INVESTORFUND = Base + "/investorfund/add/update/investorfund";
            public const string ADD_UPDATE_INVESTORFUND_ROLLOVER = Base + "/investorfund/update/investorfund/rollover";
            public const string ADD_UPDATE_INVESTORFUND_TOPUP = Base + "/investorfund/update/investorfund/topup";
            public const string ADD_GO_FOR_APPROVAL = Base + "/investorfund/update/go/for/approval";
            public const string GET_ALL_INVESTORFUND = Base + "/investorfund/get/all/investorfund";
            public const string GET_ALL_INVESTORFUND_APPROVAL_LIST = Base + "/investorfund/get/approval/list";
            public const string GET_INVESTORFUND_BY_ID = Base + "/investorfund/get/investorfundbyid";
            public const string DELETE_INVESTORFUND = Base + "/investorfund/delete/investorfund";
            public const string DOWNLOAD_INVESTORFUND = Base + "/investorfund/download/investorfund";
            public const string UPLOAD_INVESTORFUND = Base + "/investorfund/upload/investorfund";
            public const string ADD_INVESTORFUND_CUSTOMER = Base + "/investorfund/add/update/customer";

            public const string ADD_INVESTORFUND_InvestmentRecommendation = Base + "/investorfund/add/update/investmentrecommendation";
            public const string GET_ALL_INVESTORRUNNING_FACILITIES = Base + "/investorfund/get/all/investorrunningfacilities";
            public const string GET_ALL_INVESTMENT_BY_CUSTOMER_ID = Base + "/investorfund/get/investmentbycustomerid";
            public const string GET_CURRENT_BAL_BY_INVESTFUND_ID = Base + "/investorfund/get/current/balance";
            public const string GET_ALL_INVESTMENT_LIST = Base + "/investorfund/get/all/investmentlist";
            public const string GET_ALL_INVESTOR_LIST = Base + "/investorfund/get/all/investorlist";
            public const string GET_ALL_RUNNING_INVESTMENT_BY_CUSTOMER = Base + "/investorfund/get/runninginvestmentbycustomer";
            public const string GET_ALL_INVESTMENT_CERTIFICATES = Base + "/investorfund/get/all/investmentcertificates";
            public const string GET_ALL_RUNNING_INVESTMENT = Base + "/investorfund/get/all/running/investment";
            public const string GET_ALL_RUNNING_INVESTMENT_BY_STATUS = Base + "/investorfund/get/all/running/investmentbystatus";
            public const string GET_INVESTORFUND_RECOMMENDATION = Base + "/investorfund/get/recommendation";
            public const string GET_ALL_INVESTORFUND_WEBSITE_LIST = Base + "/investorfund/get/all/website/list";
            public const string GET_ALL_INVESTORFUND_WEBSITE_ID = Base + "/investorfund/get/website/Id";
            public const string GET_ALL_INVESTORFUND_WEBSITE_ROLLOVER_LIST = Base + "/investorfund/get/all/website/rollover/list";
            public const string GET_ALL_INVESTORFUND_WEBSITE_ROLLOVER_ID = Base + "/investorfund/get/website/rollover/Id";
            public const string GET_ALL_INVESTORFUND_WEBSITE_TOPUP_LIST = Base + "/investorfund/get/all/website/topup/list";
            public const string GET_ALL_INVESTORFUND_WEBSITE_TOPUP_ID = Base + "/investorfund/get/website/topup/Id";
            public const string ADD_INVESTORFUND_TOPUP_CUSTOMER = Base + "/investorfund/add/update/topup/customer";
            public const string ADD_INVESTORFUND_ROLLOVER_CUSTOMER = Base + "/investorfund/add/update/rollover/customer";
        }

        public static class Collection
        {
            public const string ADD_UPDATE_COLLECTION = Base + "/investorfund/add/update/collection";
            public const string GET_ALL_COLLECTION = Base + "/investorfund/get/all/collection";
            public const string GET_COLLECTION_BY_ID = Base + "/investorfund/get/collectionbyid";
            public const string DELETE_COLLECTION = Base + "/investorfund/delete/collection";
            public const string DOWNLOAD_COLLECTION = Base + "/investorfund/download/collection";
            public const string UPLOAD_COLLECTION = Base + "/investorfund/upload/collection";

            public const string ADD_COLLECTION_RECOMMENDATION = Base + "/investorfund/add/update/collectionrecommendation";
            public const string ADD_COLLECTION_CUSTOMER = Base + "/investorfund/add/update/collectionbycustomer";
            public const string GET_COLLECTION_RECOMMENDATION = Base + "/investorfund/get/collectionrecommendation";
            public const string GET_ALL_COLLECTION_WEBSITE_LIST = Base + "/investorfund/get/all/collectionwebsite/list";
            public const string GET_ALL_COLLECTION_WEBSITE_ID = Base + "/investorfund/get/collectionwebsite/Id";
            public const string ADD_GO_FOR_COLLECTION_APPROVAL = Base + "/investorfund/update/go/for/collection/approval";
            public const string GET_ALL_COLLECTION_APPROVAL_LIST = Base + "/investorfund/get/collection/approval/list";
        }

        public static class Liquidation
        {
            public const string ADD_UPDATE_LIQUIDATION = Base + "/investorfund/add/update/liquidation";
            public const string GET_ALL_LIQUIDATION = Base + "/investorfund/get/all/liquidation";
            public const string GET_LIQUIDATION_BY_ID = Base + "/investorfund/get/liquidationbyid";
            public const string DELETE_LIQUIDATION = Base + "/investorfund/delete/liquidation";
            public const string DOWNLOAD_LIQUIDATION = Base + "/investorfund/download/liquidation";
            public const string UPLOAD_LIQUIDATION = Base + "/investorfund/upload/liquidation";

            public const string ADD_LIQUIDATION_RECOMMENDATION = Base + "/investorfund/add/update/liquidationrecommendation";
            public const string ADD_LIQUIDATION_CUSTOMER = Base + "/investorfund/add/update/liquidationcustomer";
            public const string GET_LIQUIDATION_RECOMMENDATION = Base + "/investorfund/get/liquidationrecommendation";
            public const string GET_ALL_LIQUIDATION_WEBSITE_LIST = Base + "/investorfund/get/all/liquidationwebsite/list";
            public const string GET_ALL_LIQUIDATION_WEBSITE_ID = Base + "/investorfund/get/liquidationwebsite/Id";
            public const string ADD_GO_FOR_LIQUIDATION_APPROVAL = Base + "/investorfund/update/go/for/liquidation/approval";
            public const string GET_ALL_LIQUIDATION_APPROVAL_LIST = Base + "/investorfund/get/liquidation/approval/list";
        }
        #endregion

    }
}
