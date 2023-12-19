namespace Client.App.Pages.Orders
{
    public partial class Index : ComponentBase
    {
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public IOrderService OrderService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        private List<Order> Orders { get; set; } = new();
        private int IdOrderDetail { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Orders = await OrderService.GetAsync();
            await base.OnInitializedAsync();
        }
    }
}