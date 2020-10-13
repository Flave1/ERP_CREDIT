using Banking.Contracts.Response.InvestorFund;
using Banking.DomainObjects.InvestorFund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.Repository.Interface.InvestorFund
{
    public interface IProductService
    {
        #region Product
        bool AddUpdateProduct(inf_product model);
        bool DeleteProduct(int id);
        byte[] GenerateExportProduct();
        List<InfProductObj> GetAllProduct();
        InfProductObj GetProduct(int Id);
        InfProductRegRespObj UploadProduct(List<byte[]> record, string createdBy);
        #endregion

        #region ProductType
        bool AddUpdateProductType(inf_producttype model);
        bool DeleteProductType(int id);
        byte[] GenerateExportProductType();
        List<InfProductTypeObj> GetAllProductType();
        InfProductTypeObj GetProductType(int Id);
        InfProductTypeRegRespObj UploadProductType(List<byte[]> record, string createdBy);
        #endregion
    }
}
