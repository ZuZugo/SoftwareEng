using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopping_cart.Models;


namespace Shopping_cart.Controllers
{
  
    public class HomeController : Controller
    {
        MyConn myConn = new MyConn();
        // GET: Home
        public ActionResult Index()
        {
            return View(myConn.Warehouses.ToList());
        }

        
    }
}