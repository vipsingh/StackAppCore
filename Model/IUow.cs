using System;
using System.Collections.Generic;

namespace StackErp.Model
{
    public interface IUOW
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}