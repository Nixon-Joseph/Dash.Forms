using Dash.Forms.Models.Storage;

namespace Dash.Forms.Models.Run
{
    public class LocationData : DBSubItemBase
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
        public long Timestamp { get; set; }
    }
}
