using System;
using System.Collections.Generic;
using StackErp.Model;

namespace StackErp.UI.View
{
    public class WebAppContext : StackAppContext
    {
        public ISessionHelper SessionHelper {get;}

        public WebAppContext(ISessionHelper sessionHelper)
        {
            SessionHelper = sessionHelper;
        }
    }

    public interface ISessionHelper {
        void AddSession(string key, object value);

        T GetSession<T>(string key);
    }
}
