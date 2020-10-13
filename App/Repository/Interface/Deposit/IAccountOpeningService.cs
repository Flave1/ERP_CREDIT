using Banking.Contracts.Response.Deposit;
using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.Deposit
{
    public interface IAccountOpeningService
    {
        #region CustomerDetails
        IEnumerable<CustomerDetailsObj> GetAllCustomerLite();
        DepositAccountOpeningObj GetCustomerDetails(int CustomerId);
        Task<int> AddUpdateCustomerAsync(DepositAccountOpeningObj model);
        Task<bool> DeleteCustomerAsync(int id);
        IEnumerable<CustomerDetailsObj> GetAllCustomerCasaList();
        #endregion

        #region DocumentUpload
        Task<bool> AddUpdateKYCustomerDocAsync(deposit_customerkycdocumentupload model);
        Task<bool> DeleteKYCustomerDocAsync(int id);
        Task<deposit_customerkycdocumentupload> GetKYCustomerDocByIdAsync(int id);
        Task<IEnumerable<deposit_customerkycdocumentupload>> GetAllKYCustomerDocAsync();

        #endregion

        #region SignatureUpload
        Task<bool> AddUpdateSignatureAsync(deposit_customersignature model);
        Task<bool> DeleteSignatureAsync(int id);
        Task<deposit_customersignature> GetSignatureByIdAsync(int id);
        Task<deposit_customersignature> GetSignaturesByIdsAsync(int id, int sid);
        Task<IEnumerable<deposit_customersignature>> GetAllSignatureAsync();

        #endregion

        #region Signatory
        Task<bool> AddUpdateSignatoryAsync(deposit_customersignatory model);
        #endregion

        #region Signatory SignatureUpload
        public bool SignatoryUpload(byte[] image, CustomerSignatureObj entity);

        Task<bool> DeleteSignatoryAsync(int id);

        Task<deposit_customersignatory> GetSignatoryByIdAsync(int id);

        Task<IEnumerable<deposit_customersignatory>> GetAllSignatoryAsync(int cid);

        #endregion

        #region Directors
        public int AddUpdateDirector(CustomerDirectorsObj entity);
        #endregion

        #region Director SignatureUpload
        public bool DirectorsignatureUpload(byte[] image, DirectorSignatureObj entity);

        Task<bool> DeleteDirectorsAsync(int id);

        Task<deposit_customerdirectors> GetDirectorByIdAsync(int id);

        Task<IEnumerable<deposit_customerdirectors>> GetAllDirectorsAsync(int cid);

        #endregion

        #region KYCustomer
        Task<bool> AddUpdateKYCustomerAsync(deposit_customerkyc model);

        Task<bool> DeleteKYCustomerAsync(int id);

        Task<deposit_customerkyc> GetKYCustomerByIdAsync(int id);

        Task<IEnumerable<deposit_customerkyc>> GetAllKYCustomerAsync(int cid);

        #endregion

        #region ContactPersons
        Task<bool> AddUpdateContactPersonsAsync(deposit_customercontactpersons model);

        Task<bool> DeleteContactPersonsAsync(int id);

        Task<deposit_customercontactpersons> GetContactPersonsByIdAsync(int id);

        Task<IEnumerable<deposit_customercontactpersons>> GetContactPersonsAsync(int cid);
        #endregion

        #region NextOfKin
        Task<bool> AddUpdateNextOfKinAsync(deposit_customernextofkin model);

        Task<bool> DeleteNextOfKinAsync(int id);

        Task<deposit_customernextofkin> GetNextOfKinByIdAsync(int id);

        Task<IEnumerable<deposit_customernextofkin>> GetAllNextOfKinAsync(int cid);

        #endregion

        #region Identity Details
        Task<bool> AddUpdateIdentityDetailsAsync(deposit_customeridentification model);

        Task<bool> DeleteIdentityDetailsAsync(int id);

        Task<deposit_customeridentification> GetIdentityDetailsByIdAsync(int id);

        Task<IEnumerable<deposit_customeridentification>> GetAllIdentityDetailsAsync(int cid);

        #endregion

        #region DepositForm
        object SignatoryUpload(byte[] image, string createdBy);
        Task DirectorsignatureUpload(byte[] image, string createdBy);
        #endregion
    }
}
