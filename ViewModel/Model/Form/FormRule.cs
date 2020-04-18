using System;
using System.Collections.Generic;
using StackErp.Model.DataList;

namespace StackErp.ViewModel.Model
{
    public class FormRule
    {
        public int Id {set;get;}
        public string Type {set;get;}
        public FormFilterExpression Criteria {set;get;} 
        public List<string> Fields {set;get;} 
    }
}
