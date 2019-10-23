using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fast.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Web.Areas.Admin.Controllers
{

    /// <summary>
    /// 
    /// </summary>

    public class LoginController : AdminController
    {











        public IActionResult Index()
        {
            return View();
        }
    }
}