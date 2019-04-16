using Dash.Forms.Helpers.Storage;
using Dash.Forms.Models.Storage;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace Dash.Forms.Models.Run
{
    public class TrainingWeek : DBSubItemBase
    {
        private IEnumerable<TrainingDay> _Days { get; set; }
        [Ignore()]
        public IEnumerable<TrainingDay> Days
        {
            get
            {
                if (_Days == null)
                {
                    var storage = new TrainingDayStorageHelper();
                    var response = storage.GetByParentId(Id);
                    if (response != null)
                    {
                        _Days = response.OrderBy(d => d.DayNumber);
                    }
                }
                return _Days;
            }
            set { _Days = value; }
        }
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
