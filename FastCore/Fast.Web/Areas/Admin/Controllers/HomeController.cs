using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminController
    {


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("",Name = "mainIndex")]
        public IActionResult HomeIndex()
        {
            return View();
        }



    }
}