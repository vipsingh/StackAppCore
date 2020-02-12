using System;
using StackErp.Model;

namespace StackErp.Model.Form
{
    
    public class UIFormField
    {
        public string ControlId {set; get;}
        public FormControlType WidgetType {set; get;}
        public DynamicObj Properties {set; get;}
        public object Value {set; get;}
        public string ErrorMessage {set; get;}
    }
}