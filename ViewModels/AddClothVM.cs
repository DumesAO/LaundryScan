using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LaundryScan.Models;
using LaundryScan.Views;
using System.Collections.ObjectModel;

namespace LaundryScan.ViewModels
{
    public partial class AddClothVM : ObservableObject
    {
        [ObservableProperty] private ClothingItem clothingItem=new();

        public ObservableCollection<Category> Categories { get; } = new();
        public ObservableCollection<Material> Materials { get; } = new();
        [ObservableProperty] private string image ="placeholder.png";
        [ObservableProperty] private Category category = null;
        [ObservableProperty] private Material material = null;

        public ObservableCollection<CareSymbol> CareSymbols { get; } = [];
        public AddClothVM(AddClothPage page, ClothingItem clothingitem)
        {
            this.Page = page;
            this.ClothingItem = clothingitem;
            GetData();
            this.Category = ClothingItem.Category;
            this.Material = ClothingItem.Material;
            this.CareSymbols = new ObservableCollection<CareSymbol>(ClothingItem.CareSymbols);
        }
        public AddClothVM(AddClothPage page)
        {
            this.Page = page;
            GetData();
            GetData();
        }
        public AddClothPage Page { get; }
        void GetData()
        {
            var categories=App.Database.GetCategories();
            var materials=App.Database.GetMaterials();

            Categories.Clear();
            Materials.Clear();
            foreach (var category in categories)
                Categories.Add(category);
            foreach (var material in materials)
                Materials.Add(material);
        }

        [RelayCommand]
        private async Task PickImage()
        {
            var action = await Page.DisplayActionSheet("Оберіть джерело зображення", "Відміна", null, "Камера", "Галерея");
            if (action == "Камера")
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    var stream = await photo.OpenReadAsync();
                    ClothingItem.ImageURL = photo.FullPath;
                    this.Image = ClothingItem.ImageURL;
                }
            }
            else if (action == "Галерея")
            {
                var photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null)
                {
                    ClothingItem.ImageURL = photo.FullPath;
                    this.Image = ClothingItem.ImageURL;
                }
            }
        }

        [RelayCommand]
        private async Task SelectCareSymbols()
        {
            if(Category!=null)
                ClothingItem.CategoryID = Category.ID;
            ClothingItem.ImageURL = image;
            if(Material!=null)
                ClothingItem.MaterialID=Material.ID;
            await Page.Navigation.PushAsync(new SelectCareSymbolsPage(ClothingItem));
        }

        [RelayCommand]
        private async Task ScanCareSymbols()
        {
            if (Category != null)
                ClothingItem.CategoryID = Category.ID;
            ClothingItem.ImageURL = image;
            if (Material != null)
                ClothingItem.MaterialID = Material.ID;
            await Page.Navigation.PushAsync(new CameraPage(ClothingItem));
        }

        [RelayCommand]
        private async Task AddCloth()
        {
            if (Category != null)
                ClothingItem.CategoryID = Category.ID;
            ClothingItem.ImageURL = image;
            if (Material != null)
                ClothingItem.MaterialID = Material.ID;
            if (ClothingItem.Name == String.Empty || ClothingItem.Category == null || ClothingItem.Material == null || ClothingItem.ImageURL == null)
                return;

            App.Database.SaveClothingItem(ClothingItem);

            await Page.DisplayAlert("Успіх", "Новий елемент одягу створено!", "OK");
            await Page.Navigation.PushAsync(new MainPage());
        }
    }
}
