using Avalonia;
using Avalonia.Controls;
using HarfBuzzSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using Variant2.Models;
using Variant2.Views;

namespace Variant2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<string> _filteringList = new();
        private List<ProductMaterial> _productMaterialsList;
        public AddNewRecord addNewRecord { get; set; } = new();

        public List<ProductMaterial> ProductMaterialsList
        {
            get => _productMaterialsList;
            set => _productMaterialsList = value;
        }
        public List<string> FilteringList { get => _filteringList; set => _filteringList = value; }

        public MainWindowViewModel()
        {
          

            DoTheThing = ReactiveCommand.Create(RunTheThing);

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


        public ReactiveCommand<Unit, Unit> DoTheThing { get; }

        void RunTheThing()
        {
            addNewRecord.Show();
        }
    }
}