using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using toDoList.ViewModels;

namespace toDoList.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpSattusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch(statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found";
                    ViewBag.Path = statusCodeResult.OriginalPath;
                    ViewBag.QS = statusCodeResult.OriginalQueryString;
                    break;
            }

            //return View("NotFound");
            return View("~/Views/Error/NotFound.cshtml");

        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ExceptionPath = exceptionDetails.Path;
            ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            ViewBag.Stacktrace = exceptionDetails.Error.Message;
            return View("Error");
        }
         [AllowAnonymous]
        [Route("Error/GeneralError")]
        [HttpGet]
        public IActionResult GeneralError(string Signal, string ErrorTitle,string ErrorMessage, string UrlToRedirect, string optionalData, string StringButton)
        {
            GeneralErrorViewModel model = new GeneralErrorViewModel();
            model.Signal = Signal;
            model.ErrorTitle = ErrorTitle;
            model.ErrorMessage = ErrorMessage;
            model.UrlToRedirect = UrlToRedirect;
            model.optionalData = optionalData;
            model.StringButton = StringButton;
            return this.PartialView("~/Views/Error/GeneralErrorModel.cshtml",model);
        }
    }
}
