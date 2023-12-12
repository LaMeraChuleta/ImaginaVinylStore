namespace SharedApp.Models
{
    public class ShippingAddress
    {
        // Primary key
        public int Id { get; set; }

        //Props
        public string StreetAddres1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public int Number { get; set; }
        public int IntNumber { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
    }
}
