using Dash.Forms.Models.Run;
using System.Collections.Generic;

namespace Dash.Forms.Helpers.Storage
{
    public class TrainingWeekStorageHelper : SubItemStorageHelperBase<TrainingWeek>
    {
        private readonly TrainingDayStorageHelper trainingStorage;

        public TrainingWeekStorageHelper()
        {
            trainingStorage = new TrainingDayStorageHelper();
        }

        public override bool InsertAll(IEnumerable<TrainingWeek> weeks, string parentId)
        {
            var insertResponse = base.InsertAll(weeks, parentId);
            if (insertResponse == true)
            {
                foreach (var week in weeks)
                {
                    var response = trainingStorage.InsertAll(week.Days, week.Id);
                    if (response == false)
                    {
                        return response;
                    }
                }
            }
            return insertResponse;
        }
    }
}
