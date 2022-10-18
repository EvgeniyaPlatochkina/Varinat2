using Avalonia.Controls;
using System;
using Variant2.ViewModels;

namespace Variant2.Views
{
    public partial class AddNewRecord : Window
    {
        public AddNewRecord()
        {
            InitializeComponent();
            DataContext = new AddNewRecordViewModel();
        }
    }
}
