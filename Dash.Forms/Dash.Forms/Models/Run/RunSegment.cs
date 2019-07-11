using Dash.Forms.Helpers.Storage;
using Dash.Forms.Models.Storage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dash.Forms.Models.Run
{
    public class RunSegment : DBSubItemBase
    {
        public RunSegment() { }

        public RunSegment(TrainingSegment segment)
        {
            Speed = segment.Speed;
            _Locations = new List<LocationData>();
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
        private List<LocationData> _Locations { get; set; }
        /// <summary>
        /// List of locations
        /// </summary>
        [Ignore()]
        public List<LocationData> Locations
        {
            get
            {
                if (_Locations == null)
                {
                    var locStorage = new LocationDataStorageHelper();
                    var response = locStorage.GetByParentId(Id);
                    if (response != null)
                    {
                        _Locations = response.OrderBy(l => l.Index).ToList();
                    }
                }
                return _Locations;
            }
            set { _Locations = value; }
        }
    }
}
