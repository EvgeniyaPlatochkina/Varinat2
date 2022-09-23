using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using Variant2.Models;

namespace Variant2.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public static List<Product> GetAllProducts()
        {
            using (Gorshunov03Context db = new Gorshunov03Context())
            {
                var getAllProducts = db.Products.ToList();
                return getAllProducts;
            }
        }

        public static List<ProductType> GetAllProductTypes()
        {
            using (Gorshunov03Context db = new Gorshunov03Context())
            {
                var getAllProductTypes = db.ProductTypes.ToList();
                return getAllProductTypes;
            }
        }
    }
}
