using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackErp.ViewModel.DataList;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.App.Controllers
{
    [SPA]
    public class WidgetController : StackErp.UI.Controllers.BaseController
    {
        public WidgetController(ILogger<AppController> logger): base(logger)
        {
            
        }

        [HttpPost]
        public IActionResult GetPickerData([FromBody] ListRequestinfo request)
        {  
            var context = new PickerListContext(this.StackAppContext, request);
            var builder = new PickerListBuilder();
            builder.Build(context);
            var res = builder.GetResponse(context);
            
            return CreateResult(res);
        }

        [HttpPost]
        public IActionResult GetFilterFieldForms([FromBody] CustomRequestInfo request)
        {  
            var form = new UI.View.CustomWidgetBuilder.FilterFormBuilder(this.StackAppContext);
            var page = form.Generate(request, this.RequestQuery);
            
            return CreatePageResult(page);
        }

        [HttpPost]
        public IActionResult GetFilterFieldData([FromBody] CustomRequestInfo request)
        {  
            var form = new UI.View.CustomWidgetBuilder.FilterFormBuilder(this.StackAppContext);
            var page = form.BuildWithData(request, this.RequestQuery);
            
            return CreateResult(page);
        }
    }
}