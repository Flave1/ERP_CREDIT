using Banking.Contracts.Response.Credit;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Repository.Interface.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.ExposureObjs;

namespace Banking.Repository.Implement.Credit
{
    public class ExposureRepository : IExposureParameter
    {
        private readonly DataContext _dataContext;
        public ExposureRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> AddUpdateExposureParameter(ExposureObj entity)
        {
            try
            {
                if (entity == null) return false;

                if (entity.ExposureParameterId > 0)
                {
                    var exposureExist = _dataContext.credit_exposureparament.Find(entity.ExposureParameterId);
                    if (exposureExist != null)
                    {
                        exposureExist.CustomerTypeId = entity.CustomerTypeId;
                        exposureExist.Description = entity.Description;
                        exposureExist.Percentage = entity.Percentage;
                        exposureExist.ShareHolderAmount = entity.ShareHolderAmount;
                        exposureExist.Amount = (entity.Percentage / 100) * entity.ShareHolderAmount;
                        exposureExist.Active = true;
                        exposureExist.Deleted = false;
                        exposureExist.UpdatedBy = entity.CreatedBy;
                        exposureExist.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    var exposure = new credit_exposureparament
                    {
                        ExposureParameterId = entity.ExposureParameterId,
                        CustomerTypeId = entity.CustomerTypeId,
                        Description = entity.Description,
                        Percentage = entity.Percentage,
                        ShareHolderAmount = entity.ShareHolderAmount,
                        Amount = (entity.Percentage / 100) * entity.ShareHolderAmount,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_exposureparament.Add(exposure);
                }

                var response = await _dataContext.SaveChangesAsync() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteExposureParameter(int id)
        {
            var itemToDelete =  _dataContext.credit_exposureparament.Find(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return  _dataContext.SaveChanges() > 0;
        }

        public IEnumerable<ExposureObj> GetExposureParameter()
        {
            try
            {
                var entity = (from a in _dataContext.credit_exposureparament
                              where a.Deleted == false
                              select new ExposureObj
                              {
                                  ExposureParameterId = a.ExposureParameterId,
                                  CustomerTypeId = a.CustomerTypeId,
                                  Description = a.Description,
                                  Percentage = a.Percentage,
                                  ShareHolderAmount = a.ShareHolderAmount,
                                  CustomerTypeName = a.CustomerTypeId == 1 ? "Individual" : "Corporate",
                              }).ToList();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ExposureObj GetExposureParameterByID(int id)
        {
            try
            {
                var entity = (from a in _dataContext.credit_exposureparament
                              where a.ExposureParameterId == id && a.Deleted == false
                              select new ExposureObj
                              {
                                  ExposureParameterId = a.ExposureParameterId,
                                  CustomerTypeId = a.CustomerTypeId,
                                  Description = a.Description,
                                  Percentage = a.Percentage,
                                  ShareHolderAmount = a.ShareHolderAmount,
                              }).FirstOrDefault();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
