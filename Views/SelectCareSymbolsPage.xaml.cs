using LaundryScan.Models;
using LaundryScan.ViewModels;
namespace LaundryScan.Views 
{ 

    public partial class SelectCareSymbolsPage : ContentPage
    {
	    public SelectCareSymbolsPage(ClothingItem clothingItem)
	    {
		    InitializeComponent();
            this.ViewModel = new(this, clothingItem);
            this.BindingContext = this.ViewModel;
        }
        public SelectCareSymbolsVM ViewModel { get;}
    }
}