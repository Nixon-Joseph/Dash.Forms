using System;
using System.Collections.Generic;

namespace Dash.Forms.Models.Run
{
    public class RunSegment
    {
        public RunSegment() { }

        public RunSegment(TrainingSegment segment)
        {
            Speed = segment.Speed;
            Locations = new List<LocationData>();
        }

        /// <summary>
        /// Start date and time for segment
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// Run duration
        /// </summary>
        public TimeSpan Duration { get; set; }
        /// <summary>
        /// Distance stored in Meteres
        /// </summary>
        public double Distance { get; set; }
        /// <summary>
        /// Segment Speed
        /// </summary>
        public SegmentSpeeds Speed { get; set; }
        /// <summary>
        /// List of locations
        /// </summary>
        public List<LocationData> Locations { get; set; }
    }
}
