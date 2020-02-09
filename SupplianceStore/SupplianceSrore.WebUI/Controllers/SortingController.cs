using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupplianceSrore.WebUI.Controllers
{
    public class SortingController : Controller
    {
        // GET: Sorting
        public ActionResult Sort(int minprice,int maxprice,string producer)
        {
            return View();
        }
    }
}