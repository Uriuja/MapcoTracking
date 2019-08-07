using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MapcoSolutionTrackings.Controllers
{
    public class UtilController : Controller
    {
        // GET: Util
        public ActionResult Index()
        {
            return View();
        }

        public Object GetResponse(Object responseData,String messageData,Boolean flag)
        {
            int responseCode = 200;
            if (!flag)
            {
                responseCode = 500;
            }
           
            return Json(new { code = responseCode, data = responseData, message = messageData});
        }


    }
}