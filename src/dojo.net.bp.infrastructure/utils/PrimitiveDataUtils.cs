using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace dojo.net.bp.infrastructure.utils
{
    internal class PrimitiveDataUtils
    {

        public static DataTable ConvertXmlElementToDataTable(XmlElement? xmlElement, string tagName)
        {
            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName(tagName);

            DataTable dt = new DataTable(tagName);
            if (xmlNodeList.Count > 0)
            {
                int TempColumn = 0;
                foreach (XmlNode node in xmlNodeList.Item(0).ChildNodes)
                {
                    TempColumn++;
                    DataColumn dc = new DataColumn(node.Name, Type.GetType("System.String"));
                    if (dt.Columns.Contains(node.Name))
                    {
                        dt.Columns.Add(dc.ColumnName = dc.ColumnName + TempColumn.ToString());
                    }
                    else
                    {
                        dt.Columns.Add(dc);
                    }
                }
                int ColumnsCount = dt.Columns.Count;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        if (xmlNodeList.Item(i).ChildNodes[j] != null)
                            dr[j] = xmlNodeList.Item(i).ChildNodes[j].InnerText;
                        else
                            dr[j] = "";
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static string GetDataTableStringColum(DataSet src, string table, string column)
        {
            return src.Tables[table] != null &&
                   src.Tables[table].Columns.Contains(column) ?
                   src.Tables[table].Rows[0][column].ToString() : string.Empty;
        }

        public static string GetDataStringXmlNode(XmlElement src, string node)
        {
            return src.GetElementsByTagName(node) != null ? src.GetElementsByTagName(node)[0].InnerText : "";
        }
    }
}
