
using LaundryScan.ViewModels;

namespace LaundryScan.Views
{
    public partial class MainPage : ContentPage
    {

        public MainPage() {
            InitializeComponent();
            this.ViewModel = new(this);
            this.BindingContext = this.ViewModel;
        }

        MainPageVM ViewModel { get; }
    }

}
