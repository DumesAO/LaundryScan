using LaundryScan.Models;
using LaundryScan.ViewModels;

namespace LaundryScan.Views;

public partial class OutfitDetailPage : ContentPage
{
	public OutfitDetailPage(Outfit outfit)
	{
		InitializeComponent();
		this.ViewModel = new(this, outfit);
		this.BindingContext = ViewModel;
	}
	public OutfitDetailVM ViewModel { get; set; }
}