using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class Product
    {
        int id;

        public Product(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                ProductEntity data = new ProductEntity("88888888", Program._DATA_TABLE.Rows[i]["PN"].ToString().Trim());
                data.SKU = Program._DATA_TABLE.Rows[i]["ProductCode"].ToString().Trim();
                data.BuyerCode = Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim();
                data.Name = Program._DATA_TABLE.Rows[i]["Name"].ToString().Trim();
                data.Price = double.Parse(Program._DATA_TABLE.Rows[i]["MinPrice"].ToString());
                data.Price1 = double.Parse(Program._DATA_TABLE.Rows[i]["price1"].ToString());
                data.Price2 = double.Parse(Program._DATA_TABLE.Rows[i]["price2"].ToString());
                data.Price3 = double.Parse(Program._DATA_TABLE.Rows[i]["price3"].ToString());
                data.Price4 = double.Parse(Program._DATA_TABLE.Rows[i]["price4"].ToString());
                data.Price5 = double.Parse(Program._DATA_TABLE.Rows[i]["price5"].ToString());
                data.Price6 = double.Parse(Program._DATA_TABLE.Rows[i]["price6"].ToString());
                data.Cost = double.Parse(Program._DATA_TABLE.Rows[i]["cost"].ToString());
                data.Stock = int.Parse(Program._DATA_TABLE.Rows[i]["stock"].ToString());
                data.CoverImage = Program._DATA_TABLE.Rows[i]["CoverImage"].ToString().Trim();
                data.Width = double.Parse(Program._DATA_TABLE.Rows[i]["Width"].ToString());
                data.Length = double.Parse(Program._DATA_TABLE.Rows[i]["Length"].ToString());
                data.Height = double.Parse(Program._DATA_TABLE.Rows[i]["Height"].ToString());
                data.Weight = double.Parse(Program._DATA_TABLE.Rows[i]["Weight"].ToString());
                data.GrossWeight = double.Parse(Program._DATA_TABLE.Rows[i]["GrossWeight"].ToString());
                data.Category = Program._DATA_TABLE.Rows[i]["GroupID"].ToString().Trim();
                data.Brand = Program._DATA_TABLE.Rows[i]["BrandID"].ToString().Trim();
                data.Warranty = int.Parse(Program._DATA_TABLE.Rows[i]["Warranty"].ToString());
                data.Location = Program._DATA_TABLE.Rows[i]["Location"].ToString().Trim();
                data.Active = Program._DATA_TABLE.Rows[i]["Active"].ToString() == "1";
                try { data.AddDate = Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["AddDate"].ToString()); }
                catch { }
                batchOperation.InsertOrMerge(data);
            }

            try
            {
                Program._RECORD++;
                Console.WriteLine("Insert Record {0}-{1}\t\tTotal {2} Records", id + 1, id + 100, Program._RECORD*100);
                Program._CLOUD_TABLE.ExecuteBatch(batchOperation);
            }
            catch (Exception e)
            {
                Program.WriteErrorLog("Record " + (id + 1) + "-" + (id + 100) + " Error \n" + e.Message + "\n" + e.StackTrace);
            }

        }
    }
}
