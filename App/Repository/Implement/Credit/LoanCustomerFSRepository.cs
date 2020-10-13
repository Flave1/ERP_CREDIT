using Banking.Contracts.Response.Credit;
using Banking.Data;
using Banking.DomainObjects.Credit;
using Banking.Repository.Interface.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Banking.Contracts.Response.Credit.LookUpViewObjs;

namespace Banking.Repository.Implement.Credit
{
    public class LoanCustomerFSRepository : ILoanCustomerFSRepository
    {
        private readonly DataContext _dataContext;
        public LoanCustomerFSRepository(DataContext context)
        {
            _dataContext = context;
        }
        public bool AddUpdateLoanCustomerFSCaption(LoanCustomerFSCaptionObj entity)
        {
            try
            {

                if (entity == null) return false;

                credit_loancustomerfscaption fsCaption = null;
                if (entity.FSCaptionId > 0)
                {
                    fsCaption = _dataContext.credit_loancustomerfscaption.Find(entity.FSCaptionId);
                    if (fsCaption != null)
                    {
                        fsCaption.FSCaptionName = entity.FSCaptionName;
                        fsCaption.FSCaptionGroupId = entity.FSCaptionGroupId;
                        fsCaption.Active = true;
                        fsCaption.Deleted = false;
                        fsCaption.UpdatedBy = entity.CreatedBy;
                        fsCaption.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    fsCaption = new credit_loancustomerfscaption
                    {
                        FSCaptionName = entity.FSCaptionName,
                        FSCaptionGroupId = entity.FSCaptionGroupId,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_loancustomerfscaption.Add(fsCaption);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AddUpdateLoanCustomerFSCaptionDetail(LoanCustomerFSCaptionDetailObj entity)
        {
            try
            {

                if (entity == null) return false;

                credit_loancustomerfscaptiondetail fsCaptionDetail = null;
                if (entity.FSDetailId > 0)
                {
                    fsCaptionDetail = _dataContext.credit_loancustomerfscaptiondetail.Find(entity.FSDetailId);
                    if (fsCaptionDetail != null)
                    {
                        fsCaptionDetail.FSCaptionId = entity.FSCaptionId;
                        fsCaptionDetail.FSDate = entity.FSDate;
                        fsCaptionDetail.Amount = entity.Amount;
                        fsCaptionDetail.CustomerId = entity.CustomerId;
                        fsCaptionDetail.Active = true;
                        fsCaptionDetail.Deleted = false;
                        fsCaptionDetail.UpdatedBy = entity.CreatedBy;
                        fsCaptionDetail.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    fsCaptionDetail = new credit_loancustomerfscaptiondetail
                    {
                        FSCaptionId = entity.FSCaptionId,
                        FSDate = entity.FSDate,
                        Amount = entity.Amount,
                        CustomerId = entity.CustomerId,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_loancustomerfscaptiondetail.Add(fsCaptionDetail);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AddUpdateLoanCustomerFSCaptionGroup(LoanCustomerFSCaptionGroupObj entity)
        {
            try
            {
                if (entity == null) return false;

                credit_loancustomerfscaptiongroup fsCaptionGroup = null;
                if (entity.FSCaptionGroupId > 0)
                {
                    fsCaptionGroup = _dataContext.credit_loancustomerfscaptiongroup.Find(entity.FSCaptionGroupId);
                    if (fsCaptionGroup != null)
                    {
                        fsCaptionGroup.FSCaptionGroupName = entity.FSCaptionGroupName;
                        fsCaptionGroup.Active = true;
                        fsCaptionGroup.Deleted = false;
                        fsCaptionGroup.UpdatedBy = entity.CreatedBy;
                        fsCaptionGroup.UpdatedOn = DateTime.Now;
                    }
                }
                else
                {
                    fsCaptionGroup = new credit_loancustomerfscaptiongroup
                    {
                        FSCaptionGroupName = entity.FSCaptionGroupName,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_loancustomerfscaptiongroup.Add(fsCaptionGroup);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool AddUpdateLoanCustomerFSRatioDetail(LoanCustomerFSRatioDetailObj entity)
        {
            try
            {

                if (entity == null) return false;

                credit_loancustomerratiodetail ratioDetail = null;

                if (!ValidateDescriptionRatio(entity.Description))
                {
                    return false;
                };

                ratioDetail = _dataContext.credit_loancustomerratiodetail.FirstOrDefault(x => x.RatioName.ToLower().TrimStart() == entity.RatioName.ToLower().TrimStart());
                if (ratioDetail != null)
                {

                        ratioDetail.Description = entity.Description;
                        ratioDetail.RatioDetailId = entity.RatioDetailId;
                        ratioDetail.RatioName = entity.RatioName;
                        ratioDetail.Active = true;
                        ratioDetail.Deleted = false;
                        ratioDetail.UpdatedBy = entity.CreatedBy;
                        ratioDetail.UpdatedOn = DateTime.Now;
                }
                else
                {
                    ratioDetail = new credit_loancustomerratiodetail
                    {
                        Description = entity.Description,
                        RatioDetailId = entity.RatioDetailId,
                        RatioName = entity.RatioName,
                        Active = true,
                        Deleted = false,
                        CreatedBy = entity.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    _dataContext.credit_loancustomerratiodetail.Add(ratioDetail);
                }

                var response = _dataContext.SaveChanges() > 0;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteLoanCustomerFSCaption(int fSCaptionId)
        {
            try
            {
                var fsCaptionGroup = _dataContext.credit_loancustomerfscaption.Find(fSCaptionId);
                if (fsCaptionGroup != null)
                {
                    fsCaptionGroup.Deleted = true;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteLoanCustomerFSCaptionDetail(int fSDetailId)
        {
            try
            {
                var fsCaptionDetail = _dataContext.credit_loancustomerfscaptiondetail.Find(fSDetailId);
                if (fsCaptionDetail != null)
                {
                    _dataContext.credit_loancustomerfscaptiondetail.Remove(fsCaptionDetail);
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteLoanCustomerFSCaptionGroup(int fSCaptionGroupId)
        {
            try
            {
                var fsCaptionGroup = _dataContext.credit_loancustomerfscaptiongroup.Find(fSCaptionGroupId);
                if (fsCaptionGroup != null)
                {
                    fsCaptionGroup.Deleted = true;
                }
                var res =  _dataContext.SaveChanges() > 0;
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteLoanCustomerFSRatioDetail(int ratioDetailId)
        {
            try
            {
                var ratio = _dataContext.credit_loancustomerratiodetail.Find(ratioDetailId);

                if (ratio != null)
                {
                    ratio.Deleted = true;
                }
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanCustomerFSCaptionObj> GetAllLoanCustomerFSCaption()
        {
            try
            {
                var fsCaption = (from a in _dataContext.credit_loancustomerfscaption
                                 join b in _dataContext.credit_loancustomerfscaptiongroup on a.FSCaptionGroupId equals b.FSCaptionGroupId
                                 where a.Deleted == false
                                 select new LoanCustomerFSCaptionObj
                                 {
                                     FSCaptionId = a.FSCaptionId,
                                     FSCaptionGroupId = a.FSCaptionGroupId,
                                     FSCaptionGroupName = b.FSCaptionGroupName,
                                     FSCaptionName = a.FSCaptionName
                                 }).ToList();

                return fsCaption;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanCustomerFSCaptionDetailObj> GetAllLoanCustomerFSCaptionDetail()
        {
            try
            {
                var fsCaption = (from a in _dataContext.credit_loancustomerfscaptiondetail
                                 where a.Deleted == false
                                 select new LoanCustomerFSCaptionDetailObj
                                 {
                                     CustomerId = a.CustomerId,
                                     FSDetailId = a.FSDetailId,
                                     FSCaptionId = a.FSCaptionId,
                                     FSCaptionName = a.credit_loancustomerfscaption.FSCaptionName,
                                     FSDate = a.FSDate,
                                     Amount = a.Amount,

                                 }).ToList();

                return fsCaption;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanCustomerFSCaptionGroupObj> GetAllLoanCustomerFSCaptionGroup()
        {
            try
            {
                var fsCaptionGroup = (from a in _dataContext.credit_loancustomerfscaptiongroup
                                      where a.Deleted == false
                                      select new LoanCustomerFSCaptionGroupObj
                                      {
                                          FSCaptionGroupId = a.FSCaptionGroupId,
                                          FSCaptionGroupName = a.FSCaptionGroupName,
                                      }).ToList();

                return fsCaptionGroup;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LookupObj> GetAllLoanCustomerFSDivisorType()
        {
            try
            {
                var ratio = (from a in _dataContext.credit_loancustomerfsratiodivisortype
                             where a.Deleted == false
                             select new LookupObj
                             {
                                 LookupId = a.DivisorTypeId,
                                 LookupName = a.DivisorTypeName
                             }).ToList();

                return ratio;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanCustomerFSRatioDetailObj> GetAllLoanCustomerFSRatioDetail()
        {
            try
            {
                var ratio = (from a in _dataContext.credit_loancustomerratiodetail
                             where a.Deleted == false
                             select new LoanCustomerFSRatioDetailObj
                             {
                                 Description = a.Description,
                                 RatioName = a.RatioName,
                                 RatioDetailId = a.RatioDetailId,

                             }).ToList();

                return ratio;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LookupObj> GetAllLoanCustomerFSValueType()
        {
            try
            {
                var ratio = (from a in _dataContext.credit_loancustomerfsratiovaluetype
                             where a.Deleted == false
                             select new LookupObj
                             {
                                 LookupId = a.ValueTypeId,
                                 LookupName = a.ValueTypeName

                             }).ToList();

                return ratio;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanCustomerFSRatioCaptionObj> GetFSRatioCaption()
        {
            var data = (from a in _dataContext.credit_loancustomerfscaption
                        where a.Deleted == false
                        select new LoanCustomerFSRatioCaptionObj
                        {
                            RatioCaptionId = a.FSCaptionId,
                            RatioCaptionName = a.FSCaptionName,

                        }).ToList();

            return data;
        }

        public IEnumerable<LoanCustomerFSCaptionObj> GetLoanCustomerFSCaptionByCaptionGroupId(int fSCaptionGroupId)
        {
            try
            {
                var fsCaption = (from a in _dataContext.credit_loancustomerfscaption
                                 join b in _dataContext.credit_loancustomerfscaptiongroup on a.FSCaptionGroupId equals b.FSCaptionGroupId
                                 where a.FSCaptionGroupId == fSCaptionGroupId && a.Deleted == false && b.Deleted == false
                                 select new LoanCustomerFSCaptionObj
                                 {
                                     FSCaptionId = a.FSCaptionId,
                                     FSCaptionGroupId = a.FSCaptionGroupId,
                                     FSCaptionGroupName = b.FSCaptionGroupName,
                                     FSCaptionName = a.FSCaptionName
                                 }).ToList();

                return fsCaption;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public LoanCustomerFSCaptionObj GetLoanCustomerFSCaptionByCaptionId(int fSCaptionId)
        {
            try
            {
                var fsCaptionGroup = (from a in _dataContext.credit_loancustomerfscaption
                                      join b in _dataContext.credit_loancustomerfscaptiongroup on a.FSCaptionGroupId equals b.FSCaptionGroupId
                                      where a.FSCaptionId == fSCaptionId && a.Deleted == false && b.Deleted == false
                                      select new LoanCustomerFSCaptionObj
                                      {
                                          FSCaptionId = a.FSCaptionId,
                                          FSCaptionGroupId = a.FSCaptionGroupId,
                                          FSCaptionGroupName = b.FSCaptionGroupName,
                                          FSCaptionName = a.FSCaptionName
                                      }).FirstOrDefault();

                return fsCaptionGroup;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public LoanCustomerFSCaptionDetailObj GetLoanCustomerFSCaptionDetailById(int fsDetailId)
        {
            try
            {
                var fsCaptionGroup = (from a in _dataContext.credit_loancustomerfscaptiondetail
                                      where a.FSDetailId == fsDetailId && a.Deleted == false
                                      select new LoanCustomerFSCaptionDetailObj
                                      {
                                          CustomerId = a.CustomerId,
                                          FSDetailId = a.FSDetailId,
                                          FSCaptionId = a.FSCaptionId,
                                          FSCaptionName = a.credit_loancustomerfscaption.FSCaptionName,
                                          FSDate = a.FSDate,
                                          Amount = a.Amount,

                                      }).FirstOrDefault();

                return fsCaptionGroup;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public LoanCustomerFSCaptionGroupObj GetLoanCustomerFSCaptionGroup(int fSCaptionGroupId)
        {
            try
            {
                var fsCaptionGroup = (from a in _dataContext.credit_loancustomerfscaptiongroup
                                      where a.FSCaptionGroupId == fSCaptionGroupId && a.Deleted == false
                                      select new LoanCustomerFSCaptionGroupObj
                                      {
                                          FSCaptionGroupId = a.FSCaptionGroupId,
                                          FSCaptionGroupName = a.FSCaptionGroupName,
                                      }).FirstOrDefault();

                return fsCaptionGroup;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanCustomerFSCaptionGroupObj> GetLoanCustomerFSCaptionGroup()
        {
            var data = (from a in _dataContext.credit_loancustomerfscaptiongroup
                        where a.Deleted == false
                        select new LoanCustomerFSCaptionGroupObj
                        {
                            FSCaptionGroupId = a.FSCaptionGroupId,
                            FSCaptionGroupName = a.FSCaptionGroupName,
                        }).ToList();
            return data.GroupBy(c => c.FSCaptionGroupId).Select(x => x.FirstOrDefault());
        }

        public IList<LoanCustomerFSCaptionObj> GetLoanCustomerFSCaptionGroupByGroupId(int fSCaptionGroupId)
        {
            try
            {
                var fsCaptions = (from a in _dataContext.credit_loancustomerfscaptiongroup
                                  join b in _dataContext.credit_loancustomerfscaption on a.FSCaptionGroupId equals b.FSCaptionGroupId
                                  where a.FSCaptionGroupId == fSCaptionGroupId && a.Deleted == false && b.Deleted == false
                                  select new LoanCustomerFSCaptionObj
                                  {
                                      FSCaptionId = b.FSCaptionId,
                                      FSCaptionName = b.FSCaptionName,
                                  }).ToList();
                return fsCaptions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public LoanCustomerFSRatioDetailObj GetLoanCustomerFSRatioDetail(int ratioDetailId)
        {
            try
            {
                var ratio = (from a in _dataContext.credit_loancustomerratiodetail
                             where a.RatioDetailId == ratioDetailId && a.Deleted == false
                             select new LoanCustomerFSRatioDetailObj
                             {
                                 RatioDetailId = a.RatioDetailId,
                                 Description = a.Description,
                                 RatioName = a.RatioName,
                             }).FirstOrDefault();

                return ratio;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LoanCustomerFSRatioDetailObj> GetLoanCustomerFSRatioDetail(int ratioCaptionId, int fsCaptionGroupId)
        {
            var data = (from a in _dataContext.credit_loancustomerratiodetail
                        where a.Deleted == false
                        select new LoanCustomerFSRatioDetailObj

                        {
                            RatioDetailId = a.RatioDetailId,
                            RatioName = a.RatioName,
                            Description = a.Description,

                        }).ToList();
            return data;
        }

        public IList<LoanCustomerFSRatiosCalculationObj> GetLoanCustomerFSRatiosCalculation(int customerId)
        {
            var customerFSDates = (from a in _dataContext.credit_loancustomerfscaptiondetail
                                   where a.CustomerId == customerId
                                   orderby a.FSDate descending
                                   select a.FSDate).Distinct();


            var lastFourDates = customerFSDates.OrderBy(x => x).Take(4).ToList();

            int count = lastFourDates.Count;

            var captionDetails = (from a in _dataContext.credit_loancustomerfscaptiondetail
                                  join b in _dataContext.credit_loancustomerfscaption on a.FSCaptionId equals b.FSCaptionId
                                  join c in _dataContext.credit_loancustomerfscaptiongroup on b.FSCaptionGroupId equals c.FSCaptionGroupId
                                  where a.Deleted == false && b.Deleted == false && c.Deleted == false
                                  select new { b.FSCaptionName, a }).ToList();

            var ratios = (from a in _dataContext.credit_loancustomerratiodetail where a.Deleted == false select a).ToList();

            List<LoanCustomerFSRatiosCalculationObj> output = new List<LoanCustomerFSRatiosCalculationObj>();
            foreach (var item in ratios)
            {
                LoanCustomerFSRatiosCalculationObj value = new LoanCustomerFSRatiosCalculationObj
                {
                    RatioName = item.RatioName,
                    RatioDetailId = item.RatioDetailId,
                    FsDate1 = count >= 4 ? lastFourDates[count - 4] : new DateTime(1900, 1, 1),
                    FsDate2 = count >= 3 ? lastFourDates[count - 3] : new DateTime(1900, 1, 1),
                    FsDate3 = count >= 2 ? lastFourDates[count - 2] : new DateTime(1900, 1, 1),
                    FsDate4 = count >= 1 ? lastFourDates[count - 1] : new DateTime(1900, 1, 1),
                    Ratio1 = count >= 4 ? GetCustomerFSRatioCalculation(customerId, item.RatioDetailId, lastFourDates[count - 4]) : "0.00",
                    Ratio2 = count >= 3 ? GetCustomerFSRatioCalculation(customerId, item.RatioDetailId, lastFourDates[count - 3]) : "0.00",
                    Ratio3 = count >= 2 ? GetCustomerFSRatioCalculation(customerId, item.RatioDetailId, lastFourDates[count - 2]) : "0.00",
                    Ratio4 = count >= 1 ? GetCustomerFSRatioCalculation(customerId, item.RatioDetailId, lastFourDates[count - 1]) : "0.00"
                };

                output.Add(value);
            }
            return output;
        }

        public List<LoanCustomerFSRatioCaptionReportObj> GetLoanCustomerFSRatioValues(int customerId)
        {
            var customerFSDates = (from a in _dataContext.credit_loancustomerfscaptiondetail
                                   where a.CustomerId == customerId
                                   orderby a.FSDate descending
                                   select a.FSDate).Distinct();


            var lastFourDates = customerFSDates.OrderBy(x => x).Take(4).ToList();

            int count = lastFourDates.Count;

            var ratioCaptions = (from a in _dataContext.credit_loancustomerfscaption
                                 join b in _dataContext.credit_loancustomerfscaptiongroup on a.FSCaptionGroupId equals b.FSCaptionGroupId
                                 where a.Deleted == false && b.Deleted == false
                                 select new { b.FSCaptionGroupName, a }).ToList();

            List<LoanCustomerFSRatioCaptionReportObj> output = new List<LoanCustomerFSRatioCaptionReportObj>();
            foreach (var item in ratioCaptions)
            {
                LoanCustomerFSRatioCaptionReportObj value = new LoanCustomerFSRatioCaptionReportObj
                {
                    RatioCaptionId = (short)item.a.FSCaptionId,
                    RatioCaptionName = item.a.FSCaptionName,
                    FsGroupCaption = item.FSCaptionGroupName,
                    FsDate1 = count >= 4 ? lastFourDates[count - 4] : new DateTime(1900, 1, 1),
                    FsDate2 = count >= 3 ? lastFourDates[count - 3] : new DateTime(1900, 1, 1),
                    FsDate3 = count >= 2 ? lastFourDates[count - 2] : new DateTime(1900, 1, 1),
                    FsDate4 = count >= 1 ? lastFourDates[count - 1] : new DateTime(1900, 1, 1),
                    RatioValue1 = count >= 4 ? GetCustomerFSRatio(customerId, (short)item.a.FSCaptionId, lastFourDates[count - 4]) : "0.00",
                    RatioValue2 = count >= 3 ? GetCustomerFSRatio(customerId, (short)item.a.FSCaptionId, lastFourDates[count - 3]) : "0.00",
                    RatioValue3 = count >= 2 ? GetCustomerFSRatio(customerId, (short)item.a.FSCaptionId, lastFourDates[count - 2]) : "0.00",
                    RatioValue4 = count >= 1 ? GetCustomerFSRatio(customerId, (short)item.a.FSCaptionId, lastFourDates[count - 1]) : "0.00"
                };

                if (Convert.ToDecimal(value.RatioValue1) > 0 || Convert.ToDecimal(value.RatioValue2) > 0 ||
                    Convert.ToDecimal(value.RatioValue3) > 0 || Convert.ToDecimal(value.RatioValue4) > 0)
                {
                    output.Add(value);
                }
            }
            return output;
        }

        public IEnumerable<LoanCustomerFSCaptionDetailObj> GetMappedCustomerFsCaptionDetail(int customerId, short fsCaptionGroupId, DateTime fsDate)
        {
            var data = (from a in _dataContext.credit_loancustomerfscaptiondetail
                        join b in _dataContext.credit_loancustomerfscaption on a.FSCaptionId equals b.FSCaptionId
                        where a.CustomerId == customerId && b.FSCaptionGroupId == fsCaptionGroupId && a.FSDate == fsDate
                        && a.Deleted == false
                        orderby a.FSDate, b.FSCaptionGroupId
                        select new LoanCustomerFSCaptionDetailObj
                        {
                            CustomerId = a.CustomerId,
                            FSDetailId = a.FSDetailId,
                            FSCaptionId = a.FSCaptionId,
                            FSCaptionName = b.FSCaptionName,
                            FSDate = a.FSDate,
                            Amount = a.Amount,
                        }).ToList();
            return data;
        }

        public IEnumerable<LoanCustomerFSCaptionDetailObj> GetMappedCustomerFsCaptions(int customerId)
        {
            var data = (from a in _dataContext.credit_loancustomerfscaptiondetail
                        join b in _dataContext.credit_loancustomerfscaption on a.FSCaptionId equals b.FSCaptionId
                        join c in _dataContext.credit_loancustomerfscaptiongroup on b.FSCaptionGroupId equals c.FSCaptionGroupId
                        where a.CustomerId == customerId
                        && a.Deleted == false
                        orderby a.FSDate
                        select new LoanCustomerFSCaptionDetailObj
                        {
                            CustomerId = a.CustomerId,
                            FSDetailId = a.FSDetailId,
                            FSCaptionId = a.FSCaptionId,
                            FSCaptionName = b.FSCaptionName,
                            FSDate = a.FSDate,
                            Amount = a.Amount,
                            FSGroupName = c.FSCaptionGroupName
                        }).ToList();
            return data;
        }

        public IEnumerable<LoanCustomerFSCaptionObj> GetUnmappedLoanCustomerFSCaption(short fSCaptionGroupId, int customerId, DateTime fsDate)
        {
            var dataList = (from data in _dataContext.credit_loancustomerfscaptiondetail
                            where data.credit_loancustomerfscaption.FSCaptionGroupId == fSCaptionGroupId && data.CustomerId == customerId
                            && data.FSDate.Date == fsDate.Date && data.Deleted == false
                            select data.FSCaptionId).ToList();

            var captions = (from a in _dataContext.credit_loancustomerfscaption
                            join b in _dataContext.credit_loancustomerfscaptiongroup on a.FSCaptionGroupId equals b.FSCaptionGroupId
                            where a.FSCaptionGroupId == fSCaptionGroupId && a.Deleted == false
                            select new LoanCustomerFSCaptionObj
                            {
                                FSCaptionId = a.FSCaptionId,
                                FSCaptionName = a.FSCaptionName,
                                FSCaptionGroupId = a.FSCaptionGroupId,
                                FSCaptionGroupName = b.FSCaptionGroupName,
                            });

            if (dataList.Any())
            {
                captions = captions.Where(x => !dataList.Contains(x.FSCaptionId));
            }

            return captions;
        }

        public Task MultipleDeleteLoanCustomerFSCaption(params int?[] targetIds)
        {
            throw new NotImplementedException();
        }

        public Task MultipleDeleteLoanCustomerFSCaptionGroup(params int[] targetIds)
        {
            throw new NotImplementedException();
        }

        public bool ValidateFSCaption(string captionName)
        {
            var exist = from a in _dataContext.credit_loancustomerfscaption where a.FSCaptionName == captionName select a;
            if (exist.Any())
            {
                return true;
            }
            return false;
        }

        public bool ValidateFSCaptionGroup(string captionGroupName)
        {
            var exist = from a in _dataContext.credit_loancustomerfscaptiongroup where a.FSCaptionGroupName == captionGroupName select a;
            if (exist.Any())
            {
                return true;
            }
            return false;
        }


        ////PRIVATE METHODS
        ///
          private bool ValidateDescriptionRatio(string description)
          {
            var allCaptionNames = _dataContext.credit_loancustomerfscaption.Where(x => x.Deleted == false).ToList();

            foreach(var caption in allCaptionNames)
            {
                var capName = "[" + caption.FSCaptionName+ "]";
                var capValue = "1";

                if (description.Contains(capName))
                {
                    description = description.Replace(capName, capValue);
                };
            }

            var afterTrim = description.TrimStart();
            //var expression = new Expression(description);
            //double result = expression.calculate();

            var val1 = afterTrim.Split(" ")[0];
            var symbol = afterTrim.Split(" ")[1];
            var val2 = afterTrim.Split(" ")[2];
            double result = 0;

            if (symbol == "/")
            {
                result = double.Parse(val1) / double.Parse(val2);
            }
            if(symbol == "-")
            {
                result = double.Parse(val1) - double.Parse(val2);
            }

            if(symbol == "+")
            {
                result = double.Parse(val1) + double.Parse(val2);
            }

            if (symbol == "*")
            {
                result = double.Parse(val1) * double.Parse(val2);
            }

            if (double.IsNaN(result) || double.IsInfinity(result))
            {
                return false;
            }

            return true;
        }
        private string GetCustomerFSRatioCalculation(int customerId, int ratioDetailId, DateTime fsDate)
        {
            var captionValues = (from a in _dataContext.credit_loancustomerfscaptiondetail
                                 where a.CustomerId == customerId && a.FSDate == fsDate
                                 select a).ToList();
            var ratio = (from a in _dataContext.credit_loancustomerratiodetail
                         where a.RatioDetailId == ratioDetailId && a.Deleted == false
                         select a).FirstOrDefault();
            var allCaptionNames = _dataContext.credit_loancustomerfscaption.Where(x => x.Deleted == false).ToList();
            var description = ratio.Description;

            foreach (var caption in allCaptionNames)
            {
                var capName = "[" + caption.FSCaptionName + "]";
                var capValue = captionValues.FirstOrDefault(x => x.FSCaptionId == caption.FSCaptionId);

                if (description.Contains(capName) && capValue != null)
                {
                    description = description.Replace(capName, capValue.Amount.ToString());
                };
            }

            //var expression = new Expression(description);

            //var result = expression.calculate();
            //double result = Convert.ToDouble(description);
            var afterTrim = description.TrimStart();

            var val1 = afterTrim.Split(" ")[0];
            var symbol = afterTrim.Split(" ")[1];
            var val2 = afterTrim.Split(" ")[2];
            double result = 0;

            if (symbol == "/")
            {
                result = double.Parse(val1) / double.Parse(val2);
            }
            if (symbol == "-")
            {
                result = double.Parse(val1) - double.Parse(val2);
            }

            if (symbol == "+")
            {
                result = double.Parse(val1) + double.Parse(val2);
            }

            if (symbol == "*")
            {
                result = double.Parse(val1) * double.Parse(val2);
            }

            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                return "0.00";
            }

            return result.ToString();
        }

        private string GetCustomerFSRatio(int customerId, short fsCaptionId, DateTime fsDate)
        {
            var customerFS = (from a in _dataContext.credit_loancustomerfscaptiondetail
                              where a.CustomerId == customerId && a.FSDate == fsDate && a.FSCaptionId == fsCaptionId
                              select a).FirstOrDefault();

            if (customerFS == null)
            {
                return "0.00";
            }

            return customerFS.Amount.ToString();
        }
    }
}
