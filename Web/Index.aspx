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
            ShowDiffStyle();

            $("#rghm").removeClass("rightpPrsonal").removeClass("rightGroup").addClass("rightMiddle");
        });

        function ShowData() {
            var classOrSubjectID = $("#hidClassId").val();
            var classType = $("#hidType").val();
            var detailType = $("#hidDetailType").val();
            var maxID = $("#hidMaxID").val();
            var type = 3;//3表示小组和个人的行为同时返回
           
            if (!(classOrSubjectID == "" || classType == "")) {
                //防止url缓存
                var ms = Math.random();

                //定时去获取有没有最新的扣、加分行为
                $.post("Ajax/HonorRollDetails.ashx?op=getMax&ms=" + ms, { classOrSubjectID: classOrSubjectID, classType: classType, maxID: maxID, type: type }, function (data) {
                    var arr = data.split(',');
                    var effect = arr[0];
                    var stuName = arr[1];
                    var maxID = arr[2];

                    var prevMaxID = parseInt($("#hidMaxID").val());

                    if (maxID != "0" && parseInt(maxID) > prevMaxID) {
                        $("#hidMaxID").val(maxID);
                        
                        var content = "<div>" + stuName + "</div>";

                        //积极行为
                        if (effect == "1") {
                            $("#active").html(content);

                            $("#active").fadeIn(1000);
                            $("#active").fadeOut(3000);
                        }
                        else {
                            $("#inactive").html(content);

                            $("#inactive").fadeIn(1000);
                            $("#inactive").fadeOut(3000);
                        }

                        loadHonorRollData();

                        //显示右侧扣分榜
                        GetRollDeatailData(classOrSubjectID, classType, detailType);
                    }
                });
            }

            setTimeout("ShowData()", 1000);
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

        function returnLogin() {
            $.post("Index.aspx?op=return", function (data) {
                window.location.href = "Login.aspx";
            });
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
            <div class="leftMain">
                 <div class="header">
                    <div class="cup"></div>
                    <div class="cupLogo" onclick="ReturnHome()">校朋·光荣榜</div>
                    <div class="cupList" id="subjectList" runat="server"></div>
                    <div class="cupExit"><div onclick="returnLogin()">退出<br />光荣榜</div></div>
                </div>
                 <div class="content" id="content">
                    <div class="leftContent">
                        <div class="student">
                            <div class="roll"></div>
                            <div class="rollBar">
                                <div class="rollBarTitle">班级光荣榜</div>
                            </div>
                            <div class="topType">
                                <img src="Content/Images/classLogo.png" style="margin-left:350px;"/>
                                <div class="rollSetting">
                                    <div class="setTitle">显示控制</div>
                                    <div class="setLeft"></div>
                                    <div class="setSlide" id="barStudent" style="display:none;">
                                        <div class="setSlideLine" id="student"></div>
                                        <div id="btnStudent"></div>
                                    </div>
                                    <div class="setRight"></div>
                                </div>
                            </div>
                            <div id="studentHonorRoll"></div>
                        </div>
                        <div class="group" id="groupDiv">
                            <div class="groupLogo">
                                <img src="Content/Images/groupLogo.png" style="margin-left:350px;"/>
                            </div>
                            <div class="groupContent">
                                <div id="teamRoll"></div>
                            </div>
                        </div>
                    </div>              
                 </div>
            </div>
           
            <div class="rightMain">
                 <div class="rightContent">
                    <div class="rightTop">
                        <div class="rightIcon"><img src="Content/Images/class.png" style="width:68px;height:68px;"/></div>
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
           
            <div class="footer" style="display:none;">
                <div class="setIcon">显示控制</div>
                
                 <div class="slide" id="barTeam"> 
                     <div class="bgSlide" id="team"></div> 
                     <div id="btnTeam"></div> 
                 </div> 
            </div>  
            
            <div id="active"><div>李金云</div></div>
            <div id="inactive"><div>刘三乐</div></div>          
        </div>
    </form>
</body>
</html>
