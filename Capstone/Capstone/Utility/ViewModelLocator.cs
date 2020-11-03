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
        public static LoginViewModel LoginViewModel { get; set; }
        = new LoginViewModel();
        public static IndividualPropertyViewModel IndividualPropertyViewModel { get; set; }
        = new IndividualPropertyViewModel();
        public static ConfirmationPageViewModel ConfirmationPageViewModel { get; set; }
        = new ConfirmationPageViewModel();
        public static CreateAccountViewModel CreateAccountViewModel { get; set; }
        = new CreateAccountViewModel();
    }
}
