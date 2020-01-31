using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dash.Forms.Helpers
{
    public class WearHelper
    {
        public static void SendMessageToWear(string message)
        {
            MessagingCenter.Send(string.Empty, Constants.DroidAppWearMessageSentToWear, message);
        }
    }
}
