using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Droid.DependencyServices;
using System.IO;

//https://doumer.me/litedb-on-xamarin-forms-alternative-to-sqlite/
[assembly: Xamarin.Forms.Dependency(typeof(DatabaseAccess_Droid))]
namespace Dash.Forms.Droid.DependencyServices
{
    public class DatabaseAccess_Droid : IDatabaseAccess
    {
        public string DatabasePath()
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), Dash.Forms.Constants.OFFLINE_DATABASE_NAME);
            if (File.Exists(path) == false)
            {
                File.Create(path).Dispose();
            }
            return path;
        }
    }
}
