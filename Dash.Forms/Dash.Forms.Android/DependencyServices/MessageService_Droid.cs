using Android.Support.Design.Widget;
using Android.Widget;
using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Droid.DependencyServices;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

[assembly: Dependency(typeof(MessageService_Droid))]
namespace Dash.Forms.Droid.DependencyServices
{
    public class MessageService_Droid : IMessageService
    {
        public void LongToast(string message)
        {
            Toast.MakeText(MainActivity.Instance.BaseContext, message, ToastLength.Long).Show();
        }

        public void ShortToast(string message)
        {
            Toast.MakeText(MainActivity.Instance.BaseContext, message, ToastLength.Short).Show();
        }

        public void ShortSnackbar(string message, params KeyValuePair<string, Action>[] snackbarActions)
        {
            ShowSnackbar(message, Snackbar.LengthShort, snackbarActions);
        }

        public void LongSnackbar(string message, params KeyValuePair<string, Action>[] snackbarActions)
        {
            ShowSnackbar(message, Snackbar.LengthLong, snackbarActions);
        }

        public void IndefiniteSnackbar(string message, params KeyValuePair<string, Action>[] snackbarActions)
        {
            ShowSnackbar(message, Snackbar.LengthIndefinite, snackbarActions);
        }

        private void ShowSnackbar(string message, int length, params KeyValuePair<string, Action>[] snackbarActions)
        {
            var snackbar = Snackbar.Make(MainActivity.ContentView, message, length);
            if (snackbarActions != null && snackbarActions.Length > 0)
            {
                foreach (var action in snackbarActions)
                {
                    snackbar.SetAction(action.Key, (v) => { action.Value(); });
                }
            }
            snackbar.Show();
        }
    }
}
