using Android.Gms.Wearable;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Forms.AndroidShared
{
    public class NodeManager
    {
        public static async Task<IEnumerable<INode>> GetConnectedNodes(Android.Content.Context context)
        {
            List<INode> nodeList = new List<INode>();
            var foundNodes = await WearableClass.GetNodeClient(context).GetConnectedNodesAsync();
            if (foundNodes != null)
            {
                foreach (var node in foundNodes)
                {
                    nodeList.Add(node);
                }
            }
            return nodeList;
        }

        public static async Task<int> SendMessageToNodes(Android.Content.Context context, string message, Action<Exception> exceptionHandler = null)
        {

            try
            {
                var nodes = await GetConnectedNodes(context);
                foreach (INode node in nodes)
                {
                    try
                    {
                        var messageReturn = await Task.Run(() => Android.Gms.Tasks.TasksClass.Await(WearableClass.GetMessageClient(context).SendMessage(node.Id, Constants.WEARABLE_MESSAGE_PATH, Encoding.UTF8.GetBytes(message))));
                        return (int)messageReturn;
                    }
                    catch (Exception ex)
                    {
                        exceptionHandler?.Invoke(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
            }
            return -1;
        }
    }
}
