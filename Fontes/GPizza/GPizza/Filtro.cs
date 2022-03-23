using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Importando package para session
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//Para os filtros funcionarem...
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GPizza
{
    public class Filtro : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //é disparado após a execução do controller/action 
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //é disparado antes do controller/action ser executado

            //liberando o descritor/decoretor/annotation AllowAnonymous
            foreach (var descritor in context.ActionDescriptor.FilterDescriptors)
            {
                if (descritor.Filter.GetType() == typeof(AllowAnonymousFilter))
                {
                    return;
                }
            }

            if (context.HttpContext.Session.GetString("fun_codigo") == null)
            {
                context.Result = new RedirectResult("~/Login");
            }

        }
    }
}
