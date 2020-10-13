using Banking.DomainObjects.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.ProductObjs;

namespace Banking.Repository.Interface.Credit
{
    public interface IProduct
    {
        //Product
        //Task<int> AddUpdateProductAsync(credit_product model);
        ProductObj AddUpdateProduct(ProductObj entity);
        IEnumerable<ProductObj> GetAllProduct();
        ProductRegRespObj UploadProduct(List<byte[]> record, string createdBy);
        byte[] GenerateExportProduct();
        ProductObj GetProduct(int productId);
        Task<bool> DeleteProduct(int productId);

        //Product Type
        bool AddUpdateProductType(ProductTypeObj entity);
        Task<IEnumerable<credit_producttype>> GetAllProductTypeAsync();
        Task<credit_producttype> GetProductTypeByIdAsync(int id);
        Task<bool> DeleteProductTypeAsync(int id);
        Task<bool> UploadProductTypeAsync(List<byte[]> record, string createdBy);
        byte[] GenerateExportProductType();

        //Product Fee
        Task<bool> DeleteProductFeeAsync(int id);
        Task<ProductFeeRegRespObj> UploadProductFee(List<byte[]> record, string createdBy);
        Task<bool> UpdateProductFee(credit_productfee model);
        IEnumerable<ProductFeeObj> GetAllProductFee();
        ProductFeeObj GetProductFee(int productFeeId);
        List<ProductFeeObj> GetProductFeeByProduct(int productId);
        List<ProductFeeObj> GetProductFeeByLoanApplicationId(int Id);
        byte[] GenerateExportProductFee(int productId);


        //Others
        int GetTenorByPeriodAndFrequency(int period, int frequencyId);

        Task<IEnumerable<ProductObj>> GetProductsAsync();
    }
}
