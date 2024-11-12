using DOINHE_BusinessObject;
using DOINHE_DataAccess;
using System.Collections.Generic;

namespace DOINHE_Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public List<Category> GetAllCategories() => CategoryDAO.GetCategories();
        public Category GetCategoryById(int id) => CategoryDAO.GetCategoryById(id);
        public void SaveCategory(Category category) => CategoryDAO.InsertCategory(category);
        public void UpdateCategory(Category category) => CategoryDAO.UpdateCategory(category);
        public void DeleteCategory(Category category) => CategoryDAO.DeleteCategory(category);
    }
}
