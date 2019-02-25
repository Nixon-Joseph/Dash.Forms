using System;
using Xamarin.Forms;

namespace Dash.Forms.Models
{
    public interface IAppNavMenuItem
    {
        string Title { get; set; }
        Type TargetType { get; set; }
    }

    public class AppNavMenuItem : IAppNavMenuItem
    {
        public string Title { get; set; }
        public Type TargetType { get; set; }
    }

    public class AppNavMenuItem<T> : AppNavMenuItem, IAppNavMenuItem where T : ContentPage
    {
        public AppNavMenuItem()
        {
            TargetType = typeof(T);
        }
    }
}