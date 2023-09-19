using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTI.Project.DAL
{
    public interface IProduct
    {
        List<Product> GetAllProduct();
        Product GetProductById(int id);
        bool AddProduct( string productName, decimal? unitPrice, short? unitsInStock);
        bool DeleteProduct(int id);
        bool UpdateProduct(int id, Product product);
    }
    public interface IProductAdditional
    {
        List<Product> GetProductByPrice(decimal price);
    }
}
