using Xamarin.Forms;

namespace Dash.Forms.Controls
{
    public class CustomCarouselView : CarouselView
    {
        public static readonly BindableProperty DisableGesturesProperty = BindableProperty.Create("Items", typeof(bool), typeof(CustomCarouselView), false);

        public bool DisableGestures
        {
            get { return (bool)GetValue(DisableGesturesProperty); }
            set { SetValue(DisableGesturesProperty, value); }
        }
    }
}
