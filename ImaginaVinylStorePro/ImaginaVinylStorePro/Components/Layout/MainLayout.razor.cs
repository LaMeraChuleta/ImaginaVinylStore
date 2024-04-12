using Microsoft.AspNetCore.Components;

namespace ImaginaVinylStorePro.Components.Layout
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