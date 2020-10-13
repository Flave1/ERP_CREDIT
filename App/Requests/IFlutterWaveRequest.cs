using Banking.Contracts.Response.FlutterWave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Requests
{
    public interface IFlutterWaveRequest
    {
        Task<CardDetailsRespObj> validateCardDetails(string url);
        Task<BvnDetailsRespObj> validateBvnDetails(string url);
        Task<AccountDetailsRespObj> validateAccountDetails(AccountObj account);
        Task<TransferRespObj> createTransfer(TransferObj transfer);
        Task<TransferRespObj> createBulkTransfer(BulkTransferObj transfer);
        Task<PaymentRespObj> makePayment(PaymentObj payment);
        Task<GetTransferRespObj> getAllTransfer();
        Task<TransactionVerificationRespObj> transactionVerification(string url);
    }
}
