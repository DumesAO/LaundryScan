using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LaundryScan.Models;
using LaundryScan.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryScan.ViewModels
{
    public partial class SelectNewItemVM : ObservableObject
    {
        public SelectNewItemVM(SelectNewItemPage page, Outfit outfit)
        {
            this.Page = page;
            this.Outfit = outfit;
            GetData();
            SelectedCategory = Categories.First();
            SelectedMaterial = Materials.First();
        }
        void GetData()
        {
            ClothingItems.Clear();
            var clothingItems = App.Database.GetClothingItems();
            foreach (ClothingItem clothingItem in clothingItems)
            {
                ClothingItems.Add(clothingItem);
            }
            var categories = App.Database.GetCategories();
            Categories.Add(new Category { Name = "Все", ID = 0 });
            foreach (Category category in categories)
            {
                Categories.Add(category);
            }

            var materials = App.Database.GetMaterials();
            Materials.Add(new Material { Name = "Все", ID = 0 });
            foreach (Material material in materials)
            {
                Materials.Add(material);
            }
        }
        public ObservableCollection<ClothingItem> ClothingItems { get; } = [];
        public ObservableCollection<ClothingItem> FilteredClothingItems { get; } = [];
        [ObservableProperty] private ClothingItem selection;
        public ObservableCollection<Category> Categories { get; } = [];
        public ObservableCollection<Material> Materials { get; } = [];
        private Category _selectedCategory;
        private Material _selectedMaterial;

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                {
                    ApplyFilters();
                }
            }
        }

        public Material SelectedMaterial
        {
            get => _selectedMaterial;
            set
            {
                if (SetProperty(ref _selectedMaterial, value))
                {
                    ApplyFilters();
                }
            }
        }
        private void ApplyFilters()
        {
            var filteredItems = ClothingItems.Where(item =>
                ((SelectedCategory.ID == 0 || SelectedCategory == null) || item.CategoryID == SelectedCategory.ID) &&
                ((SelectedMaterial == null || SelectedMaterial.ID == 0) || item.MaterialID == SelectedMaterial.ID)).ToList();

            FilteredClothingItems.Clear();
            foreach (var item in filteredItems)
            {
                FilteredClothingItems.Add(item);
            }
        }
        public SelectNewItemPage Page { get; }
        public Outfit Outfit { get; }
        [RelayCommand]
        private async Task AddItem()
        {
            if (Selection == null) return;

            if (Outfit.ClothingItems.Contains(Selection)) await Page.Navigation.PushAsync(new RedactOutfitPage(Outfit));
            else
            {
                Selection=App.Database.GetCareSymbolsForItem(Selection);
                Outfit.ClothingItems.Add(Selection);
                await Page.Navigation.PushAsync(new RedactOutfitPage(Outfit));
            }
        }
    }
}
