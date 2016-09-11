using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Text;
using System.Web;
using Web.Utils;

namespace Web
{
    public partial class Login : System.Web.UI.Page
    {
        public string pwd = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (Request.Cookies["MyCook"] != null))
            {
                txtUsername.Value = Convert.ToString(Request.Cookies["MyCook"]["userName"]).Trim();
                pwd = SecurityHelper.Decrypt(Convert.ToString(Request.Cookies["MyCook"]["pwd"]).ToString().Trim());
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            StringBuilder strbld = new StringBuilder();
            strbld.Append("{");
            strbld.AppendFormat("\"username\":\"{0}\",", txtUsername.Value.Trim());
            strbld.AppendFormat("\"password\":\"{0}\"", txtPwd.Value.Trim());
            strbld.Append("}");

            HttpHelper httpHelper = new HttpHelper();
            string url = string.Format("{0}/v2/user/sign_in/format/json", ConfigurationManager.AppSettings["url"]);
            string strResult = httpHelper.HttpPost(url, strbld.ToString());

            JObject obj = JObject.Parse(strResult);

            string strErrorCode = obj["error_code"].ToString().Trim();

            if (strErrorCode.Equals("0"))
            {
                string strUID = obj["data"]["id"].ToString().Trim();
                string strUserName = obj["data"]["username"].ToString().Trim();
                string strUserSex = obj["data"]["gender"].ToString().Trim();
                string strAvatar = obj["data"]["avatar"].ToString().Trim();

                Session["userInfo"] = "{" + string.Format("\"id\":\"{0}\",\"username\":\"{1}\",\"gender\":\"{2}\",\"avatar\":\"{3}\"", strUID, strUserName, strUserSex, strAvatar) + "}";

                //记住用户名和密码
                if (cbRem.Checked)
                {
                    HttpCookie cookie = new HttpCookie("MyCook");
                    cookie.Expires = DateTime.Now.AddDays(7);

                    cookie.Values.Add("userName", txtUsername.Value.Trim());
                    cookie.Values.Add("pwd", SecurityHelper.Encrypt(txtPwd.Value.Trim()));

                    Response.AppendCookie(cookie);
                }

                Response.Redirect("Course.aspx");
            }
            else
            {
                errorTip.InnerText = "用户名或密码错误！";
                errorTip.Style.Add("display", "block");
            }
        }
    }
}