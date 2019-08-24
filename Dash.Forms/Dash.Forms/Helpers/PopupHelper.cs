using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dash.Forms.Helpers
{
    public class PopupHelper
    {
        public static Task PushAsync(Page source, PopupPage page, bool animate = true)
        {
            return source.Navigation.PushPopupAsync(page, animate);
        }

        public static Task PushAsync(Page source, View page, bool animate = true)
        {
            return PushAsync(source, new PopupPage() { Content = page }, animate);
        }
    }
}
