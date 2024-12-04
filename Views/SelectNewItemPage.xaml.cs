using LaundryScan.Models;
using LaundryScan.ViewModels;

namespace LaundryScan.Views;

public partial class SelectNewItemPage : ContentPage
{
	public SelectNewItemPage(Outfit outfit)
	{
		InitializeComponent(); 
        this.ViewModel = new(this, outfit);
        this.BindingContext = ViewModel;
    }
    public SelectNewItemVM ViewModel { get; set; }
}