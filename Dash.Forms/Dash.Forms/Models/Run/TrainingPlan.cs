using Dash.Forms.Models.Storage;
using System.Collections.Generic;

namespace Dash.Forms.Models.Run
{
    public class TrainingPlan : DBBase
    {
        public PlanType Type { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public IEnumerable<TrainingWeek> Weeks { get; set; }
    }

    public enum PlanType
    {
        FiveK,
        TenK,
        HalfMarathon,
        Custom
    }
}
