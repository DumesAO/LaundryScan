using LaundryScan.ViewModels;

namespace LaundryScan.Views;

public partial class ScanResultsPage : ContentPage
{
	public ScanResultsPage(List<string> results)
	{
		InitializeComponent();
		this.ViewModel=new(this, results);
		this.BindingContext=this.ViewModel;
	}
	public ScanResultsVM ViewModel { get; set; }
}