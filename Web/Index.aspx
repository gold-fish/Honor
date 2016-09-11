<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Web.Index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>光荣榜</title>
    <link href="Content/Css/index.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/HonorRoll.js"></script>
    <script type="text/javascript" src="Scripts/HonorRollDetails.js"></script>

    <script type="text/javascript">
        $(function () {
            ShowData();
        });

        function ShowData() {
            var classOrSubjectID = $("#hidClassId").val();
            var classType = $("#hidType").val();
            var detailType = $("#hidDetailType").val();

            loadHonorRollData();

            //显示右侧扣分榜
            GetRollDeatailData(classOrSubjectID, classType, detailType);
            $("#rghm").removeClass("rightpPrsonal").removeClass("rightGroup").addClass("rightMiddle");

            setTimeout("ShowData()", 30000);
        }

        function SelectSubject() {
            var mySelect = document.getElementById("mySubject");
            var id = mySelect.value;
            var type = 0;

            if (mySelect.selectedIndex == 0) {
                type = 0;
                $("#hidType").val(0);
            }
            else {
                type = 1;
                $("#hidType").val(1);
            }

            $("#hidClassId").val(id);
            loadHonorRollData();

            //显示右侧扣分榜
            GetRollDeatailData(id, type, "class");
            $("#rghm").removeClass("rightpPrsonal").removeClass("rightGroup").addClass("rightMiddle");
        }

        function SelectDate(e, type) {
            $(".topSelectTitle").removeClass("topSelectTitle").addClass("topUnselect");
            $(".topSelectBg").removeClass("topSelectBg").addClass("unTopSelectBg");

            $(e).find(".topUnselect").removeClass("topUnselect").addClass("topSelectTitle");
            $(e).find(".unTopSelectBg").removeClass("unTopSelectBg").addClass("topSelectBg");

            //日
            if (type == "0") {

            }
                //周
            else if (type == "1") {

            }
            else {

            }
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hidClassId" runat="server" Value="" />
        <asp:HiddenField ID="hidType" runat="server" Value="" />
        <asp:HiddenField ID="hidDetailType" runat="server" Value="class" />
        <div class="main">
            <div class="header">
                <div id="subjectList" style="height: 30px;padding-top:15px;font-size:14px; width: 400px; margin-left:142px;" runat="server"></div>
            </div>
            <div class="content">
                <div class="leftContent">
                    <div class="student">
                        <div class="roll"></div>
                        <div class="setting">显示设置</div>
                        <div class="rollBar">
                            <div class="rollBarTitle">班级光荣榜</div>
                        </div>
                        <div class="topType">
                            <div class="date" onclick="SelectDate(this,0)">
                                <div class="topSelectTitle">日排行</div>
                                <div class="topSelectBg"></div>
                            </div>
                            <div class="date" onclick="SelectDate(this,1)">
                                <div class="topUnselect">周排行</div>
                                <div class="unTopSelectBg"></div>
                            </div>
                            <div class="date" onclick="SelectDate(this,2)">
                                <div class="topUnselect">月排行</div>
                                <div class="unTopSelectBg"></div>
                            </div>
                        </div>
                        <div id="studentHonorRoll"></div>
                    </div>
                    <div class="group">
                        <div class="groupContent">
                            <div id="teamRoll"></div>
                        </div>
                    </div>
                </div>
                <div class="rightContent">
                    <div class="rightTop">
                        <div class="rightIcon"></div>
                        <div class="rightTitle">全班成员</div>
                    </div>
                    <div class="rightMiddle" id="rghm">
                        <div class="one" title="<%=strClassName%>"><span id="class"><asp:Label ID="lblClassName" runat="server" Text=""></asp:Label></span></div>
                        <div class="two"><span id="person">个人</span></div>
                        <div class="three"><span id="group">小组</span></div>
                    </div>
                    <div class="rightBottom">
                        <div id="rightRoll" style="max-height:754px;overflow-y:auto;margin-top:20px;"></div>
                    </div>
                </div>
            </div>
            <div class="footer">
                <div class="setIcon">显示控制</div>
                <div class="slide" id="barStudent">
                    <div class="bgSlide" id="student"></div>
                    <div id="btnStudent"></div>
                </div>
                <div class="slide" id="barTeam">
                    <div class="bgSlide" id="team"></div>
                    <div id="btnTeam"></div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
