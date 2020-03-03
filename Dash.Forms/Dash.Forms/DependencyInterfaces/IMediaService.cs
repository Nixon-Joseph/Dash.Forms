using System;
using System.Threading.Tasks;

namespace Dash.Forms.DependencyInterfaces
{
    public interface IMediaService
    {
        Task PauseBackgroundMusicForTask(Func<Task> onFocusGranted);
    }
}
