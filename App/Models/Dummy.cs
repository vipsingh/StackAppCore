using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StackErp.Model;

namespace StackErp.App.Models
{
    public class XScript
    {
        public void Run()
        {
            var obj = new DynamicObj();
            obj.Add("Name", "Xyz");
            obj.Add("Age", 10);
            obj.Add("Price", 2000.0);
            
            //var exp = "(Age + 11)";
        }


    }

    public class AuthenticateModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

}
