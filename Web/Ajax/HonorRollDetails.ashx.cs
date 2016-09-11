using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Web.Utils;

namespace Web.Ajax
{
    /// <summary>
    /// HonorRollDetails 的摘要说明
    /// </summary>
    public class HonorRollDetails : IHttpHandler, IRequiresSessionState
    {

        /// <summary>
        /// 接口地址
        /// </summary>
        public string ApiUrl
        {
            get
            {
                string strApiUrl = string.Format("{0}/v2/honor/detail/format/json", ConfigurationManager.AppSettings["url"].Trim());
                return strApiUrl;
            }
        }

        HttpHelper helper = new HttpHelper();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string strType = context.Request.Params["action"].Trim();//排行类型
            string strResult = string.Empty;
            int ID = 0;

            int.TryParse(context.Request.Params["Id"], out ID);
            int itemType = 0;//排行类型(0:全部,1:具体科目)
            int.TryParse(context.Request.Params["typeId"], out itemType);

            try
            {
                if (strType.Length > 0)
                {
                    switch (strType)
                    {
                        case "class":
                            strResult = CreateClassHtml(ID, itemType);
                            break;
                        case "personal":
                            strResult = CreatePersonalHtml(ID, itemType);
                            break;
                        case "group":
                            strResult = CreateGroupHtml(ID, itemType);
                            break;

                    }
                }
            }
            catch
            {
                strResult = "<div style='width:300px;line-height:100px;color:#ccc;height:100px;text-align:center;'>暂无数据</div>";
            }

            context.Response.Write(strResult);
        }

        #region 生成排行榜详情

        /// <summary>
        /// 获取班级排行榜详细信息
        /// </summary>
        /// <param name="getDataTypeId">班级/科目ID</param>  
        /// <param name="typeID"></param>
        /// <returns></returns>
        private string CreateClassHtml(int getDataTypeId, int typeID)
        {
            string strKey = "clh" + getDataTypeId;
            object obj = CacheHelper.GetCache(strKey);
            string strJson = string.Empty;
            if (obj == null)
            {
                strJson = typeID == 0 ? helper.HttpGet(ApiUrl, string.Format("class_id={0}&&type=3", getDataTypeId)) : helper.HttpGet(ApiUrl, string.Format("class_subject_id={0}&&type=3", getDataTypeId));

                if (strJson.Length > 0)
                {
                    CacheHelper.SetCache(strKey, strJson, DateTime.Now.AddSeconds(30), TimeSpan.Zero);
                }
            }
            else
            {
                strJson = obj.ToString();
            }
            return CreateHtml(strJson);
        }

        /// <summary>
        /// 获取个人排行榜详细信息
        /// </summary>
        /// <param name="getDataTypeId">班级/科目ID</param> 
        /// <param name="typeID"></param>
        /// <returns></returns>
        private string CreatePersonalHtml(int getDataTypeId, int typeID)
        {
            string strKey = "plh" + getDataTypeId;
            object obj = CacheHelper.GetCache(strKey);
            string strJson = string.Empty;
            if (obj == null)
            {
                strJson = typeID == 0 ? helper.HttpGet(ApiUrl, string.Format("class_id={0}&&type=1", getDataTypeId)) : helper.HttpGet(ApiUrl, string.Format("class_subject_id={0}&&type=1", getDataTypeId));

                if (strJson.Length > 0)
                {
                    CacheHelper.SetCache(strKey, strJson, DateTime.Now.AddSeconds(30), TimeSpan.Zero);
                }
            }
            else
            {
                strJson = obj.ToString();
            }

            return CreateHtml(strJson);
        }

        /// <summary>
        /// 获取小组排行榜详细信息
        /// </summary>
        /// <param name="getDataTypeId">班级/科目ID</param> 
        /// <param name="typeID"></param>
        /// <returns></returns>
        private string CreateGroupHtml(int getDataTypeId, int typeID)
        {
            string strKey = "gph" + getDataTypeId;
            object obj = CacheHelper.GetCache(strKey);
            string strJson = string.Empty;
            if (obj == null)
            {
                strJson = typeID == 0 ? helper.HttpGet(ApiUrl, string.Format("class_id={0}&&type=2", getDataTypeId)) : helper.HttpGet(ApiUrl, string.Format("class_subject_id={0}&&type=2", getDataTypeId));

                if (strJson.Length > 0)
                {
                    CacheHelper.SetCache(strKey, strJson, DateTime.Now.AddSeconds(30), TimeSpan.Zero);
                }
            }
            else
            {
                strJson = obj.ToString();
            }

            return CreateHtml(strJson);
        }

        /// <summary>
        /// 生成页面展示信息
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        private string CreateHtml(string strJson)
        {
            string strHtml = string.Empty;
            try
            {
                JObject jobj = JObject.Parse(strJson);

                if (jobj["error_code"].ToString().Equals("0"))
                {
                    JArray objArray = jobj["data"] as JArray;

                    if (objArray != null && objArray.Count > 0)
                    {
                        StringBuilder strbld = new StringBuilder();
                        strbld.Append("<table class=\"rightBottomTable\">");

                        for (int i = 0; i < objArray.Count; i++)
                        {
                            JObject studentObj = objArray[i]["student"] as JObject;
                            string stuName = studentObj["name_class"].ToString().Trim();
                            string stuImage = studentObj["icon_class"].ToString().Trim();

                            JObject userObj = objArray[i]["user"] as JObject;
                            string teacherName = userObj["username"].ToString().Trim();

                            string behaviorStr = objArray[i]["class_behavior_name"].ToString().Trim();
                            string actionStr = objArray[i]["behavior_type"].ToString().Trim(); //1：获得，2：扣除
                            string scoreStr = objArray[i]["score"].ToString().Trim();
                            string resultScore = string.Format("扣除{0}分", scoreStr);

                            if (actionStr == "1")
                            {
                                resultScore = string.Format("获得{0}分", scoreStr);
                            }

                            strbld.Append("<tr>");
                            strbld.Append("<td rowspan=\"2\" class=\"image\">");
                            strbld.AppendFormat("<img src=\"{0}\" style='width:55px;height:50px;'/></td>", ImgIsExists(stuImage));
                            strbld.AppendFormat("<td colspan=\"2\" class=\"recordTitle\">{0}，{1}，{2}</td>", stuName, behaviorStr, resultScore);
                            strbld.Append("</tr>");

                            strbld.Append("<tr>");
                            strbld.AppendFormat("<td class=\"recordTime\">{0}记</td>", teacherName);
                            strbld.AppendFormat("<td class=\"recordTime right\">{0}</td>", objArray[i]["create_at"].ToString());
                            strbld.Append("</tr>");
                            strbld.Append("<tr>");

                            strbld.Append("<td class=\"height\"></td>");
                            strbld.Append("<td colspan=\"2\" class=\"height empty\"></td>");
                            strbld.Append("</tr>");
                        }

                        strbld.Append("</table>");
                        strHtml = strbld.ToString();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }

            return strHtml;
        }

        /// <summary>
        /// 判断当前图片是否存在
        /// </summary>
        /// <param name="imgUrl">图片地址</param>
        /// <returns></returns>
        private string ImgIsExists(string imgUrl)
        {
            if (HttpHelper.IsImageExists(ConfigurationManager.AppSettings["url"].Trim() + imgUrl))
            {
                return ConfigurationManager.AppSettings["url"].Trim() + imgUrl;
            }
            return "Content/Images/person.png";
        }

        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}