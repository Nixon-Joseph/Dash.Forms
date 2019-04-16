using Dash.Forms.Helpers.Storage;
using Dash.Forms.Models.Storage;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace Dash.Forms.Models.Run
{
    public class TrainingDay : DBSubItemBase
    {
        public TrainingType TrainingType { get; set; }
        private IEnumerable<TrainingSegment> _Segments { get; set; }
        [Ignore()]
        public IEnumerable<TrainingSegment> Segments
        {
            get
            {
                if (_Segments == null)
                {
                    var storage = new TrainingSegmentStorageHelper();
                    var response = storage.GetByParentId(Id);
                    if (response != null)
                    {
                        _Segments = response.OrderBy(s => s.SegmentNumber);
                    }
                }
                return _Segments;
            }
            set { _Segments = value; }
        }
        public int DayNumber { get; set; }

        public TrainingSegment GetSegment(int segmentNum)
        {
            if (Segments != null && Segments.Count() > segmentNum)
            {
                return Segments.ElementAt(segmentNum);
            }
            return null;
        }
    }

    public enum TrainingType
    {
        Time,
        Distance
    }
}
