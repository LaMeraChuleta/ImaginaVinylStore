using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedApp.Models;

namespace Client.App.Pages
{
    public partial class CatalogAudioCreate : ComponentBase
    {
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public IAudioCatalogService AudioCatalogService { get; set; }
        [Inject] public IProductService ProductService { get; set; }

        private AudioCatalog NewAudioCatalog { get; set; } = new();
        private List<IBrowserFile> PhotoAudioCatalog { get; } = new();
        private List<string> PhotoAudioCatalogBase64 { get; } = new();
        private EditContext _newContextAudioCatalog;
        private const int MaxAllowedFiles = 3;
        private bool IsLoading { get; set; }

        protected override void OnInitialized()
        {
            IsLoading = true;
            _newContextAudioCatalog = new EditContext(NewAudioCatalog);
            IsLoading = false;
            StateHasChanged();
        }

        private async void CreateCataloAudios()
        {
            try
            {
                if (!_newContextAudioCatalog.Validate()) return;

                NewAudioCatalog = await AudioCatalogService.CreateAsync(NewAudioCatalog);
                foreach (var file in PhotoAudioCatalog)
                {
                    var image = await AudioCatalogService.CreateImageAsync(NewAudioCatalog!, file);
                    NewAudioCatalog?.Images?.ToList().Add(image);
                }
                await ProductService.CreateAsync(NewAudioCatalog!);

                PhotoAudioCatalog.Clear();
                NewAudioCatalog = new AudioCatalog();
                ToastService.ShowToast(ToastLevel.Success, $"Exito se creo {NewAudioCatalog!.Name}-{NewAudioCatalog.Brand} en el catalogo");
                StateHasChanged();
            }
            catch (Exception exception)
            {
                ToastService.ShowToast(ToastLevel.Error, exception.Message);
            }
        }
        private async void SaveImage(InputFileChangeEventArgs e, List<IBrowserFile> photos, List<string> photoBase64)
        {
            foreach (var file in e.GetMultipleFiles(MaxAllowedFiles))
            {
                var buffer = new byte[file.Size];
                var _ = await file.OpenReadStream().ReadAsync(buffer);
                var imageDataUrl = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
                photos.Add(file);
                photoBase64.Add(imageDataUrl);
            }

            StateHasChanged();
        }
    }
}
