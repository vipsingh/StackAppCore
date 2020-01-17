using System;
using System.Collections.Generic;

namespace StackErp.Model
{
    public class AppException: Exception 
    {
        public AppException(): base() {

        }

        public AppException(string message): base(message) {
            
        }
    }
    public class DBException: AppException 
    {
        public DBException(string message): base(message) {

        }
    }
    public class EntityException: AppException 
    {
        public EntityException(string message): base(message) {

        }
    }

    public class AuthException: AppException 
    {
        public AuthException(string message): base(message) {
            
        }
    }

    public class UserException: AppException 
    {
        public UserException(string message): base(message) {
            
        }
    }

}
   