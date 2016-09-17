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
            //ShowData();
            ShowDiffStyle();

            $("#rghm").removeClass("rightpPrsonal").removeClass("rightGroup").addClass("rightMiddle");
        });

        function ShowData() {
            var classOrSubjectID = $("#hidClassId").val();
            var classType = $("#hidType").val();
            var detailType = $("#hidDetailType").val();
            var maxID = $("#hidMaxID").val();
            var type = 3;//3表示小组和个人的行为同时返回

            alert(classOrSubjectID);
            alert(classType);
            //防止url缓存
            var ms = Math.random();

            //定时去获取有没有最新的扣、加分行为
            $.post("Ajax/HonorRollDetails.ashx?op=getMax&ms="+ms, { classOrSubjectID: classOrSubjectID, classType: classType, maxID: maxID, type: type }, function (data) {
                alert(data);
            });

            //loadHonorRollData();

            //显示右侧扣分榜
            //GetRollDeatailData(classOrSubjectID, classType, detailType);
            setTimeout("ShowData()", 3000);
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
            $("#hidDetailType").val("class");
            loadHonorRollData();

            //显示右侧扣分榜
            GetRollDeatailData(id, type, "class");
            $("#rghm").removeClass("rightpPrsonal").removeClass("rightGroup").addClass("rightMiddle");

            ShowDiffStyle();
        }

        //根据有无分组显示不同的样式
        function ShowDiffStyle()
        {
            var type = $("#hidType").val();

            if (type == 0) {
                $("#groupDiv").removeClass("group").addClass("nogroup");
                $("#content").removeClass("content").addClass("nocontent");
                $("#main").removeClass("main").addClass("nomain");
                $("#rightBottom").removeClass("rightBottom").addClass("norightBottom");
                $("#rightRoll").removeClass("rightRoll").addClass("norightRoll");
                $("#barTeam").removeClass("slide").addClass("noslide");
            }
            else {
                $("#groupDiv").removeClass("nogroup").addClass("group");
                $("#content").removeClass("nocontent").addClass("content");
                $("#main").removeClass("nomain").addClass("main");
                $("#rightBottom").removeClass("norightBottom").addClass("rightBottom");
                $("#rightRoll").removeClass("norightRoll").addClass("rightRoll");
                $("#barTeam").removeClass("noslide").addClass("slide");
            }
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

        function ReturnHome()
        {
            window.location.href = "Course.aspx";
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hidClassId" runat="server" Value="" />
        <asp:HiddenField ID="hidType" runat="server" Value="" />
        <asp:HiddenField ID="hidDetailType" runat="server" Value="class" />
        <input type="hidden" id="hidMaxID" value="0" />

        <div class="main" id="main">
            <div class="header">
                <div style="cursor:pointer;width:192px;height:31px;background-image:url('Content/Images/return.png'); position:absolute;top:7px;left:45px;" onclick="ReturnHome()"></div>
                <div id="subjectList" style="height: 30px;font-size:14px; width: 400px; position:absolute;left:320px;top:15px;" runat="server"></div>
            </div>
            <div class="content" id="content">
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
                    <div class="group" id="groupDiv">
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
                    <div class="rightBottom" id="rightBottom">
                        <div id="rightRoll" class="rightRoll"></div>
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
