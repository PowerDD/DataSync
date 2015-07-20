using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class BarcodeStockEntity : TableEntity
    {
        public BarcodeStockEntity() { }

        public BarcodeStockEntity(string shopId)
        {
            this.PartitionKey = shopId;
        }

        public BarcodeStockEntity(string shopId, string barcode)
        {
            this.PartitionKey = shopId;
            this.RowKey = barcode;
        }

        public string OrderNo { get; set; }
        public string Product { get; set; }
        public bool SellFinished { get; set; }
        public double Cost { get; set; }
    }
}
