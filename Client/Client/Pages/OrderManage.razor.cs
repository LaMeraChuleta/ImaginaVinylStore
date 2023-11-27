using Blazored.Toast.Services;
using Client.App.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedApp.Models;

namespace Client.App.Pages
{
    public partial class OrderManage : ComponentBase
    {
        [Inject] public IToastService ToastService { get; set; }
        [Inject] public IOrderService OrderService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        private List<Order> Orders { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            Orders = await OrderService.GetAsync();
            await base.OnInitializedAsync();
        }

    }
}
