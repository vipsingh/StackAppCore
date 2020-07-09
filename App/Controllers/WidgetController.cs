using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackErp.Model;
using StackErp.UI.View.DataList;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.App.Controllers
{
    public class WidgetController : StackErp.UI.Controllers.BaseController
    {
        public WidgetController(ILogger<FilterController> logger,IOptions<AppKeySetting> appSettings): base(logger,appSettings)
        {
            
        }

        [HttpPost]
        public IActionResult GetPickerData([FromBody] ListRequestinfo request)
        {  
            var context = new PickerListContext(this.StackAppContext, RequestQuery, request);
            var builder = new PickerListBuilder();
            builder.Build(context);
            var res = builder.GetResponse(context);
            
            return CreateResult(res);
        }        
    
        public IActionResult GetListFormData()
        {  
            var pages =  StackErp.UI.View.CustomWidgetBuilder.ListFormDataProcesser.GetData(this.StackAppContext, this.RequestQuery);
            
            return CreateResult(pages);
        }

        public IActionResult GetRelatedListData([FromBody] ListRequestinfo request)
        {
            var context = new DataListContext(this.StackAppContext, RequestQuery, request);
            var builder = new EntityListBuilder();
            builder.Build(context);
            var res = builder.GetResponse(context);
            
            return CreateResult(res);
        }
    }
}