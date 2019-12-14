using System;
using System.Collections.Generic;

namespace Model
{
    public class EntityException: Exception 
    {
        public EntityException(): base() {

        }

        public EntityException(string message): base(message) {

        }
    }

    public class AuthException: Exception 
    {
        public AuthException(): base() {

        }

        public AuthException(string message): base(message) {
            
        }
    }

    public class AppException: Exception 
    {
        public AppException(): base() {

        }

        public AppException(string message): base(message) {
            
        }
    }
}
   