using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SpaceDC.Services;

public sealed class AppExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not AppException app)
            return;

        context.Result = new ObjectResult(new { message = app.Message })
        {
            StatusCode = app.StatusCode
        };
        context.ExceptionHandled = true;
    }
}
