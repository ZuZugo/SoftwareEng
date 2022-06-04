using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Shopping_cart.Models
{
    public class Cart
    {   
        private MyConn myConn = new MyConn();
        public int GoodID { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Paid { get; set; }
        public double Total 
        {
            get { return UnitPrice * Quantity; } 
        }
        public Cart(int iGoodID)
        {
            GoodID = iGoodID;
            Warehouse w = myConn.Warehouses.Single(n => n.GoodID == iGoodID);
            Name = w.Item;
            UnitPrice = double.Parse(w.Price.ToString());
            Quantity = 1;
        }
        public Cart(string paid)
        {
            Paid = paid;
        }

    }
}