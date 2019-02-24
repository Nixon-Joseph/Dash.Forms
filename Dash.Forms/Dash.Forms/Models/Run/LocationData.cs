using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Forms.Models.Run
{
    public class LocationData
    {
        public int Index { get; set; }
        public string SegmentId { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public float Bearing { get; set; }
        public float Accuracy { get; set; }
        public double Altitude { get; set; }

        public bool HasAltitude { get; set; }
        public bool HasAccuracy { get; set; }
        public bool HasBearing { get; set; }
        public bool IsTracked { get; set; }
    }
}
