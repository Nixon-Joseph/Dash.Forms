using Dash.Forms.Models.Storage;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Dash.Forms.Helpers.Storage
{
    public class StorageHelperBase<T> where T : DBBase, new()
    {
        protected string DBPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db3"); } }
        protected object Locker = new object();
        protected SQLiteConnection Connection { get { return new SQLiteConnection(DBPath); } }
        protected bool Initialized { get; set; }

        public StorageHelperBase()
        {
            Init();
        }

        protected void Init()
        {
            if (Initialized == false)
            {
                using (SQLiteConnection connection = Connection)
                {
                    lock (Locker)
                    {
                        connection.CreateTable<T>();
                    }
                }
                Initialized = true;
            }
        }

        public virtual string Insert(T obj)
        {
            using (SQLiteConnection connection = Connection)
            {
                lock (Locker)
                {
                    Init();
                    var objId = obj.Id;
                    var rows = connection.Insert(obj);
                    if (rows > 0)
                    {
                        return objId;
                    }
                }
            }
            return null;
        }

        public virtual bool InsertAll(IEnumerable<T> objs)
        {
            using (SQLiteConnection connection = Connection)
            {
                lock (Locker)
                {
                    Init();
                    var rows = connection.InsertAll(objs, true);
                    if (rows == objs.Count())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual bool Update(T obj)
        {
            using (SQLiteConnection connection = Connection)
            {
                lock (Locker)
                {
                    Init();
                    var rows = connection.Update(obj);
                    if (rows > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual T Get(string id)
        {
            using (SQLiteConnection connection = Connection)
            {
                lock (Locker)
                {
                    Init();
                    var obj = connection.Get<T>(id);
                    if (obj != null)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }

        public virtual IEnumerable<T> GetAll()
        {
            using (SQLiteConnection connection = Connection)
            {
                lock (Locker)
                {
                    Init();
                    var objs = connection.Table<T>();
                    if (objs != null)
                    {
                        return objs.ToList();
                    }
                }
            }
            return null;
        }

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            using (SQLiteConnection connection = Connection)
            {
                lock (Locker)
                {
                    Init();
                    var objs = connection.Table<T>().Where(predicate);
                    if (objs != null)
                    {
                        return objs.ToList();
                    }
                }
            }
            return null;
        }

        public virtual bool Delete(string id)
        {
            using (SQLiteConnection connection = Connection)
            {
                lock (Locker)
                {
                    Init();
                    var rows = connection.Delete<T>(id);
                    if (rows > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
