using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class BankEntity : TableEntity
    {
        public BankEntity(string shopId, string BankId)
        {
            this.PartitionKey = shopId;
            this.RowKey = BankId;
        }
        public string Bank { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public int AccountType { get; set; }
    }
}
