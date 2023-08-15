using Microsoft.AspNetCore.Components;

namespace Client.App.Shared;

public partial class NavBar : ComponentBase
{
    public delegate void SearchCatalogHandler(string query);

    private bool ShowLeftBar { get; set; }
    private bool ShowSearchInput { get; set; }
    private string QuerySearch { get; set; } = string.Empty;

    [Parameter] public EventCallback SendOpenLeftBar { get; set; }
    [Parameter] public EventCallback<string> SendRedirectPage { get; set; }
    [Parameter] public EventCallback<bool> SendOpenSearchList { get; set; }

    public static event SearchCatalogHandler OnSearchCatalog;

    private void ChangeShowSearch()
    {
        ShowSearchInput = !ShowSearchInput;
        SendOpenSearchList.InvokeAsync(ShowSearchInput);
    }

    private void SearchCatalog(ChangeEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Value!.ToString()))
            OnSearchCatalog.Invoke(e.Value.ToString()!);
    }
}