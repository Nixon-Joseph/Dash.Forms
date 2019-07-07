using Dash.Forms.Helpers.Storage;
using Dash.Forms.Models.Storage;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace Dash.Forms.Models.Run
{
    public class TrainingPlan : DBBase
    {
        public PlanType Type { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        private IEnumerable<TrainingWeek> _Weeks { get; set; }
        [Ignore()]
        public IEnumerable<TrainingWeek> Weeks
        {
            get
            {
                if (_Weeks == null)
                {
                    var storage = new TrainingWeekStorageHelper();
                    var response = storage.GetByParentId(Id);
                    if (response != null)
                    {
                        _Weeks = response.OrderBy(w => w.WeekNumber);
                    }
                }
                return _Weeks;
            }
            set { _Weeks = value; }
        }
    }

    public enum PlanType
    {
        FiveK,
        TenK,
        HalfMarathon,
        Custom
    }
}
