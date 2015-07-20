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
                        ProductEntity data = new ProductEntity("88888888", Program._DATA_TABLE.Rows[i]["PN"].ToString().Trim().PadLeft(8, '0'));
                        data.SKU = Program._DATA_TABLE.Rows[i]["ProductCode"].ToString().Trim();
                        data.BuyerCode = Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim();
                        data.Name = Program._DATA_TABLE.Rows[i]["Name"].ToString().Trim();
                        data.Price = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["MinPrice"].ToString().Trim());
                        data.Price1 = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["price1"].ToString().Trim());
                        data.Price2 = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["price2"].ToString().Trim());
                        data.Price3 = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["price3"].ToString().Trim());
                        data.Price4 = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["price4"].ToString().Trim());
                        data.Price5 = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["price5"].ToString().Trim());
                        data.Price6 = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["price6"].ToString().Trim());
                        data.Price7 = 0;
                        data.Price8 = 0;
                        data.Stock = Convert.ToInt32(Program._DATA_TABLE.Rows[i]["stock"].ToString().Trim());
                        data.Cost = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["cost"].ToString().Trim());
                        data.CoverImage = Program._DATA_TABLE.Rows[i]["CoverImage"].ToString().Trim();
                        data.Image1 = Program._DATA_TABLE.Rows[i]["Image1"].ToString().Trim();
                        data.Image2 = Program._DATA_TABLE.Rows[i]["Image2"].ToString().Trim();
                        data.Image3 = Program._DATA_TABLE.Rows[i]["Image3"].ToString().Trim();
                        data.Image4 = Program._DATA_TABLE.Rows[i]["Image4"].ToString().Trim();
                        data.Image5 = Program._DATA_TABLE.Rows[i]["Image5"].ToString().Trim();
                        data.Image6 = Program._DATA_TABLE.Rows[i]["Image6"].ToString().Trim();
                        data.Image7 = Program._DATA_TABLE.Rows[i]["Image7"].ToString().Trim();
                        data.Image8 = "";
                        data.Image9 = "";
                        data.Image10 = "";
                        data.Image11 = "";
                        data.Image12 = "";
                        data.Width = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["Width"].ToString().Trim());
                        data.Length = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["Length"].ToString().Trim());
                        data.Height = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["Height"].ToString().Trim());
                        data.Weight = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["Weight"].ToString().Trim());
                        data.GrossWeight = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["GrossWeight"].ToString().Trim());
                        data.Rating = 0;
                        data.ReviewCount = 0;
                        data.Category = Program._DATA_TABLE.Rows[i]["GroupId"].ToString().Trim().PadLeft(5, '0');
                        data.Brand = Program._DATA_TABLE.Rows[i]["BrandId"].ToString().Trim().PadLeft(5, '0');
                        data.Warranty = Convert.ToInt32(Program._DATA_TABLE.Rows[i]["Warranty"].ToString().Trim());
                        data.Location = Program._DATA_TABLE.Rows[i]["Location"].ToString().Trim();
                        if (Program._UPDATE == false) { data.View = 0; }
                        string ac = Program._DATA_TABLE.Rows[i]["active"].ToString().Trim();
                        if (ac == "1") { data.Active = true; } else { data.Active = false; }
                        data.Visible = Convert.ToBoolean(Program._DATA_TABLE.Rows[i]["visible"].ToString().Trim());
                        try { data.AddDate = Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["AddDate"].ToString()); }
                        catch { }
                        data.OnCart = Convert.ToInt32(Program._DATA_TABLE.Rows[i]["onCart"].ToString().Trim());
                        data.OnOrder = Convert.ToInt32(Program._DATA_TABLE.Rows[i]["onOrder"].ToString().Trim());
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
            catch (Exception e)
            {
                Program.WriteErrorLog("Record " + (id + 1) + "-" + (id + 100) + " Error \n" + e.Message + "\n" + e.StackTrace);
            }
            

        }
    }
}
