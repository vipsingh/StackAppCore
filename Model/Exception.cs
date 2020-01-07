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
    public class EntityException: AppException 
    {
        public EntityException(): base() {

        }

        public EntityException(string message): base(message) {

        }
    }

    public class AuthException: AppException 
    {
        public AuthException(): base() {

        }

        public AuthException(string message): base(message) {
            
        }
    }

}
   