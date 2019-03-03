using Android.Content;
using Dash.Forms.Controls;
using Dash.Forms.Droid.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomCarouselView), typeof(CustomCarouselViewRenderer))]
namespace Dash.Forms.Droid.Renderers
{
    public class CustomCarouselViewRenderer : CarouselViewRenderer
    {
        public CustomCarouselViewRenderer(Context context) : base (context)
        {

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs changedProperty)
        {
            base.OnElementPropertyChanged(sender, changedProperty);

            if (changedProperty.PropertyName == nameof(CustomCarouselView.DisableGestures))
            {

            }
        }
    }
}
