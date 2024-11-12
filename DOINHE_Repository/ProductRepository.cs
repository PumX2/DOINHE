using DOINHE_BusinessObject;
using DOINHE_DataAccess;
using System.Collections.Generic;

namespace DOINHE_Repository
{
    public class ProductRepository : IProductRepository
    {
        public List<Product> GetAllProducts() => ProductDAO.GetProducts();
        public Product GetProductById(int id) => ProductDAO.GetProductById(id);
        public void SaveProduct(Product product) => ProductDAO.InsertProduct(product);
        public void UpdateProduct(Product product) => ProductDAO.UpdateProduct(product);
        public void DeleteProduct(Product product) => ProductDAO.DeleteProduct(product);
    }
}
