using System.Collections.Generic;

namespace Dash.Forms.Models.Run
{
    public class TrainingPlanHalfMarathon : TrainingPlan
    {
        public const string ID = "plan_10k";
        public TrainingPlanHalfMarathon()
        {
            Id = ID;
            Type = PlanType.TenK;
            Title = "Half Marathon Training";
            Image = "plan_half";
            Caption = "Think you can handle 13.1 miles? Lets get training!";
            Weeks = BuildWeeks();
        }

        private IEnumerable<TrainingWeek> BuildWeeks()
        {
            var weeks = new List<TrainingWeek>();
            for (int i = 1; i <= 12; i++)
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
                    switch (dayNum)
                    {
                        case 1:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 20d));
                            break;
                        case 2:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 10d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.TempoRun, 3d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 10d));
                            break;
                        case 3:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 30d));
                            break;
                    }
                    break;
                case 2:
                    switch (dayNum)
                    {
                        case 1:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 20d));
                            break;
                        case 2:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 10d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 8d));
                            break;
                        case 3:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 35d));
                            break;
                    }
                    break;
                case 3:
                    switch (dayNum)
                    {
                        case 1:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 20d));
                            break;
                        case 2:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 10d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.TempoRun, 5d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 10d));
                            break;
                        case 3:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 40d));
                            break;
                    }
                    break;
                case 4:
                    switch (dayNum)
                    {
                        case 1:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 20d));
                            break;
                        case 2:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 10d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 8d));
                            break;
                        case 3:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 45d));
                            break;
                    }
                    break;
                case 5:
                    switch (dayNum)
                    {
                        case 1:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 15d));
                            break;
                        case 2:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 10d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 1d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 1d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 1d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 1d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                            break;
                        case 3:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 30d));
                            break;
                    }
                    break;
                case 6:
                    switch (dayNum)
                    {
                        case 1:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 25d));
                            break;
                        case 2:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 10d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.TempoRun, 7d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 10d));
                            break;
                        case 3:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 50d));
                            break;
                    }
                    break;
                case 7:
                    switch (dayNum)
                    {
                        case 1:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 25d));
                            break;
                        case 2:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 10d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 2d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 8d));
                            break;
                        case 3:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 55d));
                            break;
                    }
                    break;
                case 8:
                    switch (dayNum)
                    {
                        case 1:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 25d));
                            break;
                        case 2:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 10d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.TempoRun, 9d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 10d));
                            break;
                        case 3:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 60d));
                            break;
                    }
                    break;
                case 9:
                    switch (dayNum)
                    {
                        case 1:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 15d));
                            break;
                        case 2:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 10d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 1d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 1d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 1d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Sprint, 1d));
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.Jog, 1d));
                            break;
                        case 3:
                            segments.Add(new TrainingSegment(segmentNum++, SegmentSpeeds.SteadyRun, 30d));
                            break;
                    }
                    break;
            }

            segments.Add(new TrainingSegment(segmentNum, SegmentSpeeds.FastWalk, 5d));
            return segments;
        }
    }
}
