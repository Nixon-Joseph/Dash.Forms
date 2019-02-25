using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Forms.DependencyInterfaces
{
    public interface IMessageService
    {
        void ShortToast(string message);
        void LongToast(string message);
        void ShortSnackbar(string message, params KeyValuePair<string, Action>[] snackbarActions);
        void LongSnackbar(string message, params KeyValuePair<string, Action>[] snackbarActions);
        void IndefiniteSnackbar(string message, params KeyValuePair<string, Action>[] snackbarActions);
    }
}
