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
                string classID = context.Request.Params["classID"].ToString().Trim();
                returnStr = GetStudentList(classID, httpHelper);
            }

            #endregion

            #region 添加学生(此方法暂未使用)

            else if (context.Request.QueryString["op"].ToString().Trim() == "add")
            {
                string class_id = context.Request.Params["class_id"].ToString().Trim();
                string create_by = context.Request.Params["create_by"].ToString().Trim();
                string class_name = context.Request.Params["class_name"].ToString().Trim();
                string user_name = context.Request.Params["user_name"].ToString().Trim();
                
                string icon_class = context.Request.Params["icon_class"].ToString().Trim();
                string name_class = context.Request.Params["name_class"].ToString().Trim();
                string telephone = context.Request.Params["telephone"].ToString().Trim();

                StringBuilder postStr = new StringBuilder();
                postStr.Append("{");
                postStr.AppendFormat("\"class_id\":\"{0}\",", class_id);
                postStr.AppendFormat("\"create_by\":\"{0}\",", create_by);
                postStr.AppendFormat("\"class_name\":\"{0}\",", class_name);
                postStr.AppendFormat("\"user_name\":\"{0}\",", user_name);
                
                if (telephone==string.Empty)
                {
                    postStr.AppendFormat("\"students\":[{{\"icon_class\":\"{0}\",\"name_class\":\"{1}\"}}]", icon_class, name_class);
                }
                else
                {
                    postStr.AppendFormat("\"students\":[{{\"icon_class\":\"{0}\",\"name_class\":\"{1}\",\"telephone\":\"{2}\"}}]", icon_class, name_class, telephone);
                }

                postStr.Append("}");

                string postUrl = string.Format("{0}/v2/student/createBatchForPc/format/json", ConfigurationManager.AppSettings["url"]);
                httpHelper.HttpPost(postUrl, postStr.ToString());

                //返回添加完学生后的列表
                returnStr = GetStudentList(class_id, httpHelper);
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
                        sb.AppendFormat("<div class=\"detailImage\"><img src=\"{0}\" style=\"width:96%;height:96%;\"/></div>", icon);
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