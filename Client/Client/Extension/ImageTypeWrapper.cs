namespace Client.App.Components
{
    public class ImageTypeWrapper
    {
        public ImageTypeWrapper(ImageCatalog image)
        {
            Id = Guid.NewGuid();
            Url = image.Url;
            IsInDb = true;
        }
        public ImageTypeWrapper(ImageAudio image)
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
