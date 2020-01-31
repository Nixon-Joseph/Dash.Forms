using Dash.Forms.Models.Run;
using LiteDB;

namespace Dash.Forms.Helpers.Storage
{
    public class RunDataStorageHelper : StorageHelperBase<RunData>
    {
        private static bool _MappingInitialized = false;

        public RunDataStorageHelper()
        {
            if (_MappingInitialized == false)
            {
                _MappingInitialized = true;
                var mapper = BsonMapper.Global;
                mapper.Entity<RunData>()
                    .Id(x => x.Id)
                    .Ignore(x => x.DataDisplay);
            }
        }

        public override RunData DeleteItem(RunData item)
        {
            return Collection.Delete(i => i.Id == item.Id) == 0 ? null : item;
        }
    }
}
