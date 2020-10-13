using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Banking.Migrations
{
    public partial class INITIAL_MIGRTION : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(maxLength: 550, nullable: false),
                    CustomerTypeId = table.Column<int>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    SecurityAnswered = table.Column<string>(maxLength: 256, nullable: true),
                    IsItQuestionTime = table.Column<bool>(nullable: false),
                    EnableAtThisTime = table.Column<DateTime>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfirmEmailCode",
                columns: table => new
                {
                    ConfirmEmailCodeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    ConfirnamationTokenCode = table.Column<string>(nullable: true),
                    IssuedDate = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmEmailCode", x => x.ConfirmEmailCodeId);
                });

            migrationBuilder.CreateTable(
                name: "cor_allowable_collateraltype",
                columns: table => new
                {
                    AllowableCollateralId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    CollateralTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_allowable_collateraltype", x => x.AllowableCollateralId);
                });

            migrationBuilder.CreateTable(
                name: "cor_approvaldetail",
                columns: table => new
                {
                    ApprovalDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusId = table.Column<int>(nullable: false),
                    StaffId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    ArrivalDate = table.Column<DateTime>(nullable: true),
                    TargetId = table.Column<int>(nullable: false),
                    WorkflowToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_approvaldetail", x => x.ApprovalDetailId);
                });

            migrationBuilder.CreateTable(
                name: "credit_allowable_collateraltype",
                columns: table => new
                {
                    AllowableCollateralId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    CollateralTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_allowable_collateraltype", x => x.AllowableCollateralId);
                });

            migrationBuilder.CreateTable(
                name: "credit_casa",
                columns: table => new
                {
                    CasaAccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(maxLength: 50, nullable: false),
                    AccountName = table.Column<string>(maxLength: 100, nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    BranchId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    IsCurrentAccount = table.Column<bool>(nullable: false),
                    Tenor = table.Column<int>(nullable: true),
                    InterestRate = table.Column<decimal>(nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    TerminalDate = table.Column<DateTime>(nullable: true),
                    ActionBy = table.Column<int>(nullable: true),
                    ActionDate = table.Column<DateTime>(nullable: true),
                    AccountStatusId = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: true),
                    AvailableBalance = table.Column<decimal>(nullable: false),
                    LedgerBalance = table.Column<decimal>(nullable: false),
                    RelationshipOfficerId = table.Column<int>(nullable: true),
                    RelationshipManagerId = table.Column<int>(nullable: true),
                    MISCode = table.Column<string>(maxLength: 50, nullable: true),
                    TEAMMISCode = table.Column<string>(maxLength: 50, nullable: true),
                    OverdraftAmount = table.Column<decimal>(nullable: true),
                    OverdraftInterestRate = table.Column<decimal>(nullable: true),
                    OverdraftExpiryDate = table.Column<DateTime>(nullable: true),
                    HasOverdraft = table.Column<bool>(nullable: true),
                    LienAmount = table.Column<decimal>(nullable: false),
                    HasLien = table.Column<bool>(nullable: false),
                    PostNoStatusId = table.Column<int>(nullable: true),
                    OldAccountNumber1 = table.Column<string>(maxLength: 50, nullable: true),
                    OldAccountNumber2 = table.Column<string>(maxLength: 50, nullable: true),
                    OldAccountNumber3 = table.Column<string>(maxLength: 50, nullable: true),
                    AprovalStatusId = table.Column<int>(nullable: true),
                    CustomerSensitivityLevelId = table.Column<int>(nullable: true),
                    FromDeposit = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_casa", x => x.CasaAccountId);
                });

            migrationBuilder.CreateTable(
                name: "credit_casa_lien",
                columns: table => new
                {
                    LienId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceReferenceNumber = table.Column<string>(maxLength: 50, nullable: false),
                    ProductAccountNumber = table.Column<string>(maxLength: 50, nullable: false),
                    LienReferenceNumber = table.Column<string>(maxLength: 50, nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    LienCreditAmount = table.Column<decimal>(nullable: false),
                    LienDebitAmount = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    LienTypeId = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_casa_lien", x => x.LienId);
                });

            migrationBuilder.CreateTable(
                name: "credit_collateralcustomerconsumption",
                columns: table => new
                {
                    CollateralCustomerConsumptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollateralCustomerId = table.Column<int>(nullable: false),
                    LoanApplicationId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: true),
                    CollateralCurrentAmount = table.Column<decimal>(nullable: false),
                    ActualCollateralValue = table.Column<decimal>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    CollateralType = table.Column<string>(maxLength: 50, nullable: true),
                    ExpectedCollateralValue = table.Column<int>(nullable: false),
                    CollateralCode = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_collateralcustomerconsumption", x => x.CollateralCustomerConsumptionId);
                });

            migrationBuilder.CreateTable(
                name: "credit_collateraltype",
                columns: table => new
                {
                    CollateralTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Details = table.Column<string>(nullable: false),
                    RequireInsurancePolicy = table.Column<bool>(nullable: true),
                    ValuationCycle = table.Column<int>(nullable: true),
                    HairCut = table.Column<int>(nullable: true),
                    AllowSharing = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_collateraltype", x => x.CollateralTypeId);
                });

            migrationBuilder.CreateTable(
                name: "credit_creditbureau",
                columns: table => new
                {
                    CreditBureauId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditBureauName = table.Column<string>(maxLength: 300, nullable: false),
                    CorporateChargeAmount = table.Column<decimal>(nullable: false),
                    IndividualChargeAmount = table.Column<decimal>(nullable: false),
                    GLAccountId = table.Column<int>(nullable: false),
                    IsMandatory = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_creditbureau", x => x.CreditBureauId);
                });

            migrationBuilder.CreateTable(
                name: "credit_creditclassification",
                columns: table => new
                {
                    CreditClassificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    ProvisioningRequirement = table.Column<int>(nullable: false),
                    UpperLimit = table.Column<int>(nullable: true),
                    LowerLimit = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_creditclassification", x => x.CreditClassificationId);
                });

            migrationBuilder.CreateTable(
                name: "credit_creditrating",
                columns: table => new
                {
                    CreditRiskRatingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rate = table.Column<string>(maxLength: 50, nullable: false),
                    MinRange = table.Column<decimal>(nullable: false),
                    MaxRange = table.Column<decimal>(nullable: false),
                    AdvicedRange = table.Column<decimal>(nullable: true),
                    RateDescription = table.Column<string>(maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_creditrating", x => x.CreditRiskRatingId);
                });

            migrationBuilder.CreateTable(
                name: "credit_creditratingpd",
                columns: table => new
                {
                    CreditRiskRatingPDId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PD = table.Column<decimal>(nullable: false),
                    MinRangeScore = table.Column<decimal>(nullable: false),
                    MaxRangeScore = table.Column<decimal>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    InterestRate = table.Column<decimal>(nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_creditratingpd", x => x.CreditRiskRatingPDId);
                });

            migrationBuilder.CreateTable(
                name: "credit_creditriskattribute",
                columns: table => new
                {
                    CreditRiskAttributeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditRiskAttribute = table.Column<string>(maxLength: 500, nullable: false),
                    CreditRiskCategoryId = table.Column<int>(nullable: false),
                    AttributeField = table.Column<string>(maxLength: 50, nullable: false),
                    FriendlyName = table.Column<string>(maxLength: 500, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_creditriskattribute", x => x.CreditRiskAttributeId);
                });

            migrationBuilder.CreateTable(
                name: "credit_creditriskcategory",
                columns: table => new
                {
                    CreditRiskCategoryId = table.Column<int>(nullable: false),
                    CreditRiskCategoryName = table.Column<string>(maxLength: 255, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    UseInOrigination = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_creditriskcategory", x => x.CreditRiskCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "credit_creditriskscorecard",
                columns: table => new
                {
                    CreditRiskScoreCardId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditRiskAttributeId = table.Column<int>(nullable: false),
                    CustomerTypeId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 250, nullable: false),
                    Score = table.Column<decimal>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_creditriskscorecard", x => x.CreditRiskScoreCardId);
                });

            migrationBuilder.CreateTable(
                name: "credit_customercarddetails",
                columns: table => new
                {
                    CustomerCardDetailsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    CardNumber = table.Column<string>(maxLength: 250, nullable: false),
                    Cvv = table.Column<string>(maxLength: 50, nullable: false),
                    ExpiryMonth = table.Column<string>(maxLength: 550, nullable: false),
                    ExpiryYear = table.Column<string>(maxLength: 550, nullable: false),
                    currencyCode = table.Column<string>(nullable: false),
                    IssuingBank = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_customercarddetails", x => x.CustomerCardDetailsId);
                });

            migrationBuilder.CreateTable(
                name: "credit_customerloanlgd_history_final",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(maxLength: 250, nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 250, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 250, nullable: true),
                    ProductName = table.Column<string>(maxLength: 250, nullable: true),
                    LGD = table.Column<decimal>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    Year = table.Column<int>(nullable: true),
                    Variable1 = table.Column<decimal>(nullable: true),
                    Variable2 = table.Column<decimal>(nullable: true),
                    Variable3 = table.Column<decimal>(nullable: true),
                    Variable4 = table.Column<decimal>(nullable: true),
                    Variable5 = table.Column<decimal>(nullable: true),
                    Variable6 = table.Column<decimal>(nullable: true),
                    Variable7 = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_customerloanlgd_history_final", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "credit_customerloanpd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditRiskAttribute = table.Column<string>(maxLength: 250, nullable: true),
                    CustomerId = table.Column<int>(nullable: true),
                    LoanApplicationId = table.Column<int>(nullable: true),
                    AttributeField = table.Column<string>(maxLength: 250, nullable: true),
                    Score = table.Column<double>(nullable: true),
                    Coefficients = table.Column<double>(nullable: true),
                    AverageCoefficients = table.Column<double>(nullable: true),
                    Year = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_customerloanpd", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "credit_customerloanpd_application",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditRiskAttribute = table.Column<string>(maxLength: 250, nullable: true),
                    CustomerId = table.Column<int>(nullable: true),
                    LoanApplicationId_ = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    AttributeField = table.Column<string>(maxLength: 250, nullable: true),
                    Score = table.Column<double>(nullable: true),
                    Coefficients = table.Column<double>(nullable: true),
                    AverageCoefficients = table.Column<double>(nullable: true),
                    PD = table.Column<double>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    Year = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_customerloanpd_application", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "credit_customerloanpd_history",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditRiskAttribute = table.Column<string>(maxLength: 250, nullable: true),
                    CustomerName = table.Column<string>(maxLength: 250, nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 250, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 250, nullable: true),
                    ProductName = table.Column<string>(maxLength: 250, nullable: true),
                    AttributeField = table.Column<string>(maxLength: 250, nullable: true),
                    Score = table.Column<double>(nullable: true),
                    Coefficients = table.Column<double>(nullable: true),
                    AverageCoefficients = table.Column<double>(nullable: true),
                    PD = table.Column<double>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    Year = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_customerloanpd_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "credit_customerloanpd_history_final",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(maxLength: 250, nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 250, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 250, nullable: true),
                    ProductName = table.Column<string>(maxLength: 250, nullable: true),
                    PD = table.Column<decimal>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    Year = table.Column<int>(nullable: true),
                    Variable1 = table.Column<decimal>(nullable: true),
                    Variable2 = table.Column<decimal>(nullable: true),
                    Variable3 = table.Column<decimal>(nullable: true),
                    Variable4 = table.Column<decimal>(nullable: true),
                    Variable5 = table.Column<decimal>(nullable: true),
                    Variable6 = table.Column<decimal>(nullable: true),
                    Variable7 = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_customerloanpd_history_final", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "credit_customerloanscorecard",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditRiskAttribute = table.Column<string>(maxLength: 250, nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    LoanApplicationId = table.Column<int>(nullable: false),
                    AttributeField = table.Column<string>(maxLength: 250, nullable: false),
                    Score = table.Column<decimal>(nullable: false),
                    AttributeWeightedScore = table.Column<decimal>(nullable: false),
                    AverageWeightedScore = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_customerloanscorecard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "credit_daily_accural",
                columns: table => new
                {
                    DailyAccuralId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceNumber = table.Column<string>(maxLength: 50, nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    TransactionTypeId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    BranchId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    ExchangeRate = table.Column<double>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    InterestRate = table.Column<double>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    DailyAccuralAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_daily_accural", x => x.DailyAccuralId);
                });

            migrationBuilder.CreateTable(
                name: "credit_daycountconvention",
                columns: table => new
                {
                    DayCountConventionId = table.Column<int>(nullable: false),
                    DayCountConventionName = table.Column<string>(maxLength: 250, nullable: false),
                    DaysInAYear = table.Column<int>(nullable: false),
                    IsVisible = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_daycountconvention", x => x.DayCountConventionId);
                });

            migrationBuilder.CreateTable(
                name: "credit_directortype",
                columns: table => new
                {
                    DirectorTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_directortype", x => x.DirectorTypeId);
                });

            migrationBuilder.CreateTable(
                name: "credit_documenttype",
                columns: table => new
                {
                    DocumentTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_documenttype", x => x.DocumentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "credit_exposureparament",
                columns: table => new
                {
                    ExposureParameterId = table.Column<int>(nullable: false),
                    CustomerTypeId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(maxLength: 50, nullable: true),
                    Percentage = table.Column<decimal>(nullable: true),
                    ShareHolderAmount = table.Column<decimal>(nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_exposureparament", x => x.ExposureParameterId);
                });

            migrationBuilder.CreateTable(
                name: "credit_fee",
                columns: table => new
                {
                    FeeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeeName = table.Column<string>(maxLength: 250, nullable: false),
                    IsIntegral = table.Column<bool>(nullable: false),
                    TotalFeeGL = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_fee", x => x.FeeId);
                });

            migrationBuilder.CreateTable(
                name: "credit_fee_charge",
                columns: table => new
                {
                    fee_charge_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fee_id = table.Column<int>(nullable: true),
                    feename = table.Column<string>(maxLength: 50, nullable: true),
                    feecharge = table.Column<decimal>(nullable: true),
                    loanreviewId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_fee_charge", x => x.fee_charge_id);
                });

            migrationBuilder.CreateTable(
                name: "credit_frequencytype",
                columns: table => new
                {
                    FrequencyTypeId = table.Column<int>(nullable: false),
                    Mode = table.Column<string>(maxLength: 50, nullable: false),
                    Value = table.Column<double>(nullable: false),
                    Days = table.Column<int>(nullable: true),
                    IsVisible = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_frequencytype", x => x.FrequencyTypeId);
                });

            migrationBuilder.CreateTable(
                name: "credit_impairment",
                columns: table => new
                {
                    ImpairmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCode = table.Column<string>(maxLength: 250, nullable: false),
                    ProductName = table.Column<string>(maxLength: 250, nullable: false),
                    ECLType = table.Column<string>(maxLength: 50, nullable: true),
                    C12MonthPD = table.Column<decimal>(nullable: false),
                    LifeTimePD = table.Column<decimal>(nullable: false),
                    C12MonthLGD = table.Column<decimal>(nullable: false),
                    LifeTimeLGD = table.Column<decimal>(nullable: false),
                    C12MonthEAD = table.Column<decimal>(nullable: false),
                    LifeTimeEAD = table.Column<decimal>(nullable: false),
                    C12MonthECL = table.Column<decimal>(nullable: true),
                    LifeTimeECL = table.Column<decimal>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_impairment", x => x.ImpairmentId);
                });

            migrationBuilder.CreateTable(
                name: "credit_impairment_final",
                columns: table => new
                {
                    ImpairmentFinalId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCode = table.Column<string>(maxLength: 250, nullable: false),
                    ProductName = table.Column<string>(maxLength: 250, nullable: false),
                    ECLType = table.Column<string>(maxLength: 50, nullable: false),
                    Stage = table.Column<string>(maxLength: 50, nullable: true),
                    Scenario = table.Column<string>(maxLength: 50, nullable: true),
                    Likelihood = table.Column<decimal>(nullable: true),
                    Rate = table.Column<decimal>(nullable: true),
                    PD = table.Column<decimal>(nullable: false),
                    LGD = table.Column<decimal>(nullable: false),
                    EAD = table.Column<decimal>(nullable: true),
                    ECL = table.Column<decimal>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_impairment_final", x => x.ImpairmentFinalId);
                });

            migrationBuilder.CreateTable(
                name: "credit_individualapplicationscorecard_history",
                columns: table => new
                {
                    ApplicationScoreCardId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanAmount = table.Column<decimal>(nullable: false),
                    OutstandingBalance = table.Column<decimal>(nullable: true),
                    CustomerName = table.Column<string>(maxLength: 250, nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 250, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 250, nullable: true),
                    ProductName = table.Column<string>(maxLength: 250, nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    MaturityDate = table.Column<DateTime>(nullable: true),
                    CustomerTypeId = table.Column<int>(nullable: true),
                    Field1 = table.Column<decimal>(nullable: true),
                    Field2 = table.Column<decimal>(nullable: true),
                    Field3 = table.Column<decimal>(nullable: true),
                    Field4 = table.Column<decimal>(nullable: true),
                    Field5 = table.Column<decimal>(nullable: true),
                    Field6 = table.Column<decimal>(nullable: true),
                    Field7 = table.Column<decimal>(nullable: true),
                    Field8 = table.Column<decimal>(nullable: true),
                    Field9 = table.Column<decimal>(nullable: true),
                    Field10 = table.Column<decimal>(nullable: true),
                    Field11 = table.Column<decimal>(nullable: true),
                    Field12 = table.Column<decimal>(nullable: true),
                    Field13 = table.Column<decimal>(nullable: true),
                    Field14 = table.Column<decimal>(nullable: true),
                    Field15 = table.Column<decimal>(nullable: true),
                    Field16 = table.Column<decimal>(nullable: true),
                    Field17 = table.Column<decimal>(nullable: true),
                    Field18 = table.Column<decimal>(nullable: true),
                    Field19 = table.Column<decimal>(nullable: true),
                    Field20 = table.Column<decimal>(nullable: true),
                    Field21 = table.Column<decimal>(nullable: true),
                    Field22 = table.Column<decimal>(nullable: true),
                    Field23 = table.Column<decimal>(nullable: true),
                    Field24 = table.Column<decimal>(nullable: true),
                    Field25 = table.Column<decimal>(nullable: true),
                    Field26 = table.Column<decimal>(nullable: true),
                    Field27 = table.Column<decimal>(nullable: true),
                    Field28 = table.Column<decimal>(nullable: true),
                    Field29 = table.Column<decimal>(nullable: true),
                    Field30 = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_individualapplicationscorecard_history", x => x.ApplicationScoreCardId);
                });

            migrationBuilder.CreateTable(
                name: "credit_individualapplicationscorecarddetails_history",
                columns: table => new
                {
                    ApplicationCreditScoreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanAmount = table.Column<decimal>(nullable: false),
                    CustomerName = table.Column<string>(maxLength: 250, nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 250, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 250, nullable: true),
                    ProductName = table.Column<string>(maxLength: 250, nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    MaturityDate = table.Column<DateTime>(nullable: true),
                    AttributeField = table.Column<string>(maxLength: 50, nullable: false),
                    Score = table.Column<decimal>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_individualapplicationscorecarddetails_history", x => x.ApplicationCreditScoreId);
                });

            migrationBuilder.CreateTable(
                name: "credit_lgd_historyinformation",
                columns: table => new
                {
                    HistoricalLGDId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanAmount = table.Column<decimal>(nullable: false),
                    CustomerName = table.Column<string>(maxLength: 250, nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 250, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 250, nullable: true),
                    ProductName = table.Column<string>(maxLength: 250, nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    MaturityDate = table.Column<DateTime>(nullable: true),
                    CR = table.Column<double>(nullable: true),
                    EIR = table.Column<double>(nullable: true),
                    Frequency = table.Column<string>(maxLength: 50, nullable: true),
                    Field1 = table.Column<decimal>(nullable: true),
                    Field2 = table.Column<decimal>(nullable: true),
                    Field3 = table.Column<decimal>(nullable: true),
                    Field4 = table.Column<decimal>(nullable: true),
                    Field5 = table.Column<decimal>(nullable: true),
                    Field6 = table.Column<decimal>(nullable: true),
                    Field7 = table.Column<decimal>(nullable: true),
                    Field8 = table.Column<decimal>(nullable: true),
                    Field9 = table.Column<decimal>(nullable: true),
                    Field10 = table.Column<decimal>(nullable: true),
                    Field11 = table.Column<decimal>(nullable: true),
                    Field12 = table.Column<decimal>(nullable: true),
                    Field13 = table.Column<decimal>(nullable: true),
                    Field14 = table.Column<decimal>(nullable: true),
                    Field15 = table.Column<decimal>(nullable: true),
                    Field16 = table.Column<decimal>(nullable: true),
                    Field17 = table.Column<decimal>(nullable: true),
                    Field18 = table.Column<decimal>(nullable: true),
                    Field19 = table.Column<decimal>(nullable: true),
                    Field20 = table.Column<decimal>(nullable: true),
                    Field21 = table.Column<decimal>(nullable: true),
                    Field22 = table.Column<decimal>(nullable: true),
                    Field23 = table.Column<decimal>(nullable: true),
                    Field24 = table.Column<decimal>(nullable: true),
                    Field25 = table.Column<decimal>(nullable: true),
                    Field26 = table.Column<decimal>(nullable: true),
                    Field27 = table.Column<decimal>(nullable: true),
                    Field28 = table.Column<decimal>(nullable: true),
                    Field29 = table.Column<decimal>(nullable: true),
                    Field30 = table.Column<decimal>(nullable: true),
                    Field31 = table.Column<decimal>(nullable: true),
                    Field32 = table.Column<decimal>(nullable: true),
                    Field33 = table.Column<decimal>(nullable: true),
                    Field34 = table.Column<decimal>(nullable: true),
                    Field35 = table.Column<decimal>(nullable: true),
                    Field36 = table.Column<decimal>(nullable: true),
                    Field37 = table.Column<decimal>(nullable: true),
                    Field38 = table.Column<decimal>(nullable: true),
                    Field39 = table.Column<decimal>(nullable: true),
                    Field40 = table.Column<decimal>(nullable: true),
                    Field41 = table.Column<decimal>(nullable: true),
                    Field42 = table.Column<decimal>(nullable: true),
                    Field43 = table.Column<decimal>(nullable: true),
                    Field44 = table.Column<decimal>(nullable: true),
                    Field45 = table.Column<decimal>(nullable: true),
                    Field46 = table.Column<decimal>(nullable: true),
                    Field47 = table.Column<decimal>(nullable: true),
                    Field48 = table.Column<decimal>(nullable: true),
                    Field49 = table.Column<decimal>(nullable: true),
                    Field50 = table.Column<decimal>(nullable: true),
                    Field51 = table.Column<decimal>(nullable: true),
                    Field52 = table.Column<decimal>(nullable: true),
                    Field53 = table.Column<decimal>(nullable: true),
                    Field54 = table.Column<decimal>(nullable: true),
                    Field55 = table.Column<decimal>(nullable: true),
                    Field56 = table.Column<decimal>(nullable: true),
                    Field57 = table.Column<decimal>(nullable: true),
                    Field58 = table.Column<decimal>(nullable: true),
                    Field59 = table.Column<decimal>(nullable: true),
                    Field60 = table.Column<decimal>(nullable: true),
                    Field61 = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_lgd_historyinformation", x => x.HistoricalLGDId);
                });

            migrationBuilder.CreateTable(
                name: "credit_lgd_historyinformationdetails",
                columns: table => new
                {
                    HistoricalLGDId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanAmount = table.Column<decimal>(nullable: false),
                    CustomerName = table.Column<string>(maxLength: 250, nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 250, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 250, nullable: true),
                    ProductName = table.Column<string>(maxLength: 250, nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    MaturityDate = table.Column<DateTime>(nullable: true),
                    AttributeField = table.Column<string>(maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_lgd_historyinformationdetails", x => x.HistoricalLGDId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loan",
                columns: table => new
                {
                    LoanId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    LoanApplicationId = table.Column<int>(nullable: false),
                    PrincipalFrequencyTypeId = table.Column<int>(nullable: true),
                    InterestFrequencyTypeId = table.Column<int>(nullable: true),
                    ScheduleTypeId = table.Column<int>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    ExchangeRate = table.Column<double>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovedBy = table.Column<int>(nullable: false),
                    ApprovedComments = table.Column<string>(maxLength: 50, nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: false),
                    BookingDate = table.Column<DateTime>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    MaturityDate = table.Column<DateTime>(nullable: false),
                    LoanStatusId = table.Column<int>(nullable: true),
                    IsDisbursed = table.Column<bool>(nullable: false),
                    DisbursedBy = table.Column<int>(nullable: true),
                    DisbursedComments = table.Column<string>(maxLength: 50, nullable: true),
                    DisbursedDate = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    LoanRefNumber = table.Column<string>(maxLength: 50, nullable: true),
                    PrincipalAmount = table.Column<decimal>(nullable: false),
                    EquityContribution = table.Column<decimal>(nullable: true),
                    FirstPrincipalPaymentDate = table.Column<DateTime>(nullable: true),
                    FirstInterestPaymentDate = table.Column<DateTime>(nullable: true),
                    OutstandingAmortisedPrincipal = table.Column<decimal>(nullable: true),
                    OutstandingPrincipal = table.Column<decimal>(nullable: true),
                    OutstandingOldPrincipal = table.Column<decimal>(nullable: true),
                    OutstandingOldInterest = table.Column<decimal>(nullable: true),
                    OutstandingInterest = table.Column<decimal>(nullable: true),
                    AccrualBasis = table.Column<int>(nullable: true),
                    FirstDayType = table.Column<int>(nullable: true),
                    NPLDate = table.Column<DateTime>(nullable: true),
                    CustomerRiskRatingId = table.Column<int>(nullable: true),
                    LoanOperationId = table.Column<int>(nullable: true),
                    StaffId = table.Column<int>(nullable: true),
                    CasaAccountId = table.Column<int>(nullable: true),
                    BranchId = table.Column<int>(nullable: true),
                    PastDuePrincipal = table.Column<decimal>(nullable: true),
                    PastDueInterest = table.Column<decimal>(nullable: true),
                    InterestRate = table.Column<double>(nullable: false),
                    InterestOnPastDueInterest = table.Column<decimal>(nullable: true),
                    InterestOnPastDuePrincipal = table.Column<decimal>(nullable: true),
                    IntegralFeeAmount = table.Column<decimal>(nullable: true),
                    WorkflowToken = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loan", x => x.LoanId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loan_archive",
                columns: table => new
                {
                    LoanArchiveId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeEffectiveDate = table.Column<DateTime>(nullable: false),
                    ChangeReason = table.Column<string>(maxLength: 250, nullable: true),
                    LoanId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    LoanApplicationId = table.Column<int>(nullable: false),
                    PrincipalFrequencyTypeId = table.Column<int>(nullable: true),
                    InterestFrequencyTypeId = table.Column<int>(nullable: true),
                    ScheduleTypeId = table.Column<int>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    ExchangeRate = table.Column<double>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovedBy = table.Column<int>(nullable: false),
                    ApprovedComments = table.Column<string>(maxLength: 50, nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: false),
                    BookingDate = table.Column<DateTime>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    MaturityDate = table.Column<DateTime>(nullable: false),
                    LoanStatusId = table.Column<int>(nullable: true),
                    IsDisbursed = table.Column<bool>(nullable: false),
                    DisbursedBy = table.Column<int>(nullable: true),
                    DisbursedComments = table.Column<string>(maxLength: 50, nullable: true),
                    DisbursedDate = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    LoanRefNumber = table.Column<string>(maxLength: 50, nullable: true),
                    PrincipalAmount = table.Column<decimal>(nullable: false),
                    EquityContribution = table.Column<decimal>(nullable: true),
                    FirstPrincipalPaymentDate = table.Column<DateTime>(nullable: true),
                    FirstInterestPaymentDate = table.Column<DateTime>(nullable: true),
                    OutstandingPrincipal = table.Column<decimal>(nullable: true),
                    OutstandingInterest = table.Column<decimal>(nullable: true),
                    AccrualBasis = table.Column<int>(nullable: true),
                    FirstDayType = table.Column<int>(nullable: true),
                    NPLDate = table.Column<DateTime>(nullable: true),
                    CustomerRiskRatingId = table.Column<int>(nullable: true),
                    OperationId = table.Column<int>(nullable: true),
                    StaffId = table.Column<int>(nullable: true),
                    CasaAccountId = table.Column<int>(nullable: true),
                    BranchId = table.Column<int>(nullable: true),
                    PastDuePrincipal = table.Column<decimal>(nullable: true),
                    PastDueInterest = table.Column<decimal>(nullable: true),
                    InterestRate = table.Column<double>(nullable: false),
                    InterestOnPastDueInterest = table.Column<decimal>(nullable: true),
                    InterestOnPastDuePrincipal = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loan_archive", x => x.LoanArchiveId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loan_cheque",
                columns: table => new
                {
                    LoanChequeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(nullable: false),
                    Start = table.Column<string>(nullable: true),
                    End = table.Column<string>(nullable: true),
                    ChequeNo = table.Column<string>(nullable: true),
                    GeneralUpload = table.Column<byte[]>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loan_cheque", x => x.LoanChequeId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loan_cheque_list",
                columns: table => new
                {
                    LoanChequeListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(nullable: false),
                    LoanChequeId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ChequeNo = table.Column<string>(nullable: true),
                    StatusName = table.Column<string>(nullable: true),
                    SingleUpload = table.Column<byte[]>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loan_cheque_list", x => x.LoanChequeListId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loan_past_due",
                columns: table => new
                {
                    PastDueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(nullable: false),
                    ProductTypeId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    DateWithDefault = table.Column<DateTime>(nullable: true),
                    TransactionTypeId = table.Column<int>(nullable: false),
                    PastDueCode = table.Column<string>(maxLength: 50, nullable: false),
                    Parent_PastDueCode = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 800, nullable: false),
                    DebitAmount = table.Column<decimal>(nullable: false),
                    CreditAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loan_past_due", x => x.PastDueId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loan_repayment",
                columns: table => new
                {
                    LoanRepaymentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    InterestAmount = table.Column<decimal>(nullable: false),
                    PrincipalAmount = table.Column<decimal>(nullable: false),
                    ClosingBalance = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loan_repayment", x => x.LoanRepaymentId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loan_review_operation",
                columns: table => new
                {
                    LoanReviewOperationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(nullable: false),
                    ProductTypeId = table.Column<int>(nullable: false),
                    OperationTypeId = table.Column<int>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    ReviewDatials = table.Column<string>(nullable: false),
                    InterestRate = table.Column<double>(nullable: true),
                    Prepayment = table.Column<decimal>(nullable: true),
                    PrincipalFrequencyTypeId = table.Column<int>(nullable: true),
                    InterestFrequencyTypeId = table.Column<int>(nullable: true),
                    PrincipalFirstPaymentDate = table.Column<DateTime>(nullable: true),
                    InterestFirstPaymentDate = table.Column<DateTime>(nullable: true),
                    MaturityDate = table.Column<DateTime>(nullable: true),
                    Tenor = table.Column<int>(nullable: true),
                    CasaAccountId = table.Column<int>(nullable: true),
                    FeeCharges = table.Column<decimal>(nullable: true),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ISManagementRate = table.Column<bool>(nullable: false),
                    OperationCompleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loan_review_operation", x => x.LoanReviewOperationId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loan_temp",
                columns: table => new
                {
                    Loan_temp_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    LoanApplicationId = table.Column<int>(nullable: false),
                    PrincipalFrequencyTypeId = table.Column<int>(nullable: true),
                    InterestFrequencyTypeId = table.Column<int>(nullable: true),
                    ScheduleTypeId = table.Column<int>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    ExchangeRate = table.Column<double>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovedBy = table.Column<int>(nullable: false),
                    ApprovedComments = table.Column<string>(maxLength: 50, nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: false),
                    BookingDate = table.Column<DateTime>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    MaturityDate = table.Column<DateTime>(nullable: false),
                    LoanStatusId = table.Column<int>(nullable: true),
                    IsDisbursed = table.Column<bool>(nullable: false),
                    DisbursedBy = table.Column<int>(nullable: true),
                    DisbursedComments = table.Column<string>(maxLength: 50, nullable: true),
                    DisbursedDate = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    LoanRefNumber = table.Column<string>(maxLength: 50, nullable: true),
                    PrincipalAmount = table.Column<decimal>(nullable: false),
                    EquityContribution = table.Column<decimal>(nullable: true),
                    FirstPrincipalPaymentDate = table.Column<DateTime>(nullable: true),
                    FirstInterestPaymentDate = table.Column<DateTime>(nullable: true),
                    OutstandingPrincipal = table.Column<decimal>(nullable: true),
                    OutstandingInterest = table.Column<decimal>(nullable: true),
                    OldBalance = table.Column<decimal>(nullable: true),
                    OldInterest = table.Column<decimal>(nullable: true),
                    AccrualBasis = table.Column<int>(nullable: true),
                    FirstDayType = table.Column<int>(nullable: true),
                    NPLDate = table.Column<DateTime>(nullable: true),
                    CustomerRiskRatingId = table.Column<int>(nullable: true),
                    LoanOperationId = table.Column<int>(nullable: true),
                    StaffId = table.Column<int>(nullable: true),
                    CasaAccountId = table.Column<int>(nullable: true),
                    BranchId = table.Column<int>(nullable: true),
                    PastDuePrincipal = table.Column<decimal>(nullable: true),
                    PastDueInterest = table.Column<decimal>(nullable: true),
                    InterestRate = table.Column<double>(nullable: false),
                    InterestOnPastDueInterest = table.Column<decimal>(nullable: true),
                    InterestOnPastDuePrincipal = table.Column<decimal>(nullable: true),
                    IntegralFeeAmount = table.Column<decimal>(nullable: true),
                    WorkflowToken = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loan_temp", x => x.Loan_temp_Id);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanapplication_website",
                columns: table => new
                {
                    WebsiteLoanApplicationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    ProposedProductId = table.Column<int>(nullable: false),
                    ProposedTenor = table.Column<int>(nullable: false),
                    ProposedRate = table.Column<double>(nullable: false),
                    ProposedAmount = table.Column<decimal>(nullable: false),
                    ApprovedProductId = table.Column<int>(nullable: false),
                    ApprovedTenor = table.Column<int>(nullable: false),
                    ApprovedRate = table.Column<double>(nullable: false),
                    ApprovedAmount = table.Column<decimal>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    ExchangeRate = table.Column<double>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    HasDoneChecklist = table.Column<bool>(nullable: false),
                    ApplicationDate = table.Column<DateTime>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    MaturityDate = table.Column<DateTime>(nullable: false),
                    FirstPrincipalDate = table.Column<DateTime>(nullable: true),
                    FirstInterestDate = table.Column<DateTime>(nullable: true),
                    LoanApplicationStatusId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    ApplicationRefNumber = table.Column<string>(maxLength: 50, nullable: false),
                    Score = table.Column<decimal>(nullable: true),
                    PD = table.Column<decimal>(nullable: true),
                    GenerateOfferLetter = table.Column<bool>(nullable: false),
                    Purpose = table.Column<string>(maxLength: 2000, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanapplication_website", x => x.WebsiteLoanApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanapplicationcollateraldocument",
                columns: table => new
                {
                    LoanApplicationCollateralDocumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanApplicationId = table.Column<int>(nullable: true),
                    CollateralTypeId = table.Column<int>(nullable: true),
                    Document = table.Column<byte[]>(nullable: true),
                    DocumentName = table.Column<string>(maxLength: 256, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CollateralCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanapplicationcollateraldocument", x => x.LoanApplicationCollateralDocumentId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loancomment",
                columns: table => new
                {
                    LoanCommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: true),
                    Comment = table.Column<string>(maxLength: 500, nullable: true),
                    NextStep = table.Column<string>(maxLength: 500, nullable: true),
                    LoanId = table.Column<int>(nullable: false),
                    LoanScheduleId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loancomment", x => x.LoanCommentId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loancustomerfscaptiongroup",
                columns: table => new
                {
                    FSCaptionGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FSCaptionGroupName = table.Column<string>(maxLength: 50, nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loancustomerfscaptiongroup", x => x.FSCaptionGroupId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loancustomerfsratiodivisortype",
                columns: table => new
                {
                    DivisorTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DivisorTypeName = table.Column<string>(maxLength: 50, nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loancustomerfsratiodivisortype", x => x.DivisorTypeId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loancustomerfsratiovaluetype",
                columns: table => new
                {
                    ValueTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValueTypeName = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loancustomerfsratiovaluetype", x => x.ValueTypeId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loancustomerratiodetail",
                columns: table => new
                {
                    RatioDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RatioName = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loancustomerratiodetail", x => x.RatioDetailId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loandecision",
                columns: table => new
                {
                    LoanDecisionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: true),
                    Decision = table.Column<string>(maxLength: 500, nullable: true),
                    LoanId = table.Column<int>(nullable: false),
                    LoanScheduleId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loandecision", x => x.LoanDecisionId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanoperation",
                columns: table => new
                {
                    LoanOperationId = table.Column<byte>(nullable: false),
                    LoanOperationName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanoperation", x => x.LoanOperationId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanreviewapplication",
                columns: table => new
                {
                    LoanReviewApplicationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ReviewDetails = table.Column<string>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    GenerateOfferLetter = table.Column<bool>(nullable: false),
                    OperationPerformed = table.Column<bool>(nullable: true),
                    ProposedTenor = table.Column<int>(nullable: false),
                    ProposedRate = table.Column<double>(nullable: false),
                    ProposedAmount = table.Column<decimal>(nullable: false),
                    ApprovedTenor = table.Column<int>(nullable: false),
                    ApprovedRate = table.Column<double>(nullable: false),
                    ApprovedAmount = table.Column<decimal>(nullable: false),
                    PrincipalFrequencyTypeId = table.Column<int>(nullable: true),
                    InterestFrequencyTypeId = table.Column<int>(nullable: true),
                    FirstPrincipalPaymentDate = table.Column<DateTime>(nullable: true),
                    FirstInterestPaymentDate = table.Column<DateTime>(nullable: true),
                    Prepayment = table.Column<decimal>(nullable: true),
                    WorkflowToken = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanreviewapplication", x => x.LoanReviewApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanreviewapplicationlog",
                columns: table => new
                {
                    LoanReviewApplicationLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanReviewApplicationId = table.Column<int>(nullable: false),
                    LoanId = table.Column<int>(nullable: false),
                    ApprovedTenor = table.Column<int>(nullable: false),
                    ApprovedRate = table.Column<double>(nullable: false),
                    ApprovedAmount = table.Column<decimal>(nullable: false),
                    PrincipalFrequencyTypeId = table.Column<int>(nullable: true),
                    InterestFrequencyTypeId = table.Column<int>(nullable: true),
                    FirstPrincipalPaymentDate = table.Column<DateTime>(nullable: true),
                    FirstInterestPaymentDate = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanreviewapplicationlog", x => x.LoanReviewApplicationLogId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanreviewofferletter",
                columns: table => new
                {
                    LoanreviewOfferLetterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanReviewApplicationId = table.Column<int>(nullable: false),
                    ReportStatus = table.Column<string>(maxLength: 50, nullable: true),
                    SupportDocument = table.Column<byte[]>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanreviewofferletter", x => x.LoanreviewOfferLetterId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanreviewoperation",
                columns: table => new
                {
                    LoanReviewOperationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(nullable: false),
                    InterestRate = table.Column<double>(nullable: true),
                    Prepayment = table.Column<decimal>(nullable: true),
                    PrincipalFrequencyTypeId = table.Column<int>(nullable: true),
                    InterestFrequencyTypeId = table.Column<int>(nullable: true),
                    PrincipalFirstPaymentDate = table.Column<DateTime>(nullable: true),
                    InterestFirstPaymentDate = table.Column<DateTime>(nullable: true),
                    MaturityDate = table.Column<DateTime>(nullable: true),
                    Tenor = table.Column<int>(nullable: true),
                    CASA_AccountId = table.Column<int>(nullable: true),
                    OverDraftTopUp = table.Column<decimal>(nullable: true),
                    FeeCharges = table.Column<decimal>(nullable: true),
                    ScheduleTypeId = table.Column<int>(nullable: true),
                    ScheduleDayCountConventionId = table.Column<int>(nullable: true),
                    SchedultDayInterestTypeId = table.Column<int>(nullable: true),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    IsManagementInterest = table.Column<bool>(nullable: false),
                    OperationComleted = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanreviewoperation", x => x.LoanReviewOperationId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanschedulecategory",
                columns: table => new
                {
                    LoanScheduleCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanScheduleCategoryName = table.Column<string>(maxLength: 250, nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanschedulecategory", x => x.LoanScheduleCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanscheduledaily",
                columns: table => new
                {
                    LoanScheduleDailyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(nullable: false),
                    PaymentNumber = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    OpeningBalance = table.Column<decimal>(nullable: false),
                    StartPrincipalAmount = table.Column<decimal>(nullable: false),
                    DailyPaymentAmount = table.Column<decimal>(nullable: false),
                    DailyInterestAmount = table.Column<decimal>(nullable: false),
                    DailyPrincipalAmount = table.Column<decimal>(nullable: false),
                    ClosingBalance = table.Column<decimal>(nullable: false),
                    EndPrincipalAmount = table.Column<decimal>(nullable: false),
                    AccruedInterest = table.Column<decimal>(nullable: false),
                    AmortisedCost = table.Column<decimal>(nullable: false),
                    InterestRate = table.Column<double>(nullable: false),
                    AmortisedOpeningBalance = table.Column<decimal>(nullable: false),
                    AmortisedStartPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedDailyPaymentAmount = table.Column<decimal>(nullable: false),
                    AmortisedDailyInterestAmount = table.Column<decimal>(nullable: false),
                    AmortisedDailyPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedClosingBalance = table.Column<decimal>(nullable: false),
                    AmortisedEndPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedAccruedInterest = table.Column<decimal>(nullable: false),
                    Amortised_AmortisedCost = table.Column<decimal>(nullable: false),
                    UnearnedFee = table.Column<decimal>(nullable: false),
                    EarnedFee = table.Column<decimal>(nullable: false),
                    EffectiveInterestRate = table.Column<double>(nullable: false),
                    StaffId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanscheduledaily", x => x.LoanScheduleDailyId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanscheduledailyarchive",
                columns: table => new
                {
                    LoanScheduleDailyArchiveId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArchiveDate = table.Column<DateTime>(nullable: false),
                    ArchiveBatchCode = table.Column<string>(maxLength: 250, nullable: true),
                    LoanId = table.Column<int>(nullable: false),
                    PaymentNumber = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    OpeningBalance = table.Column<decimal>(nullable: false),
                    StartPrincipalAmount = table.Column<decimal>(nullable: false),
                    DailyPaymentAmount = table.Column<decimal>(nullable: false),
                    DailyInterestAmount = table.Column<decimal>(nullable: false),
                    DailyPrincipalAmount = table.Column<decimal>(nullable: false),
                    ClosingBalance = table.Column<decimal>(nullable: false),
                    EndPrincipalAmount = table.Column<decimal>(nullable: false),
                    AccruedInterest = table.Column<decimal>(nullable: false),
                    AmortisedCost = table.Column<decimal>(nullable: false),
                    InterestRate = table.Column<double>(nullable: false),
                    AmortisedOpeningBalance = table.Column<decimal>(nullable: false),
                    AmortisedStartPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedDailyPaymentAmount = table.Column<decimal>(nullable: false),
                    AmortisedDailyInterestAmount = table.Column<decimal>(nullable: false),
                    AmortisedDailyPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedClosingBalance = table.Column<decimal>(nullable: false),
                    AmortisedEndPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedAccruedInterest = table.Column<decimal>(nullable: false),
                    Amortised_AmortisedCost = table.Column<decimal>(nullable: false),
                    UnearnedFee = table.Column<decimal>(nullable: false),
                    EarnedFee = table.Column<decimal>(nullable: false),
                    EffectiveInterestRate = table.Column<double>(nullable: false),
                    StaffId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanscheduledailyarchive", x => x.LoanScheduleDailyArchiveId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanscheduleirrigular",
                columns: table => new
                {
                    LoanScheduleIrrigularId = table.Column<byte>(nullable: false),
                    LoanId = table.Column<int>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    PaymentAmount = table.Column<decimal>(nullable: false),
                    StaffId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanscheduleirrigular", x => x.LoanScheduleIrrigularId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanscheduleperiodic",
                columns: table => new
                {
                    LoanSchedulePeriodicId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(nullable: false),
                    PaymentNumber = table.Column<int>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    StartPrincipalAmount = table.Column<decimal>(nullable: false),
                    PeriodPaymentAmount = table.Column<decimal>(nullable: false),
                    PeriodInterestAmount = table.Column<decimal>(nullable: false),
                    PeriodPrincipalAmount = table.Column<decimal>(nullable: false),
                    ClosingBalance = table.Column<decimal>(nullable: false),
                    EndPrincipalAmount = table.Column<decimal>(nullable: false),
                    InterestRate = table.Column<double>(nullable: false),
                    AmortisedStartPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedPeriodPaymentAmount = table.Column<decimal>(nullable: false),
                    AmortisedPeriodInterestAmount = table.Column<decimal>(nullable: false),
                    AmortisedPeriodPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedClosingBalance = table.Column<decimal>(nullable: false),
                    AmortisedEndPrincipalAmount = table.Column<decimal>(nullable: false),
                    EffectiveInterestRate = table.Column<double>(nullable: false),
                    ActualRepayment = table.Column<decimal>(nullable: true),
                    ActualRepaymentInterest = table.Column<decimal>(nullable: true),
                    PaymentPending = table.Column<decimal>(nullable: true),
                    PaymentPendingInterest = table.Column<decimal>(nullable: true),
                    StaffId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanscheduleperiodic", x => x.LoanSchedulePeriodicId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanscheduleperiodicarchive",
                columns: table => new
                {
                    LoanSchedulePeriodicArchiveId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArchiveDate = table.Column<DateTime>(nullable: false),
                    ArchiveBatchCode = table.Column<string>(maxLength: 250, nullable: true),
                    LoanId = table.Column<int>(nullable: false),
                    PaymentNumber = table.Column<int>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    StartPrincipalAmount = table.Column<decimal>(nullable: false),
                    PeriodPaymentAmount = table.Column<decimal>(nullable: false),
                    PeriodInterestAmount = table.Column<decimal>(nullable: false),
                    PeriodPrincipalAmount = table.Column<decimal>(nullable: false),
                    ClosingBalance = table.Column<decimal>(nullable: false),
                    EndPrincipalAmount = table.Column<decimal>(nullable: false),
                    InterestRate = table.Column<double>(nullable: false),
                    AmortisedStartPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedPeriodPaymentAmount = table.Column<decimal>(nullable: false),
                    AmortisedPeriodInterestAmount = table.Column<decimal>(nullable: false),
                    AmortisedPeriodPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedClosingBalance = table.Column<decimal>(nullable: false),
                    AmortisedEndPrincipalAmount = table.Column<decimal>(nullable: false),
                    EffectiveInterestRate = table.Column<double>(nullable: false),
                    StaffId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanscheduleperiodicarchive", x => x.LoanSchedulePeriodicArchiveId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanstaging",
                columns: table => new
                {
                    LoanStagingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProbationPeriod = table.Column<int>(nullable: false),
                    From = table.Column<int>(nullable: false),
                    To = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanstaging", x => x.LoanStagingId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loantable",
                columns: table => new
                {
                    LoanTableId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanAmount = table.Column<decimal>(nullable: false),
                    CustomerName = table.Column<string>(maxLength: 250, nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 250, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 250, nullable: true),
                    ProductName = table.Column<string>(maxLength: 250, nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    MaturityDate = table.Column<DateTime>(nullable: true),
                    CustomerTypeId = table.Column<int>(nullable: true),
                    Field1 = table.Column<decimal>(nullable: true),
                    Field2 = table.Column<decimal>(nullable: true),
                    Field3 = table.Column<decimal>(nullable: true),
                    Field4 = table.Column<decimal>(nullable: true),
                    Field5 = table.Column<decimal>(nullable: true),
                    Field6 = table.Column<decimal>(nullable: true),
                    Field7 = table.Column<decimal>(nullable: true),
                    Field8 = table.Column<decimal>(nullable: true),
                    Field9 = table.Column<decimal>(nullable: true),
                    Field10 = table.Column<decimal>(nullable: true),
                    Field11 = table.Column<decimal>(nullable: true),
                    Field12 = table.Column<decimal>(nullable: true),
                    Field13 = table.Column<decimal>(nullable: true),
                    Field14 = table.Column<decimal>(nullable: true),
                    Field15 = table.Column<decimal>(nullable: true),
                    Field16 = table.Column<decimal>(nullable: true),
                    Field17 = table.Column<decimal>(nullable: true),
                    Field18 = table.Column<decimal>(nullable: true),
                    Field19 = table.Column<decimal>(nullable: true),
                    Field20 = table.Column<decimal>(nullable: true),
                    Field21 = table.Column<decimal>(nullable: true),
                    Field22 = table.Column<decimal>(nullable: true),
                    Field23 = table.Column<decimal>(nullable: true),
                    Field24 = table.Column<decimal>(nullable: true),
                    Field25 = table.Column<decimal>(nullable: true),
                    Field26 = table.Column<decimal>(nullable: true),
                    Field27 = table.Column<decimal>(nullable: true),
                    Field28 = table.Column<decimal>(nullable: true),
                    Field29 = table.Column<decimal>(nullable: true),
                    Field30 = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loantable", x => x.LoanTableId);
                });

            migrationBuilder.CreateTable(
                name: "credit_loantransactiontype",
                columns: table => new
                {
                    LoanTransactionTypeId = table.Column<byte>(nullable: false),
                    LoanTransactionTypeName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loantransactiontype", x => x.LoanTransactionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "credit_offerletter",
                columns: table => new
                {
                    OfferLetterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanApplicationId = table.Column<int>(nullable: false),
                    ReportStatus = table.Column<string>(maxLength: 50, nullable: true),
                    SupportDocument = table.Column<byte[]>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_offerletter", x => x.OfferLetterId);
                });

            migrationBuilder.CreateTable(
                name: "credit_productfeestatus",
                columns: table => new
                {
                    ProductFeeStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanApplicationId = table.Column<int>(nullable: false),
                    ProductFeeId = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    LoanId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_productfeestatus", x => x.ProductFeeStatusId);
                });

            migrationBuilder.CreateTable(
                name: "credit_producthistoricalpd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    ProductName = table.Column<string>(maxLength: 250, nullable: true),
                    PD = table.Column<double>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_producthistoricalpd", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "credit_producttype",
                columns: table => new
                {
                    ProductTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductTypeName = table.Column<string>(maxLength: 50, nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_producttype", x => x.ProductTypeId);
                });

            migrationBuilder.CreateTable(
                name: "credit_repaymenttype",
                columns: table => new
                {
                    RepaymentTypeId = table.Column<int>(nullable: false),
                    RepaymentTypeName = table.Column<string>(maxLength: 250, nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_repaymenttype", x => x.RepaymentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "credit_systemattribute",
                columns: table => new
                {
                    SystemAttributeId = table.Column<int>(nullable: false),
                    SystemAttributeName = table.Column<string>(maxLength: 250, nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_systemattribute", x => x.SystemAttributeId);
                });

            migrationBuilder.CreateTable(
                name: "credit_temp_loanscheduledaily",
                columns: table => new
                {
                    LoanScheduleDailyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(nullable: false),
                    PaymentNumber = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    OpeningBalance = table.Column<decimal>(nullable: false),
                    StartPrincipalAmount = table.Column<decimal>(nullable: false),
                    DailyPaymentAmount = table.Column<decimal>(nullable: false),
                    DailyInterestAmount = table.Column<decimal>(nullable: false),
                    DailyPrincipalAmount = table.Column<decimal>(nullable: false),
                    ClosingBalance = table.Column<decimal>(nullable: false),
                    EndPrincipalAmount = table.Column<decimal>(nullable: false),
                    AccruedInterest = table.Column<decimal>(nullable: false),
                    AmortisedCost = table.Column<decimal>(nullable: false),
                    InterestRate = table.Column<double>(nullable: false),
                    AmortisedOpeningBalance = table.Column<decimal>(nullable: false),
                    AmortisedStartPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedDailyPaymentAmount = table.Column<decimal>(nullable: false),
                    AmortisedDailyInterestAmount = table.Column<decimal>(nullable: false),
                    AmortisedDailyPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedClosingBalance = table.Column<decimal>(nullable: false),
                    AmortisedEndPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedAccruedInterest = table.Column<decimal>(nullable: false),
                    Amortised_AmortisedCost = table.Column<decimal>(nullable: false),
                    UnearnedFee = table.Column<decimal>(nullable: false),
                    EarnedFee = table.Column<decimal>(nullable: false),
                    EffectiveInterestRate = table.Column<double>(nullable: false),
                    StaffId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_temp_loanscheduledaily", x => x.LoanScheduleDailyId);
                });

            migrationBuilder.CreateTable(
                name: "credit_temp_loanscheduleirrigular",
                columns: table => new
                {
                    LoanScheduleIrrigularId = table.Column<byte>(nullable: false),
                    LoanId = table.Column<int>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    PaymentAmount = table.Column<decimal>(nullable: false),
                    StaffId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_temp_loanscheduleirrigular", x => x.LoanScheduleIrrigularId);
                });

            migrationBuilder.CreateTable(
                name: "credit_temp_loanscheduleperiodic",
                columns: table => new
                {
                    LoanSchedulePeriodicId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<int>(nullable: false),
                    PaymentNumber = table.Column<int>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    StartPrincipalAmount = table.Column<decimal>(nullable: false),
                    PeriodPaymentAmount = table.Column<decimal>(nullable: false),
                    PeriodInterestAmount = table.Column<decimal>(nullable: false),
                    PeriodPrincipalAmount = table.Column<decimal>(nullable: false),
                    ClosingBalance = table.Column<decimal>(nullable: false),
                    EndPrincipalAmount = table.Column<decimal>(nullable: false),
                    InterestRate = table.Column<double>(nullable: false),
                    AmortisedStartPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedPeriodPaymentAmount = table.Column<decimal>(nullable: false),
                    AmortisedPeriodInterestAmount = table.Column<decimal>(nullable: false),
                    AmortisedPeriodPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedClosingBalance = table.Column<decimal>(nullable: false),
                    AmortisedEndPrincipalAmount = table.Column<decimal>(nullable: false),
                    EffectiveInterestRate = table.Column<double>(nullable: false),
                    StaffId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_temp_loanscheduleperiodic", x => x.LoanSchedulePeriodicId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_accountopening",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerTypeId = table.Column<int>(nullable: false),
                    AccountTypeId = table.Column<int>(nullable: false),
                    AccountCategoryId = table.Column<int>(nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Title = table.Column<int>(nullable: true),
                    Surname = table.Column<string>(maxLength: 50, nullable: true),
                    Firstname = table.Column<string>(maxLength: 50, nullable: true),
                    Othername = table.Column<string>(maxLength: 50, nullable: true),
                    MaritalStatusId = table.Column<int>(nullable: true),
                    GenderId = table.Column<int>(nullable: true),
                    BirthCountryId = table.Column<int>(nullable: true),
                    DOB = table.Column<DateTime>(type: "date", nullable: true),
                    MotherMaidenName = table.Column<string>(maxLength: 50, nullable: true),
                    RelationshipOfficerId = table.Column<int>(nullable: true),
                    TaxIDNumber = table.Column<string>(maxLength: 500, nullable: true),
                    BVN = table.Column<string>(maxLength: 500, nullable: true),
                    Nationality = table.Column<int>(nullable: true),
                    ResidentPermitNumber = table.Column<string>(maxLength: 50, nullable: true),
                    PermitIssueDate = table.Column<DateTime>(type: "date", nullable: true),
                    PermitExpiryDate = table.Column<DateTime>(type: "date", nullable: true),
                    SocialSecurityNumber = table.Column<string>(maxLength: 50, nullable: true),
                    StateOfOrigin = table.Column<int>(nullable: true),
                    LocalGovernment = table.Column<int>(nullable: true),
                    ResidentOfCountry = table.Column<bool>(nullable: true),
                    Address1 = table.Column<string>(maxLength: 50, nullable: true),
                    Address2 = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 50, nullable: true),
                    StateId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    MailingAddress = table.Column<string>(maxLength: 50, nullable: true),
                    MobileNumber = table.Column<string>(maxLength: 50, nullable: true),
                    InternetBanking = table.Column<bool>(nullable: true),
                    EmailStatement = table.Column<bool>(nullable: true),
                    Card = table.Column<bool>(nullable: true),
                    SmsAlert = table.Column<bool>(nullable: true),
                    EmailAlert = table.Column<bool>(nullable: true),
                    Token = table.Column<bool>(nullable: true),
                    EmploymentType = table.Column<int>(nullable: true),
                    EmployerName = table.Column<string>(maxLength: 50, nullable: true),
                    EmployerAddress = table.Column<string>(maxLength: 50, nullable: true),
                    EmployerState = table.Column<string>(maxLength: 50, nullable: true),
                    Occupation = table.Column<string>(maxLength: 50, nullable: true),
                    BusinessName = table.Column<string>(maxLength: 50, nullable: true),
                    BusinessAddress = table.Column<string>(maxLength: 50, nullable: true),
                    BusinessState = table.Column<string>(maxLength: 50, nullable: true),
                    JobTitle = table.Column<string>(maxLength: 50, nullable: true),
                    Other = table.Column<string>(maxLength: 50, nullable: true),
                    DeclarationDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeclarationCompleted = table.Column<bool>(nullable: true),
                    SignatureUpload = table.Column<byte[]>(nullable: true),
                    SoleSignatory = table.Column<int>(nullable: true),
                    MaxNoOfSignatory = table.Column<int>(nullable: true),
                    RegistrationNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Industry = table.Column<string>(maxLength: 50, nullable: true),
                    Jurisdiction = table.Column<string>(maxLength: 50, nullable: true),
                    Website = table.Column<string>(maxLength: 50, nullable: true),
                    NatureOfBusiness = table.Column<string>(maxLength: 50, nullable: true),
                    AnnualRevenue = table.Column<string>(maxLength: 50, nullable: true),
                    IsStockExchange = table.Column<bool>(nullable: true),
                    Stock = table.Column<string>(maxLength: 50, nullable: true),
                    RegisteredAddress = table.Column<string>(maxLength: 50, nullable: true),
                    ScumlNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_accountopening", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_accountreactivation",
                columns: table => new
                {
                    ReactivationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: true),
                    Substructure = table.Column<int>(nullable: true),
                    AccountName = table.Column<string>(maxLength: 50, nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 50, nullable: true),
                    AccountBalance = table.Column<decimal>(nullable: true),
                    Currency = table.Column<int>(nullable: true),
                    Balance = table.Column<decimal>(nullable: true),
                    Reason = table.Column<string>(maxLength: 50, nullable: true),
                    Charges = table.Column<string>(maxLength: 50, nullable: true),
                    ApproverName = table.Column<string>(maxLength: 50, nullable: true),
                    ApproverComment = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_accountreactivation", x => x.ReactivationId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_accountreactivationsetup",
                columns: table => new
                {
                    ReactivationSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: true),
                    Product = table.Column<int>(nullable: true),
                    ChargesApplicable = table.Column<bool>(nullable: true),
                    Charge = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    ChargeType = table.Column<string>(maxLength: 50, nullable: true),
                    PresetChart = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_accountreactivationsetup", x => x.ReactivationSetupId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_accountype",
                columns: table => new
                {
                    AccountTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_accountype", x => x.AccountTypeId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_bankclosure",
                columns: table => new
                {
                    BankClosureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: true),
                    SubStructure = table.Column<int>(nullable: true),
                    AccountName = table.Column<string>(maxLength: 50, nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<bool>(nullable: true),
                    AccountBalance = table.Column<string>(maxLength: 50, nullable: true),
                    Currency = table.Column<int>(nullable: true),
                    ClosingDate = table.Column<DateTime>(type: "date", nullable: true),
                    Reason = table.Column<string>(maxLength: 50, nullable: true),
                    Charges = table.Column<decimal>(nullable: true),
                    FinalSettlement = table.Column<string>(maxLength: 50, nullable: true),
                    Beneficiary = table.Column<string>(maxLength: 50, nullable: true),
                    ModeOfSettlement = table.Column<bool>(nullable: true),
                    TransferAccount = table.Column<string>(maxLength: 50, nullable: true),
                    ApproverName = table.Column<string>(maxLength: 50, nullable: true),
                    ApproverComment = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_bankclosure", x => x.BankClosureId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_bankclosuresetup",
                columns: table => new
                {
                    BankClosureSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    ClosureChargeApplicable = table.Column<bool>(nullable: true),
                    Charge = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    ChargeType = table.Column<string>(maxLength: 50, nullable: true),
                    SettlementBalance = table.Column<bool>(nullable: true),
                    PresetChart = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_bankclosuresetup", x => x.BankClosureSetupId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_businesscategory",
                columns: table => new
                {
                    BusinessCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_businesscategory", x => x.BusinessCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_cashiertellerform",
                columns: table => new
                {
                    DepositCashierTellerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: false),
                    SubStructure = table.Column<int>(nullable: true),
                    Currency = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    OpeningBalance = table.Column<decimal>(nullable: true),
                    ClosingBalance = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_cashiertellerform", x => x.DepositCashierTellerId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_cashiertellersetup",
                columns: table => new
                {
                    DepositCashierTellerSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    PresetChart = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_cashiertellersetup", x => x.DepositCashierTellerSetupId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_changeofrates",
                columns: table => new
                {
                    ChangeOfRateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: true),
                    Product = table.Column<int>(nullable: true),
                    CurrentRate = table.Column<decimal>(nullable: true),
                    ProposedRate = table.Column<decimal>(nullable: true),
                    Reasons = table.Column<string>(maxLength: 500, nullable: true),
                    ApproverName = table.Column<string>(maxLength: 50, nullable: true),
                    ApproverComment = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_changeofrates", x => x.ChangeOfRateId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_changeofratesetup",
                columns: table => new
                {
                    ChangeOfRateSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    CanApply = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_changeofratesetup", x => x.ChangeOfRateSetupId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_depositform",
                columns: table => new
                {
                    DepositFormId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: false),
                    Operation = table.Column<int>(nullable: true),
                    TransactionId = table.Column<int>(nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    ValueDate = table.Column<DateTime>(type: "date", nullable: true),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    TransactionDescription = table.Column<string>(maxLength: 50, nullable: true),
                    TransactionParticulars = table.Column<string>(maxLength: 50, nullable: true),
                    Remark = table.Column<string>(maxLength: 50, nullable: true),
                    ModeOfTransaction = table.Column<string>(maxLength: 50, nullable: true),
                    InstrumentNumber = table.Column<string>(maxLength: 50, nullable: true),
                    InstrumentDate = table.Column<DateTime>(type: "date", nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_depositform", x => x.DepositFormId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_selectedTransactioncharge",
                columns: table => new
                {
                    SelectedTransChargeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionChargeId = table.Column<int>(nullable: true),
                    AccountId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_selectedTransactioncharge", x => x.SelectedTransChargeId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_selectedTransactiontax",
                columns: table => new
                {
                    SelectedTransTaxId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionTaxId = table.Column<int>(nullable: true),
                    AccountId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_selectedTransactiontax", x => x.SelectedTransTaxId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_tillvaultform",
                columns: table => new
                {
                    TillVaultId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TillId = table.Column<int>(nullable: true),
                    Currency = table.Column<int>(nullable: true),
                    OpeningBalance = table.Column<decimal>(nullable: true),
                    IncomingCash = table.Column<decimal>(nullable: true),
                    OutgoingCash = table.Column<decimal>(nullable: true),
                    ClosingBalance = table.Column<decimal>(nullable: true),
                    CashAvailable = table.Column<decimal>(nullable: true),
                    Shortage = table.Column<string>(maxLength: 10, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_tillvaultform", x => x.TillVaultId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_tillvaultopeningclose",
                columns: table => new
                {
                    TillVaultOpeningCloseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: true),
                    Currency = table.Column<int>(nullable: true),
                    AmountPerSystem = table.Column<decimal>(nullable: true),
                    CashAvailable = table.Column<decimal>(nullable: true),
                    Shortage = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_tillvaultopeningclose", x => x.TillVaultOpeningCloseId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_tillvaultsetup",
                columns: table => new
                {
                    TillVaultSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: true),
                    PresetChart = table.Column<bool>(nullable: true),
                    StructureTillIdPrefix = table.Column<string>(maxLength: 50, nullable: true),
                    TellerTillIdPrefix = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_tillvaultsetup", x => x.TillVaultSetupId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_tillvaultsummary",
                columns: table => new
                {
                    TillVaultSummaryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TillVaultId = table.Column<int>(nullable: true),
                    TransactionCount = table.Column<int>(nullable: true),
                    TotalAmountCurrency = table.Column<decimal>(nullable: true),
                    TransferAmount = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_tillvaultsummary", x => x.TillVaultSummaryId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_transactioncharge",
                columns: table => new
                {
                    TransactionChargeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    FixedOrPercentage = table.Column<string>(maxLength: 50, nullable: true),
                    Amount_Percentage = table.Column<decimal>(nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_transactioncharge", x => x.TransactionChargeId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_transactioncorrectionform",
                columns: table => new
                {
                    TransactionCorrectionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: true),
                    SubStructure = table.Column<int>(nullable: true),
                    QueryStartDate = table.Column<DateTime>(nullable: true),
                    QueryEndDate = table.Column<DateTime>(nullable: true),
                    Currency = table.Column<int>(nullable: true),
                    OpeningBalance = table.Column<decimal>(nullable: true),
                    ClosingBalance = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_transactioncorrectionform", x => x.TransactionCorrectionId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_transactioncorrectionsetup",
                columns: table => new
                {
                    TransactionCorrectionSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: true),
                    PresetChart = table.Column<bool>(nullable: true),
                    JobTitleId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_transactioncorrectionsetup", x => x.TransactionCorrectionSetupId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_transactiontax",
                columns: table => new
                {
                    TransactionTaxId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    FixedOrPercentage = table.Column<string>(maxLength: 50, nullable: true),
                    Amount_Percentage = table.Column<decimal>(nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_transactiontax", x => x.TransactionTaxId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_transferform",
                columns: table => new
                {
                    TransferFormId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: true),
                    Product = table.Column<int>(nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "date", nullable: true),
                    ValueDate = table.Column<DateTime>(type: "date", nullable: true),
                    ExternalReference = table.Column<string>(maxLength: 50, nullable: true),
                    TransactionReference = table.Column<string>(maxLength: 50, nullable: true),
                    PayingAccountNumber = table.Column<string>(maxLength: 50, nullable: true),
                    PayingAccountName = table.Column<string>(maxLength: 50, nullable: true),
                    PayingAccountCurrency = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    BeneficiaryAccountNumber = table.Column<string>(maxLength: 50, nullable: true),
                    BeneficiaryAccountName = table.Column<string>(maxLength: 50, nullable: true),
                    BeneficiaryAccountCurrency = table.Column<string>(maxLength: 50, nullable: true),
                    TransactionNarration = table.Column<string>(maxLength: 50, nullable: true),
                    ExchangeRate = table.Column<decimal>(nullable: true),
                    TotalCharge = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_transferform", x => x.TransferFormId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_transfersetup",
                columns: table => new
                {
                    TransferSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: true),
                    Product = table.Column<int>(nullable: true),
                    PresetChart = table.Column<bool>(nullable: true),
                    AccountType = table.Column<int>(nullable: true),
                    DailyWithdrawalLimit = table.Column<string>(maxLength: 50, nullable: true),
                    ChargesApplicable = table.Column<bool>(nullable: true),
                    Charges = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    ChargeType = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_transfersetup", x => x.TransferSetupId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_withdrawalform",
                columns: table => new
                {
                    WithdrawalFormId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: true),
                    Product = table.Column<int>(nullable: true),
                    TransactionReference = table.Column<string>(maxLength: 50, nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 50, nullable: true),
                    AccountType = table.Column<int>(nullable: true),
                    Currency = table.Column<int>(nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    TransactionDescription = table.Column<string>(maxLength: 50, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "date", nullable: true),
                    ValueDate = table.Column<DateTime>(type: "date", nullable: true),
                    WithdrawalType = table.Column<string>(maxLength: 50, nullable: true),
                    InstrumentNumber = table.Column<string>(maxLength: 50, nullable: true),
                    InstrumentDate = table.Column<DateTime>(type: "date", nullable: true),
                    ExchangeRate = table.Column<decimal>(nullable: true),
                    TotalCharge = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_withdrawalform", x => x.WithdrawalFormId);
                });

            migrationBuilder.CreateTable(
                name: "deposit_withdrawalsetup",
                columns: table => new
                {
                    WithdrawalSetupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Structure = table.Column<int>(nullable: true),
                    Product = table.Column<int>(nullable: true),
                    PresetChart = table.Column<bool>(nullable: true),
                    AccountType = table.Column<int>(nullable: true),
                    DailyWithdrawalLimit = table.Column<decimal>(nullable: true),
                    WithdrawalCharges = table.Column<bool>(nullable: true),
                    Charge = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    ChargeType = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_withdrawalsetup", x => x.WithdrawalSetupId);
                });

            migrationBuilder.CreateTable(
                name: "fin_customertransaction",
                columns: table => new
                {
                    CustomerTransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionCode = table.Column<string>(maxLength: 50, nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 50, nullable: true),
                    TransactionDate = table.Column<DateTime>(nullable: true),
                    ValueDate = table.Column<DateTime>(nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    DebitAmount = table.Column<decimal>(nullable: true),
                    CreditAmount = table.Column<decimal>(nullable: true),
                    AvailableBalance = table.Column<decimal>(nullable: true),
                    Beneficiary = table.Column<string>(maxLength: 50, nullable: true),
                    BatchNo = table.Column<string>(maxLength: 50, nullable: true),
                    TransactionType = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fin_customertransaction", x => x.CustomerTransactionId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_computed_forcasted_pd_lgd",
                columns: table => new
                {
                    ComputedPDId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: true),
                    PD_LGD = table.Column<double>(nullable: true),
                    PD = table.Column<double>(nullable: true),
                    Rundate = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(maxLength: 8, rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_computed_forcasted_pd_lgd", x => x.ComputedPDId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_forecasted_lgd",
                columns: table => new
                {
                    ForeCastedId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: true),
                    LGD1 = table.Column<decimal>(nullable: true),
                    LGD2 = table.Column<decimal>(nullable: true),
                    LGD3 = table.Column<decimal>(nullable: true),
                    LGD4 = table.Column<decimal>(nullable: true),
                    LGD5 = table.Column<decimal>(nullable: true),
                    LGD6 = table.Column<decimal>(nullable: true),
                    LGD7 = table.Column<decimal>(nullable: true),
                    LifeTimeLGD = table.Column<decimal>(nullable: true),
                    LGDType = table.Column<string>(maxLength: 50, nullable: true),
                    ApplicableLGD = table.Column<decimal>(nullable: true),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 50, nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 50, nullable: true),
                    RunDate = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_forecasted_lgd", x => x.ForeCastedId);
                });

            migrationBuilder.CreateTable(
                name: "Ifrs_forecasted_macroeconimcs_mapping",
                columns: table => new
                {
                    ForecastedMacroEconomicMappingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: true),
                    Position = table.Column<int>(nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Type = table.Column<int>(nullable: true),
                    Variable = table.Column<string>(maxLength: 50, nullable: true),
                    value = table.Column<double>(nullable: true),
                    Rundate = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(maxLength: 8, rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ifrs_forecasted_macroeconimcs_mapping", x => x.ForecastedMacroEconomicMappingId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_forecasted_pd",
                columns: table => new
                {
                    ForeCastedId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: true),
                    PD1 = table.Column<decimal>(nullable: true),
                    PD2 = table.Column<decimal>(nullable: true),
                    PD3 = table.Column<decimal>(nullable: true),
                    PD4 = table.Column<decimal>(nullable: true),
                    PD5 = table.Column<decimal>(nullable: true),
                    PD6 = table.Column<decimal>(nullable: true),
                    PD7 = table.Column<decimal>(nullable: true),
                    LifeTimePD = table.Column<decimal>(nullable: true),
                    PDType = table.Column<string>(maxLength: 50, nullable: true),
                    Stage = table.Column<string>(maxLength: 50, nullable: true),
                    ApplicablePD = table.Column<decimal>(nullable: true),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 50, nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 50, nullable: true),
                    RunDate = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_forecasted_pd", x => x.ForeCastedId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_historical_macroeconimcs_mapping",
                columns: table => new
                {
                    HistoricalMacroEconomicMappingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: true),
                    Variable = table.Column<string>(maxLength: 150, nullable: true),
                    Position = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: true),
                    Value = table.Column<double>(nullable: true),
                    Year = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(maxLength: 8, rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_historical_macroeconimcs_mapping", x => x.HistoricalMacroEconomicMappingId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_historical_product_pd",
                columns: table => new
                {
                    HistoricalProductPDId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: true),
                    Period = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    PD = table.Column<double>(nullable: true),
                    AvgPD = table.Column<double>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(maxLength: 8, rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_historical_product_pd", x => x.HistoricalProductPDId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_macroeconomic_variables",
                columns: table => new
                {
                    MacroEconomicVariableId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 150, nullable: true),
                    IsGeneric = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(maxLength: 8, rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_macroeconomic_variables", x => x.MacroEconomicVariableId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_macroeconomic_variables_year",
                columns: table => new
                {
                    MacroEconomicVariableId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: true),
                    GDP = table.Column<double>(nullable: true),
                    Unemployement = table.Column<double>(nullable: true),
                    Inflation = table.Column<double>(nullable: true),
                    erosion = table.Column<double>(nullable: true),
                    ForegnEx = table.Column<double>(nullable: true),
                    Others = table.Column<double>(nullable: true),
                    otherfactor = table.Column<double>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_macroeconomic_variables_year", x => x.MacroEconomicVariableId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_pit_formula",
                columns: table => new
                {
                    PitFormularId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanReferenceNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Equation = table.Column<string>(nullable: true),
                    ComputedPd = table.Column<double>(nullable: true),
                    Type = table.Column<int>(nullable: true),
                    Rundate = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    Updatedby = table.Column<string>(maxLength: 50, nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    Updatedon = table.Column<DateTime>(nullable: true),
                    Createdon = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_pit_formula", x => x.PitFormularId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_product_regressed_lgd",
                columns: table => new
                {
                    ProductRegressedLGDId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanReferenceNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: false),
                    AnnualLGD = table.Column<double>(nullable: false),
                    LifeTimeLGD = table.Column<double>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    RunDate = table.Column<DateTime>(nullable: true),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(maxLength: 8, rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_product_regressed_lgd", x => x.ProductRegressedLGDId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_product_regressed_pd",
                columns: table => new
                {
                    ProductRegressedPDId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanReferenceNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: false),
                    AnnualPD = table.Column<double>(nullable: false),
                    LifeTimePD = table.Column<double>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    RunDate = table.Column<DateTime>(nullable: true),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(maxLength: 8, rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_product_regressed_pd", x => x.ProductRegressedPDId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_regress_macro_variable",
                columns: table => new
                {
                    RegressMacroVariableId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PD = table.Column<double>(nullable: true),
                    Variable1 = table.Column<double>(nullable: true),
                    Variable2 = table.Column<double>(nullable: true),
                    Variable3 = table.Column<double>(nullable: true),
                    Variable4 = table.Column<double>(nullable: true),
                    Variable5 = table.Column<double>(nullable: true),
                    Variable6 = table.Column<double>(nullable: true),
                    Variable7 = table.Column<double>(nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: true),
                    Rundate = table.Column<DateTime>(nullable: true),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_regress_macro_variable", x => x.RegressMacroVariableId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_regress_macro_variable_lgd",
                columns: table => new
                {
                    RegressMacroVariableId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LGD = table.Column<double>(nullable: true),
                    Variable1 = table.Column<double>(nullable: true),
                    Variable2 = table.Column<double>(nullable: true),
                    Variable3 = table.Column<double>(nullable: true),
                    Variable4 = table.Column<double>(nullable: true),
                    Variable5 = table.Column<double>(nullable: true),
                    Variable6 = table.Column<double>(nullable: true),
                    Variable7 = table.Column<double>(nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 50, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: true),
                    Rundate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_regress_macro_variable_lgd", x => x.RegressMacroVariableId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_scenario_setup",
                columns: table => new
                {
                    ScenarioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Scenario = table.Column<string>(maxLength: 50, nullable: true),
                    Likelihood = table.Column<decimal>(nullable: true),
                    Rate = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_scenario_setup", x => x.ScenarioId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_setup_data",
                columns: table => new
                {
                    SetUpId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Threshold = table.Column<double>(nullable: true),
                    Deteroriation_Level = table.Column<int>(nullable: true),
                    Classification_Type = table.Column<int>(nullable: true),
                    Historical_PD_Year_Count = table.Column<int>(nullable: true),
                    PDBasis = table.Column<bool>(nullable: true),
                    Ltpdapproach = table.Column<int>(nullable: true),
                    CCF = table.Column<double>(nullable: true),
                    GroupBased = table.Column<string>(maxLength: 50, nullable: true),
                    RunDate = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_setup_data", x => x.SetUpId);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_temp_table1",
                columns: table => new
                {
                    id4 = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName4 = table.Column<string>(maxLength: 250, nullable: true),
                    LoanReferenceNumber4 = table.Column<string>(maxLength: 250, nullable: true),
                    ProductCode4 = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_temp_table1", x => x.id4);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_var_coeff_comp_lgd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Intercept = table.Column<double>(nullable: true),
                    coeff1 = table.Column<double>(nullable: true),
                    coeff2 = table.Column<double>(nullable: true),
                    coeff3 = table.Column<double>(nullable: true),
                    coeff4 = table.Column<double>(nullable: true),
                    coeff5 = table.Column<double>(nullable: true),
                    coeff6 = table.Column<double>(nullable: true),
                    coeff7 = table.Column<double>(nullable: true),
                    Macro_Var1 = table.Column<double>(nullable: true),
                    Macro_var2 = table.Column<double>(nullable: true),
                    Macro_var3 = table.Column<double>(nullable: true),
                    Macro_var4 = table.Column<double>(nullable: true),
                    Macro_var5 = table.Column<double>(nullable: true),
                    Macro_var6 = table.Column<double>(nullable: true),
                    Macro_var7 = table.Column<double>(nullable: true),
                    LGD_Computed = table.Column<double>(nullable: true),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 50, nullable: true),
                    RunDate = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_var_coeff_comp_lgd", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_var_coeff_comp_pd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Intercept = table.Column<double>(nullable: true),
                    coeff1 = table.Column<double>(nullable: true),
                    coeff2 = table.Column<double>(nullable: true),
                    coeff3 = table.Column<double>(nullable: true),
                    coeff4 = table.Column<double>(nullable: true),
                    coeff5 = table.Column<double>(nullable: true),
                    coeff6 = table.Column<double>(nullable: true),
                    coeff7 = table.Column<double>(nullable: true),
                    Macro_Var1 = table.Column<double>(nullable: true),
                    Macro_var2 = table.Column<double>(nullable: true),
                    Macro_var3 = table.Column<double>(nullable: true),
                    Macro_var4 = table.Column<double>(nullable: true),
                    Macro_var5 = table.Column<double>(nullable: true),
                    Macro_var6 = table.Column<double>(nullable: true),
                    Macro_var7 = table.Column<double>(nullable: true),
                    PD_Computed = table.Column<double>(nullable: true),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    LoanReferenceNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 50, nullable: true),
                    RunDate = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_var_coeff_comp_pd", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ifrs_xxxxx",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    y = table.Column<double>(nullable: true),
                    x1 = table.Column<double>(nullable: true),
                    x2 = table.Column<double>(nullable: true),
                    x3 = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifrs_xxxxx", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "inf_collection",
                columns: table => new
                {
                    CollectionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestorFundId = table.Column<int>(nullable: false),
                    InvestorFundCustomerId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    ProposedTenor = table.Column<int>(nullable: true),
                    ProposedRate = table.Column<decimal>(nullable: true),
                    FrequencyId = table.Column<int>(nullable: true),
                    Period = table.Column<int>(nullable: true),
                    ProposedAmount = table.Column<decimal>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    InvestmentPurpose = table.Column<string>(maxLength: 50, nullable: true),
                    CollectionDate = table.Column<DateTime>(nullable: true),
                    AmountPayable = table.Column<decimal>(nullable: true),
                    DrProductPrincipal = table.Column<int>(nullable: true),
                    CrReceiverPrincipalGL = table.Column<int>(nullable: true),
                    ApprovalStatus = table.Column<int>(nullable: true),
                    PaymentAccount = table.Column<string>(maxLength: 500, nullable: true),
                    Account = table.Column<string>(maxLength: 500, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    WorkflowToken = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_collection", x => x.CollectionId);
                });

            migrationBuilder.CreateTable(
                name: "inf_collection_website",
                columns: table => new
                {
                    WebsiteCollectionOperationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestorFundId = table.Column<int>(nullable: false),
                    InvestorFundCustomerId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    ProposedTenor = table.Column<int>(nullable: true),
                    ProposedRate = table.Column<decimal>(nullable: true),
                    FrequencyId = table.Column<int>(nullable: true),
                    Period = table.Column<int>(nullable: true),
                    ProposedAmount = table.Column<decimal>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    InvestmentPurpose = table.Column<string>(maxLength: 50, nullable: true),
                    CollectionDate = table.Column<DateTime>(nullable: true),
                    AmountPayable = table.Column<decimal>(nullable: true),
                    DrProductPrincipal = table.Column<int>(nullable: true),
                    CrReceiverPrincipalGL = table.Column<int>(nullable: true),
                    ApprovalStatus = table.Column<int>(nullable: true),
                    PaymentAccount = table.Column<string>(maxLength: 500, nullable: true),
                    Account = table.Column<string>(maxLength: 500, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    Updatedby = table.Column<string>(maxLength: 50, nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    Updatedon = table.Column<DateTime>(nullable: true),
                    Createdon = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_collection_website", x => x.WebsiteCollectionOperationId);
                });

            migrationBuilder.CreateTable(
                name: "inf_collectionrecommendationLog",
                columns: table => new
                {
                    CollectionRecommendationLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvInvestorFundId = table.Column<int>(nullable: false),
                    ApprovedProductId = table.Column<int>(nullable: false),
                    ApprovedTenor = table.Column<int>(nullable: false),
                    ApprovedRate = table.Column<decimal>(nullable: false),
                    ApprovedAmount = table.Column<decimal>(type: "money", nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_collectionrecommendationLog", x => x.CollectionRecommendationLogId);
                });

            migrationBuilder.CreateTable(
                name: "inf_customer",
                columns: table => new
                {
                    InvestorFundCustormerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerTypeId = table.Column<int>(nullable: true),
                    TitleId = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    DateofBirth = table.Column<DateTime>(nullable: true),
                    GenderId = table.Column<int>(nullable: true),
                    MiddleName = table.Column<string>(maxLength: 50, nullable: true),
                    MaritalStatusId = table.Column<int>(nullable: true),
                    CompanyStructureId = table.Column<int>(nullable: true),
                    Industry = table.Column<string>(maxLength: 50, nullable: true),
                    Size = table.Column<int>(nullable: true),
                    DateOfIncorporation = table.Column<DateTime>(nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    PostalAddress = table.Column<string>(maxLength: 50, nullable: true),
                    RelationshipOfficerId = table.Column<int>(nullable: true),
                    PoliticallyExposed = table.Column<bool>(nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    Updatedby = table.Column<string>(maxLength: 50, nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    Updatedon = table.Column<DateTime>(type: "date", nullable: true),
                    Createdon = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_customer", x => x.InvestorFundCustormerId);
                });

            migrationBuilder.CreateTable(
                name: "inf_daily_accural",
                columns: table => new
                {
                    InvestmentDailyAccuralId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceNumber = table.Column<string>(maxLength: 50, nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    TransactionTypeId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    BranchId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    ExchangeRate = table.Column<double>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    InterestRate = table.Column<double>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    DailyAccuralAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_daily_accural", x => x.InvestmentDailyAccuralId);
                });

            migrationBuilder.CreateTable(
                name: "inf_investdailyschedule",
                columns: table => new
                {
                    InvestmentDailyScheduleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Period = table.Column<int>(nullable: true),
                    OB = table.Column<decimal>(nullable: true),
                    InterestAmount = table.Column<decimal>(nullable: true),
                    CB = table.Column<decimal>(nullable: true),
                    Repayment = table.Column<decimal>(nullable: true),
                    PeriodDate = table.Column<DateTime>(nullable: true),
                    InvestorFundId = table.Column<int>(nullable: true),
                    PeriodId = table.Column<int>(nullable: true),
                    EndPeriod = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    Updatedby = table.Column<string>(maxLength: 50, nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    Updatedon = table.Column<DateTime>(type: "date", nullable: true),
                    Createdon = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_investdailyschedule", x => x.InvestmentDailyScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "inf_investdailyschedule_topup",
                columns: table => new
                {
                    InvestmentDailyScheduleTopUpId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Period = table.Column<int>(nullable: true),
                    OB = table.Column<decimal>(nullable: true),
                    InterestAmount = table.Column<decimal>(nullable: true),
                    CB = table.Column<decimal>(nullable: true),
                    Repayment = table.Column<decimal>(nullable: true),
                    PeriodDate = table.Column<DateTime>(nullable: true),
                    InvestorFundId = table.Column<int>(nullable: false),
                    ReferenceNo = table.Column<string>(nullable: true),
                    PeriodId = table.Column<int>(nullable: true),
                    EndPeriod = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    Updatedby = table.Column<string>(maxLength: 50, nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    Updatedon = table.Column<DateTime>(nullable: true),
                    Createdon = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_investdailyschedule_topup", x => x.InvestmentDailyScheduleTopUpId);
                });

            migrationBuilder.CreateTable(
                name: "inf_investmentperiodicschedule",
                columns: table => new
                {
                    InvestmentPeriodicScheduleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Period = table.Column<int>(nullable: true),
                    OB = table.Column<decimal>(nullable: true),
                    InterestAmount = table.Column<decimal>(nullable: true),
                    CB = table.Column<decimal>(nullable: true),
                    PeriodDate = table.Column<DateTime>(nullable: true),
                    Repayment = table.Column<decimal>(nullable: true),
                    InvestorFundId = table.Column<int>(nullable: true),
                    FrequencyType = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_investmentperiodicschedule", x => x.InvestmentPeriodicScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "inf_investmentrecommendationlog",
                columns: table => new
                {
                    InvestmentRecommendationLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestorFundId = table.Column<int>(nullable: false),
                    ApprovedProductId = table.Column<int>(nullable: false),
                    ApprovedTenor = table.Column<int>(nullable: false),
                    ApprovedRate = table.Column<decimal>(nullable: false),
                    ApprovedAmount = table.Column<decimal>(type: "money", nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_investmentrecommendationlog", x => x.InvestmentRecommendationLogId);
                });

            migrationBuilder.CreateTable(
                name: "inf_investorfund_rollover_website",
                columns: table => new
                {
                    InvestorFundIdWebsiteRolloverId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestorFundId = table.Column<int>(nullable: false),
                    ApprovedTenor = table.Column<decimal>(nullable: true),
                    RollOverAmount = table.Column<decimal>(nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    ApprovalStatus = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_investorfund_rollover_website", x => x.InvestorFundIdWebsiteRolloverId);
                });

            migrationBuilder.CreateTable(
                name: "inf_investorfund_topup_website",
                columns: table => new
                {
                    InvestorFundIdWebsiteTopupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestorFundId = table.Column<int>(nullable: false),
                    TopUpAmount = table.Column<decimal>(nullable: true),
                    ApprovalStatus = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_investorfund_topup_website", x => x.InvestorFundIdWebsiteTopupId);
                });

            migrationBuilder.CreateTable(
                name: "inf_investorfund_website",
                columns: table => new
                {
                    WebsiteInvestorFundId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestorFundCustomerId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    ProposedTenor = table.Column<decimal>(nullable: true),
                    ProposedRate = table.Column<decimal>(nullable: true),
                    FrequencyId = table.Column<int>(nullable: true),
                    Period = table.Column<string>(maxLength: 50, nullable: true),
                    ProposedAmount = table.Column<decimal>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    InvestmentPurpose = table.Column<string>(maxLength: 50, nullable: true),
                    EnableRollOver = table.Column<bool>(nullable: true),
                    InstrumentId = table.Column<int>(nullable: true),
                    InstrumentNumer = table.Column<string>(maxLength: 50, nullable: true),
                    InstrumentDate = table.Column<DateTime>(nullable: true),
                    CustomerNameId = table.Column<int>(nullable: true),
                    ProductName = table.Column<string>(maxLength: 50, nullable: true),
                    ApprovedTenor = table.Column<decimal>(nullable: true),
                    ApprovedRate = table.Column<decimal>(nullable: true),
                    ApprovedProductId = table.Column<int>(nullable: true),
                    FirstPrincipalDate = table.Column<DateTime>(nullable: true),
                    MaturityDate = table.Column<DateTime>(nullable: true),
                    ApprovedAmount = table.Column<decimal>(nullable: true),
                    ExpectedPayout = table.Column<decimal>(nullable: true),
                    ExpectedInterest = table.Column<decimal>(nullable: true),
                    ApprovalStatus = table.Column<int>(nullable: true),
                    InvestmentStatus = table.Column<int>(nullable: true),
                    GenerateCertificate = table.Column<bool>(nullable: true),
                    RefNumber = table.Column<string>(maxLength: 10, nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    Updatedby = table.Column<string>(maxLength: 50, nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    Updatedon = table.Column<DateTime>(type: "date", nullable: true),
                    Createdon = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_investorfund_website", x => x.WebsiteInvestorFundId);
                });

            migrationBuilder.CreateTable(
                name: "inf_liquidation",
                columns: table => new
                {
                    LiquidationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestorFundId = table.Column<int>(nullable: false),
                    InvestorFundCustomerId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    ProposedTenor = table.Column<int>(nullable: true),
                    ProposedRate = table.Column<decimal>(nullable: true),
                    FrequencyId = table.Column<int>(nullable: true),
                    Period = table.Column<int>(nullable: true),
                    ProposedAmount = table.Column<decimal>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    InvestmentPurpose = table.Column<string>(maxLength: 50, nullable: true),
                    LiquidationDate = table.Column<DateTime>(nullable: true),
                    EarlyTerminationCharge = table.Column<decimal>(nullable: true),
                    AmountPayable = table.Column<decimal>(nullable: true),
                    DrProductPrincipal = table.Column<int>(nullable: true),
                    CrIntExpense = table.Column<int>(nullable: true),
                    DrIntPayable = table.Column<int>(nullable: true),
                    CrReceiverPrincipalGL = table.Column<int>(nullable: true),
                    ApprovalStatus = table.Column<int>(nullable: true),
                    PaymentAccount = table.Column<string>(maxLength: 500, nullable: true),
                    Account = table.Column<string>(maxLength: 500, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: true),
                    WorkflowToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_liquidation", x => x.LiquidationId);
                });

            migrationBuilder.CreateTable(
                name: "inf_liquidation_website",
                columns: table => new
                {
                    WebsiteLiquidationOperationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestorFundId = table.Column<int>(nullable: false),
                    InvestorFundCustomerId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    ProposedTenor = table.Column<int>(nullable: true),
                    ProposedRate = table.Column<decimal>(nullable: true),
                    FrequencyId = table.Column<int>(nullable: true),
                    Period = table.Column<int>(nullable: true),
                    ProposedAmount = table.Column<decimal>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    InvestmentPurpose = table.Column<string>(maxLength: 50, nullable: true),
                    LiquidationDate = table.Column<DateTime>(nullable: true),
                    EarlyTerminationCharge = table.Column<decimal>(nullable: true),
                    AmountPayable = table.Column<decimal>(nullable: true),
                    DrProductPrincipal = table.Column<int>(nullable: true),
                    CrIntExpense = table.Column<int>(nullable: true),
                    DrIntPayable = table.Column<int>(nullable: true),
                    CrReceiverPrincipalGL = table.Column<int>(nullable: true),
                    ApprovalStatus = table.Column<int>(nullable: true),
                    PaymentAccount = table.Column<string>(maxLength: 500, nullable: true),
                    Account = table.Column<string>(maxLength: 500, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    Updatedby = table.Column<string>(maxLength: 50, nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    Updatedon = table.Column<DateTime>(nullable: true),
                    Createdon = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_liquidation_website", x => x.WebsiteLiquidationOperationId);
                });

            migrationBuilder.CreateTable(
                name: "inf_liquidationrecommendationlog",
                columns: table => new
                {
                    LiquidationRecommendationLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvInvestorFundId = table.Column<int>(nullable: false),
                    ApprovedProductId = table.Column<int>(nullable: false),
                    ApprovedTenor = table.Column<int>(nullable: false),
                    ApprovedRate = table.Column<decimal>(nullable: false),
                    ApprovedAmount = table.Column<decimal>(type: "money", nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_liquidationrecommendationlog", x => x.LiquidationRecommendationLogId);
                });

            migrationBuilder.CreateTable(
                name: "inf_product",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: true),
                    ProductName = table.Column<string>(maxLength: 50, nullable: true),
                    Rate = table.Column<decimal>(nullable: true),
                    ProductTypeId = table.Column<int>(nullable: true),
                    ProductLimit = table.Column<int>(nullable: true),
                    InterestRateMax = table.Column<decimal>(nullable: true),
                    InterestRepaymentTypeId = table.Column<int>(nullable: true),
                    ScheduleMethodId = table.Column<int>(nullable: true),
                    FrequencyId = table.Column<int>(nullable: true),
                    MaximumPeriod = table.Column<decimal>(nullable: true),
                    InterestRateAnnual = table.Column<decimal>(nullable: true),
                    InterestRateFrequency = table.Column<decimal>(nullable: true),
                    ProductPrincipalGl = table.Column<int>(nullable: true),
                    ReceiverPrincipalGl = table.Column<int>(nullable: true),
                    InterstExpenseGl = table.Column<int>(nullable: true),
                    InterestPayableGl = table.Column<int>(nullable: true),
                    ProductLimitId = table.Column<int>(nullable: true),
                    EarlyTerminationCharge = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    Updatedby = table.Column<string>(maxLength: 50, nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    Updatedon = table.Column<DateTime>(nullable: true),
                    Createdon = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_product", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "inf_producttype",
                columns: table => new
                {
                    ProductTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    Updatedby = table.Column<string>(maxLength: 50, nullable: true),
                    Createdby = table.Column<string>(maxLength: 50, nullable: true),
                    Updatedon = table.Column<DateTime>(nullable: true),
                    Createdon = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_producttype", x => x.ProductTypeId);
                });

            migrationBuilder.CreateTable(
                name: "OTPTracker",
                columns: table => new
                {
                    OTPId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    OTP = table.Column<string>(nullable: true),
                    DateIssued = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTPTracker", x => x.OTPId);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Token = table.Column<string>(nullable: false),
                    JwtId = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    Used = table.Column<bool>(nullable: false),
                    Invalidated = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Token);
                });

            migrationBuilder.CreateTable(
                name: "tmp_loanapplicationscheduleperiodic",
                columns: table => new
                {
                    LoanSchedulePeriodicId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanApplicationId = table.Column<int>(nullable: false),
                    PaymentNumber = table.Column<int>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    StartPrincipalAmount = table.Column<decimal>(nullable: false),
                    PeriodPaymentAmount = table.Column<decimal>(nullable: false),
                    PeriodInterestAmount = table.Column<decimal>(nullable: false),
                    PeriodPrincipalAmount = table.Column<decimal>(nullable: false),
                    ClosingBalance = table.Column<decimal>(nullable: false),
                    EndPrincipalAmount = table.Column<decimal>(nullable: false),
                    InterestRate = table.Column<double>(nullable: false),
                    AmortisedStartPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedPeriodPaymentAmount = table.Column<decimal>(nullable: false),
                    AmortisedPeriodInterestAmount = table.Column<decimal>(nullable: false),
                    AmortisedPeriodPrincipalAmount = table.Column<decimal>(nullable: false),
                    AmortisedClosingBalance = table.Column<decimal>(nullable: false),
                    AmortisedEndPrincipalAmount = table.Column<decimal>(nullable: false),
                    EffectiveInterestRate = table.Column<double>(nullable: false),
                    StaffId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tmp_loanapplicationscheduleperiodic", x => x.LoanSchedulePeriodicId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "credit_loancustomer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerTypeId = table.Column<int>(nullable: false),
                    TitleId = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: true),
                    MiddleName = table.Column<string>(maxLength: 100, nullable: true),
                    GenderId = table.Column<int>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    Signature = table.Column<byte[]>(nullable: true),
                    RegistrationSource = table.Column<string>(maxLength: 50, nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    Address = table.Column<string>(maxLength: 550, nullable: false),
                    PostaAddress = table.Column<string>(maxLength: 550, nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    Occupation = table.Column<string>(maxLength: 150, nullable: true),
                    EmploymentType = table.Column<int>(nullable: true),
                    PoliticallyExposed = table.Column<bool>(nullable: true),
                    CompanyName = table.Column<string>(maxLength: 250, nullable: true),
                    CompanyWebsite = table.Column<string>(maxLength: 250, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNo = table.Column<string>(maxLength: 50, nullable: false),
                    RegistrationNo = table.Column<string>(maxLength: 50, nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    Industry = table.Column<string>(maxLength: 250, nullable: true),
                    IncorporationCountry = table.Column<string>(maxLength: 250, nullable: true),
                    AnnualTurnover = table.Column<decimal>(nullable: true),
                    ShareholderFund = table.Column<decimal>(nullable: true),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    MaritalStatusId = table.Column<int>(nullable: true),
                    CASAAccountNumber = table.Column<string>(maxLength: 50, nullable: true),
                    RelationshipManagerId = table.Column<int>(nullable: true),
                    CompanyStructureId = table.Column<int>(nullable: true),
                    Size = table.Column<int>(nullable: true),
                    ProfileStatus = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UserIdentity = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loancustomer", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_credit_loancustomer_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_loancustomerfscaption",
                columns: table => new
                {
                    FSCaptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FSCaptionName = table.Column<string>(maxLength: 1000, nullable: false),
                    FSCaptionGroupId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_loancustomerfscaptiongroupFSCaptionGroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loancustomerfscaption", x => x.FSCaptionId);
                    table.ForeignKey(
                        name: "FK_credit_loancustomerfscaption_credit_loancustomerfscaptiongroup_credit_loancustomerfscaptiongroupFSCaptionGroupId",
                        column: x => x.credit_loancustomerfscaptiongroupFSCaptionGroupId,
                        principalTable: "credit_loancustomerfscaptiongroup",
                        principalColumn: "FSCaptionGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanreviewoperationirregularinput",
                columns: table => new
                {
                    IrregularScheduleInputId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanReviewOperationId = table.Column<int>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    PaymentAmount = table.Column<decimal>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_loanreviewoperationLoanReviewOperationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanreviewoperationirregularinput", x => x.IrregularScheduleInputId);
                    table.ForeignKey(
                        name: "FK_credit_loanreviewoperationirregularinput_credit_loanreviewoperation_credit_loanreviewoperationLoanReviewOperationId",
                        column: x => x.credit_loanreviewoperationLoanReviewOperationId,
                        principalTable: "credit_loanreviewoperation",
                        principalColumn: "LoanReviewOperationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanscheduletype",
                columns: table => new
                {
                    LoanScheduleTypeId = table.Column<int>(nullable: false),
                    LoanScheduleTypeName = table.Column<string>(maxLength: 250, nullable: false),
                    LoanScheduleCategoryId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_loanschedulecategoryLoanScheduleCategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanscheduletype", x => x.LoanScheduleTypeId);
                    table.ForeignKey(
                        name: "FK_credit_loanscheduletype_credit_loanschedulecategory_credit_loanschedulecategoryLoanScheduleCategoryId",
                        column: x => x.credit_loanschedulecategoryLoanScheduleCategoryId,
                        principalTable: "credit_loanschedulecategory",
                        principalColumn: "LoanScheduleCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_product",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCode = table.Column<string>(maxLength: 50, nullable: false),
                    ProductName = table.Column<string>(maxLength: 250, nullable: false),
                    PaymentType = table.Column<int>(nullable: false),
                    CollateralRequired = table.Column<bool>(nullable: false),
                    QuotedInstrument = table.Column<bool>(nullable: false),
                    Rate = table.Column<double>(nullable: false),
                    LateRepayment = table.Column<bool>(nullable: false),
                    Period = table.Column<int>(nullable: true),
                    CleanUpCircle = table.Column<int>(nullable: true),
                    WeightedMaxScore = table.Column<decimal>(nullable: true),
                    Default = table.Column<int>(nullable: true),
                    DefaultRange = table.Column<int>(nullable: true),
                    Significant2 = table.Column<decimal>(nullable: true),
                    Significant3 = table.Column<decimal>(nullable: true),
                    ProductTypeId = table.Column<int>(nullable: true),
                    PrincipalGL = table.Column<int>(nullable: true),
                    InterestIncomeExpenseGL = table.Column<int>(nullable: true),
                    InterestReceivablePayableGL = table.Column<int>(nullable: true),
                    FrequencyTypeId = table.Column<int>(nullable: true),
                    TenorInDays = table.Column<int>(nullable: true),
                    ScheduleTypeId = table.Column<int>(nullable: true),
                    CollateralPercentage = table.Column<decimal>(nullable: true),
                    ProductLimit = table.Column<decimal>(nullable: true),
                    LateTerminationCharge = table.Column<double>(nullable: true),
                    EarlyTerminationCharge = table.Column<double>(nullable: true),
                    LowRiskDefinition = table.Column<double>(nullable: true),
                    FeeIncomeGL = table.Column<int>(nullable: true),
                    InterestType = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_repaymenttypeRepaymentTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_credit_product_credit_repaymenttype_credit_repaymenttypeRepaymentTypeId",
                        column: x => x.credit_repaymenttypeRepaymentTypeId,
                        principalTable: "credit_repaymenttype",
                        principalColumn: "RepaymentTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "deposit_customercontactpersons",
                columns: table => new
                {
                    ContactPersonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    SurName = table.Column<string>(maxLength: 50, nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    OtherName = table.Column<string>(maxLength: 50, nullable: true),
                    Relationship = table.Column<string>(maxLength: 50, nullable: true),
                    GenderId = table.Column<int>(nullable: true),
                    MobileNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    deposit_accountopeningCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_customercontactpersons", x => x.ContactPersonId);
                    table.ForeignKey(
                        name: "FK_deposit_customercontactpersons_deposit_accountopening_deposit_accountopeningCustomerId",
                        column: x => x.deposit_accountopeningCustomerId,
                        principalTable: "deposit_accountopening",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "deposit_customerdirectors",
                columns: table => new
                {
                    DirectorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    TitleId = table.Column<int>(nullable: true),
                    GenderId = table.Column<int>(nullable: true),
                    MaritalStatusId = table.Column<int>(nullable: true),
                    Surname = table.Column<string>(maxLength: 50, nullable: true),
                    Firstname = table.Column<string>(maxLength: 50, nullable: true),
                    Othername = table.Column<string>(maxLength: 50, nullable: true),
                    IdentificationType = table.Column<string>(maxLength: 50, nullable: true),
                    IdentificationNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Telephone = table.Column<string>(maxLength: 50, nullable: true),
                    Mobile = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    SignatureUpload = table.Column<byte[]>(nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    DoB = table.Column<DateTime>(type: "date", nullable: true),
                    PlaceOfBirth = table.Column<string>(maxLength: 50, nullable: true),
                    MaidenName = table.Column<string>(maxLength: 50, nullable: true),
                    NextofKin = table.Column<string>(maxLength: 50, nullable: true),
                    LGA = table.Column<int>(nullable: true),
                    StateOfOrigin = table.Column<int>(nullable: true),
                    TaxIDNumber = table.Column<string>(maxLength: 50, nullable: true),
                    BVN = table.Column<int>(nullable: true),
                    MeansOfID = table.Column<string>(maxLength: 50, nullable: false),
                    IDExpiryDate = table.Column<DateTime>(type: "date", nullable: true),
                    IDIssueDate = table.Column<DateTime>(type: "date", nullable: true),
                    Occupation = table.Column<string>(maxLength: 50, nullable: true),
                    JobTitle = table.Column<string>(maxLength: 50, nullable: true),
                    Position = table.Column<string>(maxLength: 50, nullable: true),
                    Nationality = table.Column<int>(nullable: true),
                    ResidentOfCountry = table.Column<bool>(nullable: true),
                    ResidentPermit = table.Column<string>(maxLength: 50, nullable: true),
                    PermitIssueDate = table.Column<DateTime>(type: "date", nullable: true),
                    PermitExpiryDate = table.Column<DateTime>(type: "date", nullable: true),
                    SocialSecurityNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Address1 = table.Column<string>(maxLength: 50, nullable: true),
                    City1 = table.Column<string>(maxLength: 50, nullable: true),
                    State1 = table.Column<string>(maxLength: 50, nullable: true),
                    Country1 = table.Column<string>(maxLength: 50, nullable: true),
                    Address2 = table.Column<string>(maxLength: 50, nullable: true),
                    City2 = table.Column<string>(maxLength: 50, nullable: true),
                    State2 = table.Column<string>(maxLength: 50, nullable: true),
                    Country2 = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    deposit_accountopeningCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_customerdirectors", x => x.DirectorId);
                    table.ForeignKey(
                        name: "FK_deposit_customerdirectors_deposit_accountopening_deposit_accountopeningCustomerId",
                        column: x => x.deposit_accountopeningCustomerId,
                        principalTable: "deposit_accountopening",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "deposit_customeridentification",
                columns: table => new
                {
                    CustomerIdentityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    MeansOfID = table.Column<int>(nullable: false),
                    IDNumber = table.Column<string>(maxLength: 50, nullable: true),
                    DateIssued = table.Column<DateTime>(type: "date", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "date", nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    deposit_accountopeningCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_customeridentification", x => x.CustomerIdentityId);
                    table.ForeignKey(
                        name: "FK_deposit_customeridentification_deposit_accountopening_deposit_accountopeningCustomerId",
                        column: x => x.deposit_accountopeningCustomerId,
                        principalTable: "deposit_accountopening",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "deposit_customerkyc",
                columns: table => new
                {
                    KycId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    Financiallydisadvantaged = table.Column<bool>(nullable: true),
                    Bankpolicydocuments = table.Column<string>(maxLength: 500, nullable: true),
                    TieredKycrequirement = table.Column<bool>(nullable: true),
                    RiskCategoryId = table.Column<int>(nullable: true),
                    PoliticallyExposedPerson = table.Column<bool>(nullable: true),
                    Details = table.Column<string>(maxLength: 500, nullable: true),
                    AddressVisited = table.Column<string>(maxLength: 500, nullable: true),
                    CommentOnLocation = table.Column<string>(maxLength: 500, nullable: true),
                    LocationColor = table.Column<string>(maxLength: 50, nullable: true),
                    LocationDescription = table.Column<string>(maxLength: 50, nullable: true),
                    NameOfVisitingStaff = table.Column<string>(maxLength: 50, nullable: true),
                    DateOfVisitation = table.Column<DateTime>(type: "date", nullable: true),
                    UtilityBillSubmitted = table.Column<bool>(nullable: true),
                    AccountOpeningCompleted = table.Column<bool>(nullable: true),
                    RecentPassportPhoto = table.Column<bool>(nullable: true),
                    ConfirmationName = table.Column<string>(maxLength: 50, nullable: true),
                    ConfirmationDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeferralFullName = table.Column<string>(maxLength: 50, nullable: true),
                    DeferralDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeferralApproved = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    deposit_accountopeningCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_customerkyc", x => x.KycId);
                    table.ForeignKey(
                        name: "FK_deposit_customerkyc_deposit_accountopening_deposit_accountopeningCustomerId",
                        column: x => x.deposit_accountopeningCustomerId,
                        principalTable: "deposit_accountopening",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "deposit_customernextofkin",
                columns: table => new
                {
                    NextOfKinId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    Surname = table.Column<string>(maxLength: 50, nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    OtherName = table.Column<string>(maxLength: 50, nullable: true),
                    DOB = table.Column<DateTime>(type: "date", nullable: true),
                    GenderId = table.Column<int>(nullable: true),
                    Relationship = table.Column<string>(maxLength: 50, nullable: true),
                    MobileNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 50, nullable: true),
                    State = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    deposit_accountopeningCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_customernextofkin", x => x.NextOfKinId);
                    table.ForeignKey(
                        name: "FK_deposit_customernextofkin_deposit_accountopening_deposit_accountopeningCustomerId",
                        column: x => x.deposit_accountopeningCustomerId,
                        principalTable: "deposit_accountopening",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "deposit_customersignatory",
                columns: table => new
                {
                    SignatoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    TitleId = table.Column<int>(nullable: true),
                    GenderId = table.Column<int>(nullable: true),
                    MaritalStatusId = table.Column<int>(nullable: true),
                    Surname = table.Column<string>(maxLength: 50, nullable: true),
                    Firstname = table.Column<string>(maxLength: 50, nullable: true),
                    Othername = table.Column<string>(maxLength: 50, nullable: true),
                    ClassofSignatory = table.Column<string>(maxLength: 50, nullable: true),
                    IdentificationType = table.Column<string>(maxLength: 50, nullable: true),
                    IdentificationNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Telephone = table.Column<string>(maxLength: 50, nullable: true),
                    Mobile = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    SignatureUpload = table.Column<byte[]>(nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    DoB = table.Column<DateTime>(type: "date", nullable: true),
                    PlaceOfBirth = table.Column<string>(maxLength: 50, nullable: true),
                    MaidenName = table.Column<string>(maxLength: 50, nullable: true),
                    NextofKin = table.Column<string>(maxLength: 50, nullable: true),
                    LGA = table.Column<int>(nullable: true),
                    StateOfOrigin = table.Column<int>(nullable: true),
                    TaxIDNumber = table.Column<string>(maxLength: 50, nullable: true),
                    MeansOfID = table.Column<string>(maxLength: 50, nullable: true),
                    IDExpiryDate = table.Column<DateTime>(type: "date", nullable: true),
                    IDIssueDate = table.Column<DateTime>(type: "date", nullable: true),
                    Occupation = table.Column<string>(maxLength: 50, nullable: true),
                    JobTitle = table.Column<string>(maxLength: 50, nullable: true),
                    Position = table.Column<string>(maxLength: 50, nullable: true),
                    Nationality = table.Column<int>(nullable: true),
                    ResidentPermit = table.Column<string>(maxLength: 50, nullable: true),
                    PermitIssueDate = table.Column<DateTime>(type: "date", nullable: true),
                    PermitExpiryDate = table.Column<DateTime>(type: "date", nullable: true),
                    SocialSecurityNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Address1 = table.Column<string>(maxLength: 50, nullable: true),
                    City1 = table.Column<string>(maxLength: 50, nullable: true),
                    State1 = table.Column<string>(maxLength: 50, nullable: true),
                    Country1 = table.Column<string>(maxLength: 50, nullable: true),
                    Address2 = table.Column<string>(maxLength: 50, nullable: true),
                    City2 = table.Column<string>(maxLength: 50, nullable: true),
                    State2 = table.Column<string>(maxLength: 50, nullable: true),
                    Country2 = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    deposit_accountopeningCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_customersignatory", x => x.SignatoryId);
                    table.ForeignKey(
                        name: "FK_deposit_customersignatory_deposit_accountopening_deposit_accountopeningCustomerId",
                        column: x => x.deposit_accountopeningCustomerId,
                        principalTable: "deposit_accountopening",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "deposit_accountsetup",
                columns: table => new
                {
                    DepositAccountId = table.Column<int>(nullable: false),
                    AccountName = table.Column<string>(maxLength: 500, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    AccountTypeId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: true),
                    DormancyDays = table.Column<string>(maxLength: 50, nullable: true),
                    InitialDeposit = table.Column<decimal>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    BusinessCategoryId = table.Column<int>(nullable: true),
                    GLMapping = table.Column<int>(nullable: true),
                    BankGl = table.Column<int>(nullable: true),
                    InterestRate = table.Column<decimal>(nullable: true),
                    InterestType = table.Column<string>(maxLength: 50, nullable: true),
                    CheckCollecting = table.Column<bool>(nullable: true),
                    MaturityType = table.Column<string>(maxLength: 50, nullable: true),
                    PreTerminationLiquidationCharge = table.Column<bool>(nullable: true),
                    InterestAccrual = table.Column<int>(nullable: true),
                    Status = table.Column<bool>(nullable: true),
                    OperatedByAnother = table.Column<bool>(nullable: true),
                    CanNominateBenefactor = table.Column<bool>(nullable: true),
                    UsePresetChartofAccount = table.Column<bool>(nullable: true),
                    TransactionPrefix = table.Column<string>(maxLength: 50, nullable: true),
                    CancelPrefix = table.Column<string>(maxLength: 50, nullable: true),
                    RefundPrefix = table.Column<string>(maxLength: 50, nullable: true),
                    Useworkflow = table.Column<bool>(nullable: true),
                    CanPlaceOnLien = table.Column<bool>(nullable: true),
                    InUse = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    deposit_accountypeAccountTypeId = table.Column<int>(nullable: true),
                    deposit_categoryCategoryId = table.Column<int>(nullable: true),
                    deposit_transactionchargeTransactionChargeId = table.Column<int>(nullable: true),
                    deposit_transactiontaxTransactionTaxId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_accountsetup", x => x.DepositAccountId);
                    table.ForeignKey(
                        name: "FK_deposit_accountsetup_deposit_accountype_deposit_accountypeAccountTypeId",
                        column: x => x.deposit_accountypeAccountTypeId,
                        principalTable: "deposit_accountype",
                        principalColumn: "AccountTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_deposit_accountsetup_deposit_category_deposit_categoryCategoryId",
                        column: x => x.deposit_categoryCategoryId,
                        principalTable: "deposit_category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_deposit_accountsetup_deposit_transactioncharge_deposit_transactionchargeTransactionChargeId",
                        column: x => x.deposit_transactionchargeTransactionChargeId,
                        principalTable: "deposit_transactioncharge",
                        principalColumn: "TransactionChargeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_deposit_accountsetup_deposit_transactiontax_deposit_transactiontaxTransactionTaxId",
                        column: x => x.deposit_transactiontaxTransactionTaxId,
                        principalTable: "deposit_transactiontax",
                        principalColumn: "TransactionTaxId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_collateralcustomer",
                columns: table => new
                {
                    CollateralCustomerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    CollateralTypeId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    CollateralValue = table.Column<decimal>(nullable: false),
                    Location = table.Column<string>(maxLength: 250, nullable: false),
                    CollateralCode = table.Column<string>(maxLength: 50, nullable: false),
                    CollateralVerificationStatus = table.Column<bool>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_collateraltypeCollateralTypeId = table.Column<int>(nullable: true),
                    credit_loancustomerCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_collateralcustomer", x => x.CollateralCustomerId);
                    table.ForeignKey(
                        name: "FK_credit_collateralcustomer_credit_collateraltype_credit_collateraltypeCollateralTypeId",
                        column: x => x.credit_collateraltypeCollateralTypeId,
                        principalTable: "credit_collateraltype",
                        principalColumn: "CollateralTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_collateralcustomer_credit_loancustomer_credit_loancustomerCustomerId",
                        column: x => x.credit_loancustomerCustomerId,
                        principalTable: "credit_loancustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_customerbankdetails",
                columns: table => new
                {
                    CustomerBankDetailsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    BVN = table.Column<string>(maxLength: 250, nullable: false),
                    Account = table.Column<string>(maxLength: 50, nullable: false),
                    Bank = table.Column<string>(maxLength: 550, nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_loancustomerCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_customerbankdetails", x => x.CustomerBankDetailsId);
                    table.ForeignKey(
                        name: "FK_credit_customerbankdetails_credit_loancustomer_credit_loancustomerCustomerId",
                        column: x => x.credit_loancustomerCustomerId,
                        principalTable: "credit_loancustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_customeridentitydetails",
                columns: table => new
                {
                    CustomerIdentityDetailsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    IdentificationId = table.Column<int>(nullable: false),
                    Number = table.Column<string>(maxLength: 250, nullable: false),
                    Issuer = table.Column<string>(maxLength: 50, nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_loancustomerCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_customeridentitydetails", x => x.CustomerIdentityDetailsId);
                    table.ForeignKey(
                        name: "FK_credit_customeridentitydetails_credit_loancustomer_credit_loancustomerCustomerId",
                        column: x => x.credit_loancustomerCustomerId,
                        principalTable: "credit_loancustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_customernextofkin",
                columns: table => new
                {
                    CustomerNextOfKinId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Relationship = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 550, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNo = table.Column<string>(maxLength: 50, nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_loancustomerCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_customernextofkin", x => x.CustomerNextOfKinId);
                    table.ForeignKey(
                        name: "FK_credit_customernextofkin_credit_loancustomer_credit_loancustomerCustomerId",
                        column: x => x.credit_loancustomerCustomerId,
                        principalTable: "credit_loancustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_directorshareholder",
                columns: table => new
                {
                    DirectorShareHolderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(maxLength: 250, nullable: false),
                    PercentageHolder = table.Column<double>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_loancustomerCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_directorshareholder", x => x.DirectorShareHolderId);
                    table.ForeignKey(
                        name: "FK_credit_directorshareholder_credit_loancustomer_credit_loancustomerCustomerId",
                        column: x => x.credit_loancustomerCustomerId,
                        principalTable: "credit_loancustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_loancustomerdirector",
                columns: table => new
                {
                    CustomerDirectorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DirectorTypeId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Position = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 550, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNo = table.Column<string>(maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Signature = table.Column<byte[]>(type: "image", nullable: true),
                    PoliticallyPosition = table.Column<bool>(nullable: false),
                    PercentageShare = table.Column<decimal>(nullable: true),
                    RelativePoliticallyPosition = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_directortypeDirectorTypeId = table.Column<int>(nullable: true),
                    credit_loancustomerCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loancustomerdirector", x => x.CustomerDirectorId);
                    table.ForeignKey(
                        name: "FK_credit_loancustomerdirector_credit_directortype_credit_directortypeDirectorTypeId",
                        column: x => x.credit_directortypeDirectorTypeId,
                        principalTable: "credit_directortype",
                        principalColumn: "DirectorTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_loancustomerdirector_credit_loancustomer_credit_loancustomerCustomerId",
                        column: x => x.credit_loancustomerCustomerId,
                        principalTable: "credit_loancustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_loancustomerdocument",
                columns: table => new
                {
                    CustomerDocumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    PhysicalLocation = table.Column<string>(maxLength: 500, nullable: true),
                    DocumentName = table.Column<string>(maxLength: 255, nullable: false),
                    DocumentExtension = table.Column<string>(maxLength: 50, nullable: false),
                    DocumentFile = table.Column<byte[]>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_documenttypeDocumentTypeId = table.Column<int>(nullable: true),
                    credit_loancustomerCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loancustomerdocument", x => x.CustomerDocumentId);
                    table.ForeignKey(
                        name: "FK_credit_loancustomerdocument_credit_documenttype_credit_documenttypeDocumentTypeId",
                        column: x => x.credit_documenttypeDocumentTypeId,
                        principalTable: "credit_documenttype",
                        principalColumn: "DocumentTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_loancustomerdocument_credit_loancustomer_credit_loancustomerCustomerId",
                        column: x => x.credit_loancustomerCustomerId,
                        principalTable: "credit_loancustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "inf_investorfund",
                columns: table => new
                {
                    InvestorFundId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestorFundCustomerId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    ProposedTenor = table.Column<decimal>(nullable: true),
                    ProposedRate = table.Column<decimal>(nullable: true),
                    FrequencyId = table.Column<int>(nullable: true),
                    Period = table.Column<string>(maxLength: 50, nullable: true),
                    ProposedAmount = table.Column<decimal>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: true),
                    InvestmentPurpose = table.Column<string>(maxLength: 50, nullable: true),
                    EnableRollOver = table.Column<bool>(nullable: true),
                    InstrumentId = table.Column<int>(nullable: true),
                    InstrumentNumer = table.Column<string>(maxLength: 50, nullable: true),
                    InstrumentDate = table.Column<DateTime>(nullable: true),
                    CustomerNameId = table.Column<int>(nullable: true),
                    ProductName = table.Column<string>(maxLength: 50, nullable: true),
                    ApprovedTenor = table.Column<decimal>(nullable: true),
                    ApprovedRate = table.Column<decimal>(nullable: true),
                    ApprovedProductId = table.Column<int>(nullable: true),
                    FirstPrincipalDate = table.Column<DateTime>(nullable: true),
                    MaturityDate = table.Column<DateTime>(nullable: true),
                    ApprovedAmount = table.Column<decimal>(nullable: true),
                    ExpectedPayout = table.Column<decimal>(nullable: true),
                    ExpectedInterest = table.Column<decimal>(nullable: true),
                    ApprovalStatus = table.Column<int>(nullable: true),
                    InvestmentStatus = table.Column<int>(nullable: true),
                    GenerateCertificate = table.Column<bool>(nullable: true),
                    RefNumber = table.Column<string>(maxLength: 10, nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    WorkflowToken = table.Column<string>(nullable: true),
                    credit_loancustomerCustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inf_investorfund", x => x.InvestorFundId);
                    table.ForeignKey(
                        name: "FK_inf_investorfund_credit_loancustomer_credit_loancustomerCustomerId",
                        column: x => x.credit_loancustomerCustomerId,
                        principalTable: "credit_loancustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_loancustomerfscaptiondetail",
                columns: table => new
                {
                    FSDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    FSCaptionId = table.Column<int>(nullable: false),
                    FSDate = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_loancustomerCustomerId = table.Column<int>(nullable: true),
                    credit_loancustomerfscaptionFSCaptionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loancustomerfscaptiondetail", x => x.FSDetailId);
                    table.ForeignKey(
                        name: "FK_credit_loancustomerfscaptiondetail_credit_loancustomer_credit_loancustomerCustomerId",
                        column: x => x.credit_loancustomerCustomerId,
                        principalTable: "credit_loancustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_loancustomerfscaptiondetail_credit_loancustomerfscaption_credit_loancustomerfscaptionFSCaptionId",
                        column: x => x.credit_loancustomerfscaptionFSCaptionId,
                        principalTable: "credit_loancustomerfscaption",
                        principalColumn: "FSCaptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_individualapplicationscorecard",
                columns: table => new
                {
                    ApplicationScoreCardId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanApplicationId = table.Column<int>(nullable: false),
                    LoanRefNo = table.Column<string>(maxLength: 50, nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Field1 = table.Column<decimal>(nullable: true),
                    Field2 = table.Column<decimal>(nullable: true),
                    Field3 = table.Column<decimal>(nullable: true),
                    Field4 = table.Column<decimal>(nullable: true),
                    Field5 = table.Column<decimal>(nullable: true),
                    Field6 = table.Column<decimal>(nullable: true),
                    Field7 = table.Column<decimal>(nullable: true),
                    Field8 = table.Column<decimal>(nullable: true),
                    Field9 = table.Column<decimal>(nullable: true),
                    Field10 = table.Column<decimal>(nullable: true),
                    Field11 = table.Column<decimal>(nullable: true),
                    Field12 = table.Column<decimal>(nullable: true),
                    Field13 = table.Column<decimal>(nullable: true),
                    Field14 = table.Column<decimal>(nullable: true),
                    Field15 = table.Column<decimal>(nullable: true),
                    Field16 = table.Column<decimal>(nullable: true),
                    Field17 = table.Column<decimal>(nullable: true),
                    Field18 = table.Column<decimal>(nullable: true),
                    Field19 = table.Column<decimal>(nullable: true),
                    Field20 = table.Column<decimal>(nullable: true),
                    Field21 = table.Column<decimal>(nullable: true),
                    Field22 = table.Column<decimal>(nullable: true),
                    Field23 = table.Column<decimal>(nullable: true),
                    Field24 = table.Column<decimal>(nullable: true),
                    Field25 = table.Column<decimal>(nullable: true),
                    Field26 = table.Column<decimal>(nullable: true),
                    Field27 = table.Column<decimal>(nullable: true),
                    Field28 = table.Column<decimal>(nullable: true),
                    Field29 = table.Column<decimal>(nullable: true),
                    Field30 = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_productProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_individualapplicationscorecard", x => x.ApplicationScoreCardId);
                    table.ForeignKey(
                        name: "FK_credit_individualapplicationscorecard_credit_product_credit_productProductId",
                        column: x => x.credit_productProductId,
                        principalTable: "credit_product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanapplication",
                columns: table => new
                {
                    LoanApplicationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    ProposedProductId = table.Column<int>(nullable: false),
                    ProposedTenor = table.Column<int>(nullable: false),
                    ProposedRate = table.Column<double>(nullable: false),
                    ProposedAmount = table.Column<decimal>(nullable: false),
                    ApprovedProductId = table.Column<int>(nullable: false),
                    ApprovedTenor = table.Column<int>(nullable: false),
                    ApprovedRate = table.Column<double>(nullable: false),
                    ApprovedAmount = table.Column<decimal>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    ExchangeRate = table.Column<double>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    HasDoneChecklist = table.Column<bool>(nullable: false),
                    ApplicationDate = table.Column<DateTime>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    MaturityDate = table.Column<DateTime>(nullable: false),
                    FirstPrincipalDate = table.Column<DateTime>(nullable: true),
                    FirstInterestDate = table.Column<DateTime>(nullable: true),
                    LoanApplicationStatusId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    PaymentMode = table.Column<int>(nullable: true),
                    RepaymentMode = table.Column<int>(nullable: true),
                    PaymentAccount = table.Column<string>(nullable: true),
                    RepaymentAccount = table.Column<string>(nullable: true),
                    ApplicationRefNumber = table.Column<string>(maxLength: 50, nullable: false),
                    Score = table.Column<decimal>(nullable: true),
                    PD = table.Column<decimal>(nullable: true),
                    GenerateOfferLetter = table.Column<bool>(nullable: false),
                    Purpose = table.Column<string>(maxLength: 2000, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    WorkflowToken = table.Column<string>(nullable: true),
                    credit_loancustomerCustomerId = table.Column<int>(nullable: true),
                    credit_productProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanapplication", x => x.LoanApplicationId);
                    table.ForeignKey(
                        name: "FK_credit_loanapplication_credit_loancustomer_credit_loancustomerCustomerId",
                        column: x => x.credit_loancustomerCustomerId,
                        principalTable: "credit_loancustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_loanapplication_credit_product_credit_productProductId",
                        column: x => x.credit_productProductId,
                        principalTable: "credit_product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_productfee",
                columns: table => new
                {
                    ProductFeeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeeId = table.Column<int>(nullable: false),
                    ProductPaymentType = table.Column<int>(nullable: false),
                    ProductFeeType = table.Column<int>(nullable: false),
                    ProductAmount = table.Column<double>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_feeFeeId = table.Column<int>(nullable: true),
                    credit_productProductId = table.Column<int>(nullable: true),
                    credit_repaymenttypeRepaymentTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_productfee", x => x.ProductFeeId);
                    table.ForeignKey(
                        name: "FK_credit_productfee_credit_fee_credit_feeFeeId",
                        column: x => x.credit_feeFeeId,
                        principalTable: "credit_fee",
                        principalColumn: "FeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_productfee_credit_product_credit_productProductId",
                        column: x => x.credit_productProductId,
                        principalTable: "credit_product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_productfee_credit_repaymenttype_credit_repaymenttypeRepaymentTypeId",
                        column: x => x.credit_repaymenttypeRepaymentTypeId,
                        principalTable: "credit_repaymenttype",
                        principalColumn: "RepaymentTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_weightedriskscore",
                columns: table => new
                {
                    WeightedRiskScoreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditRiskAttributeId = table.Column<int>(nullable: true),
                    FeildName = table.Column<string>(maxLength: 50, nullable: true),
                    UseAtOrigination = table.Column<bool>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    CustomerTypeId = table.Column<int>(nullable: true),
                    ProductMaxWeight = table.Column<decimal>(nullable: true),
                    WeightedScore = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_creditriskattributeCreditRiskAttributeId = table.Column<int>(nullable: true),
                    credit_productProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_weightedriskscore", x => x.WeightedRiskScoreId);
                    table.ForeignKey(
                        name: "FK_credit_weightedriskscore_credit_creditriskattribute_credit_creditriskattributeCreditRiskAttributeId",
                        column: x => x.credit_creditriskattributeCreditRiskAttributeId,
                        principalTable: "credit_creditriskattribute",
                        principalColumn: "CreditRiskAttributeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_weightedriskscore_credit_product_credit_productProductId",
                        column: x => x.credit_productProductId,
                        principalTable: "credit_product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "deposit_customerkycdocumentupload",
                columns: table => new
                {
                    DocumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    KycId = table.Column<int>(nullable: true),
                    DocumentName = table.Column<string>(maxLength: 50, nullable: true),
                    DocumentUpload = table.Column<byte[]>(nullable: true),
                    PhysicalLocation = table.Column<string>(maxLength: 50, nullable: true),
                    FileExtension = table.Column<string>(maxLength: 50, nullable: true),
                    DocumentType = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    deposit_customerkycKycId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_customerkycdocumentupload", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_deposit_customerkycdocumentupload_deposit_customerkyc_deposit_customerkycKycId",
                        column: x => x.deposit_customerkycKycId,
                        principalTable: "deposit_customerkyc",
                        principalColumn: "KycId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "deposit_customersignature",
                columns: table => new
                {
                    SignatureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    SignatoryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    SignatureImg = table.Column<byte[]>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    deposit_accountopeningCustomerId = table.Column<int>(nullable: true),
                    deposit_customersignatorySignatoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_customersignature", x => x.SignatureId);
                    table.ForeignKey(
                        name: "FK_deposit_customersignature_deposit_accountopening_deposit_accountopeningCustomerId",
                        column: x => x.deposit_accountopeningCustomerId,
                        principalTable: "deposit_accountopening",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_deposit_customersignature_deposit_customersignatory_deposit_customersignatorySignatoryId",
                        column: x => x.deposit_customersignatorySignatoryId,
                        principalTable: "deposit_customersignatory",
                        principalColumn: "SignatoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_corporateapplicationscorecard",
                columns: table => new
                {
                    ApplicationScoreCardId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanApplicationId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Field1 = table.Column<decimal>(nullable: true),
                    Field2 = table.Column<decimal>(nullable: true),
                    Field3 = table.Column<decimal>(nullable: true),
                    Field4 = table.Column<decimal>(nullable: true),
                    Field5 = table.Column<decimal>(nullable: true),
                    Field6 = table.Column<decimal>(nullable: true),
                    Field7 = table.Column<decimal>(nullable: true),
                    Field8 = table.Column<decimal>(nullable: true),
                    Field9 = table.Column<decimal>(nullable: true),
                    Field10 = table.Column<decimal>(nullable: true),
                    Field11 = table.Column<decimal>(nullable: true),
                    Field12 = table.Column<decimal>(nullable: true),
                    Field13 = table.Column<decimal>(nullable: true),
                    Field14 = table.Column<decimal>(nullable: true),
                    Field15 = table.Column<decimal>(nullable: true),
                    Field16 = table.Column<decimal>(nullable: true),
                    Field17 = table.Column<decimal>(nullable: true),
                    Field18 = table.Column<decimal>(nullable: true),
                    Field19 = table.Column<decimal>(nullable: true),
                    Field20 = table.Column<decimal>(nullable: true),
                    Field21 = table.Column<decimal>(nullable: true),
                    Field22 = table.Column<decimal>(nullable: true),
                    Field23 = table.Column<decimal>(nullable: true),
                    Field24 = table.Column<decimal>(nullable: true),
                    Field25 = table.Column<decimal>(nullable: true),
                    Field26 = table.Column<decimal>(nullable: true),
                    Field27 = table.Column<decimal>(nullable: true),
                    Field28 = table.Column<decimal>(nullable: true),
                    Field29 = table.Column<decimal>(nullable: true),
                    Field30 = table.Column<decimal>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_loanapplicationLoanApplicationId = table.Column<int>(nullable: true),
                    credit_loancustomerCustomerId = table.Column<int>(nullable: true),
                    credit_productProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_corporateapplicationscorecard", x => x.ApplicationScoreCardId);
                    table.ForeignKey(
                        name: "FK_credit_corporateapplicationscorecard_credit_loanapplication_credit_loanapplicationLoanApplicationId",
                        column: x => x.credit_loanapplicationLoanApplicationId,
                        principalTable: "credit_loanapplication",
                        principalColumn: "LoanApplicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_corporateapplicationscorecard_credit_loancustomer_credit_loancustomerCustomerId",
                        column: x => x.credit_loancustomerCustomerId,
                        principalTable: "credit_loancustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_corporateapplicationscorecard_credit_product_credit_productProductId",
                        column: x => x.credit_productProductId,
                        principalTable: "credit_product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_corporateapplicationscorecarddetails",
                columns: table => new
                {
                    ApplicationCreditScoreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanApplicationId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    AttributeField = table.Column<string>(maxLength: 50, nullable: false),
                    Score = table.Column<decimal>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_loanapplicationLoanApplicationId = table.Column<int>(nullable: true),
                    credit_loancustomerCustomerId = table.Column<int>(nullable: true),
                    credit_productProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_corporateapplicationscorecarddetails", x => x.ApplicationCreditScoreId);
                    table.ForeignKey(
                        name: "FK_credit_corporateapplicationscorecarddetails_credit_loanapplication_credit_loanapplicationLoanApplicationId",
                        column: x => x.credit_loanapplicationLoanApplicationId,
                        principalTable: "credit_loanapplication",
                        principalColumn: "LoanApplicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_corporateapplicationscorecarddetails_credit_loancustomer_credit_loancustomerCustomerId",
                        column: x => x.credit_loancustomerCustomerId,
                        principalTable: "credit_loancustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_corporateapplicationscorecarddetails_credit_product_credit_productProductId",
                        column: x => x.credit_productProductId,
                        principalTable: "credit_product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_individualapplicationscorecarddetails",
                columns: table => new
                {
                    ApplicationCreditScoreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanApplicationId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    AttributeField = table.Column<string>(maxLength: 50, nullable: false),
                    Score = table.Column<decimal>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_loanapplicationLoanApplicationId = table.Column<int>(nullable: true),
                    credit_loancustomerCustomerId = table.Column<int>(nullable: true),
                    credit_productProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_individualapplicationscorecarddetails", x => x.ApplicationCreditScoreId);
                    table.ForeignKey(
                        name: "FK_credit_individualapplicationscorecarddetails_credit_loanapplication_credit_loanapplicationLoanApplicationId",
                        column: x => x.credit_loanapplicationLoanApplicationId,
                        principalTable: "credit_loanapplication",
                        principalColumn: "LoanApplicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_individualapplicationscorecarddetails_credit_loancustomer_credit_loancustomerCustomerId",
                        column: x => x.credit_loancustomerCustomerId,
                        principalTable: "credit_loancustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_individualapplicationscorecarddetails_credit_product_credit_productProductId",
                        column: x => x.credit_productProductId,
                        principalTable: "credit_product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanapplicationcollateral",
                columns: table => new
                {
                    LoanApplicationCollateralId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollateralCustomerId = table.Column<int>(nullable: true),
                    LoanApplicationId = table.Column<int>(nullable: true),
                    ActualCollateralValue = table.Column<decimal>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_collateralcustomerCollateralCustomerId = table.Column<int>(nullable: true),
                    credit_loanapplicationLoanApplicationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanapplicationcollateral", x => x.LoanApplicationCollateralId);
                    table.ForeignKey(
                        name: "FK_credit_loanapplicationcollateral_credit_collateralcustomer_credit_collateralcustomerCollateralCustomerId",
                        column: x => x.credit_collateralcustomerCollateralCustomerId,
                        principalTable: "credit_collateralcustomer",
                        principalColumn: "CollateralCustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_loanapplicationcollateral_credit_loanapplication_credit_loanapplicationLoanApplicationId",
                        column: x => x.credit_loanapplicationLoanApplicationId,
                        principalTable: "credit_loanapplication",
                        principalColumn: "LoanApplicationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_loanapplicationrecommendationlog",
                columns: table => new
                {
                    LoanApplicationRecommendationLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanApplicationId = table.Column<int>(nullable: false),
                    ApprovedProductId = table.Column<int>(nullable: false),
                    ApprovedTenor = table.Column<int>(nullable: false),
                    ApprovedRate = table.Column<double>(nullable: false),
                    ApprovedAmount = table.Column<decimal>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_loanapplicationLoanApplicationId = table.Column<int>(nullable: true),
                    credit_productProductId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loanapplicationrecommendationlog", x => x.LoanApplicationRecommendationLogId);
                    table.ForeignKey(
                        name: "FK_credit_loanapplicationrecommendationlog_credit_loanapplication_credit_loanapplicationLoanApplicationId",
                        column: x => x.credit_loanapplicationLoanApplicationId,
                        principalTable: "credit_loanapplication",
                        principalColumn: "LoanApplicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_loanapplicationrecommendationlog_credit_product_credit_productProductId",
                        column: x => x.credit_productProductId,
                        principalTable: "credit_product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credit_loancreditbureau",
                columns: table => new
                {
                    LoanCreditBureauId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanApplicationId = table.Column<int>(nullable: false),
                    CreditBureauId = table.Column<int>(nullable: false),
                    ChargeAmount = table.Column<decimal>(nullable: true),
                    ReportStatus = table.Column<bool>(nullable: true),
                    SupportDocument = table.Column<byte[]>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    credit_creditbureauCreditBureauId = table.Column<int>(nullable: true),
                    credit_loanapplicationLoanApplicationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_loancreditbureau", x => x.LoanCreditBureauId);
                    table.ForeignKey(
                        name: "FK_credit_loancreditbureau_credit_creditbureau_credit_creditbureauCreditBureauId",
                        column: x => x.credit_creditbureauCreditBureauId,
                        principalTable: "credit_creditbureau",
                        principalColumn: "CreditBureauId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_credit_loancreditbureau_credit_loanapplication_credit_loanapplicationLoanApplicationId",
                        column: x => x.credit_loanapplicationLoanApplicationId,
                        principalTable: "credit_loanapplication",
                        principalColumn: "LoanApplicationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_credit_collateralcustomer_credit_collateraltypeCollateralTypeId",
                table: "credit_collateralcustomer",
                column: "credit_collateraltypeCollateralTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_collateralcustomer_credit_loancustomerCustomerId",
                table: "credit_collateralcustomer",
                column: "credit_loancustomerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_corporateapplicationscorecard_credit_loanapplicationLoanApplicationId",
                table: "credit_corporateapplicationscorecard",
                column: "credit_loanapplicationLoanApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_corporateapplicationscorecard_credit_loancustomerCustomerId",
                table: "credit_corporateapplicationscorecard",
                column: "credit_loancustomerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_corporateapplicationscorecard_credit_productProductId",
                table: "credit_corporateapplicationscorecard",
                column: "credit_productProductId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_corporateapplicationscorecarddetails_credit_loanapplicationLoanApplicationId",
                table: "credit_corporateapplicationscorecarddetails",
                column: "credit_loanapplicationLoanApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_corporateapplicationscorecarddetails_credit_loancustomerCustomerId",
                table: "credit_corporateapplicationscorecarddetails",
                column: "credit_loancustomerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_corporateapplicationscorecarddetails_credit_productProductId",
                table: "credit_corporateapplicationscorecarddetails",
                column: "credit_productProductId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_customerbankdetails_credit_loancustomerCustomerId",
                table: "credit_customerbankdetails",
                column: "credit_loancustomerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_customeridentitydetails_credit_loancustomerCustomerId",
                table: "credit_customeridentitydetails",
                column: "credit_loancustomerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_customernextofkin_credit_loancustomerCustomerId",
                table: "credit_customernextofkin",
                column: "credit_loancustomerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_directorshareholder_credit_loancustomerCustomerId",
                table: "credit_directorshareholder",
                column: "credit_loancustomerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_individualapplicationscorecard_credit_productProductId",
                table: "credit_individualapplicationscorecard",
                column: "credit_productProductId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_individualapplicationscorecarddetails_credit_loanapplicationLoanApplicationId",
                table: "credit_individualapplicationscorecarddetails",
                column: "credit_loanapplicationLoanApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_individualapplicationscorecarddetails_credit_loancustomerCustomerId",
                table: "credit_individualapplicationscorecarddetails",
                column: "credit_loancustomerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_individualapplicationscorecarddetails_credit_productProductId",
                table: "credit_individualapplicationscorecarddetails",
                column: "credit_productProductId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loanapplication_credit_loancustomerCustomerId",
                table: "credit_loanapplication",
                column: "credit_loancustomerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loanapplication_credit_productProductId",
                table: "credit_loanapplication",
                column: "credit_productProductId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loanapplicationcollateral_credit_collateralcustomerCollateralCustomerId",
                table: "credit_loanapplicationcollateral",
                column: "credit_collateralcustomerCollateralCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loanapplicationcollateral_credit_loanapplicationLoanApplicationId",
                table: "credit_loanapplicationcollateral",
                column: "credit_loanapplicationLoanApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loanapplicationrecommendationlog_credit_loanapplicationLoanApplicationId",
                table: "credit_loanapplicationrecommendationlog",
                column: "credit_loanapplicationLoanApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loanapplicationrecommendationlog_credit_productProductId",
                table: "credit_loanapplicationrecommendationlog",
                column: "credit_productProductId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loancreditbureau_credit_creditbureauCreditBureauId",
                table: "credit_loancreditbureau",
                column: "credit_creditbureauCreditBureauId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loancreditbureau_credit_loanapplicationLoanApplicationId",
                table: "credit_loancreditbureau",
                column: "credit_loanapplicationLoanApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loancustomer_UserId",
                table: "credit_loancustomer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loancustomerdirector_credit_directortypeDirectorTypeId",
                table: "credit_loancustomerdirector",
                column: "credit_directortypeDirectorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loancustomerdirector_credit_loancustomerCustomerId",
                table: "credit_loancustomerdirector",
                column: "credit_loancustomerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loancustomerdocument_credit_documenttypeDocumentTypeId",
                table: "credit_loancustomerdocument",
                column: "credit_documenttypeDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loancustomerdocument_credit_loancustomerCustomerId",
                table: "credit_loancustomerdocument",
                column: "credit_loancustomerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loancustomerfscaption_credit_loancustomerfscaptiongroupFSCaptionGroupId",
                table: "credit_loancustomerfscaption",
                column: "credit_loancustomerfscaptiongroupFSCaptionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loancustomerfscaptiondetail_credit_loancustomerCustomerId",
                table: "credit_loancustomerfscaptiondetail",
                column: "credit_loancustomerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loancustomerfscaptiondetail_credit_loancustomerfscaptionFSCaptionId",
                table: "credit_loancustomerfscaptiondetail",
                column: "credit_loancustomerfscaptionFSCaptionId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loanreviewoperationirregularinput_credit_loanreviewoperationLoanReviewOperationId",
                table: "credit_loanreviewoperationirregularinput",
                column: "credit_loanreviewoperationLoanReviewOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_loanscheduletype_credit_loanschedulecategoryLoanScheduleCategoryId",
                table: "credit_loanscheduletype",
                column: "credit_loanschedulecategoryLoanScheduleCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_product_credit_repaymenttypeRepaymentTypeId",
                table: "credit_product",
                column: "credit_repaymenttypeRepaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_productfee_credit_feeFeeId",
                table: "credit_productfee",
                column: "credit_feeFeeId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_productfee_credit_productProductId",
                table: "credit_productfee",
                column: "credit_productProductId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_productfee_credit_repaymenttypeRepaymentTypeId",
                table: "credit_productfee",
                column: "credit_repaymenttypeRepaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_weightedriskscore_credit_creditriskattributeCreditRiskAttributeId",
                table: "credit_weightedriskscore",
                column: "credit_creditriskattributeCreditRiskAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_weightedriskscore_credit_productProductId",
                table: "credit_weightedriskscore",
                column: "credit_productProductId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_accountsetup_deposit_accountypeAccountTypeId",
                table: "deposit_accountsetup",
                column: "deposit_accountypeAccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_accountsetup_deposit_categoryCategoryId",
                table: "deposit_accountsetup",
                column: "deposit_categoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_accountsetup_deposit_transactionchargeTransactionChargeId",
                table: "deposit_accountsetup",
                column: "deposit_transactionchargeTransactionChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_accountsetup_deposit_transactiontaxTransactionTaxId",
                table: "deposit_accountsetup",
                column: "deposit_transactiontaxTransactionTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customercontactpersons_deposit_accountopeningCustomerId",
                table: "deposit_customercontactpersons",
                column: "deposit_accountopeningCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customerdirectors_deposit_accountopeningCustomerId",
                table: "deposit_customerdirectors",
                column: "deposit_accountopeningCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customeridentification_deposit_accountopeningCustomerId",
                table: "deposit_customeridentification",
                column: "deposit_accountopeningCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customerkyc_deposit_accountopeningCustomerId",
                table: "deposit_customerkyc",
                column: "deposit_accountopeningCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customerkycdocumentupload_deposit_customerkycKycId",
                table: "deposit_customerkycdocumentupload",
                column: "deposit_customerkycKycId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customernextofkin_deposit_accountopeningCustomerId",
                table: "deposit_customernextofkin",
                column: "deposit_accountopeningCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customersignatory_deposit_accountopeningCustomerId",
                table: "deposit_customersignatory",
                column: "deposit_accountopeningCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customersignature_deposit_accountopeningCustomerId",
                table: "deposit_customersignature",
                column: "deposit_accountopeningCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_deposit_customersignature_deposit_customersignatorySignatoryId",
                table: "deposit_customersignature",
                column: "deposit_customersignatorySignatoryId");

            migrationBuilder.CreateIndex(
                name: "IX_inf_investorfund_credit_loancustomerCustomerId",
                table: "inf_investorfund",
                column: "credit_loancustomerCustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ConfirmEmailCode");

            migrationBuilder.DropTable(
                name: "cor_allowable_collateraltype");

            migrationBuilder.DropTable(
                name: "cor_approvaldetail");

            migrationBuilder.DropTable(
                name: "credit_allowable_collateraltype");

            migrationBuilder.DropTable(
                name: "credit_casa");

            migrationBuilder.DropTable(
                name: "credit_casa_lien");

            migrationBuilder.DropTable(
                name: "credit_collateralcustomerconsumption");

            migrationBuilder.DropTable(
                name: "credit_corporateapplicationscorecard");

            migrationBuilder.DropTable(
                name: "credit_corporateapplicationscorecarddetails");

            migrationBuilder.DropTable(
                name: "credit_creditclassification");

            migrationBuilder.DropTable(
                name: "credit_creditrating");

            migrationBuilder.DropTable(
                name: "credit_creditratingpd");

            migrationBuilder.DropTable(
                name: "credit_creditriskcategory");

            migrationBuilder.DropTable(
                name: "credit_creditriskscorecard");

            migrationBuilder.DropTable(
                name: "credit_customerbankdetails");

            migrationBuilder.DropTable(
                name: "credit_customercarddetails");

            migrationBuilder.DropTable(
                name: "credit_customeridentitydetails");

            migrationBuilder.DropTable(
                name: "credit_customerloanlgd_history_final");

            migrationBuilder.DropTable(
                name: "credit_customerloanpd");

            migrationBuilder.DropTable(
                name: "credit_customerloanpd_application");

            migrationBuilder.DropTable(
                name: "credit_customerloanpd_history");

            migrationBuilder.DropTable(
                name: "credit_customerloanpd_history_final");

            migrationBuilder.DropTable(
                name: "credit_customerloanscorecard");

            migrationBuilder.DropTable(
                name: "credit_customernextofkin");

            migrationBuilder.DropTable(
                name: "credit_daily_accural");

            migrationBuilder.DropTable(
                name: "credit_daycountconvention");

            migrationBuilder.DropTable(
                name: "credit_directorshareholder");

            migrationBuilder.DropTable(
                name: "credit_exposureparament");

            migrationBuilder.DropTable(
                name: "credit_fee_charge");

            migrationBuilder.DropTable(
                name: "credit_frequencytype");

            migrationBuilder.DropTable(
                name: "credit_impairment");

            migrationBuilder.DropTable(
                name: "credit_impairment_final");

            migrationBuilder.DropTable(
                name: "credit_individualapplicationscorecard");

            migrationBuilder.DropTable(
                name: "credit_individualapplicationscorecard_history");

            migrationBuilder.DropTable(
                name: "credit_individualapplicationscorecarddetails");

            migrationBuilder.DropTable(
                name: "credit_individualapplicationscorecarddetails_history");

            migrationBuilder.DropTable(
                name: "credit_lgd_historyinformation");

            migrationBuilder.DropTable(
                name: "credit_lgd_historyinformationdetails");

            migrationBuilder.DropTable(
                name: "credit_loan");

            migrationBuilder.DropTable(
                name: "credit_loan_archive");

            migrationBuilder.DropTable(
                name: "credit_loan_cheque");

            migrationBuilder.DropTable(
                name: "credit_loan_cheque_list");

            migrationBuilder.DropTable(
                name: "credit_loan_past_due");

            migrationBuilder.DropTable(
                name: "credit_loan_repayment");

            migrationBuilder.DropTable(
                name: "credit_loan_review_operation");

            migrationBuilder.DropTable(
                name: "credit_loan_temp");

            migrationBuilder.DropTable(
                name: "credit_loanapplication_website");

            migrationBuilder.DropTable(
                name: "credit_loanapplicationcollateral");

            migrationBuilder.DropTable(
                name: "credit_loanapplicationcollateraldocument");

            migrationBuilder.DropTable(
                name: "credit_loanapplicationrecommendationlog");

            migrationBuilder.DropTable(
                name: "credit_loancomment");

            migrationBuilder.DropTable(
                name: "credit_loancreditbureau");

            migrationBuilder.DropTable(
                name: "credit_loancustomerdirector");

            migrationBuilder.DropTable(
                name: "credit_loancustomerdocument");

            migrationBuilder.DropTable(
                name: "credit_loancustomerfscaptiondetail");

            migrationBuilder.DropTable(
                name: "credit_loancustomerfsratiodivisortype");

            migrationBuilder.DropTable(
                name: "credit_loancustomerfsratiovaluetype");

            migrationBuilder.DropTable(
                name: "credit_loancustomerratiodetail");

            migrationBuilder.DropTable(
                name: "credit_loandecision");

            migrationBuilder.DropTable(
                name: "credit_loanoperation");

            migrationBuilder.DropTable(
                name: "credit_loanreviewapplication");

            migrationBuilder.DropTable(
                name: "credit_loanreviewapplicationlog");

            migrationBuilder.DropTable(
                name: "credit_loanreviewofferletter");

            migrationBuilder.DropTable(
                name: "credit_loanreviewoperationirregularinput");

            migrationBuilder.DropTable(
                name: "credit_loanscheduledaily");

            migrationBuilder.DropTable(
                name: "credit_loanscheduledailyarchive");

            migrationBuilder.DropTable(
                name: "credit_loanscheduleirrigular");

            migrationBuilder.DropTable(
                name: "credit_loanscheduleperiodic");

            migrationBuilder.DropTable(
                name: "credit_loanscheduleperiodicarchive");

            migrationBuilder.DropTable(
                name: "credit_loanscheduletype");

            migrationBuilder.DropTable(
                name: "credit_loanstaging");

            migrationBuilder.DropTable(
                name: "credit_loantable");

            migrationBuilder.DropTable(
                name: "credit_loantransactiontype");

            migrationBuilder.DropTable(
                name: "credit_offerletter");

            migrationBuilder.DropTable(
                name: "credit_productfee");

            migrationBuilder.DropTable(
                name: "credit_productfeestatus");

            migrationBuilder.DropTable(
                name: "credit_producthistoricalpd");

            migrationBuilder.DropTable(
                name: "credit_producttype");

            migrationBuilder.DropTable(
                name: "credit_systemattribute");

            migrationBuilder.DropTable(
                name: "credit_temp_loanscheduledaily");

            migrationBuilder.DropTable(
                name: "credit_temp_loanscheduleirrigular");

            migrationBuilder.DropTable(
                name: "credit_temp_loanscheduleperiodic");

            migrationBuilder.DropTable(
                name: "credit_weightedriskscore");

            migrationBuilder.DropTable(
                name: "deposit_accountreactivation");

            migrationBuilder.DropTable(
                name: "deposit_accountreactivationsetup");

            migrationBuilder.DropTable(
                name: "deposit_accountsetup");

            migrationBuilder.DropTable(
                name: "deposit_bankclosure");

            migrationBuilder.DropTable(
                name: "deposit_bankclosuresetup");

            migrationBuilder.DropTable(
                name: "deposit_businesscategory");

            migrationBuilder.DropTable(
                name: "deposit_cashiertellerform");

            migrationBuilder.DropTable(
                name: "deposit_cashiertellersetup");

            migrationBuilder.DropTable(
                name: "deposit_changeofrates");

            migrationBuilder.DropTable(
                name: "deposit_changeofratesetup");

            migrationBuilder.DropTable(
                name: "deposit_customercontactpersons");

            migrationBuilder.DropTable(
                name: "deposit_customerdirectors");

            migrationBuilder.DropTable(
                name: "deposit_customeridentification");

            migrationBuilder.DropTable(
                name: "deposit_customerkycdocumentupload");

            migrationBuilder.DropTable(
                name: "deposit_customernextofkin");

            migrationBuilder.DropTable(
                name: "deposit_customersignature");

            migrationBuilder.DropTable(
                name: "deposit_depositform");

            migrationBuilder.DropTable(
                name: "deposit_selectedTransactioncharge");

            migrationBuilder.DropTable(
                name: "deposit_selectedTransactiontax");

            migrationBuilder.DropTable(
                name: "deposit_tillvaultform");

            migrationBuilder.DropTable(
                name: "deposit_tillvaultopeningclose");

            migrationBuilder.DropTable(
                name: "deposit_tillvaultsetup");

            migrationBuilder.DropTable(
                name: "deposit_tillvaultsummary");

            migrationBuilder.DropTable(
                name: "deposit_transactioncorrectionform");

            migrationBuilder.DropTable(
                name: "deposit_transactioncorrectionsetup");

            migrationBuilder.DropTable(
                name: "deposit_transferform");

            migrationBuilder.DropTable(
                name: "deposit_transfersetup");

            migrationBuilder.DropTable(
                name: "deposit_withdrawalform");

            migrationBuilder.DropTable(
                name: "deposit_withdrawalsetup");

            migrationBuilder.DropTable(
                name: "fin_customertransaction");

            migrationBuilder.DropTable(
                name: "ifrs_computed_forcasted_pd_lgd");

            migrationBuilder.DropTable(
                name: "ifrs_forecasted_lgd");

            migrationBuilder.DropTable(
                name: "Ifrs_forecasted_macroeconimcs_mapping");

            migrationBuilder.DropTable(
                name: "ifrs_forecasted_pd");

            migrationBuilder.DropTable(
                name: "ifrs_historical_macroeconimcs_mapping");

            migrationBuilder.DropTable(
                name: "ifrs_historical_product_pd");

            migrationBuilder.DropTable(
                name: "ifrs_macroeconomic_variables");

            migrationBuilder.DropTable(
                name: "ifrs_macroeconomic_variables_year");

            migrationBuilder.DropTable(
                name: "ifrs_pit_formula");

            migrationBuilder.DropTable(
                name: "ifrs_product_regressed_lgd");

            migrationBuilder.DropTable(
                name: "ifrs_product_regressed_pd");

            migrationBuilder.DropTable(
                name: "ifrs_regress_macro_variable");

            migrationBuilder.DropTable(
                name: "ifrs_regress_macro_variable_lgd");

            migrationBuilder.DropTable(
                name: "ifrs_scenario_setup");

            migrationBuilder.DropTable(
                name: "ifrs_setup_data");

            migrationBuilder.DropTable(
                name: "ifrs_temp_table1");

            migrationBuilder.DropTable(
                name: "ifrs_var_coeff_comp_lgd");

            migrationBuilder.DropTable(
                name: "ifrs_var_coeff_comp_pd");

            migrationBuilder.DropTable(
                name: "ifrs_xxxxx");

            migrationBuilder.DropTable(
                name: "inf_collection");

            migrationBuilder.DropTable(
                name: "inf_collection_website");

            migrationBuilder.DropTable(
                name: "inf_collectionrecommendationLog");

            migrationBuilder.DropTable(
                name: "inf_customer");

            migrationBuilder.DropTable(
                name: "inf_daily_accural");

            migrationBuilder.DropTable(
                name: "inf_investdailyschedule");

            migrationBuilder.DropTable(
                name: "inf_investdailyschedule_topup");

            migrationBuilder.DropTable(
                name: "inf_investmentperiodicschedule");

            migrationBuilder.DropTable(
                name: "inf_investmentrecommendationlog");

            migrationBuilder.DropTable(
                name: "inf_investorfund");

            migrationBuilder.DropTable(
                name: "inf_investorfund_rollover_website");

            migrationBuilder.DropTable(
                name: "inf_investorfund_topup_website");

            migrationBuilder.DropTable(
                name: "inf_investorfund_website");

            migrationBuilder.DropTable(
                name: "inf_liquidation");

            migrationBuilder.DropTable(
                name: "inf_liquidation_website");

            migrationBuilder.DropTable(
                name: "inf_liquidationrecommendationlog");

            migrationBuilder.DropTable(
                name: "inf_product");

            migrationBuilder.DropTable(
                name: "inf_producttype");

            migrationBuilder.DropTable(
                name: "OTPTracker");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "tmp_loanapplicationscheduleperiodic");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "credit_collateralcustomer");

            migrationBuilder.DropTable(
                name: "credit_creditbureau");

            migrationBuilder.DropTable(
                name: "credit_loanapplication");

            migrationBuilder.DropTable(
                name: "credit_directortype");

            migrationBuilder.DropTable(
                name: "credit_documenttype");

            migrationBuilder.DropTable(
                name: "credit_loancustomerfscaption");

            migrationBuilder.DropTable(
                name: "credit_loanreviewoperation");

            migrationBuilder.DropTable(
                name: "credit_loanschedulecategory");

            migrationBuilder.DropTable(
                name: "credit_fee");

            migrationBuilder.DropTable(
                name: "credit_creditriskattribute");

            migrationBuilder.DropTable(
                name: "deposit_accountype");

            migrationBuilder.DropTable(
                name: "deposit_category");

            migrationBuilder.DropTable(
                name: "deposit_transactioncharge");

            migrationBuilder.DropTable(
                name: "deposit_transactiontax");

            migrationBuilder.DropTable(
                name: "deposit_customerkyc");

            migrationBuilder.DropTable(
                name: "deposit_customersignatory");

            migrationBuilder.DropTable(
                name: "credit_collateraltype");

            migrationBuilder.DropTable(
                name: "credit_loancustomer");

            migrationBuilder.DropTable(
                name: "credit_product");

            migrationBuilder.DropTable(
                name: "credit_loancustomerfscaptiongroup");

            migrationBuilder.DropTable(
                name: "deposit_accountopening");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "credit_repaymenttype");
        }
    }
}
