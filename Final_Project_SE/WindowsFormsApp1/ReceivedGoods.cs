using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class ReceivedGoods
    {
        private int _goodID;
        private int _quantity;
        private Decimal _price;

        public int GoodID { get { return _goodID; } set { _goodID = value; } }
        public int Quantity { get { return _quantity; } set { _quantity = value; } }
        public Decimal Price { get { return _price; } set { _price = value; } }

        public ReceivedGoods(int goodID, int quantity, Decimal price) 
        {
            this._goodID = goodID;
            this._quantity = quantity;
            this._price = price;
        }
       
    }

}
