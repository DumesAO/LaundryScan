using LaundryScan.Models;
using LaundryScan.ViewModels;

namespace LaundryScan.Views;

public partial class ClothingItemDetailPage : ContentPage
{
	public ClothingItemDetailPage(ClothingItem clothingItem)
	{
		InitializeComponent();
		this.ViewModel = new(this, clothingItem);
		BindingContext = ViewModel;
	}
	public CothingItemDetailVm ViewModel { get; }
}