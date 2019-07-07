using System.Collections.Generic;

namespace Dash.Forms.Models.Run
{
    public class TrainingPlan5K : TrainingPlan
    {
        public TrainingPlan5K()
        {
            Id = "plan_5k";
            Type = PlanType.FiveK;
            Title = "5k Training";
            Image = "plan_5k.jpg";
            Caption = "3.1 miles. Wherever you want, as fast as you can!";
            Weeks = BuildWeeks();
        }

        private IEnumerable<TrainingWeek> BuildWeeks()
        {
            var weeks = new List<TrainingWeek>();
            for (int i = 1; i <= 9; i++)
            {
                weeks.Add(BuildWeek(i));
            }
            return weeks;
        }

        private TrainingWeek BuildWeek(int weekNum)
        {
            var days = BuildDays(weekNum);
            return new TrainingWeek()
            {
                WeekNumber = weekNum,
                Days = days
            };
        }

        private IEnumerable<TrainingDay> BuildDays(int weekNum)
        {
            var days = new List<TrainingDay>();
            for (int i = 1; i <= 3; i++)
            {
                days.Add(BuildDay(weekNum, i));
            }
            return days;
        }

        private TrainingDay BuildDay(int weekNum, int dayNum)
        {
            return new TrainingDay()
            {
                DayNumber = dayNum,
                TrainingType = TrainingType.Time,
                Segments = BuildSegments(weekNum, dayNum)
            };
        }

        private IEnumerable<TrainingSegment> BuildSegments(int weekNum, int dayNum)
        {
            var segmentNum = 1;
            var segments = new List<TrainingSegment>() { new TrainingSegment(segmentNum++, SegmentSpeeds.FastWalk, 5d) };

            switch (weekNum)
            {
                case 1:
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 1.5d));
                    break;
                case 2:
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 2d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 2d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 2d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 2d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 2d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 2d));
                    break;
                case 3:
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 3d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 3d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 3d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 3d));
                    break;
                case 4:
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 3d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 2.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 3d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 1.5d));
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 5d));
                    break;
                case 5:
                    switch (dayNum)
                    {
                        case 1:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 5d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 3d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 5d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 3d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 5d));
                            break;
                        case 2:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 8d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 5d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 8d));
                            break;
                        case 3:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 20d));
                            break;
                    }
                    break;
                case 6:
                    switch (dayNum)
                    {
                        case 1:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 5d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 3d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 8d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 3d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 5d));
                            break;
                        case 2:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 10d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Walk, 5d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 10d));
                            break;
                        case 3:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 25d));
                            break;
                    }
                    break;
                case 7:
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 25d));
                    break;
                case 8:
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 28d));
                    break;
                case 9:
                    segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 30d));
                    break;
            }

            segments.Add(new TrainingSegment(segmentNum, SegmentSpeeds.FastWalk, 5d));
            return segments;
        }
    }
}
