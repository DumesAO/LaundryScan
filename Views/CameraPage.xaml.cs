using LaundryScan.Models;
using LaundryScan.ViewModels;

namespace LaundryScan.Views;

public partial class CameraPage : ContentPage
{
	public CameraPage()
	{
		InitializeComponent();
		this.ViewModel = new(this);
		this.BindingContext = ViewModel;
	}
    public CameraPage(ClothingItem clothingItem)
    {
        InitializeComponent();
        this.ViewModel = new(this, clothingItem);
        this.BindingContext = ViewModel;
    }
    public CameraVM ViewModel { get; set; }

    private async void CameraView_MediaCaptured(object sender, CommunityToolkit.Maui.Views.MediaCapturedEventArgs e)
    {
		var image= ImageSource.FromStream(() => e.Media);
        
        await ViewModel.ProcessCapturedPhoto(image);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        this.CameraView.Focus();
        if (ViewModel.isGenerating)
            return;
		await CameraView.CaptureImage(CancellationToken.None);
    }
}