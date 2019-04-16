using Dash.Forms.Models.Run;
using System.Collections.Generic;

namespace Dash.Forms.Helpers.Storage
{
    public class RunSegmentStorageHelper : SubItemStorageHelperBase<RunSegment>
    {
        private readonly LocationDataStorageHelper locationStorage;

        public RunSegmentStorageHelper()
        {
            locationStorage = new LocationDataStorageHelper();
        }

        public override bool InsertAll(IEnumerable<RunSegment> segments, string parentId)
        {
            var segmentResponse = base.InsertAll(segments, parentId);
            if (segmentResponse == true)
            {
                foreach (var segment in segments)
                {
                    var response = locationStorage.InsertAll(segment.Locations, segment.Id);
                    if (response == false)
                    {
                        return response;
                    }
                }
            }
            return segmentResponse;
        }
    }
}
