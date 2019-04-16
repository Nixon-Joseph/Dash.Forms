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
            Type = segment.Type;
            Speed = segment.Speed;
            ValueType = segment.ValueType;
        }

        public DateTime StartDate { get; set; }
        public TimeSpan Duration { get; set; }
        public double Distance { get; set; }
        public SegmentTypes Type { get; set; }
        public SegmentSpeeds Speed { get; set; }
        public SegmentValueTypes ValueType { get; set; }
        private IEnumerable<LocationData> _Locations { get; set; }
        [Ignore()]
        public IEnumerable<LocationData> Locations
        {
            get
            {
                if (_Locations == null)
                {
                    var locStorage = new LocationDataStorageHelper();
                    var response = locStorage.GetByParentId(Id);
                    if (response != null)
                    {
                        _Locations = response.OrderBy(l => l.Index);
                    }
                }
                return _Locations;
            }
            set { _Locations = value; }
        }
    }
}
