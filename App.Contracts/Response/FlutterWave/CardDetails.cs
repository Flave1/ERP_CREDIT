using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Contracts.Response.FlutterWave
{
    public class CardDetails
    {
        public string issuing_country { get; set; }
        public string bin { get; set; }
        public string card_type { get; set; }
        public string issuer_info { get; set; }
    }

    public class CardDetailsRespObj
    {
        public string status { get; set; }
        public string message { get; set; }
        public CardDetails data { get; set; }
    }

    public class BvnDetails
    {
        public string bvn { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string date_of_birth { get; set; }
        public string phone_number { get; set; }
        public string registration_date { get; set; }
        public string enrollment_bank { get; set; }
        public string enrollment_branch { get; set; }
        public string gender { get; set; }
        public string nationality { get; set; }
    }

    public class BvnDetailsRespObj
    {
        public string status { get; set; }
        public string message { get; set; }
        public BvnDetails data { get; set; }
    }

    public class AccountDetails
    {
        public string account_number { get; set; }
        public string account_name { get; set; }
    }

    public class AccountDetailsRespObj
    {
        public string status { get; set; }
        public string message { get; set; }
        public AccountDetails data { get; set; }
    }
    public class AccountObj
    {
        public string account_number { get; set; }
        public string account_bank { get; set; }
    }

    public class TransferObj
    {
        public string account_bank { get; set; }
        public string account_number { get; set; }
        public int amount { get; set; }
        public string narration { get; set; }
        public string currency { get; set; }
        public string reference { get; set; }
        public string callback_url { get; set; }
        public string debit_currency { get; set; }
    }

    public class BulkData
    {
        public string bank_code { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string narration { get; set; }
        public string reference { get; set; }
        public string account_number { get; set; }
    }

    public class BulkTransferObj
    {
        public string title { get; set; }
        public List<BulkData> bulk_data { get; set; }
    }


    public class TransferDetails
    {
        public int id { get; set; }
        public string account_number { get; set; }
        public string bank_code { get; set; }
        public string full_name { get; set; }
        public DateTime created_at { get; set; }
        public string currency { get; set; }
        public string debit_currency { get; set; }
        public int amount { get; set; }
        public int fee { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public string narration { get; set; }
        public int requires_approval { get; set; }
        public int is_approved { get; set; }
        public string bank_name { get; set; }
    }

    public class TransferRespObj
    {
        public string status { get; set; }
        public string message { get; set; }
        //public TransferDetails data { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public string account_number { get; set; }
        public string bank_code { get; set; }
        public string full_name { get; set; }
        public DateTime created_at { get; set; }
        public string currency { get; set; }
        public string debit_currency { get; set; }
        public int amount { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public object meta { get; set; }
        public string narration { get; set; }
        public object approver { get; set; }
        public string complete_message { get; set; }
        public int requires_approval { get; set; }
        public int is_approved { get; set; }
        public string bank_name { get; set; }
    }

    public class GetTransferRespObj
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Authorization
    {
        public string mode { get; set; }
        public string pin { get; set; }
    }

    public class CardChargeObj
    {
        public string card_number { get; set; }
        public string cvv { get; set; }
        public string expiry_month { get; set; }
        public string expiry_year { get; set; }
        public string currency { get; set; }
        public string amount { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string tx_ref { get; set; }
        public string redirect_url { get; set; }
        public Authorization authorization { get; set; }
    }

    public class DirectDebitObj
    {
        public string tx_ref { get; set; }
        public string amount { get; set; }
        public string account_bank { get; set; }
        public string account_number { get; set; }
        public string currency { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string fullname { get; set; }
    }

    public class Customer
    {
        public string email { get; set; }
        public string phonenumber { get; set; }
        public string name { get; set; }
    }

    public class Customizations
    {
        public string title { get; set; }
        public string description { get; set; }
        public string logo { get; set; }
    }

    public class PaymentObj
    {
        public string tx_ref { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string redirect_url { get; set; }
        public string payment_options { get; set; }
        public Customer customer { get; set; }
        public Customizations customizations { get; set; }
    }

    public class Data
    {
        public string link { get; set; }
    }

    public class PaymentRespObj
    {
        public string status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class TransactionResponse
    {
        public int id { get; set; }
        public string tx_ref { get; set; }
        public string flw_ref { get; set; }
        public string device_fingerprint { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public int charged_amount { get; set; }
        public int app_fee { get; set; }
        public int merchant_fee { get; set; }
        public string processor_response { get; set; }
        public string auth_model { get; set; }
        public string ip { get; set; }
        public string narration { get; set; }
        public string status { get; set; }
        public string payment_type { get; set; }
        public DateTime created_at { get; set; }
        public int account_id { get; set; }
        public int amount_settled { get; set; }
    }

    public class TransactionVerificationRespObj
    {
        public string status { get; set; }
        public string message { get; set; }
        public TransactionResponse data { get; set; }
    }

}
