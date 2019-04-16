using Dash.Forms.Extensions;
using Dash.Forms.Models.Storage;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dash.Forms.Helpers.Storage
{
    public class SubItemStorageHelperBase<T> : StorageHelperBase<T> where T : DBSubItemBase, new()
    {
        private string _ParentIdSelect { get; set; }
        private string ParentIdSelect
        {
            get
            {
                if (_ParentIdSelect.IsNullOrEmpty() == true)
                {
                    _ParentIdSelect = BuildParentIdSelect();
                }
                return _ParentIdSelect;
            }
        }

        public virtual IEnumerable<T> GetByParentId(string id)
        {
            using (SQLiteConnection connection = Connection)
            {
                lock (Locker)
                {
                    Init();
                    var objs = connection.Query<T>(ParentIdSelect, id);
                    if (objs != null)
                    {
                        return objs;
                    }
                }
            }
            return null;
        }

        public virtual bool InsertAll(IEnumerable<T> objs, string parentId)
        {
            foreach (var obj in objs)
            {
                obj.ParentId = parentId;
            }
            return base.InsertAll(objs);
        }

        private string BuildParentIdSelect()
        {
            var type = typeof(T);

            var sb = new StringBuilder();
            sb.Append("SELECT * FROM ");

            var tableAttribute = type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();
            if (tableAttribute != null)
            {
                sb.Append(((TableAttribute)tableAttribute).Name);
            }
            else
            {
                sb.Append(type.Name);
            }

            sb.Append(" WHERE ParentId = ?");

            return sb.ToString();
        }
    }
}
