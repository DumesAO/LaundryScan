
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LaundryScan.Models;
using LaundryScan.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryScan.ViewModels
{
    public partial class CameraVM : ObservableObject
    {
        private bool _isTorchOn;

        public bool IsTorchOn
        {
            get => _isTorchOn;
            set => SetProperty(ref _isTorchOn, value);
        }

        [ObservableProperty]private string predictions;
        public CameraVM(CameraPage page)
        {
            this.Page = page;
            button="photo_icon.png";
        }
        public CameraVM(CameraPage page,ClothingItem clothingItem)
        {
            this.Page = page;
            this.clothingItem = clothingItem;
        }
        public CameraPage Page { get; set; }
        private ClothingItem clothingItem=null;
        public bool isGenerating { get; set; }
        public ImageSource button {  get; set; }
        public async Task ProcessCapturedPhoto(ImageSource image)
        {
            this.isGenerating = true;
            var predictions = await Model.GetPredictions(image);
            if (clothingItem==null)
                await ExecuteOnMainThreadAsync(async () =>
            {
                await Page.Navigation.PushAsync(new ScanResultsPage(predictions));
                button = "photo_icon.png";
                this.isGenerating = false;
            });


            else
            {
                List<CareSymbol> careSymbols = [];
                foreach (var item in predictions)
                {
                    CareSymbol careSymbol = App.Database.GetCareSymbolByCode(item);
                    if (careSymbol != null)
                    {
                        careSymbols.Add(careSymbol);
                    }

                }
                await ExecuteOnMainThreadAsync(async () =>
                {
                    await Page.Navigation.PushAsync(new AddClothPage(careSymbols, clothingItem));
                    button = "photo_icon.png";
                    this.isGenerating= false;
                });
                
            }
                
        }

        async Task ExecuteOnMainThreadAsync(Func<Task> asyncAction)
        {
            var tcs = new TaskCompletionSource();
            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await asyncAction();
                    tcs.SetResult();
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            await tcs.Task;
        }
        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Page.Navigation.PopAsync();
        }
    }
}
