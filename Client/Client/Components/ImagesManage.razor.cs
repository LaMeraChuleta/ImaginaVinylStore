namespace Client.App.Components
{
    public partial class ImagesManage : ComponentBase, IDisposable
    {
        private const int MaxAllowedFiles = 3;
        [Parameter] public IEnumerable<ImageCatalog>? Images { get; set; }
        [Parameter] public IEnumerable<ImageAudio>? ImagesAudio { get; set; }
        [Parameter] public EventCallback<bool> SetIsAnyImage { get; set; }
        [Inject] public ICatalogMusicService CatalogMusicService { get; set; }
        [Inject] public IAudioCatalogService AudioCatalogService { get; set; }
        private List<ImageTypeWrapper> ImagesData { get; set; } = new();
        private bool IsImageMusic { get; set; }

        protected override void OnInitialized()
        {
            CatalogMusicCreate.OnCompleteInsert += InsertImagesInDb;
            CatalogAudioCreate.OnCompleteInsert += InsertImagesInDb;
            CatalogMusicEdit.OnCompleteEdit += InsertImagesInDb;
            CatalogAudioEdit.OnCompleteEdit += InsertImagesInDb;
            base.OnInitialized();
        }
        public void Dispose()
        {
            CatalogMusicCreate.OnCompleteInsert -= InsertImagesInDb;
            CatalogAudioCreate.OnCompleteInsert -= InsertImagesInDb;
            CatalogMusicEdit.OnCompleteEdit -= InsertImagesInDb;
            CatalogAudioEdit.OnCompleteEdit -= InsertImagesInDb;
        }
        protected override async Task OnInitializedAsync()
        {
            if (Images is not null)
            {
                IsImageMusic = true;
                Images.ToList().ForEach(x => ImagesData.Add(new ImageTypeWrapper(x)));
                await SetIsAnyImage.InvokeAsync(ImagesData.Any());
            }
            if (ImagesAudio is not null)
            {
                IsImageMusic = false;
                ImagesAudio.ToList().ForEach(x => ImagesData.Add(new ImageTypeWrapper(x)));
                await SetIsAnyImage.InvokeAsync(ImagesData.Any());
            }

            await base.OnInitializedAsync();
        }
        private void InsertImagesInDb(int id)
        {
            try
            {
                ImagesData.ForEach(x =>
                {
                    if (IsImageMusic)
                        CatalogMusicService.CreateImageAsync(id, x.BrowserImage);
                    else
                        AudioCatalogService.CreateImageAsync(id, x.BrowserImage);
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
            ImagesData.RemoveAll(x => x.Id != id);
            await SetIsAnyImage.InvokeAsync(ImagesData.Any());
            StateHasChanged();
        }
    }
}
