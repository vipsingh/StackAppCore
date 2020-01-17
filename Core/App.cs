using System;

namespace StackErp.Core
{
    public class App
    {
        public static void ConfigureDB(string conStr)
        {
            DB.DBService.Init(conStr);
        }
    }
}
