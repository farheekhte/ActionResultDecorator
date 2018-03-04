using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConcreteDecorator.DecorationClass;
using Microsoft.AspNetCore.Mvc;

namespace ConcreteDecorator.Controllers
{
    public class DefaultController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            //TempData.GetAlert();
            return RedirectToAction("Index")
            .With(" With Success !", "Body WithSuccess Message !", AlertType.success)
            .With(" With Danger ֍", "Body WithDanger Message .. ֍", AlertType.danger)
            .With(" With Info i", "Body WithInfo Message .. i", AlertType.info)
            .With(" With Warning ▲", "Body WithWarning Message .. ▲", AlertType.warning);
        }
        
    }
}