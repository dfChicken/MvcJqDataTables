using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcJqDataTables.Attribute;
using MvcJqDataTables.Enums;
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

        private Search search = new Search();
        private string type { get; set; }

        #region Options

        private string _cellType;
        private string _className;
        private string _contentPadding;
        private string _createdCell;
        private string _defaultContent;
        private int[] _orderData;
        //        private string _orderDataType; //disabled because it is live dom function
        private List<OrderDirection> _orderSequence = new List<OrderDirection>();

        #endregion

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

        #region Methods

        public Column SetOrderSequence(List<OrderDirection> orderSequence)
        {
            this._orderSequence = orderSequence;
            return this;
        }

        public Column SetOrderData(List<int> orderColumnIndexList)
        {
            this._orderData = orderColumnIndexList.ToArray();
            return this;
        }

        public Column SetDefaultContent(string defaultContent)
        {
            this._defaultContent = defaultContent;
            return this;
        }

        public Column OnCreatedCell(string jsFunctionName)
        {
            this._createdCell = jsFunctionName;
            return this;
        }

        public Column SetContentPadding(string contentPadding)
        {
            this._contentPadding = contentPadding;
            return this;
        }

        public Column SetClassName(string clazzName)
        {
            this._className = clazzName;
            return this;
        }

        public Column SetCellType(string cellType)
        {
            this._cellType = cellType;
            return this;
        }

        public Column SetSearchable(bool canSearch)
        {
            this.searchable = canSearch;
            return this;
        }

        public Column SetSearch(Search s)
        {
            this.search = s;
            return this;
        }

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

        #endregion

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

            if (this.searchable.HasValue)
                stringBuilder.AppendFormat("searchable:{0},", (object)this.searchable.Value.ToString().ToLower()).AppendLine();

            if (this.visible.HasValue)
                stringBuilder.AppendFormat("visible:{0},", (object)this.visible.Value.ToString().ToLower()).AppendLine();

            if (!this.width.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("width:'{0}',", (object)this.width).AppendLine();

            if (!this.render.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("render:{0},", (object)this.render).AppendLine();

            #region Options

            if (!this._cellType.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("cellType:'{0}',", (object)this._cellType).AppendLine();

            if (!this._className.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("className:'{0}',", (object)this._className).AppendLine();

            if (!this._contentPadding.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("contentPadding:'{0}',", (object)this._contentPadding).AppendLine();

            if (!this._createdCell.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("createdCell:'{0}',", (object)this._createdCell).AppendLine();

            if (!this._defaultContent.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("defaultContent:'{0}',", (object)this._defaultContent).AppendLine();

            if (this._orderData != null && this._orderData.Length > 0)
            {
                if (this._orderData.Length == 1)
                {
                    stringBuilder.AppendFormat("orderData:{0},", (object)this._orderData[0]).AppendLine();
                }
                else
                {
                    stringBuilder.AppendFormat("orderData:[{0}],", (object)string.Join(",", this._orderData.Select(c => c.ToString()))).AppendLine();
                }
            }

            if (this._orderSequence.Count > 0 )
            {
                stringBuilder.AppendFormat("orderSequence:[{0}],", (object)string.Join(",", this._orderSequence.Select(c => string.Format("'{0}'", c.GetStringValue())))).AppendLine();
            }

            #endregion

            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }

    public class Search
    {
        public bool? caseInsensitive { get; set; }
        public bool? smart { get; set; }
        public string value { get; set; }
        public bool? regex { get; set; }
    }
}
