using Android.Speech.Tts;
using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Droid.DependencyServices;
using Xamarin.Forms;

//https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/dependency-service/text-to-speech

[assembly: Dependency(typeof(TextToSpeach_Droid))]
namespace Dash.Forms.Droid.DependencyServices
{
    public class TextToSpeach_Droid : Java.Lang.Object, ITextToSpeach, TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;
        string toSpeak;

        public void Speak(string text)
        {
            toSpeak = text;
            if (speaker == null)
            {
                speaker = new TextToSpeech(MainActivity.Instance, this);
            }
            speaker?.Speak(toSpeak, QueueMode.Add, null, null);
        }

        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.Speak(toSpeak, QueueMode.Add, null, null);
            }
        }
    }
}
