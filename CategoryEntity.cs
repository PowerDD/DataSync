using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class CategoryEntity : TableEntity
    {
        public CategoryEntity(string shopId, string categoryId)
        {
            this.PartitionKey = shopId;
            this.RowKey = categoryId;
        }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }
        public int Priority { get; set; }
        public int ProductCount { get; set; }
    }
}
