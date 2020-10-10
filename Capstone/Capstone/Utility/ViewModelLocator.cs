using Capstone.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Utility
{
    public static class ViewModelLocator
    {
        public static PropertyExplorerViewModel PropertyExplorerViewModel { get; set; }
        = new PropertyExplorerViewModel();
    }
}
