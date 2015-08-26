using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class Product
    {
         int id;
         public static int recProduct = 0;

         public Product(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            Hashtable product = new Hashtable();

            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                if (!product.ContainsKey(Program._DATA_TABLE.Rows[i]["PN"].ToString().Trim()) || Program._UPDATE == true)
                {
                    try
                    {
                        DynamicTableEntity data = new DynamicTableEntity();
                        data.PartitionKey = "88888888";
                        data.RowKey = Program._DATA_TABLE.Rows[i]["PN"].ToString().Trim().PadLeft(8, '0');
                        Dictionary<string, EntityProperty> properties = new Dictionary<string, EntityProperty>();
                        properties.Add("SKU", new EntityProperty(Program._DATA_TABLE.Rows[i]["ProductCode"].ToString().Trim()));
                        properties.Add("BuyerCode", new EntityProperty(Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim()));
                        properties.Add("Name", new EntityProperty(Program._DATA_TABLE.Rows[i]["Name"].ToString().Trim()));
                        properties.Add("Price", new EntityProperty(Program._DATA_TABLE.Rows[i]["MinPrice"].ToString().Trim()));
                        properties.Add("Price1", new EntityProperty(Program._DATA_TABLE.Rows[i]["price1"].ToString().Trim()));
                        properties.Add("Price2", new EntityProperty(Program._DATA_TABLE.Rows[i]["price2"].ToString().Trim()));
                        properties.Add("Price3", new EntityProperty(Program._DATA_TABLE.Rows[i]["price3"].ToString().Trim()));
                        properties.Add("Price4", new EntityProperty(Program._DATA_TABLE.Rows[i]["price4"].ToString().Trim()));
                        properties.Add("Price5", new EntityProperty(Program._DATA_TABLE.Rows[i]["price5"].ToString().Trim()));
                        properties.Add("Price6", new EntityProperty(Program._DATA_TABLE.Rows[i]["Price6"].ToString().Trim()));
                        properties.Add("Price7", new EntityProperty("0"));
                        properties.Add("Price8", new EntityProperty("0"));
                        properties.Add("Stock", new EntityProperty(Convert.ToInt32(Program._DATA_TABLE.Rows[i]["stock"].ToString().Trim())));
                        properties.Add("Cost", new EntityProperty(Convert.ToDouble(Program._DATA_TABLE.Rows[i]["cost"].ToString().Trim())));
                        properties.Add("CoverImage", new EntityProperty(Program._DATA_TABLE.Rows[i]["CoverImage"].ToString().Trim()));
                        properties.Add("Image1", new EntityProperty(Program._DATA_TABLE.Rows[i]["Image1"].ToString().Trim()));
                        properties.Add("Image2", new EntityProperty(Program._DATA_TABLE.Rows[i]["Image2"].ToString().Trim()));
                        properties.Add("Image3", new EntityProperty(Program._DATA_TABLE.Rows[i]["Image3"].ToString().Trim()));
                        properties.Add("Image4", new EntityProperty(Program._DATA_TABLE.Rows[i]["Image4"].ToString().Trim()));
                        properties.Add("Image5", new EntityProperty(Program._DATA_TABLE.Rows[i]["Image5"].ToString().Trim()));
                        properties.Add("Image6", new EntityProperty(Program._DATA_TABLE.Rows[i]["Image6"].ToString().Trim()));
                        properties.Add("Image7", new EntityProperty(Program._DATA_TABLE.Rows[i]["Image7"].ToString().Trim()));
                        properties.Add("Image8", new EntityProperty(""));
                        properties.Add("Image9", new EntityProperty(""));
                        properties.Add("Image10", new EntityProperty(""));
                        properties.Add("Image11", new EntityProperty(""));
                        properties.Add("Image12", new EntityProperty(""));
                        properties.Add("Width", new EntityProperty(Convert.ToDouble(Program._DATA_TABLE.Rows[i]["Width"].ToString().Trim())));
                        properties.Add("Length", new EntityProperty(Convert.ToDouble(Program._DATA_TABLE.Rows[i]["Length"].ToString().Trim())));
                        properties.Add("Height", new EntityProperty(Convert.ToDouble(Program._DATA_TABLE.Rows[i]["Height"].ToString().Trim())));
                        properties.Add("GrossWeight", new EntityProperty(Convert.ToDouble(Program._DATA_TABLE.Rows[i]["GrossWeight"].ToString().Trim())));
                        properties.Add("Rating", new EntityProperty("0"));
                        properties.Add("ReviewCount", new EntityProperty("0"));
                        properties.Add("Category", new EntityProperty(Program._DATA_TABLE.Rows[i]["GroupId"].ToString().Trim().PadLeft(5, '0')));
                        properties.Add("Brand", new EntityProperty(Program._DATA_TABLE.Rows[i]["BrandId"].ToString().Trim().PadLeft(5, '0')));
                        properties.Add("Warranty", new EntityProperty(Convert.ToInt32(Program._DATA_TABLE.Rows[i]["Warranty"].ToString().Trim())));
                        properties.Add("Location", new EntityProperty(Program._DATA_TABLE.Rows[i]["Location"].ToString().Trim()));
                        if (!Program._UPDATE) { properties.Add("View", new EntityProperty("0")); }
                        string ac = Program._DATA_TABLE.Rows[i]["active"].ToString().Trim();
                        if (ac == "1") {properties.Add("Active", new EntityProperty(true)); } else { properties.Add("Active", new EntityProperty(false)); }
                        properties.Add("Visible", new EntityProperty(Convert.ToBoolean(Program._DATA_TABLE.Rows[i]["visible"].ToString().Trim())));
                        try { properties.Add("AddDate", new EntityProperty(Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["AddDate"].ToString()))); }
                        catch { }
                        properties.Add("OnCart", new EntityProperty(Convert.ToInt32(Program._DATA_TABLE.Rows[i]["onCart"].ToString().Trim())));
                        properties.Add("OnOrder", new EntityProperty(Convert.ToInt32(Program._DATA_TABLE.Rows[i]["onOrder"].ToString().Trim())));
                       
                        data.Properties = properties;
                        //ProductEntity data = new ProductEntity("88888888", Program._DATA_TABLE.Rows[i]["PN"].ToString().Trim().PadLeft(8, '0'));
                        //data.SKU = Program._DATA_TABLE.Rows[i]["ProductCode"].ToString().Trim();
                        //data.BuyerCode = Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim();
                        //data.Name = Program._DATA_TABLE.Rows[i]["Name"].ToString().Trim();
                        //data.Price = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["MinPrice"].ToString().Trim());
                        //data.Price1 = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["price1"].ToString().Trim());
                        //data.Price2 = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["price2"].ToString().Trim());
                        //data.Price3 = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["price3"].ToString().Trim());
                        //data.Price4 = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["price4"].ToString().Trim());
                        //data.Price5 = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["price5"].ToString().Trim());
                        //data.Price6 = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["price6"].ToString().Trim());
                        //data.Price7 = 0;
                        //data.Price8 = 0;
                        //data.Stock = Convert.ToInt32(Program._DATA_TABLE.Rows[i]["stock"].ToString().Trim());
                        //data.Cost = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["cost"].ToString().Trim());
                        //data.CoverImage = Program._DATA_TABLE.Rows[i]["CoverImage"].ToString().Trim();
                        //data.Image1 = Program._DATA_TABLE.Rows[i]["Image1"].ToString().Trim();
                        //data.Image2 = Program._DATA_TABLE.Rows[i]["Image2"].ToString().Trim();
                        //data.Image3 = Program._DATA_TABLE.Rows[i]["Image3"].ToString().Trim();
                        //data.Image4 = Program._DATA_TABLE.Rows[i]["Image4"].ToString().Trim();
                        //data.Image5 = Program._DATA_TABLE.Rows[i]["Image5"].ToString().Trim();
                        //data.Image6 = Program._DATA_TABLE.Rows[i]["Image6"].ToString().Trim();
                        //data.Image7 = Program._DATA_TABLE.Rows[i]["Image7"].ToString().Trim();
                        //data.Image8 = "";
                        //data.Image9 = "";
                        //data.Image10 = "";
                        //data.Image11 = "";
                        //data.Image12 = "";
                        //data.Width = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["Width"].ToString().Trim());
                        //data.Length = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["Length"].ToString().Trim());
                        //data.Height = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["Height"].ToString().Trim());
                        //data.Weight = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["Weight"].ToString().Trim());
                        //data.GrossWeight = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["GrossWeight"].ToString().Trim());
                        //data.Rating = 0;
                        //data.ReviewCount = 0;
                        //data.Category = Program._DATA_TABLE.Rows[i]["GroupId"].ToString().Trim().PadLeft(5, '0');
                        //data.Brand = Program._DATA_TABLE.Rows[i]["BrandId"].ToString().Trim().PadLeft(5, '0');
                        //data.Warranty = Convert.ToInt32(Program._DATA_TABLE.Rows[i]["Warranty"].ToString().Trim());
                        //data.Location = Program._DATA_TABLE.Rows[i]["Location"].ToString().Trim();
                        //if (Program._UPDATE == false) { data.View = 0; }
                        //string ac = Program._DATA_TABLE.Rows[i]["active"].ToString().Trim();
                        //if (ac == "1") { data.Active = true; } else { data.Active = false; }
                        //data.Visible = Convert.ToBoolean(Program._DATA_TABLE.Rows[i]["visible"].ToString().Trim());
                        //try { data.AddDate = Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["AddDate"].ToString()); }
                        //catch { }
                        //data.OnCart = Convert.ToInt32(Program._DATA_TABLE.Rows[i]["onCart"].ToString().Trim());
                        //data.OnOrder = Convert.ToInt32(Program._DATA_TABLE.Rows[i]["onOrder"].ToString().Trim());
                        batchOperation.InsertOrMerge(data);
                        recProduct++;
                        product[Program._DATA_TABLE.Rows[i]["PN"].ToString().Trim()] = true;
                    }
                    catch
                    {
                        Console.WriteLine(Program._DATA_TABLE.Rows[i]["PN"].ToString().Trim());
                    }
                }
            }

            try
            {
                if (Program._DATA_TABLE.Rows.Count > 0)
                {
                    if (Program._UPDATE)
                    {
                        Program._RECORD++;
                        Console.WriteLine("Update Record {0}-{1}\t\tTotal {2} Records", id + 1, id + 100, Program._RECORD * 100);
                        Program._CLOUD_TABLE.ExecuteBatch(batchOperation);
                    }
                    else
                    {
                        Program._RECORD++;
                        Console.WriteLine("Insert Record {0}-{1}\t\tTotal {2} Records", id + 1, id + 100, Program._RECORD * 100);
                        Program._CLOUD_TABLE.ExecuteBatch(batchOperation);
                    }
                }

            }
            catch (Exception e)
            {
                Program.WriteErrorLog("Record " + (id + 1) + "-" + (id + 100) + " Error \n" + e.Message + "\n" + e.StackTrace);
            }

            //DynamicTableEntity data = new DynamicTableEntity();
            ////{
            ////    PartitionKey = "88888888",
            ////    RowKey = "1234",
            ////    ETag = "*",
            ////};

            //Dictionary<string, EntityProperty> properties = new Dictionary<string, EntityProperty>();
            //properties.Add(key, new EntityProperty(int.Parse(value.ToString())));
            //data.Properties = properties;

            //var azureTable = Param.AzureTableClient.GetTableReference(table);
            //TableOperation updateOperation = TableOperation.Merge(data);
            //azureTable.Execute(updateOperation);
        }
    }
}
