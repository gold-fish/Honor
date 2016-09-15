using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Text;
using System.Web;
using Web.Utils;

namespace Web.Ajax
{
    public class CourseList : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            #region  获取班级及科目列表

            HttpHelper httpHelper = new HttpHelper();

            string userID = context.Request.Params["userID"].ToString().Trim();
            string urlUserID = SecurityHelper.Encrypt(userID);
            string listUrl = string.Format("{0}/v2/honor/listHonorClass/format/json", ConfigurationManager.AppSettings["url"]);
            string listResult = httpHelper.HttpGet(listUrl, string.Format("user_id={0}", userID));

            JObject json = JObject.Parse(listResult);

            string errorCode = json["error_code"].ToString().Trim();
            StringBuilder sb = new StringBuilder("<div class=\"c_bottom_title\">您参与的班级</div>");

            if (errorCode.Equals("0"))
            {
                var classArr = json["data"];

                foreach (JObject item in classArr)
                {
                    string className = item["class_name"].ToString().Trim();
                    string studentCount = item["studentNum"].ToString().Trim();
                    string classID = item["class_id"].ToString().Trim();
                    string classIcon = ConfigurationManager.AppSettings["url"] + item["class"]["icon"].ToString().Trim();

                    if (!HttpHelper.IsImageExists(classIcon))
                    {
                        classIcon = ConfigurationManager.AppSettings["url"] + "/uploads/class/default.png";
                    }

                    var subjectArr = item["subjects"] as JArray;

                    if (subjectArr != null && subjectArr.Count > 0)
                    {
                        foreach (JObject subject in subjectArr)
                        {
                            string subjectName = subject["name"].ToString().Trim();
                            string subjectImg = ConfigurationManager.AppSettings["url"] + subject["icon"].ToString().Trim();
                            string subjectID = SecurityHelper.Encrypt(subject["class_subject_id"].ToString().Trim());

                            if (!HttpHelper.IsImageExists(subjectImg))
                            {
                                subjectImg = ConfigurationManager.AppSettings["url"] + "/uploads/class/default.png";
                            }

                            string courseID = SecurityHelper.Encrypt(classID);

                            sb.Append("<div class=\"c_bottom_class\">");
                            sb.AppendFormat("<div class=\"c_class_setting\" title=\"点击添加学生\" onclick=\"popDiv('{0}','{1}','{2}')\"></div><div class=\"c_class_image\">", classID, className, subject["class_subject_id"].ToString().Trim());
                            sb.Append(string.Format("<img src='{0}' style='width:100%;height:100%;' onclick=\"showRoll('{1}','{2}','{3}')\" /></div>", subjectImg, urlUserID, courseID, subjectID));
                            sb.Append(string.Format("<div class=\"c_class_name\"><p onclick=\"showRoll('{0}','{1}','{2}')\">", urlUserID, courseID, subjectID));
                            sb.Append(string.Format("{0}-{1}", className, subjectName));
                            sb.Append("</p></div><div class=\"c_class_count\">");
                            sb.Append(string.Format("{0}个学生", studentCount));
                            sb.Append("</div></div>");
                        }
                    }
                    else
                    {
                        string courseID = SecurityHelper.Encrypt(classID);

                        sb.Append("<div class=\"c_bottom_class\">");
                        sb.AppendFormat("<div class=\"c_class_setting\" title=\"点击添加学生\" onclick=\"popDiv('{0}','{1}','{2}')\"></div><div class=\"c_class_image\">", classID, className, 0);
                        sb.Append(string.Format("<img src='{0}' style='width:100%;height:100%;' onclick=\"showRoll('{1}','{2}','{3}')\" /></div>", classIcon, urlUserID, courseID, 0));
                        sb.Append(string.Format("<div class=\"c_class_name\"><p onclick=\"showRoll('{0}','{1}','{2}')\">", urlUserID, courseID, 0));
                        sb.Append(string.Format("{0}", className));
                        sb.Append("</p></div><div class=\"c_class_count\">");
                        sb.Append(string.Format("{0}个学生", studentCount));
                        sb.Append("</div></div>");
                    }
                }
            }

            sb.Append("<div style=\"clear:both;height:30px;\"></div>");

            #endregion

            context.Response.Write(sb.ToString());
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}