//using ConcreteDecorator.DecorationClass;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ConcreteDecorator.DecorationClass
//{
//    public class ActionResultDecorator : IActionResult
//    {

//        public IActionResult ActionResult { get; }
//        public string Type { get; }
//        public string Title { get; }
//        public string Body { get; }
//        public ActionResultDecorator(IActionResult _actionResult, string type, string title, string body)
//        {
//            ActionResult = _actionResult;
//            Type = type;
//            Title = title;
//            Body = body;
//        }
//        public async Task ExecuteResultAsync(ActionContext context)
//        {
//            var factory = context.HttpContext.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
//            var tempData = factory.GetTempData(context.HttpContext);

//            tempData.AddAlert(new DecoratorConcrete(Type, Title, Body));

//            await ActionResult.ExecuteResultAsync(context);
//        }
//    }
//    public static class DecoratorConcreteExtensions
//    {
//        private static AlertType _type { get; set; }
//        public static IActionResult With(this IActionResult result, string title, string body, AlertType type)
//        {
//            _type = type;
//            return Alert(result, "alert alert-" + type.ToString(), title, body);
//        }


//        public static List<DecoratorConcrete> GetAlertList(this ITempDataDictionary tempData, string value) =>
//             value != null ? JsonConvert.DeserializeObject<List<DecoratorConcrete>>(value) : null;

//        private static string Alerts = _type.ToString();

//        public static List<DecoratorConcrete> GetAlert(this ITempDataDictionary tempData)
//        {
//            CreateAlertTempData(tempData);
//            return DeserializeAlerts(tempData[Alerts] as string);
//        }

//        public static void CreateAlertTempData(this ITempDataDictionary tempData)
//        {
//            if (!tempData.ContainsKey(Alerts))
//            {
//                tempData[Alerts] = "";
//            }
//        }

//        public static void AddAlert(this ITempDataDictionary tempData, DecoratorConcrete alert)
//        {
//            if (alert == null)
//            {
//                throw new ArgumentNullException(nameof(alert));
//            }
//            var deserializeAlertList = tempData.GetAlert();
//            deserializeAlertList.Add(alert);
//            tempData[Alerts] = SerializeAlerts(deserializeAlertList);
//        }

//        public static string SerializeAlerts(List<DecoratorConcrete> tempData)
//        {
//            return JsonConvert.SerializeObject(tempData);
//        }

//        public static List<DecoratorConcrete> DeserializeAlerts(string tempData)
//        {
//            if (tempData.Length == 0)
//            {
//                return new List<DecoratorConcrete>();
//            }
//            return JsonConvert.DeserializeObject<List<DecoratorConcrete>>(tempData);
//        }

//        private static IActionResult Alert(IActionResult result, string type, string title, string body)
//        {
//            return new ActionResultDecorator(result, type, title, body);
//        }
//    }
//    public enum AlertType
//    {
//        success = 1, info = 2, warning = 3, danger = 4,
//    }
//    public class DecoratorConcrete
//    {
//        public string Type { get; }
//        public string Title { get; }
//        public string Body { get; }

//        public DecoratorConcrete(string type, string title, string body)
//        {
//            Type = type;
//            Title = title;
//            Body = body;
//        }
//    }
//}






