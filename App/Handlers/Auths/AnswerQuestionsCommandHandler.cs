using Banking.Contracts.Response;
using Banking.Data;
using Banking.DomainObjects.Auth;
using Banking.Requests;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Handlers.Auths
{
    public class AnswerQuestionsCommand : IRequest<QuestionsRegRespObj>
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public string UserName { get; set; }
        public class AnswerQuestionsCommandHandler : IRequestHandler<AnswerQuestionsCommand, QuestionsRegRespObj>
        {
            private readonly ILoggerService _logger;
            private readonly DataContext _data;
            private readonly UserManager<ApplicationUser> _userManager;
            public AnswerQuestionsCommandHandler(
                ILoggerService loggerService,
                DataContext data,
                UserManager<ApplicationUser> userManager)
            {
                _logger = loggerService;
                _data = data;
                _userManager = userManager;
            }
            public async Task<QuestionsRegRespObj> Handle(AnswerQuestionsCommand request, CancellationToken cancellationToken)
            {
                var response = new QuestionsRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };

                try
                {
                    var user = await _userManager.FindByNameAsync(request.UserName);
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(user.SecurityAnswered))
                        {
                            if (user.SecurityAnswered.Trim().ToLower() == request.Answer.Trim().ToLower())
                            {
                                user.IsItQuestionTime = false;
                                user.EnableAtThisTime = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(10));
                                await _userManager.UpdateAsync(user);
                            }
                            else
                            {
                                response.Status.IsSuccessful = false;
                                response.Status.Message.FriendlyMessage = $"Unable to Identify user account with this answer";
                                return response;
                            }
                        }
                        else
                        {
                            response.Status.IsSuccessful = false;
                            response.Status.Message.FriendlyMessage = $"No Security questions found";
                            return response;
                        }
                    }
                    else
                    {
                        response.Status.IsSuccessful = false;
                        response.Status.Message.FriendlyMessage = $"Invalid user Name";
                        return response;
                    }

                    response.Status.IsSuccessful = true;
                    response.Status.Message.FriendlyMessage = $"Successfully";
                    return response;
                }
                catch (Exception ex)
                {
                    #region Log error to file 
                    var errorCode = ErrorID.Generate(4);
                    _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");

                    response.Status.Message.FriendlyMessage = "Error occured!! Unable to process request";
                    response.Status.Message.TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}";
                    return response;
                    #endregion
                }
            }
        }
    }


    public class GetQuestionQuery : IRequest<SingleQuestionsRespObj>
    {
        public string UserName { get; set; } 
        public class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, SingleQuestionsRespObj>
        {
            private readonly ILoggerService _logger;
            private readonly DataContext _data;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IIdentityServerRequest _serverRequest;
            public GetQuestionQueryHandler(
                ILoggerService loggerService,
                DataContext data,
                UserManager<ApplicationUser> userManager,
                IIdentityServerRequest serverRequest)
            {
                _logger = loggerService;
                _data = data;
                _serverRequest = serverRequest;
                _userManager = userManager;
            }
            public async Task<SingleQuestionsRespObj> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
            {
                var response = new SingleQuestionsRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };

                try
                {
                    var solList = new List<QuestionsObj>();
                    var user = await _userManager.FindByNameAsync(request.UserName);
                    var Questionss = new QuestionsObj();
                    if (user != null)
                    {
                        
                        if (!string.IsNullOrEmpty(user.SecurityAnswered))
                        {
                            var Questions = await _serverRequest.GetQuestionsAsync();
                            
                            if (Questions.Questions.Count() > 0)
                            {
                                var actuaulQuestion = Questions.Questions.FirstOrDefault(d => d.QuestionId == user.QuestionId);
                                
                                Questionss.QuestionId = actuaulQuestion.QuestionId;
                                Questionss.Qiestion = actuaulQuestion.Qiestion;
                            }
                        }
                        else
                        {
                            response.Status.IsSuccessful = false;
                            response.Status.Message.FriendlyMessage = $"No Security questions found";
                            return response;
                        }
                    }
                    else
                    {
                        response.Status.IsSuccessful = false;
                        response.Status.Message.FriendlyMessage = $"Invalid user Name";
                        return response;
                    }

                    response.Status.IsSuccessful = true;
                    response.Question = Questionss;
                    response.Status.Message.FriendlyMessage = $"Successfully";
                    return response;
                }
                catch (Exception ex)
                {
                    #region Log error to file 
                    var errorCode = ErrorID.Generate(4);
                    _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");

                    response.Status.Message.FriendlyMessage = "Error occured!! Unable to process request";
                    response.Status.Message.TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}";
                    return response;
                    #endregion
                }
            }
        }
    }
}
