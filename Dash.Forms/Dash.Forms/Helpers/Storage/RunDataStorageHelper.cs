using Dash.Forms.Extensions;
using Dash.Forms.Models.Run;
using System;
using System.Linq;

namespace Dash.Forms.Helpers.Storage
{
    public class RunDataStorageHelper : StorageHelperBase<RunData>
    {
        private readonly RunSegmentStorageHelper segmentStorage;
        public RunDataStorageHelper()
        {
            segmentStorage = new RunSegmentStorageHelper();
        }

        public override string Insert(RunData run)
        {
            var runResponse = base.Insert(run);
            if (runResponse.IsNullOrEmpty() == false)
            {
                segmentStorage.InsertAll(run.Segments, run.Id);
            }
            return runResponse;
        }

        public double GetTotalDistance()
        {
            return SumColumn(nameof(RunData.Distance));
        }

        public TimeSpan GetTotalElapsed()
        {
            return TimeSpan.FromMinutes(GetAllFromColumns(nameof(RunData.Elapsed))?.Sum(r => r.Elapsed.TotalMinutes) ?? 0d);
        }
    }
}
