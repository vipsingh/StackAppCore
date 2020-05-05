using System;
using StackErp.Model;
using StackErp.Model.Layout;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel
{
    public class CustomPage
    {
        public ViewPage View {private set; get;}
        public TView Layout {set;get;}

        private TPage page;

        public CustomPage() {
            View = new ViewPage();
            View.Layout = new TView();
            page = new TPage();
            page.Groups.Add(new TGroup());
            View.Layout.Pages.Add(page);
        }

        public bool AddField(IWidget field)
        {
            if (View.Widgets.ContainsKey(field.WidgetId))
            {
                return false;
            }

            View.Widgets.Add(field.WidgetId, field);

            return true;
        }

        public TRow AddFieldsInRow(IWidget[] fields) {
            var TRow = new TRow();
            foreach(var f in fields) 
            {
                if (AddField(f)) {
                    var tField = new TField(){
                        FieldId = f.WidgetId
                    };

                    TRow.Fields.Add(tField);
                }
            }

            page.Groups[0].Rows.Add(TRow);

            return TRow;
        }
    }
}