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
using static System.Net.Mime.MediaTypeNames;

namespace Variant2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<string> _filteringList = new();
        private List<string> _sortingList = new();
        private List<ProductMaterial> _productMaterialsList;
        private List<Sort> _sorts = new();
        private List<Sort> _searchList = new();

        public List<Sort> SearchList
        {
            get { return _searchList; }
            set { _searchList = value; }
        }

        private List<Sort> _publicList = new();
        private string _selectedFilteringItem;
        private string _selectedSortingItem;
        private string _search;

        public string Search
        {
            get => _search;
            set 
            {
                _search = value;
                Sorts = Sorts
                    .Where(p => p.ProductTitle.ToLower().Contains(value.ToLower()))
                    .ToList();

                if (Sorts.Count() == 0)
                {

                }
            }
        }


        #region Свойства
        public List<string> FilteringList
        {
            get => _filteringList;
            set
            {
                _filteringList = value;
                OnPropertyChanged(nameof(FilteringList));
            }
        }

        public List<string> SortingList
        {
            get => _sortingList;
            set
            {
                _sortingList = value;
                OnPropertyChanged(nameof(SortingList));
            }
        }

        public List<ProductMaterial> ProductMaterialsList
        {
            get => _productMaterialsList;
            set 
            { 
                _productMaterialsList = value;
                OnPropertyChanged(nameof(ProductMaterialsList));
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

        public List<Sort> PublicList
        {
            get => _publicList;
            set
            {
                _publicList = value;
                OnPropertyChanged(nameof(PublicList));
            }
        }

        public string SelectedFilteringItem
        {
            get => _selectedFilteringItem;
            set
            {
                _selectedFilteringItem = value;
                Filtering(value);
                OnPropertyChanged(nameof(SelectedFilteringItem));
            }
        }

        public string SelectedSortingItem
        {
            get => _selectedSortingItem;
            set
            {
                _selectedSortingItem = value;
                Sorting(value);
                OnPropertyChanged(nameof(SelectedSortingItem));
            }
        }
        #endregion

        public MainWindowViewModel()
        {
            CteateSortingAndFilteringList();
            CreatePublicList();
            AddNewRecord = ReactiveCommand.Create(OpenAddNewRecord);
        }   

        private void CteateSortingAndFilteringList()
        {
            using (TestContext db = new TestContext())
            {
                FilteringList.Add("Все продукты");
                foreach (var item in db.ProductTypes)
                {
                    FilteringList.Add(item.Title);
                }
            }
            SortingList.Add("По названию продукта");
            SortingList.Add("По типу продукта");
            SortingList.Add("По стоимость");
            SortingList.Add("По артикулу");
        }

        private void CreatePublicList()
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
        }

        private void Sorting(string str)
        {
            var list = Sorts;
            if (str == "По типу продукта")
            {
                Sorts = list.OrderBy(p => p.ProductTypeTitle).ToList();
            }
            else if (str == "По названию продукта")
            {
                Sorts = list.OrderBy(p => p.ProductTitle).ToList();
            }
            else if (str == "По стоимость")
            {
                Sorts = list.OrderBy(p => p.MinCostForAgent).ToList();
            }
            else if (str == "По артикулу")
            {
                Sorts = list.OrderBy(p => p.ArticleNumber).ToList();
            }
        }

        private void Filtering(string str)
        {
            Sorts = PublicList.Where(p => p.ProductTypeTitle == str).ToList();
            if (str == "Все продукты")
            {
                Sorts = PublicList;
            }
            SelectedSortingItem = "По названию продукта";
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
