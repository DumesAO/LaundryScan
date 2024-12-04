using LaundryScan.Models;
using LaundryScan.ViewModels;
using System.Collections.ObjectModel;
namespace LaundryScan.Views
{
    public partial class AddClothPage : ContentPage
    {

        public AddClothPage(IList<CareSymbol> careSymbols,ClothingItem clothingItem)
        {
            clothingItem.CareSymbols = careSymbols.ToList();
            InitializeComponent();
            this.ViewModel = new(this,clothingItem);
            this.BindingContext = this.ViewModel;

        }
        public AddClothPage()
        {
            InitializeComponent();
            this.ViewModel = new(this);
            this.BindingContext = this.ViewModel;
        }
        public AddClothVM ViewModel { get; }

    }
}
