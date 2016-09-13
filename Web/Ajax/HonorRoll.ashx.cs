using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Text;
using System.Web;
using Web.Utils;

namespace Web.Ajax
{
    /// <summary>
    /// HonorRoll 的摘要说明
    /// </summary>
    public class HonorRoll : IHttpHandler
    {
        HttpHelper helper = new HttpHelper();

        public string Url
        {
            get
            {
                string strUrl = "http://test.keji110.com";

                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["url"].Trim()))
                {
                    strUrl = ConfigurationManager.AppSettings["url"].Trim();
                }

                return strUrl;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int num = 0;
            string strType = string.Empty;
            int.TryParse(context.Request.Params["num"], out num);
            strType = context.Request.Params["type"].Trim();

            int classID = 0;
            int.TryParse(context.Request.Params["cId"], out classID);
            string strResult = string.Empty;

            int objType = 0;
            int rollType = 0;

            int.TryParse(context.Request.Params["objType"], out objType);
            int.TryParse(context.Request.Params["rollType"], out rollType);

            try
            {
                GetClassRollData(classID, objType);

                if (strType.Length > 0)
                {
                    if (strType.ToLower().Equals("team"))
                    {
                        strResult = CreateTeamHtml(num, classID, objType);
                    }
                    else
                    {
                        strResult = CreateStudentHtml(num, classID, objType);
                    }
                }
            }
            catch
            {
                strResult = string.Empty;
            }

            context.Response.Write(strResult);
        }

        #region  生成字符串

        /// <summary>
        /// 生成当前班级下学生排名字符串
        /// </summary>
        /// <param name="num">显示数量</param>
        /// <param name="classId">班级ID</param>
        /// <returns></returns>
        private string CreateStudentHtml(int num, int classId, int itemType)
        {
            StringBuilder strbld = new StringBuilder();
            string strKey = string.Format("{1}str{0}", classId, itemType);
            object obj = CacheHelper.GetCache(strKey);

            if (obj != null)
            {
                JObject jObj = JObject.Parse(obj.ToString());

                if (jObj["error_code"].ToString().Trim().Equals("0"))
                {
                    JArray studentAarray = jObj["data"]["personal"] as JArray;

                    if (studentAarray != null && studentAarray.Count > 0)
                    {
                        int studentCount = studentAarray.Count > (num + 10) ? (num + 10) : studentAarray.Count;

                        #region

                        StringBuilder stgbld = new StringBuilder();

                        for (int j = 0; j < studentCount; j++)
                        {
                            #region 前三名

                            if (j < 3)
                            {
                                if (j == 0)
                                {
                                    strbld.Append("<div class=\"topThree\">");
                                }

                                switch (j)
                                {
                                    case 0:
                                        stgbld.Append("<div class=\"top1\">");
                                        stgbld.AppendFormat("<div class=\"topScore\">{0}</div>", studentAarray[0]["score"].ToString());
                                        stgbld.Append("<div class=\"topImage\">");
                                        stgbld.AppendFormat("<img style=\"width:64px; height:66px;margin: 2px 0px 0 12px\" src='{0}'/>", ImgIsExists(studentAarray[0]["student"]["icon_class"].ToString()));
                                        stgbld.Append("</div>");
                                        stgbld.Append("<div class=\"topSort1\"></div>");
                                        stgbld.AppendFormat("<div class=\"topName\">{0}</div>", studentAarray[0]["student"]["name_class"].ToString());
                                        stgbld.Append("</div>");
                                        break;
                                    case 1:
                                        strbld.Append("<div class=\"top2\">");
                                        strbld.AppendFormat("<div class=\"topScore\">{0}</div>", studentAarray[1]["score"].ToString());
                                        strbld.Append("<div class=\"topImage\">");
                                        strbld.AppendFormat("<img style=\"width:64px; height:66px;margin: 2px 0px 0 12px\" src='{0}'/>", ImgIsExists(studentAarray[1]["student"]["icon_class"].ToString()));
                                        strbld.Append("</div>");
                                        strbld.Append("<div class=\"topSort2\"></div>");
                                        strbld.AppendFormat("<div class=\"topName\">{0}</div>", studentAarray[1]["student"]["name_class"].ToString());
                                        strbld.Append("</div>");
                                        strbld.Append(stgbld.ToString());
                                        break;
                                    case 2:
                                        strbld.Append("<div class=\"top3\">");
                                        strbld.AppendFormat("<div class=\"topScore\">{0}</div>", studentAarray[2]["score"].ToString());
                                        strbld.Append("<div class=\"topImage\">");
                                        strbld.AppendFormat("<img style=\"width:64px; height:66px;margin: 2px 0px 0 12px\" src='{0}'/>", ImgIsExists(studentAarray[2]["student"]["icon_class"].ToString()));
                                        strbld.Append("</div>");
                                        strbld.Append("<div class=\"topSort3\"></div>");
                                        strbld.AppendFormat("<div class=\"topName\">{0}</div>", studentAarray[2]["student"]["name_class"].ToString());
                                        strbld.Append("</div>");
                                        break;
                                }
                            }

                            #endregion

                            #region 其他名次

                            else
                            {
                                if (j == 3)
                                {
                                    strbld.Append("</div><div class=\"afterThree\">");
                                }

                                string tempStuName = studentAarray[j]["student"]["name_class"].ToString();
                                string tempIcon = studentAarray[j]["student"]["icon_class"].ToString();

                                if (!(tempStuName == "/" || tempIcon == "/"))
                                {
                                    strbld.Append("<div class=\"top4\">");
                                    strbld.AppendFormat("<div class=\"top4Score\">{0}</div>", studentAarray[j]["score"].ToString());
                                    strbld.Append("<div class=\"top4Image\">");
                                    strbld.AppendFormat("<img style=\"width:64px; height:66px; margin: 0px 7px 0 6px\" src='{0}'/>", ImgIsExists(tempIcon));
                                    strbld.Append("</div>");
                                    strbld.AppendFormat("<div class=\"top4Sort\">{0}</div>", j + 1);
                                    strbld.AppendFormat("<div class=\"top4Name\">{0}</div>", tempStuName);
                                    strbld.Append("</div>");
                                }
                            }

                            #endregion

                            if (j == studentCount - 1)
                            {
                                strbld.Append("</div>");
                            }
                        }

                        #endregion
                    }
                }

                return strbld.ToString();
            }
            return string.Empty;

        }

        /// <summary>
        /// 生成班级下各小组排行字符串
        /// </summary>
        /// <param name="num">显示的数量</param>
        /// <param name="classId">班级ID</param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private string CreateTeamHtml(int num, int classId, int itemType)
        {
            StringBuilder strbldTeam = new StringBuilder();
            string strKey = string.Format("{1}str{0}", classId, itemType);
            string strTeamHtl = string.Empty;

            //只取科目下的分组排行榜
            if (itemType == 1)
            {
                object obj = CacheHelper.GetCache(strKey);

                if (obj != null)
                {
                    JObject jObj = JObject.Parse(obj.ToString());

                    if (jObj["error_code"].ToString().Trim().Equals("0"))
                    {
                        #region

                        JArray teamAarray = jObj["data"]["group"] as JArray;

                        if (teamAarray != null && teamAarray.Count > 0)
                        {
                            int teamCount = teamAarray.Count > (num + 3) ? (num + 3) : teamAarray.Count;

                            for (int i = 0; i < teamCount; i++)
                            {
                                #region 获取组内成员姓名

                                JArray membersArr = teamAarray[i]["members"] as JArray;
                                string stuNameStr = string.Empty;

                                //判断组内是否有学生
                                if (membersArr != null)
                                {
                                    int memberCount = membersArr.Count;
                                    int memberLen = memberCount >= 3 ? 3 : memberCount;

                                    //只取三个学生
                                    for (var k = 0; k < memberLen; k++)
                                    {
                                        stuNameStr = stuNameStr + membersArr[k]["student_name"].ToString() + ",";
                                    }

                                    stuNameStr = stuNameStr.TrimEnd(',');
                                    stuNameStr = TextHelper.CutString(stuNameStr, 18, string.Empty);
                                }

                                #endregion

                                #region 前三名

                                if (i < 3)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            strbldTeam.Append("<div id=\"groupLeft\"><div class=\"groupOne\"><div class=\"sortOne\"></div>");
                                            strbldTeam.Append("<div class=\"people\">");
                                            strbldTeam.AppendFormat("<div class=\"peopleTitle\">{0}</div>", teamAarray[0]["class_group_name"].ToString().Trim());
                                            strbldTeam.AppendFormat("<div class=\"peopleName\">{0}</div>", stuNameStr);
                                            strbldTeam.Append(string.Format("<div class=\"peopleImage\"><img style=\"width: 100%; height:100%;\" src='{0}'/></div>", ImgIsExists(teamAarray[0]["class_group_icon"].ToString().Trim())));
                                            strbldTeam.AppendFormat("<div class=\"peopleScore\">{0}</div>", teamAarray[0]["score"].ToString().Trim());
                                            strbldTeam.Append("</div></div>");
                                            break;
                                        case 1:
                                            strbldTeam.Append("<div class=\"groupTwo\"><div class=\"sortTwo\"></div>");
                                            strbldTeam.Append("<div class=\"people\">");
                                            strbldTeam.AppendFormat("<div class=\"peopleTitle\">{0}</div>", teamAarray[1]["class_group_name"].ToString().Trim());
                                            strbldTeam.AppendFormat("<div class=\"peopleName\">{0}</div>", stuNameStr);
                                            strbldTeam.Append(string.Format("<div class=\"peopleImage\"><img style=\"width: 100%; height:100%;\" src='{0}'/></div>", ImgIsExists(teamAarray[1]["class_group_icon"].ToString().Trim())));
                                            strbldTeam.AppendFormat("<div class=\"peopleScore\">{0}</div>", teamAarray[1]["score"].ToString().Trim());
                                            strbldTeam.Append("</div></div>");
                                            break;
                                        case 2:
                                            strbldTeam.Append("<div class=\"groupThree\"><div class=\"sortThree\"></div>");
                                            strbldTeam.Append("<div class=\"people\">");
                                            strbldTeam.AppendFormat("<div class=\"peopleTitle\">{0}</div>", teamAarray[2]["class_group_name"].ToString().Trim());
                                            strbldTeam.AppendFormat("<div class=\"peopleName\">{0}</div>", stuNameStr);
                                            strbldTeam.Append(string.Format("<div class=\"peopleImage\"><img style=\"width: 100%; height:100%;\" src='{0}'/></div>", ImgIsExists(teamAarray[2]["class_group_icon"].ToString().Trim())));
                                            strbldTeam.AppendFormat("<div class=\"peopleScore\">{0}</div>", teamAarray[2]["score"].ToString().Trim());
                                            strbldTeam.Append("</div></div>");
                                            strbldTeam.Append("</div>");
                                            break;
                                    }
                                }

                                #endregion

                                #region 其他排名

                                if (i > 2 && i < 6)
                                {
                                    if (i == 3)
                                    {
                                        strbldTeam.Append("<div id=\"groupMiddle\">");
                                    }
                                    if (i < 6)
                                    {
                                        strbldTeam.AppendFormat("<div class=\"otherPeople\"><div class=\"otherSort\">{0}</div>", i + 1);
                                        strbldTeam.AppendFormat("<div class=\"otherTitle\">{0}</div>", teamAarray[i]["class_group_name"].ToString().Trim());
                                        strbldTeam.AppendFormat("<div class=\"otherName\">{0}</div>", stuNameStr);
                                        strbldTeam.Append(string.Format("<div class=\"peopleImage2\"><img style=\"width: 100%; height:100%;\" src='{0}'/></div>", ImgIsExists(teamAarray[i]["class_group_icon"].ToString().Trim())));
                                        strbldTeam.AppendFormat("<div class=\"otherScore\">{0}</div></div>", teamAarray[i]["score"].ToString().Trim());
                                    }
                                    if (i == 5)
                                    {
                                        strbldTeam.Append("</div>");
                                    }
                                    if (i == teamCount - 1)
                                    {
                                        strbldTeam.Append("</div>");
                                    }
                                }
                                else if (i > 5 && i < 9)
                                {
                                    if (i == 6)
                                    {
                                        strbldTeam.Append("<div id=\"groupRight\">");
                                    }
                                    if (i < 9)
                                    {
                                        strbldTeam.AppendFormat("<div class=\"otherPeople\"><div class=\"otherSort\">{0}</div>", i + 1);
                                        strbldTeam.AppendFormat("<div class=\"otherTitle\">{0}</div>", teamAarray[i]["class_group_name"].ToString().Trim());
                                        strbldTeam.AppendFormat("<div class=\"otherName\">{0}</div>", stuNameStr);
                                        strbldTeam.Append(string.Format("<div class=\"peopleImage2\"><img style=\"width: 100%; height:100%;\" src='{0}'/></div>", ImgIsExists(teamAarray[i]["class_group_icon"].ToString().Trim())));
                                        strbldTeam.AppendFormat("<div class=\"otherScore\">{0}</div></div>", teamAarray[i]["score"].ToString().Trim());
                                    }
                                    if (i == teamCount - 1) { strbldTeam.Append("</div>"); }
                                }

                                #endregion
                            }
                            strTeamHtl = strbldTeam.ToString();
                        }

                        #endregion
                    }
                }
            }

            return strTeamHtl;
        }

        /// <summary>
        /// 获取班级排行榜信息
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="itemType">0:班级,1:具体科目</param>
        private void GetClassRollData(int classId, int itemType)
        {
            string strApiUrl = itemType == 0 ? string.Format("{0}/v2/honor/listClass/format/json", Url) : string.Format("{0}/v2/honor/list/format/json", Url);
            string strKey = string.Format("{1}str{0}", classId, itemType);
            object obj = CacheHelper.GetCache(strKey);
            string strJson = string.Empty;

            if (obj == null)
            {
                strJson = itemType == 0 ? helper.HttpGet(strApiUrl, "class_id=" + classId + "") : helper.HttpGet(strApiUrl, "class_subject_id=" + classId + "");

                if (strJson != null && strJson.Length > 0)
                {
                    CacheHelper.SetCache(strKey, strJson, DateTime.Now.AddSeconds(43200), TimeSpan.Zero);
                }
            }
        }

        /// <summary>
        /// 判断当前图片是否存在
        /// </summary>
        /// <param name="imgUrl">图片地址</param>
        /// <returns></returns>
        private string ImgIsExists(string imgUrl)
        {
            if (HttpHelper.IsImageExists(Url + imgUrl))
            {
                return Url + imgUrl;
            }
            return "Content/Images/topPerson.png";
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