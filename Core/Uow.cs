using System;
using StackErp.Model;

namespace StackErp.Core
{
    public class UOW : IUOW
    {
         public UOW()
        {
            //DbSession = _sessionFactory.OpenSession();
        }
        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}