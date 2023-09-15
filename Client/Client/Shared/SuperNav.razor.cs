using Microsoft.AspNetCore.Components;

namespace Client.App.Shared;

public partial class SuperNav : ComponentBase
{
    [Inject] private NavigationManager? NavigationManager { get; set; }
    private bool ShowLeftBar { get; set; }
    private bool ShowSearList { get; set; }
    private bool ShowShopCartList { get; set; }

    private void RedirectByPathUrl(string path)
    {
        ShowLeftBar = false;
        NavigationManager?.NavigateTo(path);
    }
}