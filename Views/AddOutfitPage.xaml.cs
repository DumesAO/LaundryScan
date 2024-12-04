using LaundryScan.Models;
using LaundryScan.ViewModels;
namespace LaundryScan.Views
{
    public partial class AddOutfitPage : ContentPage
    {
        public AddOutfitPage(IList<ClothingItem> clothingItems)
        {
            InitializeComponent();
            this.ViewModel = new(this);
            this.BindingContext = this.ViewModel;
            foreach (ClothingItem item in clothingItems)
            {
                this.ViewModel.SelectedClothingItems.Add(item);
            }

        }
        public AddOutfitPage()
        {
            InitializeComponent();
            this.ViewModel = new(this);
            this.BindingContext = this.ViewModel;
        }
        public AddOutfitVM ViewModel { get; }
    }
}
