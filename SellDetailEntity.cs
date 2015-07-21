using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class SellDetailEntity : TableEntity
    {
        public SellDetailEntity(string shopId, string SellDId)
        {
            this.PartitionKey = shopId;
            this.RowKey = SellDId;
        }
        public double SellPrice { get; set; }
        public double Quantity { get; set; }
    }
}
