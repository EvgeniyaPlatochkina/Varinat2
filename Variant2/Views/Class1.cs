using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Variant2.ViewModels;

namespace Variant2.Views
{
    public class Class1 : ViewModelBase
    {
        private string _image;

        public string image
        {
            get { return _image; }
            set { _image = value; }
        }

        public Class1()
        {
            image = "\\Image\\lopushok.png";
        }
    }
}
