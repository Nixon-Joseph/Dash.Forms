using Dash.Forms.Models.Run;
using System.Collections.Generic;

namespace Dash.Forms.Helpers.Storage
{
    public class TrainingDayStorageHelper : SubItemStorageHelperBase<TrainingDay>
    {
        private readonly TrainingSegmentStorageHelper trainingStorage;

        public TrainingDayStorageHelper()
        {
            trainingStorage = new TrainingSegmentStorageHelper();
        }

        public override bool InsertAll(IEnumerable<TrainingDay> days, string parentId)
        {
            var insertResponse = base.InsertAll(days, parentId);
            if (insertResponse == true)
            {
                foreach (var day in days)
                {
                    var response = trainingStorage.InsertAll(day.Segments, day.Id);
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
