using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class WardrobeVM : ObservableObject
    {
        public WardrobeVM(WardrobePage page)
        {
            this.Page = page;
            GetData();
            SelectedCategory = Categories.First();
            SelectedMaterial = Materials.First();
        }
        
        public void GetData()
        {
            ClothingItems.Clear();
            var clothingItems = App.Database.GetClothingItems();
            foreach (ClothingItem clothingItem in clothingItems)
            {
                ClothingItems.Add(clothingItem);
            }
            Outfits.Clear();
            var outfits = App.Database.GetOutfits();
            foreach (Outfit outfit in outfits)
            {
                Outfits.Add(outfit);
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
        public WardrobePage Page { get; }
        public ObservableCollection<ClothingItem> ClothingItems { get; } = [];
        public ObservableCollection<Outfit> Outfits { get; } = [];
        public ObservableCollection<ClothingItem> FilteredClothingItems { get; } = [];
        public ObservableCollection<Category> Categories { get; } = [];
        public ObservableCollection<Material> Materials { get; } = [];
        private Category _selectedCategory;
        private Material _selectedMaterial;
        private ClothingItem _selectedClothingItem;
        private Outfit _selectedOutfit;

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
        public ClothingItem SelectedClothingItem
        {
            get => _selectedClothingItem;
            set
            {
                if (SetProperty(ref _selectedClothingItem, value))
                {
                    ShowDetails();
                }
            }
        }
        public Outfit SelectedOutfit
        {
            get => _selectedOutfit;
            set
            {
                if (SetProperty(ref _selectedOutfit, value))
                {
                    ShowDetailsOutfit();
                }
            }
        }
        private async Task ShowDetails()
        {
            if (SelectedClothingItem == null) return;
            await Page.Navigation.PushAsync(new ClothingItemDetailPage(SelectedClothingItem));
            SelectedClothingItem = null;
        }
        private async Task ShowDetailsOutfit()
        {
            if (SelectedOutfit == null) return;
            await Page.Navigation.PushAsync(new OutfitDetailPage(SelectedOutfit));
            SelectedOutfit = null;
        }
        private void ApplyFilters()
        {
            var filteredItems = ClothingItems.Where(item =>
                ((SelectedCategory.ID == 0 || SelectedCategory==null) || item.CategoryID == SelectedCategory.ID) &&
                ((SelectedMaterial == null || SelectedMaterial.ID==0 )|| item.MaterialID == SelectedMaterial.ID)).ToList();

            FilteredClothingItems.Clear();
            foreach (var item in filteredItems)
            {
                FilteredClothingItems.Add(item);
            }
        }
    }
}
