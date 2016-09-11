using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Text;
using Web.Utils;

namespace Web
{
    public partial class Course : System.Web.UI.Page
    {
        public string iconStr = string.Empty;
        public string userID = string.Empty;
        public string teacherName = string.Empty;
        public string postUrl = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userInfo"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                //15271818217 , 13521426775 ,18612065888
                if (Session["userInfo"] != null && Session["userInfo"].ToString().Trim() != string.Empty)
                {
                    postUrl = ConfigurationManager.AppSettings["url"];

                    #region 获取基础信息

                    JObject obj = JObject.Parse(Session["userInfo"].ToString().Trim());

                    teacherName = obj["username"].ToString().Trim();
                    lblUsername.Text = teacherName;
                    lblUsernameTwo.Text = teacherName;
                    string sex = obj["gender"].ToString().Trim();
                    string host = ConfigurationManager.AppSettings["url"];

                    string url = host + obj["avatar"].ToString().Trim();

                    if (obj["avatar"].ToString().Trim() != string.Empty && HttpHelper.IsImageExists(url))
                    {
                        imgBig.ImageUrl = url;
                        imgSmall.ImageUrl = url;
                    }
                    else
                    {
                        imgBig.ImageUrl = host + "/uploads/avatar/default.png";
                        imgSmall.ImageUrl = host + "/uploads/avatar/default.png";
                    }

                    if (sex == "1")
                    {
                        lblSex.Text = "先生";
                    }
                    else if (sex == "0")
                    {
                        lblSex.Text = "女士";
                    }
                    else
                    {
                        lblSex.Text = string.Empty;
                    }

                    #endregion

                    #region  获取班级及科目列表

                    HttpHelper httpHelper = new HttpHelper();

                    userID = obj["id"].ToString().Trim();
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

                            if (subjectArr != null && subjectArr.Count > 0) {
                                foreach (JObject subject in subjectArr)
                                {
                                    string subjectName = subject["name"].ToString().Trim();
                                    string subjectImg = ConfigurationManager.AppSettings["url"] + subject["icon"].ToString().Trim();
                                    string subjectID = SecurityHelper.Encrypt(subject["class_subject_id"].ToString().Trim());

                                    if (!HttpHelper.IsImageExists(subjectImg)) {
                                        subjectImg = ConfigurationManager.AppSettings["url"] + "/uploads/class/default.png";
                                    }

                                    string courseID = SecurityHelper.Encrypt(classID);

                                    sb.Append("<div class=\"c_bottom_class\">");
                                    sb.AppendFormat("<div class=\"c_class_setting\" title=\"点击添加学生\" onclick=\"popDiv('{0}','{1}')\"></div><div class=\"c_class_image\">", classID, className);
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
                                sb.AppendFormat("<div class=\"c_class_setting\" title=\"点击添加学生\" onclick=\"popDiv('{0}','{1}')\"></div><div class=\"c_class_image\">", classID, className);
                                sb.Append(string.Format("<img src='{0}' style='width:100%;height:100%;' onclick=\"showRoll('{1}','{2}','{3}')\" /></div>", classIcon, urlUserID, courseID, 0));
                                sb.Append(string.Format("<div class=\"c_class_name\"><p onclick=\"showRoll('{0}','{1}','{2}')\">", urlUserID, courseID, 0));
                                sb.Append(string.Format("{0}", className));
                                sb.Append("</p></div><div class=\"c_class_count\">");
                                sb.Append(string.Format("{0}个学生", studentCount));
                                sb.Append("</div></div>");
                            }
                        }
                    }

                    //添加班级
                    //sb.Append("<div class=\"c_bottom_class\">");
                    //sb.Append("<div class=\"c_class_image\" style=\"margin-top:22px;\"><img src='Content/Images/addClass.png' style='width:100%;height:100%;' onclick=\"popDiv()\" /></div>");
                    //sb.Append("<div class=\"c_class_name\"><p onclick=\"popDiv()\">添加一个班级</p></div>");
                    //sb.Append("</div>");

                    sb.Append("<div style=\"clear:both;height:30px;\"></div>");
                    classList.InnerHtml = sb.ToString();

                    #endregion

                    #region 获取学生图像列表

                    string iconUrl = string.Format("{0}/v2/icon/list/format/json", ConfigurationManager.AppSettings["url"]);
                    string iconResult = httpHelper.HttpGet(iconUrl, "slug=ICON_AVATAR");

                    JObject icoJson = JObject.Parse(iconResult);

                    string iconEerrorCode = icoJson["error_code"].ToString().Trim();

                    if (iconEerrorCode.Equals("0"))
                    {
                        var iconArr = icoJson["data"] as JArray;

                        if (iconArr != null && iconArr.Count > 0)
                        {
                            foreach (var icon in iconArr)
                            {
                                iconStr = iconStr + icon["icon"].ToString().Trim() + ",";
                            }

                            iconStr = iconStr.TrimEnd(',');
                        }
                    }

                    #endregion
                }
            }
        }

        #region 退出登录

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Session["userInfo"] = null;
            Response.Redirect("Login.aspx");
        }

        #endregion
    }
}