using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackErp.Model;

namespace StackErp.App.Controllers
{
    public class EntityController : BaseController
    {
        public EntityController(ILogger<EntityController> logger): base(logger)
        {

        }
        public IActionResult Save()
        {
            var ent = Core.EntityMetaData.Get("UserRole");
            var m = ent.GetDefault();
            
            m.SetValue("Name", "Role XX");
            ent.Save(m);
            
            return Json(new {r = true});
        }
    }
}