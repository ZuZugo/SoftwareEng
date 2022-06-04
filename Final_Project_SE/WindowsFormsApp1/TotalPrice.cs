using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class TotalPrice
    {
        private Decimal _price;
        private int _quantity;
       
        public Decimal Price { get { return _price; } set { _price = value; } }
        public int Quantity { get { return _quantity; } set { _quantity = value; } }
        public TotalPrice(int _quantity, Decimal _price)
        {
            this._price = _price;
            this._quantity = _quantity;
        }
        public Decimal CalTotalPrice()
        {
            return _price * _quantity;
        }
        
    }
}
