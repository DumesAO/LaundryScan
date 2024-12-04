using LaundryScan.Models;
using LaundryScan.ViewModels;

namespace LaundryScan.Views;

public partial class RedactClothingItemPage : ContentPage
{
	public RedactClothingItemPage(ClothingItem clothingItem)
	{
		InitializeComponent();
		this.ViewModel = new(this, clothingItem);
		this.BindingContext=ViewModel;
	}

	RedactClothingItemVM ViewModel { get; set; }
}