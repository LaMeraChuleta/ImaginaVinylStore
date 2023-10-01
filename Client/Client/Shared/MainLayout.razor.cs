using Microsoft.AspNetCore.Components;

namespace Client.App.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        public static event Action OnCloseSecondComponent;
        private void CloseSecondComponents()
        {
            OnCloseSecondComponent.Invoke();
        }
    }
}