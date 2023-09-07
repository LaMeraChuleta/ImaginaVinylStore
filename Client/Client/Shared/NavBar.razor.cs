using Client.App.Interfaces;
using Client.App.Services;
using Microsoft.AspNetCore.Components;

namespace Client.App.Shared;

public partial class NavBar : ComponentBase, IDisposable
{   
    
    [Inject] private IShopCartService _shopCartService { get; set; }
    public delegate void SearchCatalogHandler(string query);
    public static event SearchCatalogHandler OnSearchCatalog;

    private bool ShowSearchInput { get; set; }
    private int CountShopCart { get; set; }
    private string QuerySearch { get; set; } = string.Empty;

    [Parameter] public EventCallback SendOpenLeftBar { get; set; }
    [Parameter] public EventCallback<string> SendRedirectPage { get; set; }
    [Parameter] public EventCallback<bool> SendOpenSearchList { get; set; }


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

    protected async override Task OnInitializedAsync()
    {
        CountShopCart = await _shopCartService.GetShopCartCount();
        await base.OnInitializedAsync();
    }

    private void ChangeShowSearch()
    {
        ShowSearchInput = !ShowSearchInput;
        if(!ShowSearchInput) QuerySearch = string.Empty;
        SendOpenSearchList.InvokeAsync(ShowSearchInput);
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