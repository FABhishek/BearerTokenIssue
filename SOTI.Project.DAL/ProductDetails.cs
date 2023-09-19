using SOTI.Project.API;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SOTI.Project.DAL
{
    public class ProductDetails: IProduct, IProductAdditional
    {
        private SqlConnection con = null;
        private SqlDataAdapter adapter = null;
        private DataSet ds = null;

        public List<Product> GetAllProduct()
        {
            var products = GetProduct().Tables["Products"].AsEnumerable().Select(x => new Product
            {
                ProductId = x.Field<int>("ProductId"),
                ProductName = x.Field<string>("ProductName"),
                UnitPrice = x.Field<decimal?>("UnitPrice"),
                UnitsInStock = x.Field<short?>("UnitsInStock")
            }).ToList();
            return products;
        }

        public Product GetProductById(int id)
        {
            var products = GetProduct().Tables["Products"].AsEnumerable().Select(x => new Product
            {
                ProductId = x.Field<int>("ProductId"),
                ProductName = x.Field<string>("ProductName"),
                UnitPrice = x.Field<decimal?>("UnitPrice"),
                UnitsInStock = x.Field<short?>("UnitsInStock")
            }).FirstOrDefault(x=>x.ProductId == id);
            return products;
        }

        public bool DeleteProduct(int id)
        {
            using (con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (adapter = new SqlDataAdapter("Select ProductId from Products", con))
                {
                    using (ds = new DataSet())
                    {

                        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                        adapter.Fill(ds, "Products");
                        adapter.UpdateCommand = builder.GetUpdateCommand();

                        ds.Tables["Products"].PrimaryKey = new DataColumn[1] { ds.Tables["Products"].Columns["ProductId"] };
                        //Find a record to delete
                        DataRow dr = ds.Tables["Products"].Rows.Find(id);  
                                                                                 
                        if (dr != null)
                        {
                            dr.Delete();
                            adapter.Update(ds.Tables["Products"]);
                            return true;
                        }
                        adapter.Fill(ds, "Products"); // helps to reflect changes in dataset in case of multiple ** users working on same serve**
                        return false;
                    }
                }
            }
        } 

        


        public DataSet GetProduct()
        {
            using (SqlConnection con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (adapter = new SqlDataAdapter("Select * from Products", con))
                {
                    using (DataSet ds = new DataSet())
                    {
                        adapter.Fill(ds, "Products");
                        return ds;
                    }
                }
            }
        }

      
        public bool UpdateProduct(int id,Product prod)
        {
            using(con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (adapter = new SqlDataAdapter("Select ProductId, ProductName, UnitPrice, UnitsInStock from Products", con))
                {
                    using(ds = new DataSet())
                    {
                        adapter.Fill(ds, "Products");
                        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                        adapter.UpdateCommand = builder.GetUpdateCommand();
                        var product = ds.Tables["Products"].AsEnumerable().FirstOrDefault(x =>
                        x.Field<int>("ProductId") == id);
                        if(product != null)
                        {
                            product.BeginEdit();
                            product["ProductName"] = prod.ProductName;
                            product["UnitPrice"] = prod.UnitPrice == null ? product["UnitPrice"] : prod.UnitPrice.Value;
                            product["UnitsInStock"] = prod.UnitsInStock == null ? product["UnitsInstock"] : prod.UnitsInStock.Value;
                            product.EndEdit();

                            //update to database
                            var res = adapter.Update(ds, "Products");
                            return res > 0 ? true : false;
                        }
                        return false;
                    }
                }
            }
        }

        public bool AddProduct(string productName, decimal? unitPrice, short? unitsInStock)
        {
            using (con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (adapter = new SqlDataAdapter("Select ProductName, UnitPrice, UnitsInStock from Products", con))
                {
                    using (ds = new DataSet())
                    {
                        adapter.Fill(ds, "Products");
                        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                        adapter.InsertCommand = builder.GetInsertCommand();
                        DataRow dr = ds.Tables["Products"].NewRow();
                        dr["ProductName"] = productName;
                        dr["UnitPrice"] = unitPrice;
                        dr["UnitsInStock"] = unitsInStock;
                        ds.Tables["Products"].Rows.Add(dr);
                        //Update to Database
                        var res = adapter.Update(ds, "Products");
                        return res > 0 ? true : false;
                    }
                }
            }
        }

        public List<Product> GetProductByPrice(decimal price)
        {
            var ds = GetProduct();
            var products = ds.Tables[0].AsEnumerable()
                .Where(x => x.Field<decimal>("UnitPrice") > price)
                .Select(x => new Product
                {
                    ProductId = x.Field<int>("ProductId"),
                    ProductName = x.Field<string>("ProductName"),
                    UnitPrice = x.Field<decimal?>("UnitPrice"),
                    UnitsInStock = x.Field<short?>("UnitsInStock")
                }).ToList();
            return products;
        }

        //public DataSet GetProductById(int id)
        //{
        //using (con = new SqlConnection(SqlConnectionStrings.GetConnectionString))
        //{
        //    using (adapter = new SqlDataAdapter("Select * from Products Where ProductId = @Id", con))
        //    {
        //        adapter.SelectCommand.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4));
        //        adapter.SelectCommand.Parameters["@Id"].Value = id;
        //        using (DataSet ds = new DataSet())
        //        {
        //            adapter.Fill(ds, "Products");
        //            return ds;
        //        }
        //    }
        //}
    }

       
 }
