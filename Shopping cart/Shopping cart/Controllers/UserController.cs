using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopping_cart.Models;

namespace Shopping_cart.Controllers
{
    public class UserController : Controller
    {   
        private MyConn myConn = new MyConn();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer customer)
        {   
            
            if(ModelState.IsValid)
            {
                var check = myConn.Customers.FirstOrDefault(s => s.Account == customer.Account);
                if(check == null)
                {
                    myConn.Configuration.ValidateOnSaveEnabled = false;
                    myConn.Customers.Add(customer);
                    myConn.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Email already exist.";
                    return View();
                }
            }
            
            return View();
            
            /*
            if(ModelState.IsValid)
            {
                myConn.Customers.Add(customer);
                myConn.SaveChanges();
            }
           
            return View();
            */      
            
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f) 
        {
            String userName = f["userNameTxt"].ToString();
            String password = f["passwordTxt"].ToString();
            Customer customer = myConn.Customers.SingleOrDefault(n => n.Account==userName && n.Password==password);
            if(customer != null) 
            {
                //ViewBag.ThongBao = "Login successful";
                Session["Account"] = customer;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ThongBao = "Wrong username or password";
            return View();
        }
    }
}