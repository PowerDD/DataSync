using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class BankTransferDetailEntity : TableEntity
    {
        public BankTransferDetailEntity(string shopId, string BankTransferDetailId)
        {
            this.PartitionKey = shopId;
            this.RowKey = BankTransferDetailId;
        }
        public string SellNumber { get; set; }
        public double ReceiveMoney { get; set; }

    }
}
