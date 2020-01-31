using Dash.Forms.Extensions;
using System;

namespace Dash.Forms.Models.Storage
{
    public class DBBase
    {
        private string _Id { get; set; }
        public string Id
        {
            get
            {
                if (_Id.IsNullOrEmpty() == true)
                {
                    _Id = Guid.NewGuid().ToString();
                }
                return _Id;
            }
            set { _Id = value; }
        }
    }
}
