using Android.Content;
using Android.Media;
using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Droid.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(MediaService))]
namespace Dash.Forms.Droid.Services
{
    public class MediaService : IMediaService
    {
        public async Task PauseBackgroundMusicForTask(Func<Task> onFocusGranted)
        {
            var manager = (AudioManager)Android.App.Application.Context.GetSystemService(Context.AudioService);
            var builder = new AudioFocusRequestClass.Builder(AudioFocus.GainTransientMayDuck);
            var focusRequest = builder.Build();
            var ret = manager.RequestAudioFocus(focusRequest);
            if (ret == AudioFocusRequest.Granted)
            {
                await onFocusGranted?.Invoke();
                manager.AbandonAudioFocusRequest(focusRequest);
            }
        }
    }
}
