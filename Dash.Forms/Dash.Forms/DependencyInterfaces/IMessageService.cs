using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Forms.DependencyInterfaces
{
    public interface IMessageService
    {
        void ShowToast(string text);
        void ShowSnackbar(string text);
    }
}
