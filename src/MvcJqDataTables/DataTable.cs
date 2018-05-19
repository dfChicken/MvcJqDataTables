using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using MvcJqDataTables.Extensions;

namespace MvcJqDataTables
{
    public class DataTable : IHtmlString
    {
        //        private const string FILE_FORMAT_REGEX = @"^.*\.[^\\]+$";
        private readonly string _id;
        private readonly List<Column> _columns = new List<Column>();
        private bool? _asyncLoad;
        private bool _isCustomTable = false;

        #region Ajax

        private string _url;
        private string _urlDataFile;
        private readonly List<Parameter> _extraParams = new List<Parameter>();

        //reformat data structure got from the data source
        private string _dataSrc;

        #endregion

        #region New Value
        private bool? _autoWidth = false;

        private bool? _deferRender;

        //info box
        private bool? _info;
        //length choose box
        private bool? _lengthChange;
        //orderable 
        private bool? _ordering;
        //pagination
        private bool? _paging;
        //processing indicator
        private bool? _processing = true;
        //horizontal scroll
        private bool? _scrollX;
        //vertical scroll
        private bool? _scrollY;
        //searchable
        private bool? _searching = false;
        //serverSide
        private bool? _serverSide = true;
        //state save
        private bool? _stateSave;

        public DataTable SetAutoWidth(bool autoWidth)
        {
            this._autoWidth = autoWidth;
            return this;
        }

        public DataTable SetDeferRender(bool deferRender)
        {
            this._deferRender = deferRender;
            return this;
        }

        public DataTable SetSearchable(bool search)
        {
            this._searching = search;
            return this;
        }

        public DataTable SetSaveState(bool stateSave)
        {
            this._stateSave = stateSave;
            return this;
        }

        public DataTable SetVerticalScroll(bool scrollY)
        {
            this._scrollY = scrollY;
            return this;
        }

        public DataTable SetHorizontalScroll(bool scrollX)
        {
            this._scrollX = scrollX;
            return this;
        }

        public DataTable SetProcessing(bool processing)
        {
            this._processing = processing;
            return this;
        }

        public DataTable SetPageable(bool pageable)
        {
            this._paging = pageable;
            return this;
        }

        public DataTable SetInfo(bool info)
        {
            this._info = info;
            return this;
        }

        public DataTable SetLengthChange(bool lengthChange)
        {
            this._lengthChange = lengthChange;
            return this;
        }

        public DataTable SetOrderable(bool orderable)
        {
            this._ordering = orderable;
            return this;
        }


        public DataTable AddExtraParameter(Parameter exParam)
        {
            this._extraParams.Add(exParam);
            return this;
        }

        public DataTable AddExtraParameters(IEnumerable<Parameter> exParams)
        {
            this._extraParams.AddRange(exParams);
            return this;
        }

        public DataTable SetDataSrc(string dataSrc)
        {
            this._dataSrc = dataSrc;
            return this;
        }

        #endregion

        public DataTable SetUrlDataFile(string dataFilePath)
        {
            this._urlDataFile = dataFilePath;
            return this;
        }

        public DataTable SetUrl(string url)
        {
            this._url = url;
            return this;
        }

        public DataTable(string id)
        {
            if (id.IsNullOrWhiteSpace())
                throw new ArgumentException("Id must contain a value to identify the datatable.");
            this._id = id;
        }

        public DataTable AddColumn(Column column)
        {
            this._columns.Add(column);
            return this;
        }

        public DataTable AddColumns(IEnumerable<Column> columns)
        {
            this._columns.AddRange(columns);
            return this;
        }

        public DataTable SetAsyncLoad(bool asyncPageLoad)
        {
            this._asyncLoad = new bool?(asyncPageLoad);
            return this;
        }

        public DataTable SetCustomTable(bool isCustomTable)
        {
            this._isCustomTable = isCustomTable;
            return this;
        }

        public string RenderJavascript()
        {
            var stringBuilder = new StringBuilder();
            if (this._asyncLoad.HasValue && this._asyncLoad.Value)
                stringBuilder.AppendLine("jQuery(window).ready(function () {");
            else
                stringBuilder.AppendLine("jQuery(document).ready(function () {");

            stringBuilder.AppendLine("jQuery('#" + this._id + "').DataTable({");

            if (this._processing.HasValue)
                stringBuilder.AppendFormat("processing:{0},", (object)this._processing.ToString().ToLower()).AppendLine();
            if (this._serverSide.HasValue)
                stringBuilder.AppendFormat("serverSide:{0},", (object)this._serverSide.ToString().ToLower()).AppendLine();
            if (this._searching.HasValue)
                stringBuilder.AppendFormat("searching:{0},", (object)this._searching.ToString().ToLower()).AppendLine();
            if (this._autoWidth.HasValue)
                stringBuilder.AppendFormat("autoWidth:{0},", (object)this._autoWidth.ToString().ToLower()).AppendLine();
            if (this._deferRender.HasValue)
                stringBuilder.AppendFormat("deferRender:{0},", (object)this._deferRender.ToString().ToLower()).AppendLine();
            if (this._lengthChange.HasValue)
                stringBuilder.AppendFormat("lengthChange:{0},", (object)this._lengthChange.ToString().ToLower()).AppendLine();
            if (this._ordering.HasValue)
                stringBuilder.AppendFormat("ordering:{0},", (object)this._ordering.ToString().ToLower()).AppendLine();
            if (this._paging.HasValue)
                stringBuilder.AppendFormat("paging:{0},", (object)this._paging.ToString().ToLower()).AppendLine();
            if (this._scrollX.HasValue)
                stringBuilder.AppendFormat("scrollX:{0},", (object)this._scrollX.ToString().ToLower()).AppendLine();
            if (this._scrollY.HasValue)
                stringBuilder.AppendFormat("scrollY:{0},", (object)this._scrollY.ToString().ToLower()).AppendLine();
            if (this._stateSave.HasValue)
                stringBuilder.AppendFormat("stateSave:{0},", (object)this._stateSave.ToString().ToLower()).AppendLine();

            stringBuilder.AppendLine(
                "dom:'<\"top\"<\"row\"<\"col-sm-5\"i><\"col-sm-7\"f>>>rt<\"bottom\"<\"row\"<\"col-sm-5\"l><\"col-sm-7\"p>>>',");

            stringBuilder.AppendLine("ajax:{");
            if (!this._url.IsNullOrWhiteSpace() || !this._urlDataFile.IsNullOrWhiteSpace())
            {
                if (!this._url.IsNullOrWhiteSpace() && !this._urlDataFile.IsNullOrWhiteSpace())
                {
                    throw new ArgumentException("Cannot set url and data file path at the same time.");
                }
                if (!this._url.IsNullOrWhiteSpace())
                {
                    stringBuilder.AppendFormat("url:'{0}',", (object)this._url).AppendLine();
                    stringBuilder.AppendLine("type: 'POST',");
                    stringBuilder.AppendLine("data:{");
                    //extra params
                    if (this._extraParams.Count > 0)
                    {
                        if (this._extraParams.GroupBy(m => m._paramName).All(g => g.Count() == 1))
                        {
                            var stringDataParam = string.Join(", \n", this._extraParams.Where(c => c.GetType() == typeof(DataParameter)).Select(c => c.ToString()));
                            var stringFunctionParam = string.Join(", \n", this._extraParams.Where(c => c.GetType() == typeof(FunctionParameter)).Select(c => c.ToString()));
                            string stringParam;
                            if (!stringDataParam.IsNullOrWhiteSpace() && !stringFunctionParam.IsNullOrWhiteSpace())
                            {
                                stringParam = stringDataParam + ", \n" + stringFunctionParam;
                            }
                            else
                            {
                                stringParam = stringDataParam ?? stringFunctionParam;
                            }
                            stringBuilder.AppendLine(stringParam);
                        }
                        else
                        {
                            throw new ArgumentException("Parameter name is duplicated.");
                        }
                    }
                    stringBuilder.AppendLine("}");
                }
                if (!this._urlDataFile.IsNullOrWhiteSpace())
                {
                    stringBuilder.AppendFormat("url:'{0}'", (object)this._urlDataFile).AppendLine();
                }
                if (!this._dataSrc.IsNullOrWhiteSpace())
                {
                    stringBuilder.AppendFormat(", dataSrc:'{0}'", (object) this._dataSrc);
                }
            }
            stringBuilder.AppendLine("},");

            //columns join
            if (_columns.Count == 0)
            {
                throw new ArgumentException("Table must have at least one column.");
            }
            stringBuilder.AppendLine("columns: [");
            string str1 = string.Join(",", this._columns.Select(c => c.ToString()).ToArray());
            stringBuilder.AppendLine(str1);
            stringBuilder.AppendLine("]");

            stringBuilder.AppendLine("});");
            stringBuilder.AppendLine("});");
            return stringBuilder.ToString();
        }


        public string RenderHtmlElements()
        {
            StringBuilder stringBuilder1 = new StringBuilder();

            stringBuilder1.AppendFormat("<table id=\"{0}\" class=\"table table-striped table-bordered\"></table>", (object)this._id).AppendLine();

            return stringBuilder1.ToString();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<script type=\"text/javascript\">");
            stringBuilder.Append(this.RenderJavascript());
            stringBuilder.AppendLine("</script>");
            return this.RenderHtmlElements() + stringBuilder.ToString();
        }

        public string ToHtmlString()
        {
            return this.ToString();
        }
    }

    public class Parameter
    {
        public string _paramName { get; set; }

        protected Parameter(string name)
        {
//            if (this.GetType() == typeof(Parameter))
//            {
//                throw new ArgumentException("Use DataParameter or FunctionParameter instead.");
//            }
            if (name.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("Parameter name must contain a value.");
            }
            this._paramName = name;
        }
    }
    //Parameter với Data định sẵn
    public class DataParameter : Parameter
    {
        private string _paramData { get; set; }

        public DataParameter(string name, string data) : base(name)
        {
            if (data.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("Parameter data must contain a value.");
            }
            this._paramData = data;
        }

        public override string ToString()
        {
            return _paramName + ": '" + this._paramData + "'";
        }
    }
    //Parameter với Data có thể trả về bằng 1 function
    //Cần khởi tạo function trong javascript code
    public class FunctionParameter : Parameter
    {
        private string _paramFunction { get; set; }

        public FunctionParameter(string name, string function) : base(name)
        {
            if (function.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("Parameter function must contain a value.");
            }
            this._paramFunction = function;
        }

        public override string ToString()
        {
            return _paramName + ": " + this._paramFunction;
        }
    }

}
