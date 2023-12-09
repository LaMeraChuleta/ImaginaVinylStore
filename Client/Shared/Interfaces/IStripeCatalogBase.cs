namespace SharedApp.Interfaces
{
    public interface IStripeCatalogBase
    {
        public string? IdProductStripe { get; set; }
        public string? IdPriceStripe { get; set; }
        public bool ActiveInStripe { get; set; }
        public bool Sold { get; set; }
    }
}