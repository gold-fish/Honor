using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Text;
using System.Web;
using Web.Utils;

namespace Web.Ajax
{
    public class StudentList : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string returnStr = string.Empty;
            HttpHelper httpHelper = new HttpHelper();

            #region 获取学生列表

            if(context.Request.QueryString["op"].ToString().Trim()=="list")
            {
                //查看学生列表之前有可能是先添加了学生,所以让学生分数之类的缓存失效
                CacheHelper.RemoveAllCache();

                string classID = context.Request.Params["classID"].ToString().Trim();
                returnStr = GetStudentList(classID, httpHelper);
            }

            #endregion

            #region 获取学生列表(带邀请记录)

            else if (context.Request.QueryString["op"].ToString().Trim() == "listAndTel") {
                string classID = context.Request.Params["classID"].ToString().Trim();
                string subjectID = context.Request.Params["subjectID"].ToString().Trim();
                string userID = context.Request.Params["userID"].ToString().Trim();

                string url = ConfigurationManager.AppSettings["url"].Trim() + string.Format("/v2/student/list/format/json?class_subject_id={0}&class_id={1}&create_by={2}", subjectID, classID, userID);
                string result = httpHelper.HttpGet(url, string.Empty);

                JObject json = JObject.Parse(result);
                int count = Convert.ToInt32(json["num_row"].ToString());
                StringBuilder sb = new StringBuilder();

                //有学生
                if (count > 0)
                {
                    JArray stuArr = json["data"]["student_list"] as JArray;

                    if (stuArr != null && stuArr.Count > 0)
                    {
                        sb.Append("<div class=\"addSutdentTitle\">");
                        sb.AppendFormat("<div class=\"studentTotal\">已添加 <span>{0}</span> 名学生</div>", stuArr.Count);
                        sb.Append("<div class=\"exportIcon\"></div>");
                        sb.Append("<div class=\"exportExcel\" onclick=\"Export()\">导出未邀请学生</div>");
                        sb.Append("</div>");
                        
                        int len = stuArr.Count;

                        for (int i = len - 1; i >= 0; i--)
                        {
                            JObject item = stuArr[i] as JObject;

                            string icon = ConfigurationManager.AppSettings["url"].Trim() + item["icon_class"].ToString();
                            JArray smsArr = stuArr[i]["sms"] as JArray;

                            int width = 0;
                            int height = 0;

                            string imgSrc = HttpHelper.ImgIsExists(icon, out width, out height);
                            string imgTag = string.Empty;

                            //根据图片的大小,适当调整位置
                            if (width >= 115 && width <= 125 && height >= 115 && height <= 125)
                            {
                                imgTag = string.Format("<img style=\"width:45px; height:50px;position:absolute;top:0;left:0;\" src='{0}'/>", imgSrc);
                            }
                            else
                            {
                                imgTag = string.Format("<img style=\"width:35px; height:45px;position:absolute;top:4px;left:3px; \" src='{0}'/>", imgSrc);
                            }

                            sb.Append("<div class=\"studentDetail\">");
                            sb.AppendFormat("<div class=\"detailImage\">{0}</div>", imgTag);
                            sb.AppendFormat("<div class=\"detailName\">{0}</div>", item["name_class"].ToString());

                            if (smsArr != null && smsArr.Count > 0)
                            {
                                sb.Append("<div class=\"detailInvited\">（已邀请）</div>");
                            }
                            
                            sb.Append("</div>");
                        }

                        sb.Append("<div class=\"space\"></div>");
                    }
                }

                returnStr = sb.ToString();
            }

            #endregion

            context.Response.Write(returnStr);
        }

        #region 获取学生列表
        public string GetStudentList(string classID, HttpHelper httpHelper)
        {
            string url = ConfigurationManager.AppSettings["url"].Trim() + "/v2/student/listStudentManagement/format/json";

            string result = httpHelper.HttpGet(url, string.Format("class_id={0}", classID));

            JObject json = JObject.Parse(result);
            int count = Convert.ToInt32(json["num_row"].ToString());
            StringBuilder sb = new StringBuilder();

            //有学生
            if (count > 0)
            {
                JArray stuArr = json["data"] as JArray;

                if (stuArr != null && stuArr.Count > 0)
                {
                    sb.AppendFormat("<div class=\"studentTotal\">已添加 <span>{0}</span> 名学生</div>", stuArr.Count);
                    int len = stuArr.Count;

                    for (int i = len - 1; i >= 0;i--)
                    {
                        JObject item = stuArr[i] as JObject;

                        string icon = ConfigurationManager.AppSettings["url"].Trim() + item["icon_class"].ToString();

                        if (!HttpHelper.IsImageExists(icon))
                        {
                            icon = ConfigurationManager.AppSettings["url"].Trim() + "/uploads/avatar/default.png";
                        }

                        sb.Append("<div class=\"studentDetail\">");
                        sb.AppendFormat("<div class=\"detailImage\"><img src=\"{0}\" style=\"width:45px;height:50px;\"/></div>", icon);
                        sb.AppendFormat("<div class=\"detailName\">{0}</div>", item["name_class"].ToString());
                        sb.Append("</div>");
                    }

                    sb.Append("<div class=\"space\"></div>");
                }
            }

            return sb.ToString();
        }

        #endregion

        public bool IsReusable
        {
            get { return false; }
        }
    }
}