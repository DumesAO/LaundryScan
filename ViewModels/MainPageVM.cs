using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LaundryScan.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryScan.ViewModels
{
    public partial class MainPageVM: ObservableObject
    {
        public MainPageVM(MainPage page) 
        {
            this.Page = page;
        }
        [RelayCommand]
        private async Task GoCare()
        {
            await Page.Navigation.PushAsync(new CareSymbolsPage());
        }
        [RelayCommand]
        private async Task GoAddOutfit()
        {
            await Page.Navigation.PushAsync(new AddOutfitPage());
        }
        [RelayCommand]
        private async Task GoAddCloth()
        {
            await Page.Navigation.PushAsync(new AddClothPage());
        }
        [RelayCommand]
        private async Task GoWardrobe()
        {
            await Page.Navigation.PushAsync(new WardrobePage());
        }
        [RelayCommand]
        private async Task GoCamera()
        {
            await Page.Navigation.PushAsync(new CameraPage());
        }
        public MainPage Page { get; }
    }
}
