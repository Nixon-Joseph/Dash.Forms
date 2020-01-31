using Dash.Forms.Extensions;
using Dash.Forms.Models.Storage;
using System;
using System.Collections.Generic;

namespace Dash.Forms.Models.Run
{
    public class RunData : DBBase
    {
        public string TrainingProgramId { get; set; }
        public int WeekNumber { get; set; }
        public int DayNumber { get; set; }
        public bool IsFreeRun { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double Distance { get; set; }

        public string DataDisplay
        {
            get
            {
                if (IsFreeRun == true)
                {
                    return $"{Start.ToShortDateString()} > {Elapsed.ToString(Elapsed.Hours > 0 ? "hh':'mm':'ss" : "mm':'ss")} > {Distance.ToMiles().ToString("N2")}";
                }
                return $"Week: {WeekNumber}, Day: {DayNumber} > {Elapsed.ToString(Elapsed.Hours > 0 ? "hh':'mm':'ss" : "mm':'ss")} > {Distance.ToMiles().ToString("N2")}";
            }
        }

        public IEnumerable<RunSegment> Segments { get; set; }
        public TimeSpan Elapsed { get; set; }
    }
}
