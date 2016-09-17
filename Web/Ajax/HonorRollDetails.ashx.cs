﻿using Newtonsoft.Json.Linq;
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
            get { return string.Format("{0}/v2/honor/detail/format/json", ConfigurationManager.AppSettings["url"].Trim()); }
        }

        HttpHelper helper = new HttpHelper();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string strResult = string.Empty;

            //获取行为列表
            if (context.Request.QueryString["op"].ToString().Trim() == "list")
            {
                string strType = context.Request.Params["action"].Trim();//排行类型
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
            }
            //获取是否有新行为
            else if (context.Request.QueryString["op"].ToString().Trim() == "getMax")
            {
                var classOrSubjectID = context.Request.Params["classOrSubjectID"].ToString().Trim();
                var classType = context.Request.Params["classType"].ToString().Trim();
                var maxID = context.Request.Params["maxID"].ToString().Trim();
                var type = context.Request.Params["type"].ToString().Trim();

                string url = string.Format("{0}?class_id={1}&class_report_id={2}&type={3}", ApiUrl, classOrSubjectID, maxID, type);

                if (classType == "1") {
                    url = string.Format("{0}?class_subject_id={1}&class_report_id={2}&type={3}", ApiUrl, classOrSubjectID, maxID, type);
                }

                string jsonStr = helper.HttpGet(url, string.Empty);

                JObject jobj = JObject.Parse(jsonStr);
                int retrunMaxID = 0;

                StringBuilder strbld = new StringBuilder("<div style=\"display:none;\">");

                if (jobj != null && jobj["error_code"].ToString().Equals("0"))
                {
                    JArray objArray = jobj["data"] as JArray;

                    if (objArray != null && objArray.Count > 0)
                    {
                        JObject maxObject = objArray[0] as JObject;
                        retrunMaxID = Convert.ToInt32(maxObject["class_report_id"].ToString().Trim());

                        for (int i = 0; i < objArray.Count; i++)
                        {
                            string actionStr = objArray[i]["behavior_type"].ToString().Trim(); //1：积极行为，2：消极行为

                            if (actionStr == "1")
                            {
                                strbld.Append("<embed src=\"Wav/active.wav\"/>");
                            }
                            else {
                                strbld.Append("<embed src=\"Wav/inactive.wav\"/>");
                            }
                        }
                    }
                }

                strbld.Append("</div>");

                strResult = strbld.ToString() + "," + retrunMaxID.ToString();
            }

            context.Response.Write(strResult);
        }

        #region 获取班级/科目排行榜详细信息

        /// <summary>
        /// 获取班级/科目排行榜详细信息
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
                    CacheHelper.SetCache(strKey, strJson, DateTime.Now.AddSeconds(43200), TimeSpan.Zero);
                }
            }
            else
            {
                strJson = obj.ToString();
            }

            return CreateHtml(strJson);
        }

        #endregion

        #region 获取个人排行榜详细信息

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
                    CacheHelper.SetCache(strKey, strJson, DateTime.Now.AddSeconds(43200), TimeSpan.Zero);
                }
            }
            else
            {
                strJson = obj.ToString();
            }

            return CreateHtml(strJson);
        }

        #endregion

        #region 获取小组排行榜详细信息

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
                    CacheHelper.SetCache(strKey, strJson, DateTime.Now.AddSeconds(43200), TimeSpan.Zero);
                }
            }
            else
            {
                strJson = obj.ToString();
            }

            return CreateHtml(strJson);
        }

        #endregion

        #region 生成页面展示信息

        /// <summary>
        /// 生成页面展示信息
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        private string CreateHtml(string strJson)
        {
            string strHtml = string.Empty;
            int maxID = 0;

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

                        JObject maxObject = objArray[0] as JObject;
                        maxID = Convert.ToInt32(maxObject["class_report_id"].ToString().Trim());
                        
                        for (int i = 0; i < objArray.Count; i++)
                        {
                            string stuName = "--";
                            string stuImage = string.Empty;
                            
                            JObject studentObj = objArray[i]["student"] as JObject;

                            if (studentObj["name_class"] != null)
                            {
                                stuName = studentObj["name_class"].ToString().Trim();
                            }

                            if (studentObj["icon_class"] != null)
                            {
                                stuImage = studentObj["icon_class"].ToString().Trim();
                            }
                            
                            JObject userObj = objArray[i]["user"] as JObject;

                            string teacherName = "--";
                            
                            if (userObj["username"] != null) {
                                teacherName = userObj["username"].ToString().Trim();
                            }

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
            catch (Exception ex)
            {
                return string.Empty;
            }

            return strHtml + "," + maxID.ToString();
        }

        #endregion

        #region 判断当前图片是否存在

        /// <summary>
        /// 判断当前图片是否存在
        /// </summary>
        /// <param name="imgUrl">图片地址</param>
        /// <returns></returns>
        private string ImgIsExists(string imgUrl)
        {
            if (imgUrl != string.Empty && HttpHelper.IsImageExists(ConfigurationManager.AppSettings["url"].Trim() + imgUrl))
            {
                return ConfigurationManager.AppSettings["url"].Trim() + imgUrl;
            }

            return "Content/Images/person.png";
        }

        #endregion

        public bool IsReusable
        {
            get { return false; }
        }
    }
}