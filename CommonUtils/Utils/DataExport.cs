using System;
using System.Data;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 数据导出类
    /// </summary>
    public class DataExport
    {
        public static String ExportToExcel(DataTable dt, String tablename)
        {
            //xml格式的excel结构字符串 参数：(0:创建日期  1:列数 2:行数 3:表格名称 4:主体内容
            String xml = @"<?xml version=""1.0""?>
<?mso-application progid=""Excel.Sheet""?>
<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""
 xmlns:o=""urn:schemas-microsoft-com:office:office""
 xmlns:x=""urn:schemas-microsoft-com:office:excel""
 xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet""
 xmlns:html=""http://www.w3.org/TR/REC-html40"">
 <DocumentProperties xmlns=""urn:schemas-microsoft-com:office:office"">
  <Created>1996-12-17T01:32:42Z</Created>
  <LastSaved>2009-01-19T15:35:16Z</LastSaved>
  <Version>12.00</Version>
 </DocumentProperties>
 <OfficeDocumentSettings xmlns=""urn:schemas-microsoft-com:office:office"">
  <RemovePersonalInformation/>
 </OfficeDocumentSettings>
 <ExcelWorkbook xmlns=""urn:schemas-microsoft-com:office:excel"">
  <WindowHeight>4530</WindowHeight>
  <WindowWidth>8505</WindowWidth>
  <WindowTopX>480</WindowTopX>
  <WindowTopY>120</WindowTopY>
  <ProtectStructure>False</ProtectStructure>
  <ProtectWindows>False</ProtectWindows>
 </ExcelWorkbook>
 <Styles>
 <Style ss:ID=""Default"" ss:Name=""Normal"">
   <Alignment ss:Vertical=""Bottom""/>
   <Borders/>
   <Font ss:FontName=""宋体"" x:CharSet=""134"" ss:Size=""10""/>
   <Interior/>
   <NumberFormat/>
   <Protection/>
 </Style>
 <Style ss:ID=""s62"">
   <Borders>
    <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
    <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
    <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
    <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
   </Borders>
   <Font ss:FontName=""宋体"" x:CharSet=""134"" ss:Size=""10"" ss:Bold=""1""/>
   <Interior ss:Color=""#99CCFF"" ss:Pattern=""Solid""/>
 </Style>
  <Style ss:ID=""s63"">
   <NumberFormat ss:Format=""yyyy&quot;年&quot;m&quot;月&quot;d&quot;日&quot;;@""/>
  </Style>
 </Styles>
 <Worksheet ss:Name=""{3}"">
  <Table ss:ExpandedColumnCount=""{1}"" ss:ExpandedRowCount=""{2}"" x:FullColumns=""1""
   x:FullRows=""1"" ss:DefaultColumnWidth=""150""
   ss:DefaultRowHeight=""15.9375"">
{4}  </Table>
  <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
   <Unsynced/>
   <Print>
    <ValidPrinterInfo/>
    <PaperSizeIndex>9</PaperSizeIndex>
    <HorizontalResolution>300</HorizontalResolution>
    <VerticalResolution>300</VerticalResolution>
   </Print>
   <Selected/>
   <Panes>
    <Pane>
     <Number>3</Number>
     <ActiveCol>4</ActiveCol>
    </Pane>
   </Panes>
   <ProtectObjects>False</ProtectObjects>
   <ProtectScenarios>False</ProtectScenarios>
  </WorksheetOptions>
 </Worksheet>
</Workbook>";

            StringBuilder sb = new StringBuilder();
            foreach (DataColumn col in dt.Columns)
            {
                sb.AppendLine(String.Format(@"   <Column ss:AutoFitWidth=""0"" ss:Width=""{0}""/>", GetCellsWidth(col.DataType)));
            }
            //插入标题行
            sb.AppendLine(@"   <Row ss:AutoFitHeight=""0"" ss:Height=""16"">");
            foreach (DataColumn col in dt.Columns)
            {
                sb.AppendLine(String.Format(@"    <Cell ss:StyleID=""s62""><Data ss:Type=""String"">{0}</Data></Cell>", col.Caption));
            }
            sb.AppendLine(@"   </Row>");
            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendLine(@"   <Row ss:AutoFitHeight=""0"" ss:Height=""16"">");
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.DataType == typeof(DateTime))
                    {
                        sb.AppendLine(String.Format(@"    <Cell  ss:StyleID=""s63""><Data ss:Type=""DateTime"">{0}</Data></Cell>", String.Format("{0:yyyy-MM-ddTHH:mm:ss.000}", dr[col.ColumnName])));
                    }
                    else
                    {
                        sb.AppendLine(String.Format(@"    <Cell><Data ss:Type=""{1}"">{0}</Data></Cell>", dr[col.ColumnName], GetExcelDataType(col.DataType)));
                    }
                }
                sb.AppendLine(@"   </Row>");
            }
            String title = tablename;
            if (String.IsNullOrEmpty(title)) title = dt.TableName;
            xml = String.Format(xml, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                dt.Columns.Count, 
                dt.Rows.Count + 1, 
                title, sb.ToString());
            return xml;
        }

        protected static String GetExcelDataType(Type type)
        {
            if (type == typeof(int) || type == typeof(short)
                || type == typeof(double) || type == typeof(float)
                || type == typeof(long) || type == typeof(decimal))
            {
                return "Number";
            }
            else if (type == typeof(DateTime))
            {
                return "DateTime";
            }
            else
            {
                return "String";
            }
        }

        protected static int GetCellsWidth(Type type)
        {
            if (type == typeof(int) || type == typeof(short)
             || type == typeof(double) || type == typeof(float)
             || type == typeof(long) || type == typeof(decimal))
            {
                return 60;
            }
            else if (type == typeof(DateTime))
            {
                return 100;
            }
            else if (type == typeof(Boolean))
            {
                return 40;
            }
            else
            {
                return 120;
            }
        }
    }
}
