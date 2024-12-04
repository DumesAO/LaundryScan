using System.Collections.ObjectModel;
using LaundryScan.Models;
using LaundryScan.ViewModels;
namespace LaundryScan.Views
{
    public partial class CareSymbolsPage : ContentPage
    {
        public CareSymbolsPage()
        {
            InitializeComponent();

            this.ViewModel = new(this);
            this.BindingContext = this.ViewModel;
        }

        public CareSymbolsVM ViewModel { get; }


    }
}