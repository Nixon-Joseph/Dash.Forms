using Xamarin.Forms;

namespace Dash.Forms.Controls
{
    public class FAButton : Button
    {
        public readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(FAButton.Size), typeof(FABSize), typeof(FAButton), FABSize.Large,
            propertyChanged: (b, o, n) => {
                if (b is FAButton _This && n is FABSize newVal)
                {
                    switch (newVal)
                    {
                        case FABSize.Large:
                            _This.WidthRequest = 56;
                            _This.HeightRequest = 56;
                            _This.CornerRadius = 56;
                            break;
                        case FABSize.Mini:
                            _This.WidthRequest = 40;
                            _This.HeightRequest = 40;
                            _This.CornerRadius = 40;
                            break;
                    }
                }
            });

        public FABSize Size
        {
            get { return (FABSize)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public FAButton()
        {
            WidthRequest = 56;
            HeightRequest = 56;
            CornerRadius = 56;
            Margin = 20;
        }
    }

    public enum FABSize
    {
        Large,
        Mini
    }
}
