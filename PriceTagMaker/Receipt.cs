using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceTagMaker
{
    public class Receipt
    {
        private string _code;
        public string Code { get { return _code; } set { _code = value; } }

        private string _name;
        public string Name { 
            get {
                if (_name.IndexOf('[') > -1)
                {
                    return _name.Remove(_name.IndexOf('['), _name.IndexOf(']') - _name.IndexOf('[')+1);
                }
                else
                    return _name;
            }   
                
            set { _name = value; } }

        private string _price;
        public string Price {
            get
            {
                if (_price.Split(',').Length > 1 && _price.Split(',')[1] == "00")
                {
                    return _price.Split(',')[0];
                }
                return _price;

            }

            set { _price = value; } 
        }


        private string _oldPrice;
        public string OldPrice { 
            get {
                if (_oldPrice.Split(',').Length > 1 && _oldPrice.Split(',')[1] == "00")
                {
                    return _oldPrice.Split(',')[0];
                }
                return _oldPrice;
            } 
            set { _oldPrice = value; } 
        }


        private string _receiptType;
        public string ReceiptType { get { return _receiptType; } set { _receiptType = value; } }
    }
}
