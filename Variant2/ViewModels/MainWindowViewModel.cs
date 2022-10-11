using DynamicData;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Variant2.Models;
using Variant2.Views;

namespace Variant2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<string> _filteringList = new();
        private List<ProductMaterial> _productMaterialsList;
        private List<Sort> _sorts = new();

        public List<Sort> Sorts
        {
            get => _sorts;
            set => _sorts = value;
        }





        public List<ProductMaterial> ProductMaterialsList
        {
            get => _productMaterialsList;
            set => _productMaterialsList = value;
        }
        public List<string> FilteringList
        {
            get => _filteringList;
            set => _filteringList = value;
        }

        public MainWindowViewModel()
        {
            using (TestContext db = new TestContext())
            {
                ProductMaterialsList = db.ProductMaterials
                    .Include(m => m.Material)
                    .Include(p => p.Product)
                    .ThenInclude(pt => pt.ProductType)
                    .ToList();

                for (int i = 0; i < ProductMaterialsList.Count();)
                {
                    Sort sort = new Sort
                    {
                        ProductTitle = ProductMaterialsList[i].Product.Title,
                        ProductTypeTitle = ProductMaterialsList[i].Product.ProductType.Title,
                        Image = ProductMaterialsList[i].Product.Image,
                        MaterialTitle = ProductMaterialsList[i].Material.Title,
                        ArticleNumber = ProductMaterialsList[i].Product.ArticleNumber,
                        MinCostForAgent = ProductMaterialsList[i].Product.MinCostForAgent
                    };
                    Sorts.Add(sort);
                    ProductMaterialsList.Remove(ProductMaterialsList[i]);

                    for (int j = 0; j < ProductMaterialsList.Count(); j++)
                    {
                        if (Sorts.Last().ProductTitle == ProductMaterialsList[j].Product.Title)
                        {
                            Sorts.Last().MaterialTitle = Sorts.Last().MaterialTitle + ", " + ProductMaterialsList[j].Material.Title;
                            ProductMaterialsList.Remove(ProductMaterialsList[j]);
                            j--;
                        }
                    }
                }

                foreach (var item in db.ProductTypes)
                {
                    FilteringList.Add(item.Title);
                }
            }

            AddNewRecord = ReactiveCommand.Create(OpenAddNewRecord);
        }

        public ReactiveCommand<Unit, Unit> AddNewRecord { get; }
        void OpenAddNewRecord()
        {
            Window1 addNewRecord = new Window1();
            addNewRecord.Show();
        }
    }

    public class Sort
    {
        public string ProductTitle { get; set; } = null!;
        public string ProductTypeTitle { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string MaterialTitle { get; set; } = null!;
        public string ArticleNumber { get; set; } = null!;
        public decimal MinCostForAgent { get; set; }
    }
}
