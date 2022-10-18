using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Variant2.Models;
using Variant2.Views;

namespace Variant2.ViewModels
{
    public class AddNewRecordViewModel : ViewModelBase
    {
        private List<Material> _materialList;
        private List<Product> _productList;
        private string _countText;
        private List<MaterialAndCount> _materialAndCountList;

        public List<MaterialAndCount> MaterialAndCountList
        {
            get { return _materialAndCountList; }
            set { _materialAndCountList = value; }
        }

        private Material _materialItem;
        private Product _productItem;

        public Product ProductItem
        {
            get { return _productItem; }
            set { _productItem = value; }
        }


        public Material MaterialItem
        {
            get { return _materialItem; }
            set { _materialItem = value; }
        }




        #region Свойства
        public List<Material> MaterialList
        {
            get => _materialList;
            set
            {
                _materialList = value;
                OnPropertyChanged(nameof(MaterialList));
            }
        }
        public List<Product> ProductList
        {
            get => _productList;
            set
            {
                _productList = value;
                OnPropertyChanged(nameof(ProductList));
            }
        }
        public string CountText
        {
            get { return _countText; }
            set
            {
                _countText = value;
            }
        }
        #endregion

        public AddNewRecordViewModel()
        {
            using (TestContext db = new TestContext())
            {
                ProductList = db.Products.ToList();
                MaterialList = db.Materials.ToList();

                AddMaterial = ReactiveCommand.Create(AddMaterialToList);
            }
        }

        public ReactiveCommand<Unit, Unit> AddMaterial { get; }
        void AddMaterialToList()
        {
            MaterialAndCount pm = new MaterialAndCount
            {
                MaterialTitle = MaterialItem.Title,
                Count = Convert.ToInt32(CountText)
            };
            MaterialAndCountList.Add(pm);
        }

        public class MaterialAndCount
        {
            public string MaterialTitle { get; set; } = null!;
            public int Count { get; set; }
        }
    }
}
