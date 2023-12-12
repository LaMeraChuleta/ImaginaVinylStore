namespace Client.App.Components
{
    public class ImageWrapper
    {
        public ImageWrapper(ImageCatalog image)
        {
            Id = Guid.NewGuid();
            Url = image.Url;
            IsInDb = true;
        }
        public ImageWrapper(ImageAudio image)
        {
            Id = Guid.NewGuid();
            Url = image.Url;
            IsInDb = true;
        }
        public ImageWrapper(IBrowserFile file, byte[] buffer)
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

    public enum TypeImageWrapper
    {
        Music,
        Audio
    }
}
