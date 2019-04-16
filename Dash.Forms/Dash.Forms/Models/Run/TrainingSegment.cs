using Dash.Forms.Models.Storage;

namespace Dash.Forms.Models.Run
{
    public class TrainingSegment : DBSubItemBase
    {
        public SegmentTypes Type { get; set; }
        public SegmentSpeeds Speed { get; set; }
        public SegmentValueTypes ValueType { get; set; }
        public double Value { get; set; }
        public int SegmentNumber { get; set; }
        //public TimeSpan Duration { get; set; }
        //public double Distance { get; set; }

        public TrainingSegment() { }

        public TrainingSegment(int number, SegmentSpeeds speed, double value, SegmentValueTypes valType = SegmentValueTypes.Minutes, SegmentTypes type = SegmentTypes.Time)
        {
            SegmentNumber = number;
            Type = type;
            Speed = speed;
            ValueType = valType;
            Value = value;
        }
    }

    public enum SegmentTypes
    {
        Time,
        WarmUp,
        CoolDown,
        Distance
    }

    public enum SegmentValueTypes
    {
        Minutes,
        Miles,
        Kilometers
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
        Sprint
    }
}
