using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcJqDataTables.Extensions;

namespace MvcJqDataTables
{
    [Serializable]
    public class Column
    {
        private string title { get; set; }
        private string data { get; set; }
        private string name { get; set; }
        private bool? searchable { get; set; }
        private bool? orderable { get; set; }
        private bool? visible { get; set; }
        private string width { get; set; }
        private string render { get; set; }
        private readonly List<string> _classes = new List<string>();

        private Search search = new Search();
        private string type { get; set; }

        public Column(string name, string data = null, string title = null)
        {
            if (name.IsNullOrWhiteSpace())
                throw new ArgumentException("No column name specified");
            this.name = name;
            this.data = string.IsNullOrWhiteSpace(data) ? name : data;
            this.title = string.IsNullOrWhiteSpace(title) ? name : title;
            this.visible = true;
            this.searchable = false;
            this.orderable = true;
        }

        //        public Column(string name, string data)
        //        {
        //            this.title = name;
        //            this.name = name;
        //            this.data = data;
        //            this.visible = true;
        //            this.searchable = false;
        //            this.orderable = true;
        //        }
        //
        //        public Column(string name, string data, string title)
        //        {
        //            this.title = title;
        //            this.name = name;
        //            this.data = data;
        //            this.visible = true;
        //            this.searchable = false;
        //            this.orderable = true;
        //        }

        public Column SetData(string data)
        {
            this.data = data;
            return this;
        }

        public Column SetTitle(string title)
        {
            this.title = title;
            return this;
        }

        public Column SetVisible(bool visibility)
        {
            this.visible = visibility;
            return this;
        }

        public Column SetOrderable(bool orderability)
        {
            this.orderable = orderability;
            return this;
        }

        public Column SetRenderer(string renderer)
        {
            this.render = renderer;
            return this;
        }

        public Column SetWidth(string width)
        {
            this.width = width;
            return this;
        }

        public Column AddClass(string className)
        {
            this._classes.Add(className);
            return this;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("{").AppendLine();

            stringBuilder.AppendFormat("name:'{0}',", (object)this.name).AppendLine();

            if (!this.data.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("data:'{0}',", (object)this.data).AppendLine();

            if (!this.title.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("title:'{0}',", (object)this.title).AppendLine();

            if (this.orderable.HasValue)
                stringBuilder.AppendFormat("orderable:{0},", (object)this.orderable.Value.ToString().ToLower()).AppendLine();

            if (this.visible.HasValue)
                stringBuilder.AppendFormat("visible:{0},", (object)this.visible.Value.ToString().ToLower()).AppendLine();

            if (!this.width.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("width:'{0}',", (object)this.width).AppendLine();

            if (this._classes.Count > 0)
                stringBuilder.AppendFormat("classes:'{0}',", (object) string.Join(" ", this._classes.Select<string, string>((Func<string, string>) (c => c)).ToArray<string>())).AppendLine();

            if (!this.render.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("render:{0}", (object)this.render).AppendLine();

            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }

    public class Search
    {
        public string value { get; set; }
        public string regex { get; set; }
    }
}
