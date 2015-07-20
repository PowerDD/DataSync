using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class SellHeaderEntity : TableEntity
    {
        public SellHeaderEntity(string shopId, string SellHId)
        {
            this.PartitionKey = shopId;
            this.RowKey = SellHId;
        }
        public string DocNo { get; set; }
        public string Customer { get; set; }
        public string BillNo { get; set; }
        public int Credit { get; set; }
        public int PayType { get; set; }
        public double Cash { get; set; }
        public double DiscountPercent { get; set; }
        public double DiscountCash { get; set; }
        public DateTime SellDate { get; set; }
        public int Paid { get; set; }
        public double Profit { get; set; }
        public double TotalPrice { get; set; }
        public double PointReceived { get; set; }
        public double PointUse { get; set; }
        public string Comment { get; set; }
    }
}
