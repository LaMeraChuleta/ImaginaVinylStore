using Client.App.Interfaces;
using Client.App.Services;
using Microsoft.AspNetCore.Components;

namespace Client.App.Shared;

public partial class NavBar : ComponentBase, IDisposable
{

    [Inject] private IShopCartService ShopCartService { get; set; }
    [Parameter] public EventCallback SendOpenLeftBar { get; set; }
    [Parameter] public EventCallback<string> SendRedirectPage { get; set; }
    [Parameter] public EventCallback<bool> SendOpenSearchList { get; set; }
    [Parameter] public EventCallback SendOpenShopCartList { get; set; }
    [Parameter] public EventCallback SendCloseSecondComponent { get; set; }

    public delegate void SearchCatalogHandler(string query);
    public static event SearchCatalogHandler OnSearchCatalog;

    private bool ShowSearchInput { get; set; }
    private int CountShopCart { get; set; }
    private string QuerySearch { get; set; } = string.Empty;

    public void Dispose()
    {
        ShopCartNotificationService.OnShopCartCountUpdate -= UpdateShopItemCount;
        base.OnInitialized();
    }

    protected override void OnInitialized()
    {
        ShopCartNotificationService.OnShopCartCountUpdate += UpdateShopItemCount;
        base.OnInitialized();
    }
    protected override async Task OnInitializedAsync()
    {
        CountShopCart = await ShopCartService.GetShopCartCount();
        await base.OnInitializedAsync();
    }
    private async void ChangeShowLeftBar()
    {
        await SendOpenLeftBar.InvokeAsync();
    }

    private async void ChangeShowShopCart()
    {
        await SendOpenShopCartList.InvokeAsync();
    }

    private async void ChangeShowSearch()
    {
        if (!ShowSearchInput) QuerySearch = string.Empty;
        await SendOpenSearchList.InvokeAsync(ShowSearchInput);
        ShowSearchInput = !ShowSearchInput;
        StateHasChanged();
    }

    private void UpdateShopItemCount(int shopCartCount)
    {
        CountShopCart = shopCartCount;
        StateHasChanged();
    }

    private void SearchCatalog(ChangeEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Value!.ToString()))
        {
            var querySearch = e.Value!.ToString()!;
            if (querySearch.Length == 1) querySearch = string.Empty;
            OnSearchCatalog.Invoke(querySearch);
        }
    }
}