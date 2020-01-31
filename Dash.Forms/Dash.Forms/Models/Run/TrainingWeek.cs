using Dash.Forms.Models.Storage;
using System.Collections.Generic;
using System.Linq;

namespace Dash.Forms.Models.Run
{
    public class TrainingWeek : DBBase
    {
        public IEnumerable<TrainingDay> Days { get; set; }
        public int WeekNumber { get; set; }

        public TrainingDay GetDay(int dayNum)
        {
            if (Days != null && Days.Count() > dayNum)
            {
                return Days.ElementAt(dayNum);
            }
            return null;
        }
    }
}
