using System.Net;
using Application.Common.Exceptions;
using Domain;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class GlobalValidateModelAttribute : IActionFilter
{
    private readonly ILogger<GlobalValidateModelAttribute> _Logger;

    public GlobalValidateModelAttribute(ILogger<GlobalValidateModelAttribute> logger)
    {
        _Logger = logger;
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No es necesario implementar este método para un filtro de acción de validación global.
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;
        var errorMessage = GenerateErrorMessage(context);
        var exception = new BadRequestException(errorMessage);
        _Logger.LogError(exception, exception.Message, exception.StackTrace);
        var errorResponse = new ErrorResponse((int)HttpStatusCode.BadRequest, exception.Message, exception.GetType().Name);
        context.Result = new ObjectResult(errorResponse)
        {
            StatusCode = errorResponse.Code
        };
    }

    private static string GenerateErrorMessage(ActionContext context)
    {
        var keyError = context.ModelState.Keys.Last();
        var errorMessage = string.Format(Messages.BadRequestException, keyError);
        return errorMessage;
    }
}