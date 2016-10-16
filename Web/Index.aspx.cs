using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Text;
using Web.Utils;

namespace Web
{
    public partial class Index : System.Web.UI.Page
    {
        public string courseID = string.Empty;
        public string subjectID = string.Empty;
        public string strClassName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["op"] != null && Request.QueryString["op"].ToString() == "return") {
                    Session["userInfo"] = null;

                    Response.Clear();
                    Response.ContentType = "text/plain";
                    Response.Write(string.Empty);
                    Response.End();  
                }

                #region 判断地址中的ID是否合法

                if (Request.QueryString["CID"] != null && Request.QueryString["CID"].ToString() != string.Empty                    
                    && Request.QueryString["UID"] != null && Request.QueryString["UID"].ToString() != string.Empty)
                {
                    string UIDStr = Request.QueryString["UID"].ToString().Trim();
                    string CIDStr = Request.QueryString["CID"].ToString().Trim();

                    try
                    {
                        string userID = SecurityHelper.Decrypt(UIDStr);
                        courseID = SecurityHelper.Decrypt(CIDStr);

                        HttpHelper httpHelper = new HttpHelper();
                        string url = string.Format("{0}/v2/honor/listHonorClass/format/json", ConfigurationManager.AppSettings["url"], userID);
                        string listResult = httpHelper.HttpGet(url, string.Format("user_id={0}", userID));

                        JObject json = JObject.Parse(listResult);

                        string errorCode = json["error_code"].ToString().Trim();

                        if (errorCode.Equals("0"))
                        {
                            StringBuilder sb = new StringBuilder();
                            var classArr = json["data"];

                            foreach (JObject item in classArr)
                            {
                                strClassName = item["class_name"].ToString().Trim();
                                string classID = item["class_id"].ToString().Trim();

                                if (classID == courseID)
                                {
                                    lblClassName.Text = TextHelper.CutString(strClassName,6);

                                    sb.AppendFormat("请选择科目: &nbsp;&nbsp;&nbsp;&nbsp;<select id=\"mySubject\" onchange=\"SelectSubject()\"><option value=\"{0}\">全部</option>", classID);

                                    var subjectArr = item["subjects"] as JArray;

                                    if (Request.QueryString["SID"] != null && Request.QueryString["SID"].ToString() != string.Empty)
                                    {
                                        string SIDStr = Request.QueryString["SID"].ToString().Trim();
                                        subjectID = SecurityHelper.Decrypt(SIDStr);

                                        hidClassId.Value = subjectID;
                                        hidType.Value = "1";
                                    }
                                    else
                                    {
                                        hidClassId.Value = classID;
                                        hidType.Value = "0";
                                    }

                                    if (subjectArr != null && subjectArr.Count > 0) {
                                        foreach (JObject subject in subjectArr)
                                        {
                                            string subjectName = subject["name"].ToString().Trim();
                                            string tempSubjectID = subject["class_subject_id"].ToString().Trim();

                                            if (subjectID == tempSubjectID)
                                            {
                                                sb.AppendFormat("<option selected=\"selected\" value=\"{0}\">{1}</option>",tempSubjectID,subjectName);
                                            }
                                            else
                                            {
                                                sb.AppendFormat("<option value=\"{0}\">{1}</option>",tempSubjectID,subjectName);
                                            }
                                        }
                                    }

                                    sb.Append("</select>");

                                    break;
                                }
                            }

                            subjectList.InnerHtml = sb.ToString();
                        }
                    }
                    catch
                    {
                    }
                }

                #endregion
            }
        }
    }
}
