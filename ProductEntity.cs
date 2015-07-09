using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class ProductEntity : TableEntity
    {
        public ProductEntity(string shopId, string productId)
        {
            this.PartitionKey = shopId;
            this.RowKey = productId;
        }
        public bool Active { get; set; }
        public DateTime AddDate { get; set; }
        public string Brand { get; set; }
        public string BuyerCode { get; set; }
        public string Category { get; set; }
        public double Cost { get; set; }
        public string CoverImage { get; set; }
        public double GrossWeight { get; set; }
        public double Height { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Image6 { get; set; }
        public string Image7 { get; set; }
        public string Image8 { get; set; }
        public string Image9 { get; set; }
        public string Image10 { get; set; }
        public string Image11 { get; set; }
        public string Image12 { get; set; }
        public double Length { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public int OnCart { get; set; }
        public int OnOrder { get; set; }
        public double Price { get; set; }
        public double Price1 { get; set; }
        public double Price2 { get; set; }
        public double Price3 { get; set; }
        public double Price4 { get; set; }
        public double Price5 { get; set; }
        public double Price6 { get; set; }
        public double Price7 { get; set; }
        public double Price8 { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string SKU { get; set; }
        public int Stock { get; set; }
        public bool Visible { get; set; }
        public int Warranty { get; set; }
        public double Weight { get; set; }
        public double Width { get; set; }
        public int View { get; set; }


    }
}
