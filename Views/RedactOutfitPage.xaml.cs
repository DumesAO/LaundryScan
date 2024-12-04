using LaundryScan.Models;
using LaundryScan.ViewModels;

namespace LaundryScan.Views;

public partial class RedactOutfitPage : ContentPage
{
	public RedactOutfitPage(Outfit outfit)
	{
        InitializeComponent();
        this.ViewModel = new(this, outfit);
        this.BindingContext = ViewModel;
    }

    public RedactOutfitVM ViewModel { get; set; }
}