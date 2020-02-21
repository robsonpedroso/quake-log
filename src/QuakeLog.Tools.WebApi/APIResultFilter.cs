using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace QuakeLog.Tools.WebApi
{
    public class APIResultFilter : IAsyncActionFilter
    {
        /// <summary>
        /// Called asynchronously before the action, after model binding is complete and add APIResult to content.
        /// </summary>
        /// <param name="context">The Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext.</param>
        /// <param name="next">The Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate. 
        /// Invoked to execute the next action filter or the action itself.</param>
        /// <returns>A System.Threading.Tasks.Task that on completion indicates the filter has executed.</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            if (!context.ModelState.IsValid)
            {
                if (executedContext.HttpContext != null)
                    executedContext.HttpContext.Items.Add("IsHandledError", true);

                executedContext.Result = new BadRequestObjectResult(new APIResult(executedContext));
                executedContext.Exception = null;
            }
            else if (executedContext.Exception is ArgumentException arg)
            {
                if (executedContext.HttpContext != null)
                {
                    executedContext.HttpContext.Items.Add("Exception", arg);
                    executedContext.HttpContext.Items.Add("IsHandledError", true);
                }

                executedContext.Result = new BadRequestObjectResult(new APIResult(arg));
                executedContext.Exception = null;
            }
            else if (executedContext.Exception is Exception ex)
            {
                if (executedContext.HttpContext != null)
                {
                    ex.Data.Add("CorrelationId", executedContext.HttpContext.Request.Headers["CorrelationId"]);
                    executedContext.HttpContext.Items.Add("Exception", ex);
                    executedContext.HttpContext.Items.Add("IsHandledError", false);
                }

                executedContext.Result = new JsonResult(new APIResult(ex))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                executedContext.Exception = null;
            }
            else
            {
                executedContext.Result = new OkObjectResult(new APIResult(executedContext.Result));
            }
        }
    }
}
