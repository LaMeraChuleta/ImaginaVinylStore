namespace SharedApp.Models.DTO
{
    public class ShopCart
    {
        public IEnumerable<MusicCatalog> MusicCatalogs { get; set; }

        public int[] GetItemsId()
        {
            return MusicCatalogs.Select(x => x.Id).ToArray();
        }
    }
}