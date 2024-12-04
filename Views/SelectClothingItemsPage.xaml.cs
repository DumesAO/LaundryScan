using LaundryScan.ViewModels;

namespace LaundryScan.Views;

public partial class SelectClothingItemsPage : ContentPage
{
	public SelectClothingItemsPage()
	{
		InitializeComponent();
        this.ViewModel = new(this);
        this.BindingContext = this.ViewModel;
    }
    public SelectClothingItemsVM ViewModel { get; }
}