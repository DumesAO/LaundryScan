using LaundryScan.ViewModels;

namespace LaundryScan.Views
{
    public partial class WardrobePage : ContentPage
    {
        public WardrobePage()
        {
            InitializeComponent();
            this.ViewModel = new(this);
            this.BindingContext = this.ViewModel;
        }

        public WardrobeVM ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.GetData();
        }
    }
}