using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Globalization;

namespace Deixar.API.Commons
{
    public class LogMethodAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string controllerName = context.Controller.GetType().Name;
            string actionName = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            Log.Logger.Information("{controllerName} -> {actionName} -> Executing - {DT}", controllerName, actionName, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            string controllerName = context.Controller.GetType().Name;
            string actionName = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            Log.Logger.Information("{controllerName} -> {actionName} -> Executed - {DT}\n", controllerName, actionName, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
        }
    }
}
