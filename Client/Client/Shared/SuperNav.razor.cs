using Microsoft.AspNetCore.Components;

namespace Client.App.Shared;

public partial class SuperNav : ComponentBase, IDisposable
{
    [Inject] private NavigationManager? NavigationManager { get; set; }

    private bool ShowLeftBar { get; set; }
    private bool ShowSearList { get; set; }
    private bool ShowShopCartList { get; set; }

    protected override void OnInitialized()
    {
        MainLayout.OnCloseSecondComponent += CloseSecondComponets;
        base.OnInitialized();
    }
    public void Dispose()
    {
        MainLayout.OnCloseSecondComponent += CloseSecondComponets;
    }

    private void CloseSecondComponets()
    {
        if (ShowLeftBar)
            ShowLeftBar = false;
        if (ShowSearList)
            ShowSearList = false;
        if (ShowShopCartList)
            ShowShopCartList = false;

        StateHasChanged();
    }

    private void ChangeShowLeftBar()
    {
        if (!ShowLeftBar)
            CloseSecondComponets();

        ShowLeftBar = !ShowLeftBar;
        StateHasChanged();
    }
    private void ChangeShowSearList()
    {
        if (!ShowSearList)
            CloseSecondComponets();

        ShowSearList = !ShowSearList;
        StateHasChanged();
    }
    private void ChangeShowShopCartList()
    {
        if (!ShowShopCartList)
            CloseSecondComponets();

        ShowShopCartList = !ShowShopCartList;
        StateHasChanged();
    }

    private void RedirectByPathUrl(string path)
    {
        ShowLeftBar = false;
        NavigationManager?.NavigateTo(path);
    }
}