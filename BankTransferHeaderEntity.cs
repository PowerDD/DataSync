using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class BankTransferHeaderEntity : TableEntity
    {
        public BankTransferHeaderEntity(string shopId, string BankTransferHeaderId)
        {
            this.PartitionKey = shopId;
            this.RowKey = BankTransferHeaderId;
        }
        public string BankAccount { get; set; }
        public double TransferMoney { get; set; }
        public DateTime ReceiveDate { get; set; }
    }
}
