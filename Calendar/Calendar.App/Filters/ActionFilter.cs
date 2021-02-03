using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.App.Filters
{
    public class ActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(m => m.Errors.Select(e => e.ErrorMessage));
                
                throw new InvalidOperationException(string.Join(Environment.NewLine, errors));
            }

            await next();
        }
    }
}
