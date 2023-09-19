using SOTI.Project.API;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTI.Project.DAL
{
    public class ProductDetailsConnect : IProduct
    {
        private SqlConnection _connection = null;
        private SqlCommand _command = null;
        private SqlDataReader _reader = null;
        public List<Product> GetAllProduct()
        {
            List<Product> products = new List<Product>();
            using (_connection = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (_command = new SqlCommand("Select * from Products", _connection))
                {
                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }
                    using (_reader = _command.ExecuteReader())
                    {
                        if (_reader.HasRows)
                        {
                            while (_reader.Read())
                            {
                                products.Add(new Product
                                {
                                    ProductId = Convert.ToInt32(_reader.GetValue(0)),
                                    ProductName = _reader.GetValue(1).ToString(),
                                    UnitPrice = Convert.ToDecimal(_reader.GetValue(5)),
                                    UnitsInStock = Convert.ToInt16(_reader.GetValue(6))

                                });

                            }
                        }
                    }
                }
            }
            return products;
        }

        public Product GetProductById(int id)
        {
            Product product = null;
            using (_connection = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (_command = new SqlCommand("Select * from Products Where productId = @ProductId", _connection))
                {
                    _command.Parameters.AddWithValue("@ProductId", id);
                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }
                    using (_reader = _command.ExecuteReader())
                    {
                        if (_reader.HasRows)
                        {
                            _reader.Read();
                            product = (new Product
                            {
                                ProductId = Convert.ToInt32(_reader.GetValue(0)),
                                ProductName = _reader.GetValue(1).ToString(),
                                UnitPrice = Convert.ToDecimal(_reader.GetValue(5)),
                                UnitsInStock = Convert.ToInt16(_reader.GetValue(6))
                            });
                        }
                    }
                }
            }
            return product;
        }

        public bool AddProduct(string productName, decimal? unitPrice, short? unitsInStock)
        {
            using (_connection = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (_command = new SqlCommand("usp_AddProduct", _connection))
                {
                    _command.CommandType = CommandType.StoredProcedure;
                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }
                    _command.Parameters.Add(new SqlParameter("@productName", SqlDbType.NVarChar,40));
                    _command.Parameters.Add(new SqlParameter("@unitPrice", SqlDbType.Money,8));
                    _command.Parameters.Add(new SqlParameter("@unitsInStock", SqlDbType.SmallInt, 2));

                    _command.Parameters["@productName"].Value = productName;
                    _command.Parameters["@unitPrice"].Value = unitPrice.Value;
                    _command.Parameters["@unitsInStock"].Value = unitsInStock.Value;

                    var res = _command.ExecuteNonQuery();
                    return res > 0;
                }
            }
        }

        public bool DeleteProduct(int id)
        {

            using (_connection = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (_command = new SqlCommand("usp_DeleteProduct", _connection))
                {
                    _command.CommandType = CommandType.StoredProcedure;
                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }
                    _command.Parameters.Add(new SqlParameter("@prodID", SqlDbType.Int, 32));

                    _command.Parameters["@prodID"].Value = id;

                    var res = _command.ExecuteNonQuery();
                    return res > 0;
                }
            }
        }

        public bool UpdateProduct(int id, Product product)
        {
            using (_connection = new SqlConnection(SqlConnectionStrings.GetConnectionString))
            {
                using (_command = new SqlCommand("usp_UpdateProduct2", _connection))
                {
                    _command.CommandType = CommandType.StoredProcedure;
                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }
                    _command.Parameters.Add(new SqlParameter("@prodID", SqlDbType.Int, 32));
                    _command.Parameters.Add(new SqlParameter("@productName", SqlDbType.NVarChar, 40));
                    _command.Parameters.Add(new SqlParameter("@unitPrice", SqlDbType.Money, 8));
                    _command.Parameters.Add(new SqlParameter("@unitsInStock", SqlDbType.SmallInt, 2));

                    _command.Parameters["@productName"].Value = product.ProductName;
                    _command.Parameters["@unitPrice"].Value = product.UnitPrice.Value;
                    _command.Parameters["@unitsInStock"].Value = product.UnitsInStock.Value;


                    _command.Parameters["@prodID"].Value = id;

                    var res = _command.ExecuteNonQuery();
                    return res > 0;
                }
            }
        }
    }
}
