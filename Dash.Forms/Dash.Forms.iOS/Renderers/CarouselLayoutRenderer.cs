using Dash.Forms.Controls;
using Dash.Forms.iOS.Renderers;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CarouselLayout), typeof(CarouselLayoutRenderer))]
namespace Dash.Forms.iOS.Renderers
{
    public class CarouselLayoutRenderer : ScrollViewRenderer
    {
        UIScrollView _native;

        public CarouselLayoutRenderer()
        {
            PagingEnabled = true;
            ShowsHorizontalScrollIndicator = false;
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null) return;

            _native = (UIScrollView)NativeView;
            _native.Scrolled += NativeScrolled;
            e.NewElement.PropertyChanged += ElementPropertyChanged;
        }

        void NativeScrolled(object sender, EventArgs e)
        {
            var center = _native.ContentOffset.X + (_native.Bounds.Width / 2);
            ((CarouselLayout)Element).SelectedIndex = ((int)center) / ((int)_native.Bounds.Width);
        }

        void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CarouselLayout.SelectedIndexProperty.PropertyName && !Dragging)
            {
                ScrollToSelection(false);
            }
        }

        void ScrollToSelection(bool animate)
        {
            if (Element == null) return;

            _native.SetContentOffset(new CoreGraphics.CGPoint
                (_native.Bounds.Width *
                    Math.Max(0, ((CarouselLayout)Element).SelectedIndex),
                    _native.ContentOffset.Y),
                animate);
        }

        public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);
            ScrollToSelection(false);
        }
    }
}
