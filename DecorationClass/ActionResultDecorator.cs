using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcreteDecorator.DecorationClass
{
    public abstract class ActionResultDecorator : IActionResult
    {
        public IActionResult ActionResult { get; set; }

        public ActionResultDecorator(IActionResult _actionResult)
        {
            ActionResult = _actionResult;
        }
        public virtual Task ExecuteResultAsync(ActionContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class ActionResultErrorDec : ActionResultDecorator
    {
        public string ErrorType { get; }
        public string Title { get; }
        public string Body { get; }

        public ActionResultErrorDec(IActionResult _actionResult, string _errorType, string _title, string _body) : base(_actionResult)
        {
            ErrorType = _errorType;
            Title = _title;
            Body = _body;
        }


        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var factory = context.HttpContext.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
            var tempData = factory.GetTempData(context.HttpContext);

            tempData.AddAlert(new Allert(ErrorType, Title, Body));

            await ActionResult.ExecuteResultAsync(context);
        }

    }
    public static class AlertExtensions
    {
        private static AlertType _type { get; set; }
        public static IActionResult With(this IActionResult result, string title, string body, AlertType type)
        {
            _type = type;
            return Alert(result, "alert alert-" + type.ToString(), title, body);
        }


        public static List<Allert> GetAlertList(this ITempDataDictionary tempData, string value) =>
             value != null ? JsonConvert.DeserializeObject<List<Allert>>(value) : null;

        private static string Alerts = _type.ToString();

        public static List<Allert> GetAlert(this ITempDataDictionary tempData)
        {
            CreateAlertTempData(tempData);
            return DeserializeAlerts(tempData[Alerts] as string);
        }

        public static void CreateAlertTempData(this ITempDataDictionary tempData)
        {
            if (!tempData.ContainsKey(Alerts))
            {
                tempData[Alerts] = "";
            }
        }

        public static void AddAlert(this ITempDataDictionary tempData, Allert alert)
        {
            if (alert == null)
            {
                throw new ArgumentNullException(nameof(alert));
            }
            var deserializeAlertList = tempData.GetAlert();
            deserializeAlertList.Add(alert);
            tempData[Alerts] = SerializeAlerts(deserializeAlertList);
        }

        public static string SerializeAlerts(List<Allert> tempData)
        {
            return JsonConvert.SerializeObject(tempData);
        }

        public static List<Allert> DeserializeAlerts(string tempData)
        {
            if (tempData.Length == 0)
            {
                return new List<Allert>();
            }
            return JsonConvert.DeserializeObject<List<Allert>>(tempData);
        }

        private static IActionResult Alert(IActionResult result, string type, string title, string body)
        {
            return new ActionResultErrorDec(result, type, title, body);
        }
    }
    public enum AlertType
    {
        success = 1, info = 2, warning = 3, danger = 4,
    }
    public class Allert
    {
        public string Type { get; }
        public string Title { get; }
        public string Body { get; }

        public Allert(string type, string title, string body)
        {
            Type = type;
            Title = title;
            Body = body;
        }
    }
}

