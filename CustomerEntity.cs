using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class CustomerEntity : TableEntity
    {
        public CustomerEntity(string shopId, string customerId)
        {
            this.PartitionKey = shopId;
            this.RowKey = customerId;
        }
        public string Member { get; set; }
        public string Shop { get; set; }
        public string Name { get; set; }
        public string ShopName { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public string Tel { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string TaxCode { get; set; }
        public int Credit { get; set; }
        public int DiscountPercent { get; set; }
        public int SellPrice { get; set; }
        public string Comment { get; set; }
        public DateTime AddDate { get; set; }
    }
}
