using LaundryScan.Models;
using LaundryScan.ViewModels;

namespace LaundryScan.Views;

public partial class SelectNewLabelPage : ContentPage
{
	public SelectNewLabelPage(ClothingItem clothingItem)
	{
		InitializeComponent();
		this.ViewModel = new(this, clothingItem);
		this.BindingContext = ViewModel;
	}
	public SelectNewLabelVM ViewModel { get; set; }
}