using Dash.Forms.Models.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Forms.DependencyInterfaces
{
    public interface ILocationService
    {
        event EventHandler<LocationData> LocationChanged;
        void Start();
        void Stop();
        bool CheckGPSPermission();
    }
}
