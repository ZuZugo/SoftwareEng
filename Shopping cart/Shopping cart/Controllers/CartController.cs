using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopping_cart.Models;

namespace Shopping_cart.Controllers
{
    public class CartController : Controller
    {   
        MyConn myConn = new MyConn();
        // GET: Cart
        #region Cart
        public List<Cart> GetCart()
        {
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if (carts == null)
            {
                carts = new List<Cart>();
                Session["Cart"] = carts;
            }
            return carts;
        }
        
        public ActionResult AddToCart(int GoodID, String strURL)
        {
            Warehouse warehouse = myConn.Warehouses.SingleOrDefault(n => n.GoodID == GoodID);
            if(warehouse == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Cart> carts = GetCart();
            Cart c = carts.Find(n => n.GoodID == GoodID);
            if (c == null)
            {
                c = new Cart(GoodID);
                carts.Add(c);
                return Redirect(strURL);
            }
            else
            {
                c.Quantity++;
                return Redirect(strURL);
            }
        }
        
        public ActionResult UpdateCart(int GoodID, FormCollection f)
        {   
            Warehouse w = myConn.Warehouses.SingleOrDefault(n => n.GoodID == GoodID);
            if(w == null)
            {
                Response.StatusCode = 404;
                return null;
            } 
            List<Cart> lstCart = GetCart();
          
            Cart c = lstCart.SingleOrDefault(n => n.GoodID == GoodID);

            if(c != null)
            {
                c.Quantity = int.Parse(f["txtQuantity"].ToString());
            }
            return RedirectToAction("Cart");
        }
        public ActionResult RemoveCart(int GoodID)
        {
            Warehouse w = myConn.Warehouses.SingleOrDefault(n => n.GoodID == GoodID);
            if (w == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Cart> lstCart = GetCart();
            Cart c = lstCart.SingleOrDefault(n => n.GoodID == GoodID);

            if (c != null)
            {
                lstCart.RemoveAll(n => n.GoodID == GoodID);
                
            }
            if(lstCart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Cart");
        }
        public void CheckPayment(FormCollection f)
        {
            String payment = f["payment"].ToString();
            List<Cart> carts = GetCart();
   
            Cart c = new Cart(payment);
            carts.Add(c);
            
        }
        public ActionResult Cart()
        {
            if(Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Cart> lstCart = GetCart();
            return View(lstCart);
        }
        public double TotalQuantity()
        {
            double iTotal = 0;

            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
               iTotal = lstCart.Sum(n => n.Quantity);
            }
    

            return iTotal;
        }

        public double TotalPrice()
        {
            double dTotalPrice = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                dTotalPrice = lstCart.Sum(n => n.Total);
            }
            return dTotalPrice;
        }
        public ActionResult CartPartial()
        {   
            if(TotalQuantity() == 0)
            {
                return PartialView();
            }
            //ViewBag.TotalPrice = TotalPrice();
            ViewBag.TotalQuantity = TotalQuantity();
            ViewBag.ToalPrice = TotalPrice();
            return PartialView();
        }

        public ActionResult EditCart()
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Cart> lstCart = GetCart();
            return View(lstCart);
        }
        #endregion

        #region Order
        [HttpPost]
        public ActionResult Order(FormCollection f)
        {
            String payment = f["Payment"].ToString();

            if(Session["Account"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            if(Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Order orders = new Order();
            Customer customer = (Customer)Session["Account"];
            List<Cart> cart = GetCart();
            orders.CustomerID = customer.CustomerID;
            orders.Paid = payment;
            //orders.Status = "Waiting...";
            myConn.Orders.Add(orders);
            myConn.SaveChanges();
            foreach(var item in cart)
            {
                Detail_Order dOrder = new Detail_Order();
                
                dOrder.OrderID = orders.OrderID;
                dOrder.GoodID = item.GoodID;
                dOrder.Quantity = item.Quantity;
                dOrder.Price = (decimal)item.UnitPrice;
                myConn.Detail_Order.Add(dOrder);
            }
            myConn.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        #endregion
    }
}