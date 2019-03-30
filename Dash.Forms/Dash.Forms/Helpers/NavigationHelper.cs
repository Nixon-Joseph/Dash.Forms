using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dash.Forms.Helpers
{
    public class NavigationHelper
    {
        public static async Task PushAsync(Page page)
        {
            if (App.Current.MainPage is MasterDetailPage masterDetail)
            {
                await (masterDetail.Detail as NavigationPage).Navigation.PushAsync(page);
            }
        }
    }
}
