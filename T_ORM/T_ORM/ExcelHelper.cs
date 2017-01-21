using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Utils
{
    /// <summary>
    /// Excel操作类(包括：HSSFWorkbook、OleDb、Aspose、EPPlus、HttpResponse)
    /// Spire.xls:http://www.cnblogs.com/landeanfen/p/5888973.html
    /// </summary>
    public class ExcelHelper
    {
        #region 读取Excel数据到DataTable
        /// <summary>
        /// HSSFWorkbook读取xls
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public static DataTable GetExcelToDataTableByNPOI(string filepath)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook hssworkbook;
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                hssworkbook = new NPOI.HSSF.UserModel.HSSFWorkbook(file);
            }
            ISheet sheet = hssworkbook.GetSheetAt(0);
            IRow headerRow = sheet.GetRow(0);
            int rowCount = sheet.LastRowNum;
            int cellCount = headerRow.LastCellNum;
            DataTable dt = new DataTable();
            for (int j = 0; j < cellCount; j++)
            {
                dt.Columns.Add(Convert.ToChar(((int)('A')) + j).ToString());
            }
            for (int r = (sheet.FirstRowNum + 1); r <= rowCount; r++)
            {
                IRow row = sheet.GetRow(r);  //读取当前行数据
                if (row != null)
                {
                    DataRow dr = dt.NewRow();
                    cellCount = row.LastCellNum;
                    bool isCellNull = true;
                    for (int i = 0; i < cellCount; i++)
                    {
                        ICell cell = row.GetCell(i);
                        if (cell == null)
                        {
                            dr[i] = "";
                        }
                        else
                        {
                            cell.SetCellType(NPOI.SS.UserModel.CellType.String);
                            dr[i] = cell.StringCellValue;
                            if (isCellNull)
                            {
                                if (!string.IsNullOrWhiteSpace(cell.StringCellValue))
                                {
                                    isCellNull = false;
                                }
                            }
                        }
                    }
                    if (!isCellNull)
                    {
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// OleDb读取xls/xlsx
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public static DataTable GetExcelToDataTableByOleDb(string filepath)
        {
            string strCon = "";
            if (filepath.IndexOf(".xlsx") != -1)
                strCon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
            else if (filepath.IndexOf(".xls") != -1)
                strCon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filepath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";

            string strCom = " SELECT * FROM [Sheet1$]";

            DataTable dt_temp = new DataTable();
            using (OleDbConnection myConn = new OleDbConnection(strCon))
            using (OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn))
            {
                myConn.Open();
                myCommand.Fill(dt_temp);
            }

            return dt_temp;
        }


        /// <summary>
        /// Aspose读取xls/xlsx
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public static DataTable GetExcelToDataTableByAspose(string filepath)
        {
            DataTable dt_temp = new DataTable();
            try
            {
                Aspose.Cells.Workbook oBook = new Aspose.Cells.Workbook(filepath);
                Cells cells = oBook.Worksheets[0].Cells;
                dt_temp = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1);
            }
            catch (Exception ex)
            {
            }
            return dt_temp;
        }
        #endregion

        #region 导出DataTable/List数据到Excel

        /// <summary>
        /// 直接导出到HttpResponse输出流
        /// </summary>
        /// <param name="dt">需要导出的DataTable</param>
        /// <param name="fileName">导出文件名称</param>
        /// <param name="Title">表头</param>
        public static void ExportToExcel(DataTable dt, string fileName, List<string> Title)
        {
            HttpResponse response = System.Web.HttpContext.Current.Response; ;
            response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + ".xls");

            Workbook workbook = new Workbook();
            Worksheet sheet = (Worksheet)workbook.Worksheets[0];
            int i = 0;
            foreach (var item in Title)
            {
                Aspose.Cells.Style s = new Aspose.Cells.Style();
                s.Font.IsBold = true;
                sheet.Cells[0, i].PutValue(item);
                sheet.Cells[0, i].SetStyle(s);
                i++;
            }
            int RCount = dt.Rows.Count;
            int CCount = dt.Columns.Count;
            for (int x = 0; x < RCount; x++)
            {
                for (int y = 0; y < CCount; y++)
                {
                    sheet.Cells[x + 1, y].PutValue(dt.Rows[x][y] + "");
                }
            }
            response.Clear();
            response.Buffer = true;
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/ms-excel";
            response.BinaryWrite(workbook.SaveToStream().ToArray());
            response.End();
        }

        /// <summary>
        /// 直接导出到HttpResponse输出流，并自定义文件名
        /// </summary>
        public static void DataTableToExcel(DataTable dtData, String FileName)
        {
            GridView dgExport = null;
            HttpContext curContext = HttpContext.Current;
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;

            if (dtData != null)
            {
                curContext.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8) + ".xls");
                curContext.Response.ContentType = "application nd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                curContext.Response.Charset = "GB2312";
                strWriter = new StringWriter();
                htmlWriter = new HtmlTextWriter(strWriter);
                dgExport = new GridView();
                dgExport.DataSource = dtData.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();
                dgExport.RenderControl(htmlWriter);
                //curContext.Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=gb2312\"/>" + strWriter.ToString()); // 自动返回可下载的文件流 
                curContext.Response.Write(strWriter.ToString());
                curContext.Response.End();
            }
        }

        /// <summary> 
        /// Aspose导出数据到本地 
        /// </summary> 
        /// <param name="dt">要导出的数据</param> 
        /// <param name="tableName">表格标题</param> 
        /// <param name="path">保存路径</param> 
        public static void ExportToExcelByAspose(DataTable dt, string tableName, string path)
        {
            Workbook workbook = new Workbook(); //工作簿 
            Worksheet sheet = workbook.Worksheets[0]; //工作表 
            Cells cells = sheet.Cells;//单元格 

            ////为标题设置样式     
            //Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式 
            //styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            //styleTitle.Font.Name = "宋体";//文字字体 
            //styleTitle.Font.Size = 18;//文字大小 
            //styleTitle.Font.IsBold = true;//粗体 

            ////样式2 
            //Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            //style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            //style2.Font.Name = "宋体";//文字字体 
            //style2.Font.Size = 14;//文字大小 
            //style2.Font.IsBold = true;//粗体 
            //style2.IsTextWrapped = true;//单元格内容自动换行 
            //style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            //style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            //style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            //style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            ////样式3 
            //Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            //style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            //style3.Font.Name = "宋体";//文字字体 
            //style3.Font.Size = 12;//文字大小 
            //style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            //style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            //style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            //style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            int Colnum = dt.Columns.Count;//表格列数 
            int Rownum = dt.Rows.Count;//表格行数 

            //生成行1 标题行    
            //cells.Merge(0, 0, 1, Colnum);//合并单元格 
            //cells[0, 0].PutValue(tableName);//填写内容 
            //cells[0, 0].SetStyle(styleTitle);
            //cells.SetRowHeight(0, 38);

            //生成行2 列名行 
            for (int i = 0; i < Colnum; i++)
            {
                cells[0, i].PutValue(dt.Columns[i].ColumnName);
                //cells[1, i].SetStyle(style2);
                cells.SetRowHeight(0, 25);
            }

            //生成数据行 
            for (int i = 0; i < Rownum; i++)
            {
                for (int k = 0; k < Colnum; k++)
                {
                    cells[1 + i, k].PutValue(dt.Rows[i][k].ToString());
                    //cells[2 + i, k].SetStyle(style3);
                }
                cells.SetRowHeight(1 + i, 24);
            }

            workbook.Save(path);
        }

        /// <summary>
        /// EPPlus导出数据到本地
        /// </summary>
        /// <param name="pathFileName"></param>
        /// <param name="IsPageRegistration"></param>
        /// <param name="SearchContent"></param>
        /// <param name="competitionId"></param>
        private void ExportToExcelByEPPlus(string pathFileName, DataTable dt)
        {
            //创建存放Excel的文件夹
            FileInfo newFile = new FileInfo(pathFileName);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(pathFileName);
            }


            //创建工作簿和工作表
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add("userInfo");

                /*添加表头*/
                workSheet.InsertRow(1, 1);

                using (var range = workSheet.Cells[1, 1, 1, 6])
                {
                    range.Merge = true;
                    range.Style.Font.SetFromFont(new System.Drawing.Font("Britannic Bold", 18, FontStyle.Italic));
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    range.Style.Font.Color.SetColor(Color.Black);
                    range.Value = "参赛者基本信息";
                }
                /*设置标题*/
                workSheet.Cells[2, 1].Value = "队      别";
                workSheet.Cells[2, 2].Value = "姓      名";
                workSheet.Cells[2, 3].Value = "身份证号码";
                workSheet.Cells[2, 4].Value = "省      份";
                workSheet.Cells[2, 5].Value = "城      市";
                workSheet.Cells[2, 6].Value = "学      校";

                using (var range = workSheet.Cells[2, 1, 2, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);
                    range.AutoFilter = true;
                }
                /*设置单元格内容*/
                int row = 3;
                foreach (DataRow pUser in dt.Rows)
                {
                    workSheet.Cells[row, 1].Value = pUser[0];
                    workSheet.Cells[row, 2].Value = pUser[1];
                    workSheet.Cells[row, 3].Value = pUser[2];
                    workSheet.Cells[row, 4].Value = pUser[3];
                    workSheet.Cells[row, 5].Value = pUser[4];
                    workSheet.Cells[row, 6].Value = pUser[5];
                    row++;
                }
                /*添加表尾*/
                using (var range = workSheet.Cells[dt.Rows.Count + 3, 1, dt.Rows.Count + 3, 6])
                {
                    range.Merge = true;
                    range.Style.Font.SetFromFont(new System.Drawing.Font("Britannic Bold", 18, FontStyle.Italic));
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    range.Style.Font.Color.SetColor(Color.Black);
                    range.Value = "总人数:" + dt.Rows.Count + "人";
                }
                /*设置整个Excel样式*/
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                workSheet.Cells[workSheet.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[workSheet.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[workSheet.Dimension.Address].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                package.Save();
            }
        }

        /// <summary>
        /// OLEDB将数据导出至Excel文件
        /// </summary>
        /// <param name="Table">DataTable对象</param>
        /// <param name="Columns">要导出的数据列集合</param>
        /// <param name="ExcelFilePath">Excel文件路径</param>
        public static bool ExportToExcelByOLEDB(DataTable Table, ArrayList Columns, string ExcelFilePath)
        {
            if (File.Exists(ExcelFilePath))
            {
                throw new Exception("该文件已经存在！");
            }

            //如果数据列数大于表的列数，取数据表的所有列
            if (Columns.Count > Table.Columns.Count)
            {
                for (int s = Table.Columns.Count + 1; s <= Columns.Count; s++)
                {
                    Columns.RemoveAt(s);   //移除数据表列数后的所有列
                }
            }

            //遍历所有的数据列，如果有数据列的数据类型不是 DataColumn，则将它移除
            DataColumn column = new DataColumn();
            for (int j = 0; j < Columns.Count; j++)
            {
                try
                {
                    column = (DataColumn)Columns[j];
                }
                catch (Exception)
                {
                    Columns.RemoveAt(j);
                }
            }
            if ((Table.TableName.Trim().Length == 0) || (Table.TableName.ToLower() == "table"))
            {
                Table.TableName = "Sheet1";
            }

            //数据表的列数
            int ColCount = Columns.Count;

            //创建参数
            OleDbParameter[] para = new OleDbParameter[ColCount];

            //创建表结构的SQL语句
            string TableStructStr = @"Create Table " + Table.TableName + "(";

            //连接字符串
            string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0;";
            OleDbConnection objConn = new OleDbConnection(connString);

            //创建表结构
            OleDbCommand objCmd = new OleDbCommand();

            //数据类型集合
            ArrayList DataTypeList = new ArrayList();
            DataTypeList.Add("System.Decimal");
            DataTypeList.Add("System.Double");
            DataTypeList.Add("System.Int16");
            DataTypeList.Add("System.Int32");
            DataTypeList.Add("System.Int64");
            DataTypeList.Add("System.Single");

            DataColumn col = new DataColumn();

            //遍历数据表的所有列，用于创建表结构
            for (int k = 0; k < ColCount; k++)
            {
                col = (DataColumn)Columns[k];

                //列的数据类型是数字型
                if (DataTypeList.IndexOf(col.DataType.ToString().Trim()) >= 0)
                {
                    para[k] = new OleDbParameter("@" + col.Caption.Trim(), OleDbType.Double);
                    objCmd.Parameters.Add(para[k]);

                    //如果是最后一列
                    if (k + 1 == ColCount)
                    {
                        TableStructStr += col.Caption.Trim() + " Double)";
                    }
                    else
                    {
                        TableStructStr += col.Caption.Trim() + " Double,";
                    }
                }
                else
                {
                    para[k] = new OleDbParameter("@" + col.Caption.Trim(), OleDbType.VarChar);
                    objCmd.Parameters.Add(para[k]);

                    //如果是最后一列
                    if (k + 1 == ColCount)
                    {
                        TableStructStr += col.Caption.Trim() + " VarChar)";
                    }
                    else
                    {
                        TableStructStr += col.Caption.Trim() + " VarChar,";
                    }
                }
            }

            //创建Excel文件及文件结构
            try
            {
                objCmd.Connection = objConn;
                objCmd.CommandText = TableStructStr;

                if (objConn.State == ConnectionState.Closed)
                {
                    objConn.Open();
                }
                objCmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw exp;
            }

            //插入记录的SQL语句
            string InsertSql_1 = "Insert into " + Table.TableName + " (";
            string InsertSql_2 = " Values (";
            string InsertSql = "";

            //遍历所有列，用于插入记录，在此创建插入记录的SQL语句
            for (int colID = 0; colID < ColCount; colID++)
            {
                if (colID + 1 == ColCount)  //最后一列
                {
                    InsertSql_1 += Columns[colID].ToString().Trim() + ")";
                    InsertSql_2 += "@" + Columns[colID].ToString().Trim() + ")";
                }
                else
                {
                    InsertSql_1 += Columns[colID].ToString().Trim() + ",";
                    InsertSql_2 += "@" + Columns[colID].ToString().Trim() + ",";
                }
            }

            InsertSql = InsertSql_1 + InsertSql_2;

            //遍历数据表的所有数据行
            DataColumn DataCol = new DataColumn();
            for (int rowID = 0; rowID < Table.Rows.Count; rowID++)
            {
                for (int colID = 0; colID < ColCount; colID++)
                {
                    //因为列不连续，所以在取得单元格时不能用行列编号，列需得用列的名称
                    DataCol = (DataColumn)Columns[colID];
                    if (para[colID].DbType == DbType.Double && Table.Rows[rowID][DataCol.Caption].ToString().Trim() == "")
                    {
                        para[colID].Value = 0;
                    }
                    else
                    {
                        para[colID].Value = Table.Rows[rowID][DataCol.Caption].ToString().Trim();
                    }
                }
                try
                {
                    objCmd.CommandText = InsertSql;
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception exp)
                {
                    string str = exp.Message;
                }
            }
            try
            {
                if (objConn.State == ConnectionState.Open)
                {
                    objConn.Close();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return true;
        }
        #endregion
    }
}