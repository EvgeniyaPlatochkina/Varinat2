using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using System.Collections.Generic;
using System.IO;
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
        private List<Sort> _publicList = new();
        private string _filtering;

        #region Свойства
        public List<Sort> PublicList
        {
            get { return _publicList; }
            set { _publicList = value; }
        }
        public string Filtering
        {
            get
            { return _filtering; }
            set
            {
                _filtering = value;
                Sorts = PublicList.Where(p => p.ProductTypeTitle == value).ToList();
                if (value == "Все продукты")
                {
                    Sorts = PublicList;
                }
            }
        }
        public List<Sort> Sorts
        {
            get => _sorts;
            set
            {
                _sorts = value;
                OnPropertyChanged(nameof(Sorts));
            }
        }
        public List<ProductMaterial> ProductMaterialsList
        {
            get => _productMaterialsList;
            set { _productMaterialsList = value; }
        }
        public List<string> FilteringList
        {
            get => _filteringList;
            set => _filteringList = value;
        }
        #endregion

        public MainWindowViewModel()
        {
            using (TestContext db = new TestContext())
            {
                FilteringList.Add("Все продукты");
                foreach (var item in db.ProductTypes)
                {
                    FilteringList.Add(item.Title);
                }

                ProductMaterialsList = db.ProductMaterials
                    .Include(m => m.Material)
                    .Include(p => p.Product)
                    .ThenInclude(pt => pt.ProductType)
                    .ToList();

                for (int i = 0; i < ProductMaterialsList.Count();)
                {
                    var image = @"\products\picture.png";
                    if (ProductMaterialsList[i].Product.Image != "")
                    {
                        image = ProductMaterialsList[i].Product.Image;
                    }
                    Sort sort = new Sort
                    {
                        ProductTitle = ProductMaterialsList[i].Product.Title,
                        ProductTypeTitle = ProductMaterialsList[i].Product.ProductType.Title,
                        Image = new Bitmap("." + image),
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
                PublicList = Sorts;
            }
            AddNewRecord = ReactiveCommand.Create(OpenAddNewRecord);
        }

        public ReactiveCommand<Unit, Unit> AddNewRecord { get; }
        void OpenAddNewRecord()
        {
            AddNewRecord addNewRecord = new AddNewRecord();
            addNewRecord.Show();
        }
    }

    public class Sort
    {
        public string ProductTitle { get; set; } = null!;
        public string ProductTypeTitle { get; set; } = null!;
        public Bitmap Image { get; set; } = null!;
        public string MaterialTitle { get; set; } = null!;
        public string ArticleNumber { get; set; } = null!;
        public decimal MinCostForAgent { get; set; }
    }
}
