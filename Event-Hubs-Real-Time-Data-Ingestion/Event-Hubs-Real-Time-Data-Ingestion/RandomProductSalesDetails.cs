using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Hubs_Real_Time_Data_Ingestion
{
    class RandomProductSalesDetails
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal SellPrice { get; set; }
        public int SellQty { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
