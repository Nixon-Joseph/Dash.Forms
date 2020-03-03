using System;
using System.Collections.Generic;

namespace Dash.Forms.Models.Run
{
    public class RunSegment
    {
        public RunSegment()
        {
            Locations = new List<LocationData>();
        }

        public RunSegment(TrainingSegment segment) : this()
        {
            Speed = segment.Speed;
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
