using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace ProUpgradeEditor
{
    public interface IDeletableEntity
    {
        bool IsUpdated { get; }
        bool IsDeleted { get; }
        bool IsNew { get; }

        bool IsDirty { get; }
    }

    public abstract class DeletableEntity : IDeletableEntity
    {
        bool isUpdated;
        bool isDeleted;
        bool isNew;

        public DeletableEntity()
        {
            isNew = false;
            isUpdated = false;
            isDeleted = false;
        }

        public virtual bool IsUpdated
        {
            get { return isUpdated; }
            set { isUpdated = value; }
        }

        public virtual bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        public virtual bool IsNew
        {
            get { return isNew; }
            set { isNew = value; }
        }

        public virtual bool IsDirty
        {
            get { return isUpdated | isNew | isDeleted; }
            set { isUpdated = value; if (value == false) { isNew = false; isDeleted = false; } }
        }
    }
}
