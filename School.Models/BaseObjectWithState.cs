using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Models
{
    public abstract class BaseObjectWithState
    {
        public string Guid { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public ObjectState ObjectState { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
