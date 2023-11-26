using Client.App.Interfaces;
using Client.App.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedApp.Models;

namespace Client.App.Components
{
    public partial class ImagesManage : ComponentBase, IDisposable
    {
        private const int MaxAllowedFiles = 3;
        [Parameter] public List<ImageCatalog>? Images { get; set; }
        [Parameter] public EventCallback<bool> SetIsAnyImage { get; set; }
        [Inject] public ICatalogMusicService catalogMusicService { get; set; }
        private List<ImageTypeWrapper> ImagesData { get; set; } = new();

        protected override void OnInitialized()
        {
            CatalogMusicCreate.OnCompleteInsert += InsertImagesInDb;
            CatalogAudioCreate.OnCompleteInsert += InsertImagesInDb;
            CatalogMusicEdit.OnCompleteEdit += InsertImagesInDb;
            base.OnInitialized();
        }
        public void Dispose()
        {
            CatalogMusicCreate.OnCompleteInsert -= InsertImagesInDb;
            CatalogAudioCreate.OnCompleteInsert -= InsertImagesInDb;
            CatalogMusicEdit.OnCompleteEdit -= InsertImagesInDb;
        }
        protected override async Task OnInitializedAsync()
        {
            if (Images is not null)
            {
                Images.ForEach(x =>
                {
                    ImagesData.Add(new ImageTypeWrapper(x));
                });
                await SetIsAnyImage.InvokeAsync(ImagesData.Any());
            }
            await base.OnInitializedAsync();
        }
        private async void InsertImagesInDb(int id)
        {
            try
            {
                var value = await catalogMusicService.GetByIdAsync(id);
                ImagesData.ForEach(x =>
                {
                    catalogMusicService.CreateImageAsync(value, x.BrowserImage);
                });
                ImagesData.Clear();
            }
            catch
            {
                throw;
            }
        }
        private async Task SaveImage(InputFileChangeEventArgs e)
        {
            try
            {
                foreach (var file in e.GetMultipleFiles(MaxAllowedFiles))
                {
                    var buffer = new byte[file.Size];
                    await file.OpenReadStream().ReadAsync(buffer);
                    ImagesData.Add(new ImageTypeWrapper(file, buffer));
                }
                await SetIsAnyImage.InvokeAsync(true);
                StateHasChanged();
            }
            catch
            {
                throw;
            }
        }
        private async Task DeleteImage(Guid id)
        {
            ImagesData = ImagesData.Where(x => x.Id != id).ToList();
            await SetIsAnyImage.InvokeAsync(ImagesData.Any());
            StateHasChanged();
        }
    }

    internal class ImageTypeWrapper
    {
        public ImageTypeWrapper(ImageCatalog image)
        {
            Id = Guid.NewGuid();
            Url = image.Url;
            IsInDb = true;
        }
        public ImageTypeWrapper(IBrowserFile file, byte[] buffer)
        {
            Id = Guid.NewGuid();
            IsInDb = false;
            BrowserImage = file;
            Url = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
        }
        public Guid Id { get; set; }
        public string Url { get; set; }
        public bool IsInDb { get; set; }
        public IBrowserFile BrowserImage { get; set; }
    }
}
