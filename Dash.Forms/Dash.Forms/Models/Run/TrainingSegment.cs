using Dash.Forms.Models.Storage;

namespace Dash.Forms.Models.Run
{
    public class TrainingSegment : DBSubItemBase
    {
        public SegmentSpeeds Speed { get; set; }
        public double Value { get; set; }
        public int SegmentNumber { get; set; }

        public TrainingSegment() { }

        public TrainingSegment(int number, SegmentSpeeds speed, double value)
        {
            SegmentNumber = number;
            Speed = speed;
            Value = value;
        }
    }

    public enum SegmentSpeeds
    {
        None,
        Walk,
        FastWalk,
        Jog,
        Run,
        SteadyRun,
        TempoRun,
        Sprint,
        Extra
    }
}
