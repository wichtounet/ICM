using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace ICM.Utils
{
    /// <summary>
    ///  Contains several extensions methods useful to make easier the development. 
    /// </summary>
    /// <remarks>Baptiste Wicht</remarks>
    public static class Extensions
    {
        public static int ToInt(this String str)
        {
            return Convert.ToInt16(str);
        }

        public static int? ToIntStrict(this String str)
        {
            if (str == null)
            {
                return null;
            }

            return Convert.ToInt16(str);
        }

        public static SqlParameter[] ToArray(this SqlParameterCollection collection)
        {
            var array = new SqlParameter[collection.Count];
            collection.CopyTo(array, 0);

            return array;
        }

        public static void DataBind<T>(this DropDownList list, List<T> dataSource, string textField, string valueField )
        {
            list.DataSource = dataSource;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }

        public static void DataBindWithEmptyElement<T>(this DropDownList list, List<T> dataSource, string textField, string valueField)
        {
            list.Items.Add(new ListItem(string.Empty, string.Empty));
            list.AppendDataBoundItems = true;
            list.DataSource = dataSource;
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataBind();
        }
    }
}
