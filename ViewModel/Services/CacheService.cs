using System;
using System.Web;

namespace StackErp.ViewModel.Services
{
    

    public interface ICacheHelper
    {
        void Insert(string key, object value, double mins);
    }
}
