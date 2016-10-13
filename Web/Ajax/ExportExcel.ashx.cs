using Newtonsoft.Json.Linq;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using Web.Utils;

namespace Web.Ajax
{
    /// <summary>
    /// ExportExcel 的摘要说明
    /// </summary>
    public class ExportExcel : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if (context.Request.QueryString["op"] == "excel")
            {
                string userID = context.Request.QueryString["userID"].ToString().Trim();
                string classID = context.Request.QueryString["classID"].ToString().Trim();
                string subjectID = context.Request.QueryString["subjectID"].ToString().Trim();

                XSSFWorkbook book = new XSSFWorkbook();  
                ISheet sheet1 = book.CreateSheet("student");  

                sheet1.SetColumnWidth(0, 3500);
                sheet1.SetColumnWidth(1, 2300);
                sheet1.SetColumnWidth(2, 3000);
                sheet1.SetColumnWidth(3, 2300);
                sheet1.SetColumnWidth(4, 5000);

                XSSFCellStyle headStyle = (XSSFCellStyle)book.CreateCellStyle();
                XSSFCellStyle rowStyle = (XSSFCellStyle)book.CreateCellStyle();
                XSSFCellStyle descStyle = (XSSFCellStyle)book.CreateCellStyle();

                //标题样式
                headStyle.FillForegroundColor = 22;
                headStyle.FillBackgroundColor = 22;
                headStyle.Alignment = HorizontalAlignment.Center;
                headStyle.VerticalAlignment = VerticalAlignment.Center;

                //行样式
                rowStyle.FillForegroundColor = 22;
                rowStyle.FillBackgroundColor = 22;
                rowStyle.Alignment = HorizontalAlignment.Center;
                rowStyle.VerticalAlignment = VerticalAlignment.Center;

                //备注样式
                descStyle.FillForegroundColor = 22;
                descStyle.FillBackgroundColor = 22;
                descStyle.Alignment = HorizontalAlignment.Center;
                descStyle.VerticalAlignment = VerticalAlignment.Center;

                IFont font = book.CreateFont();
                font.IsBold = true;
                headStyle.SetFont(font);

                IFont descFont = book.CreateFont();
                descFont.Color = HSSFColor.Red.Index;
                descStyle.SetFont(descFont);

                #region  标题行
              
                IRow iRow0 = sheet1.CreateRow(0);
                
                iRow0.Height = 500;

                iRow0.CreateCell(0).SetCellValue("学生ID");
                iRow0.CreateCell(1).SetCellValue("分隔符");
                iRow0.CreateCell(2).SetCellValue("姓名");
                iRow0.CreateCell(3).SetCellValue("分隔符");
                iRow0.CreateCell(4).SetCellValue("家长手机号");

                iRow0.Cells[0].CellStyle = headStyle;
                iRow0.Cells[1].CellStyle = headStyle;
                iRow0.Cells[2].CellStyle = headStyle;
                iRow0.Cells[3].CellStyle = headStyle;
                iRow0.Cells[4].CellStyle = headStyle;

                #endregion

                #region 获取未邀请学生列表(内容行)

                HttpHelper httpHelper = new HttpHelper();

                string url = ConfigurationManager.AppSettings["url"].Trim() + string.Format("/v2/student/list/format/json?class_subject_id={0}&class_id={1}&create_by={2}", subjectID, classID, userID);
                string result = httpHelper.HttpGet(url, string.Empty);

                JObject json = JObject.Parse(result);
                int count = Convert.ToInt32(json["num_row"].ToString());
                int rowIndex = 1;

                //有学生
                if (count > 0)
                {
                    JArray stuArr = json["data"]["student_list"] as JArray;

                    if (stuArr != null && stuArr.Count > 0)
                    {
                        int len = stuArr.Count;

                        for (int i = len - 1; i >= 0; i--)
                        {
                            JObject item = stuArr[i] as JObject;

                            string studentID = Convert.ToInt32(item["student_id"].ToString().Trim()).ToString();
                            string studentName = item["name_class"].ToString().Trim();

                            JArray smsArr = item["sms"] as JArray;

                            //未邀请
                            if ((smsArr == null) || (smsArr != null && smsArr.Count == 0))
                            {
                                IRow iRow = sheet1.CreateRow(rowIndex);

                                iRow.Height = 500;

                                iRow.CreateCell(0).SetCellValue(studentID);
                                iRow.CreateCell(1).SetCellValue("|");
                                iRow.CreateCell(2).SetCellValue(studentName);
                                iRow.CreateCell(3).SetCellValue("|");
                                iRow.CreateCell(4).SetCellValue(string.Empty);

                                iRow.Cells[0].CellStyle = rowStyle;
                                iRow.Cells[1].CellStyle = rowStyle;
                                iRow.Cells[2].CellStyle = rowStyle;
                                iRow.Cells[3].CellStyle = rowStyle;
                                iRow.Cells[4].CellStyle = rowStyle;

                                rowIndex = rowIndex + 1;
                            }
                        }
                    }
                }

                #endregion

                #region 备注行

                IRow lastRow = sheet1.CreateRow(rowIndex + 2);

                lastRow.CreateCell(0).SetCellValue("备注:复制时从第二行开始复制,即只复制学生列表,不复制标题");
                lastRow.CreateCell(1).SetCellValue(string.Empty);
                lastRow.CreateCell(2).SetCellValue(string.Empty);
                lastRow.CreateCell(3).SetCellValue(string.Empty);
                lastRow.CreateCell(4).SetCellValue(string.Empty);

                lastRow.Cells[0].CellStyle = descStyle;
                lastRow.Cells[1].CellStyle = descStyle;
                lastRow.Cells[2].CellStyle = descStyle;
                lastRow.Cells[3].CellStyle = descStyle;
                lastRow.Cells[4].CellStyle = descStyle;

                lastRow.Height = 500;
                sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex + 2, rowIndex + 2, 0, 4));

                #endregion

                //输出到客户端
                MemoryStream ms = new MemoryStream();
                book.Write(ms);

                string fileName = "invite";

                if (context.Request.Browser.Browser.ToUpper() == "IE")
                {
                    fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);
                }

                context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", fileName));
                context.Response.BinaryWrite(ms.ToArray());
                book = null;
                ms.Close();
                ms.Dispose();
            }

            context.Response.Write(string.Empty);
        }

        public bool IsReusable
        {
            get{ return false; }
        }
    }
}