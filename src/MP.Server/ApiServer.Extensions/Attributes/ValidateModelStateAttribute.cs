using ApiServer.Model.Model.MsgModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace ApiServer.Extensions.Attributes
{
    /// <summary>
    /// https://github.com/FluentValidation/FluentValidation/issues/548
    /// https://stackoverflow.com/questions/45758024/use-custom-validation-responses-with-fluent-validation
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                        .SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage)
                        .ToList();

                context.Result = new JsonResult(MsgModel.Fail(errors.FirstOrDefault()))
                {
                    StatusCode = 400
                };
            }
        }
    }
}
