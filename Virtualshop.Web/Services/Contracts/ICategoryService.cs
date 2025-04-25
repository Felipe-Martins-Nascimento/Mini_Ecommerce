using Virtualshop.Web.Models;

namespace Virtualshop.Web.Services.Contracts;

public interface ICategoryService
{
    Task<IEnumerable<CategoryViewModel>> GetAllCategories();
}
