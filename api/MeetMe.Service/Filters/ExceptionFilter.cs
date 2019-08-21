using System;
using System.Collections.Generic;
using System.Net;
using FluentValidation;
using MeetMe.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MeetMe.Service.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";

            var errors = new Dictionary<string, string>();
            switch (context.Exception)
            {
                case ValidationException ve:
                    foreach (var error in ve.Errors)
                        if (!errors.TryAdd(error.PropertyName, error.ErrorMessage))
                            errors[error.PropertyName] = $"{errors[error.PropertyName]}; {error.ErrorMessage}";
                    context.Result = new BadRequestObjectResult(errors);
                    break;
                case UnauthorizedAccessException _:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errors.Add("error", context.Exception.Message);
                    context.Result = new JsonResult(errors);
                    break;
                case ForbiddenException _:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    errors.Add("error", context.Exception.Message);
                    context.Result = new JsonResult(errors);
                    break;
                case NotFoundException _:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    errors.Add("error", context.Exception.Message);
                    context.Result = new JsonResult(errors);
                    break;
                default:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errors.Add("error", context.Exception.Message);
                    context.Result = new JsonResult(errors);
                    break;
            }
        }
    }    
}