namespace Client.App.Components
{
    public partial class ImagesManage : ComponentBase, IDisposable
    {
        private const int MaxAllowedFiles = 3;
        [Parameter] public TypeImageWrapper Type { get; set; }
        [Parameter] public IEnumerable<ImageCatalog>? Images { get; set; }
        [Parameter] public IEnumerable<ImageAudio>? ImagesAudio { get; set; }
        [Parameter] public EventCallback<bool> SetIsAnyImage { get; set; }
        [Inject] public ICatalogMusicService CatalogMusicService { get; set; }
        [Inject] public IAudioCatalogService AudioCatalogService { get; set; }
        private List<ImageWrapper> ImagesData { get; set; } = new();
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
                Images.ToList().ForEach(x => ImagesData.Add(new ImageWrapper(x)));
                await SetIsAnyImage.InvokeAsync(ImagesData.Any());
            }
            if (ImagesAudio is not null)
            {
                ImagesAudio.ToList().ForEach(x => ImagesData.Add(new ImageWrapper(x)));
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
                    if (Type == TypeImageWrapper.Music)
                        CatalogMusicService.CreateImageAsync(id, x.BrowserImage);
                    if (Type == TypeImageWrapper.Audio)
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
                    ImagesData.Add(new ImageWrapper(file, buffer));
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