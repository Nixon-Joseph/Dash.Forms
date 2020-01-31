using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Models.Storage;
using LiteDB;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Dash.Forms.Helpers.Storage
{
    public abstract class StorageHelperBase<T> where T : DBBase
    {
        protected LiteCollection<T> Collection;

        public StorageHelperBase()
        {
            var db = new LiteDatabase(DependencyService.Get<IDatabaseAccess>().DatabasePath());
            Collection = db.GetCollection<T>();
        }

        public virtual T CreateItem(T item)
        {
            _ = Collection.Insert(item);
            return item;
        }

        public virtual T UpdateItem(T item)
        {
            _ = Collection.Update(item);
            return item;
        }

        public virtual T DeleteItem(T item)
        {
            _ = Collection.Delete(i => i.Id.Equals(item.Id));
            return item;
        }

        public virtual IEnumerable<T> GetItems()
        {
            return Collection.FindAll();
        }

        public virtual T GetItem(string id)
        {
            return Collection.FindById(id);
        }
    }
}
