using Banking.DomainObjects.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.ExposureObjs;

namespace Banking.Repository.Interface.Credit
{
    public interface IExposureParameter
    {
        ExposureObj GetExposureParameterByID(int id);
        IEnumerable<ExposureObj> GetExposureParameter();
        Task<bool> AddUpdateExposureParameter(ExposureObj entity);
        bool DeleteExposureParameter(int id);
    }
}
