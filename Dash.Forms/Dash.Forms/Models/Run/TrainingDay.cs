using System.Collections.Generic;
using System.Linq;

namespace Dash.Forms.Models.Run
{
    public class TrainingDay
    {
        public TrainingType TrainingType { get; set; }
        public IEnumerable<TrainingSegment> Segments { get; set; }
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
