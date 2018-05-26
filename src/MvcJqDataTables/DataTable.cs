using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using MvcJqDataTables.Enums;
using MvcJqDataTables.Extensions;

namespace MvcJqDataTables
{
    public class DataTable : IHtmlString
    {
        public static string FILE_FORMAT_REGEX = @"^.*\.[^\\]+$";
        public static string DEFAULT_BOOTSTRAP_DOM =
            "<\"top\"<\"row\"<\"col-sm-5\"i><\"col-sm-7\"f>>>rt<\"bottom\"<\"row\"<\"col-sm-5\"l><\"col-sm-7\"p>>>";

        private readonly string _id;
        private readonly List<Column> _columns = new List<Column>();
        private bool? _asyncLoad;
        private bool? _isCustomTable = false;
        private bool? rowNumberEnabled = false;

        //        private string _externalSearchFormId;
        //
        //        public DataTable SetExternalSearchFormId(string extSearchFormId)
        //        {
        //            this._externalSearchFormId = extSearchFormId;
        //            return this;
        //        }

        #region Data

        private string _url;
        private string _urlDataFile;
        private readonly List<Parameter> _extraParams = new List<Parameter>();

        //reformat data structure got from the data source
        private string _dataSrc;

        #endregion

        #region Features
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
        #endregion

        #region Callbacks

        private string _onCreatedRow;
        private string _onDrawCallback;
        private string _onFooterCallback;
        private string _onFormatNumber;
        private string _onHeaderCallback;
        private string _onInfoCallback;
        private string _onInitComplete;
        private string _onPreDrawCallback;
        private string _onRowCallback;
        private string _onStateLoadCallback;
        private string _onStateLoaded;
        private string _onStateLoadParams;
        private string _onStateSaveCallback;
        private string _onStateSaveParams;

        #endregion

        #region Options

        //private List<int> _deferLoading = new List<int>();
        //        private bool _destroy;
        private int? _displayStart;
        private string _dom;
        private int[] _lengthMenu;
        private List<Order> _order = new List<Order>();
        //        private bool _orderCellsTop;
        //        private bool _orderClasses;
        //        private bool _orderFixed;
        //        private bool _orderMulti;
        private int? _pageLength;
        private PagingType? _pagingType;
        //        private IDictionary<string,string> _renderer;
        //        private bool _retrieve; //multitable init
        private string _rowId;
        private bool? _scrollCollapse;
        private Search _search;
        //        private List<Search> _searchCols;
        private int? _searchDelay;
        private int? _stateDuration;
        private List<string> _stripeClasses = new List<string>(); //odd - even default
        private int? _tabIndex;

        #endregion

        #region Methods

        public DataTable SetRowNumbers(bool enabled)
        {
            this.rowNumberEnabled = enabled;
            return this;
        }

        public DataTable SetTabIndex(int tabIndex)
        {
            this._tabIndex = tabIndex;
            return this;
        }

        public DataTable SetStripeClasses(List<string> classes)
        {
            this._stripeClasses = classes;
            return this;
        }

        public DataTable SetStateDuration(int duration)
        {
            this._stateDuration = duration;
            return this;
        }

        public DataTable SetSearchDelay(int delay)
        {
            this._searchDelay = delay;
            return this;
        }

        public DataTable SetSearch(string value)
        {
            if (this._search == null)
                this._search = new Search();
            this._search.value = value;
            return this;
        }

        public DataTable SetSearchRegex(bool isRegex)
        {
            if (this._search == null)
                this._search = new Search();
            this._search.regex = isRegex;
            return this;
        }

        public DataTable SetSearchCaseInsensitive(bool caseInsensitive)
        {
            if (this._search == null)
                this._search = new Search();
            this._search.caseInsensitive = caseInsensitive;
            return this;
        }

        public DataTable SetSearchSmart(bool isSmartFilter)
        {
            if (this._search == null)
                this._search = new Search();
            this._search.smart = isSmartFilter;
            return this;
        }

        public DataTable SetScrollCollapse(bool scrollCollapse)
        {
            this._scrollCollapse = scrollCollapse;
            return this;
        }

        public DataTable SetRowId(string rowId)
        {
            this._rowId = rowId;
            return this;
        }

        public DataTable SetPagingType(PagingType pagingType)
        {
            this._pagingType = pagingType;
            return this;
        }

        public DataTable SetPageLength(int pageLength)
        {
            this._pageLength = pageLength;
            return this;
        }

        public DataTable SetOrder(List<Order> order)
        {
            this._order = order;
            return this;
        }

        public DataTable SetLengthMenu(int[] lengths)
        {
            this._lengthMenu = lengths;
            return this;
        }

        public DataTable SetDomStructure(string domStructure)
        {
            this._dom = domStructure;
            return this;
        }

        public DataTable SetDisplayStart(int displayStart)
        {
            this._displayStart = displayStart;
            return this;
        }

        public DataTable OnStateSaveParams(string onStateSaveParams)
        {
            this._onStateSaveParams = onStateSaveParams;
            return this;
        }

        public DataTable OnStateSaveCallback(string onStateSaveCallback)
        {
            this._onStateSaveCallback = onStateSaveCallback;
            return this;
        }

        public DataTable OnStateLoadParams(string onStateLoadParams)
        {
            this._onStateLoadParams = onStateLoadParams;
            return this;
        }

        public DataTable OnStateLoaded(string onStateLoaded)
        {
            this._onStateLoaded = onStateLoaded;
            return this;
        }

        public DataTable OnStateLoadCallback(string onStateLoadCallback)
        {
            this._onStateLoadCallback = onStateLoadCallback;
            return this;
        }

        public DataTable OnRowCallback(string onRowCallback)
        {
            this._onRowCallback = onRowCallback;
            return this;
        }

        public DataTable OnPreDrawCallback(string onPreDrawCallback)
        {
            this._onPreDrawCallback = onPreDrawCallback;
            return this;
        }

        public DataTable OnInitComplete(string onInitComplete)
        {
            this._onInitComplete = onInitComplete;
            return this;
        }

        public DataTable OnInfoCallback(string onInfoCallback)
        {
            this._onInfoCallback = onInfoCallback;
            return this;
        }

        public DataTable OnHeaderCallback(string onHeaderCallback)
        {
            this._onHeaderCallback = onHeaderCallback;
            return this;
        }

        public DataTable OnFormatNumber(string onFormatNumber)
        {
            this._onFormatNumber = onFormatNumber;
            return this;
        }

        public DataTable OnFooterCallback(string onFooterCallback)
        {
            this._onFooterCallback = onFooterCallback;
            return this;
        }

        public DataTable OnDrawCallback(string onDrawCallback)
        {
            this._onDrawCallback = onDrawCallback;
            return this;
        }

        public DataTable OnCreatedRow(string onCreatedRow)
        {
            this._onCreatedRow = onCreatedRow;
            return this;
        }

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

        #endregion

        public string RenderJavascript()
        {
            var stringBuilder = new StringBuilder();
            if (!this._url.IsNullOrWhiteSpace())
            {
                stringBuilder.AppendLine("var dataSource_" + this._id + " = '" + this._url + "';");
            }
            stringBuilder.AppendLine("var tableInstance_" + this._id + " = undefined;");
            if (this._asyncLoad.HasValue && this._asyncLoad.Value)
                stringBuilder.AppendLine("jQuery(window).ready(function () {");
            else
                stringBuilder.AppendLine("jQuery(document).ready(function () {");

            stringBuilder.AppendLine("tableInstance_" + this._id + " = jQuery('#" + this._id + "').DataTable({");

            #region Features

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
            if (this._info.HasValue)
                stringBuilder.AppendFormat("info:{0},", (object)this._info.ToString().ToLower()).AppendLine();
            if (this._scrollX.HasValue)
                stringBuilder.AppendFormat("scrollX:{0},", (object)this._scrollX.ToString().ToLower()).AppendLine();
            if (this._scrollY.HasValue)
                stringBuilder.AppendFormat("scrollY:{0},", (object)this._scrollY.ToString().ToLower()).AppendLine();
            if (this._stateSave.HasValue)
                stringBuilder.AppendFormat("stateSave:{0},", (object)this._stateSave.ToString().ToLower()).AppendLine();

            #endregion

            #region Data

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
                        if (this._extraParams.GroupBy(m => m.GetParameterName()).All(g => g.Count() == 1))
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
                                stringParam = stringDataParam + stringFunctionParam;
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
                    stringBuilder.AppendFormat(", dataSrc:{0}", (object)this._dataSrc);
                }
            }
            stringBuilder.AppendLine("},");

            #endregion

            #region Callbacks

            if (!this._onCreatedRow.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("createdRow:{0},", (object)this._onCreatedRow).AppendLine();

            if (!this._onDrawCallback.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("drawCallback:{0},", (object)this._onDrawCallback).AppendLine();

            if (!this._onFooterCallback.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("footerCallback:{0},", (object)this._onFooterCallback).AppendLine();

            if (!this._onFormatNumber.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("formatNumber:{0},", (object)this._onFormatNumber).AppendLine();

            if (!this._onHeaderCallback.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("headerCallback:{0},", (object)this._onHeaderCallback).AppendLine();

            if (!this._onInfoCallback.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("infoCallback:{0},", (object)this._onInfoCallback).AppendLine();

            if (!this._onInitComplete.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("initComplete:{0},", (object)this._onInitComplete).AppendLine();

            if (!this._onPreDrawCallback.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("preDrawCallback:{0},", (object)this._onPreDrawCallback).AppendLine();

            if (!this._onRowCallback.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("rowCallback:{0},", (object)this._onRowCallback).AppendLine();

            if (!this._onStateLoadCallback.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("stateLoadCallback:{0},", (object)this._onStateLoadCallback).AppendLine();

            if (!this._onStateLoaded.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("stateLoaded:{0},", (object)this._onStateLoaded).AppendLine();

            if (!this._onStateLoadParams.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("stateLoadParams:{0},", (object)this._onStateLoadParams).AppendLine();

            if (!this._onStateSaveCallback.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("stateSaveCallback:{0},", (object)this._onStateSaveCallback).AppendLine();

            if (!this._onStateSaveParams.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("stateSaveParams:{0},", (object)this._onStateSaveParams).AppendLine();

            #endregion

            #region Options

            if (this._displayStart.HasValue)
                stringBuilder.AppendFormat("displayStart:{0},", (object)this._displayStart.Value).AppendLine();

            if (!this._dom.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("dom:'{0}',", (object)this._dom).AppendLine();
            else
                stringBuilder.AppendFormat("dom:'{0}',", DEFAULT_BOOTSTRAP_DOM).AppendLine();

            if (this._lengthMenu != null)
                stringBuilder.AppendFormat("lengthMenu:[{0}],", (object)string.Join(",", ((IEnumerable<int>)this._lengthMenu).Select<int, string>((Func<int, string>)(p => p.ToString())).ToArray<string>())).AppendLine();

            if (this._order.Any())
                stringBuilder.AppendFormat("order:[{0}],", (object)string.Join(", \n", this._order.Select(c => c.ToString()))).AppendLine();

            if (this._pageLength.HasValue)
                stringBuilder.AppendFormat("pageLength:{0},", (object)this._pageLength.Value).AppendLine();

            if (this._pagingType.HasValue && !_pagingType.Value.GetStringValue().IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("pagingType:'{0}',", (object)this._pagingType.Value.GetStringValue()).AppendLine();

            if (!this._rowId.IsNullOrWhiteSpace())
                stringBuilder.AppendFormat("rowId:{0},", (object)this._rowId).AppendLine();

            if (this._scrollCollapse.HasValue)
                stringBuilder.AppendFormat("scrollCollapse:{0},", (object)this._scrollCollapse.Value.ToString().ToLower()).AppendLine();

            if (this._search != null)
            {
                stringBuilder.AppendLine("search: {");
                if (!this._search.value.IsNullOrWhiteSpace())
                    stringBuilder.AppendFormat("search:'{0}',", (object)this._search.value).AppendLine();
                if (this._search.regex.HasValue)
                    stringBuilder.AppendFormat("regex:{0},", (object)this._search.regex.Value.ToString().ToLower()).AppendLine();
                if (this._search.caseInsensitive.HasValue)
                    stringBuilder.AppendFormat("caseInsensitive:{0},", (object)this._search.caseInsensitive.Value.ToString().ToLower()).AppendLine();
                if (this._search.smart.HasValue)
                    stringBuilder.AppendFormat("smart:{0},", (object)this._search.smart.Value.ToString().ToLower()).AppendLine();
                stringBuilder.AppendLine("}");
            }

            if (this._searchDelay.HasValue)
                stringBuilder.AppendFormat("searchDelay:{0},", (object)this._searchDelay.Value).AppendLine();

            if (this._stateDuration.HasValue)
                stringBuilder.AppendFormat("stateDuration:{0},", (object)this._stateDuration.Value).AppendLine();

            if (this._stripeClasses.Any())
                stringBuilder.AppendFormat("stripeClasses:[{0}],", (object)string.Join(",", this._stripeClasses.Select(c => string.Format("'{0}'", c.ToString())))).AppendLine();

            if (this._tabIndex.HasValue)
                stringBuilder.AppendFormat("tabIndex:{0},", (object)this._tabIndex.Value).AppendLine();

            #endregion

            //columns join
            if (this._columns.Count == 0)
            {
                throw new ArgumentException("Table must have at least one column.");
            }
            stringBuilder.AppendLine("columns: [");
            string str1 = string.Join(",\n", this._columns.Select(c => c.ToString()).ToArray());
            stringBuilder.AppendLine(str1);
            stringBuilder.AppendLine("]");

            stringBuilder.AppendLine("});");
            //                stringBuilder.AppendLine("console.log(\'Data Source: \'+ tableInstance_" + this._id + ".ajax.url());");
//            if (rowNumberEnabled.HasValue && rowNumberEnabled.Value)
//            {
//                stringBuilder.AppendLine("tableInstance_" + this._id + ".on(\'order.dt search.dt\', function () {");
//                stringBuilder.AppendLine(
//                    "tableInstance_" + this._id + ".column(0, {search:'applied', order:'applied'}).nodes().each( function (cell, i) {");
//                stringBuilder.AppendLine("cell.innerHTML = i+1;");
//                stringBuilder.AppendLine("});");
//                stringBuilder.AppendLine("}).draw();");
//            }

            stringBuilder.AppendLine("});");
            return stringBuilder.ToString();
        }

        public string RenderHtmlElements()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("<table id=\"{0}\" class=\"table table-striped table-bordered\"></table>", (object)this._id).AppendLine();

            return (this._isCustomTable.HasValue && this._isCustomTable.Value) ? "" : stringBuilder.ToString();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<script type=\"text/javascript\">");
            stringBuilder.Append(this.RenderJavascript());
            stringBuilder.AppendLine("$.fn.dataTable.ext.errMode = 'throw';");
            stringBuilder.AppendLine("</script>");
            return this.RenderHtmlElements() + stringBuilder.ToString();
        }

        public string ToHtmlString()
        {
            return this.ToString();
        }
    }

    public abstract class Parameter
    {
        private string _paramName { get; }

        protected Parameter(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("Parameter name must contain a value.");
            }
            this._paramName = name;
        }

        public string GetParameterName()
        {
            return this._paramName;
        }
    }
    //Parameter with static data
    public class DataParameter : Parameter
    {
        private string _paramData { get; }

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
            return GetParameterName() + ": '" + this._paramData + "'";
        }
    }
    //Parameter with data can be returned by a javascript function
    //Initialize function in javascript first
    public class FunctionParameter : Parameter
    {
        private string _paramFunction { get; }

        public FunctionParameter(string name, string jsFunction) : base(name)
        {
            if (jsFunction.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("Parameter function must contain a value.");
            }
            this._paramFunction = jsFunction;
        }

        public override string ToString()
        {
            return GetParameterName() + ": " + this._paramFunction;
        }
    }

}
