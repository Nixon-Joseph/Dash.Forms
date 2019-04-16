using SQLite;

namespace Dash.Forms.Models.Storage
{
    public class DBSubItemBase : DBBase
    {
        [Indexed()]
        public string ParentId { get; set; }
    }
}
