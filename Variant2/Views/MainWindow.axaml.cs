using Avalonia.Controls;
using Variant2.ViewModels;

namespace Variant2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}