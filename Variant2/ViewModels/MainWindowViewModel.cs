using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Variant2.Models;

namespace Variant2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<string> _filteringList = new();
        private List<ProductMaterial> _productMaterialsList;

        public List<ProductMaterial> ProductMaterialsList
        {
            get => _productMaterialsList;
            set => _productMaterialsList = value;
        }
        public List<string> FilteringList { get => _filteringList; set => _filteringList = value; }

        public MainWindowViewModel()
        {
            using (Gorshunov03Context db = new Gorshunov03Context())
            {
                ProductMaterialsList = db.ProductMaterials
                    .Include(m => m.Material)
                    .Include(p => p.Product)
                    .ThenInclude(pt => pt.ProductType)
                    .ToList();

                foreach (var item in db.ProductTypes)
                {
                    FilteringList.Add(item.Title);
                }
            }
        }
    }
}