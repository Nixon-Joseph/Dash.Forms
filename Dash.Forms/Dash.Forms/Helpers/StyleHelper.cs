using Xamarin.Forms;

namespace Dash.Forms.Helpers
{
    public static class StyleHelper
    {
        public static Color GetColor(string hex)
        {
            return Color.FromHex(hex);
        }
    }

    public class DColor
    {
        public static readonly string Primary = "8cc34b";
        public static readonly string PrimaryDark = "689f3a";
        public static readonly string Accent = "4484ff";
        public static readonly string Black = "040404";
        public static readonly string White = "FFFFFF";
        public static readonly string OffWhite = "EBF2FA";
    }
}
