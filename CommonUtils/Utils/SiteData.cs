using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using MSSQLDB;

namespace CommonUtils
{
    /// <summary>
    ///SiteData 的摘要说明
    /// </summary>
    [WebService(Namespace = "WebService", Description = "")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SiteData : System.Web.Services.WebService
    {

        public SiteData()
        {
            //如果使用设计的组件，请取消注释以下行 
            //InitializeComponent();
        }

        [WebMethod]
        public DataSet Data(String Table, String Cmd)
        {
            DataSet dt = new DataSet();
            switch (Cmd)
            {
                case "0":
                    dt = db.ExecuteDataSet("SELECT * FROM " + Table);
                    break;
            }
            return dt;
        }
        [WebMethod]
        public String DataXml(String Table, String Cmd)
        {
            return "AAAAAAAAA";
            //DataTable dt = new DataTable();
            //switch (Cmd)
            //{
            //    case "0":
            //        dt = db.ExecuteDataTable("SELECT * FROM " + Table);
            //        break;
            //}
            //return SerialDataTableXml(dt);
        }
        /// <summary>
        /// 序列化成xml
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtname"></param>
        /// <returns></returns>
        private String SerialDataTableXml(DataTable dt)
        {
            //dt.TableName = "AAAAAAAAA";
            StringBuilder sb = new StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);
            XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
            serializer.Serialize(xw, dt);
            xw.Close();
            return sb.ToString();
        }
        /// <summary>
        /// 反序列化成dataTable
        /// </summary>
        /// <param name="pXml"></param>
        /// <returns></returns>
        private DataTable DeSerialXmlToDataTable(String pXml)
        {
            StringReader strReader = new StringReader(pXml);
            XmlReader xmlReader = XmlReader.Create(strReader);
            XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
            DataTable dt = serializer.Deserialize(xmlReader) as DataTable;
            return dt;
        }
        private void InitializeComponent()
        {

        }
    }
}