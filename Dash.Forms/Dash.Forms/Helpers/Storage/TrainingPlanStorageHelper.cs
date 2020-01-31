using Dash.Forms.Models.Run;
using LiteDB;

namespace Dash.Forms.Helpers.Storage
{
    public class TrainingPlanStorageHelper : StorageHelperBase<TrainingPlan>
    {
        private static bool _MappingInitialized = false;

        public TrainingPlanStorageHelper()
        {
            if (_MappingInitialized == false)
            {
                _MappingInitialized = true;
                var mapper = BsonMapper.Global;
                mapper.Entity<TrainingWeek>().Id(x => x.Id);
            }
        }
    }
}
