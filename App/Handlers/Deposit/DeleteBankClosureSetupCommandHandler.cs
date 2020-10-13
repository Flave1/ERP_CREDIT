using Banking.Contracts.Command;
using Banking.Contracts.Response;
using Banking.Contracts.Response.Deposit;
using Banking.Data; 
using Banking.Repository.Interface.Deposit;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Handlers.Deposit
{
    public class DeleteBankClosureSetupCommandHandler : IRequestHandler<DeleteBankClosureSetupCommand, DeleteRespObj>
    {
        private readonly IDepositBankClosure _depositBankClosure;
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        public DeleteBankClosureSetupCommandHandler(ILoggerService loggerService, IDepositBankClosure depositBankClosure, DataContext dataContext)
        {
            _depositBankClosure = depositBankClosure;
            _logger = loggerService;
            _dataContext = dataContext;
        }
        public async Task<DeleteRespObj> Handle(DeleteBankClosureSetupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using (var transaction = _dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (request.BankClosureSetupIds.Count() > 0)
                        {
                            foreach (var itemId in request.BankClosureSetupIds)
                            {
                                var item = await _dataContext.deposit_bankclosuresetup.FindAsync(itemId);
                                if (item != null)
                                {
                                    _depositBankClosure.DeleteBankClosureSetup(item.BankClosureSetupId);
                                }
                                else
                                {
                                    #region Any thing
                                    return new DeleteRespObj
                                    {
                                        Deleted = false,
                                        Status = new APIResponseStatus
                                        {
                                            IsSuccessful = false,
                                            Message = new APIResponseMessage
                                            {
                                                FriendlyMessage = $"No Item with this Specifier {itemId}"
                                            }
                                        }
                                    };
                                    #endregion
                                }
                            }
                            transaction.Commit();

                        }
                        return new DeleteRespObj
                        {
                            Deleted = true,
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = true,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "Successful"
                                }
                            }
                        };
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new DeleteRespObj
                        {
                            Deleted = false,
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "Unsuccessful"
                                }
                            }
                        };

                    }
                    finally { transaction.Dispose(); }


                }
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : DeleteBankClosureSetupCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new DeleteRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : DeleteBankClosureSetupCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
    }
}
