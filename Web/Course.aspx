<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Course.aspx.cs" Inherits="Web.Course" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>班级页</title>
    <link href="Content/Css/login.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-1.8.2.min.js"></script>

    <style type="text/css">
        .template {
            position: absolute;
            top: 21px;
            left: 350px;
            width:57px;
            height:30px;
            line-height:30px;
        }
        .template a:link,.template a:visited{
            color:#666;
        }
        .popTitle {
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
            width: 100%;
            height: 53px;
            background-color: #fff;
            border-bottom: 1px solid #EAE8E7;
            position: relative;
        }

        .leftTitle {
            position: absolute;
            top: 18px;
            left: 20px;
            width: 290px;
            height: 20px;
            line-height: 20px;
            font-family: SourceHanSansCN-Heavy;
            font-size: 18px;
            color: rgba(0,0,0,0.58);
            text-align: left;
        }

            .leftTitle span {
                color: #00BCF2;
            }

        .importIcon,.importFatherIcon {
            position: absolute;
            top: 21px;
            left: 320px;
            width: 10px;
            height: 12px;
            background-image: url('Content/Images/addStudent.png');
        }

        .rightTitle,.rightFatherTitle {
            position: absolute;
            top: 19px;
            left: 340px;
            width: 125px;
            height: 20px;
            line-height: 20px;
            font-family: SourceHanSansCN-Bold;
            font-size: 14px;
            color: #4a90e2;
            text-align: left;
            cursor:pointer;
        }
        .importFatherIcon {
            top:22px;
            left: 480px;
        }
        .rightFatherTitle {
            top:20px;
            left:500px;
            width:70px;
        }

        .addContent {
            width: 100%;
            height: 153px;
            background-color: #fff;
            position: relative;
            border-bottom: 1px solid #EAE8E7;
        }

        .addTitle {
            position: absolute;
            width: 100px;
            height: 20px;
            line-height: 20px;
            top: 12px;
            left: 23px;
            font-family: SourceHanSansCN-Bold;
            font-size: 14px;
            color: rgba(0,0,0,0.58);
            text-align: left;
        }

        .userName {
            position: absolute;
            top: 45px;
            left: 20px;
            font-family: SourceHanSansCN-Bold;
            border: 1px solid #ccc;
            box-shadow: inset 0px 0px 3px 0px rgba(0,0,0,0.50);
            border-radius: 4px;
            width: 544px;
            height: 37px;
            line-height: 37px;
        }
        .enter {
            position: absolute;
            top: 105px;
            left: 490px;
            width: 60px;
            height: 22px;
            line-height:22px;
            font-size:11px;
            text-align:center;
            color:white;
            background-image:url('Content/Images/enter.png');
            cursor:pointer;
        }
        .tel {
            position: absolute;
            top: 96px;
            left: 20px;
            font-family: SourceHanSansCN-Bold;
            border: 1px solid #ccc;
            box-shadow: inset 0px 0px 3px 0px rgba(0,0,0,0.50);
            border-radius: 4px;
            width: 544px;
            height: 37px;
            line-height: 37px;
        }

            .userName input, .tel input {
                border-width: 0;
                margin:2px 0;
                width: 99.8%;
                height: 32px;
                text-indent: 0.8em;
                font-size: 12px;
                color: rgba(0,0,0,0.58);
            }

        .addList {
            width: 100%;
            height: 260px;
        }
        .contentBg {
            background-color: white;
            width: 100%;
            overflow-y:auto;
            max-height:258px;
        }
        .space{
            width: 100%;
            height: 20px;
            clear:both;
        }
        .studentTotal {
            margin: 0 auto;
            width: 95%;
            height: 38px;
            line-height: 38px;
            font-family:SourceHanSansCN-Normal;
            font-size:12px;
            color:rgba(0,0,0,0.26);
            border-bottom: 1.5px solid #EAE8E7;
        }
            .studentTotal span {
                color:#00BCF2;
            }
        .studentDetail {
            margin: 0 auto;
            width: 96%;
            height: 60px;
            border-bottom: 1px solid #f5f5f5;
        }
        .detailImage {
            float:left;
            height:50px;
            margin-top:3px;
            width:60px;
        }
        .detailName {
            float:left;
            height:60px;
            line-height:60px;
            width:200px;
            margin-left:20px;
            font-family:SourceHanSansCN-Normal;
            font-size:18px;
            color:rgba(0,0,0,0.58);
        }

        .closeBtn {
            width: 100%;
            height: 60px;
            position: relative;
            border-bottom-left-radius: 8px;
            border-bottom-right-radius: 8px;
            background-color: white;
        }

        .returnBtn {
            position: absolute;
            top: 21px;
            left: 343px;
            font-family: PingFangSC-Medium;
            font-size: 14px;
            color: #bd10e0;
            text-align: left;
            width: 28px;
            height: 16px;
            line-height: 16px;
            cursor: pointer;
        }

        .saveBtn {
            position: absolute;
            top: 11px;
            left: 408px;
            font-family: SourceHanSansCN-Bold;
            font-size: 14px;
            color: #ffffff;
            text-align: center;
            background-image: url('Content/Images/addStudentBtn.png');
            width: 157px;
            height: 38px;
            line-height: 38px;
            cursor: pointer;           
        }

        #popExcel {
            background-color:#eee;
            box-shadow: 0px 0px 4px 0px rgba(0,0,0,0.12), 0px 4px 4px 0px rgba(0,0,0,0.24);
            border-radius: 8px;
            width: 410px;
            height: 360px;
            z-index: 100;
            left: 50%; /*FF IE7*/
            top: 50%; /*FF IE7*/
            margin-left: -205px !important; /*FF IE7 该值为本身宽的一半 */
            margin-top: -180px !important; /*FF IE7 该值为本身高的一半*/
            margin-top: 0px;
            position: fixed !important; /*FF IE7*/
            position: absolute; /*IE6*/
            _top: expression(eval(document.compatMode && document.compatMode=='CSS1Compat') ? documentElement.scrollTop + (document.documentElement.clientHeight-this.offsetHeight)/2 : /*IE6*/
            document.body.scrollTop + (document.body.clientHeight - this.clientHeight)/2);
        }
        .excelBg{
            width:100%;
            height:100%;
            border-radius: 8px;
            position:relative;
            color: rgba(0,0,0,0.58);
        }
        .excelImport {
            width:80%;
            height:30px;
            line-height:30px;
            position:absolute;
            left:20px;
            top:20px;
            color: rgba(0,0,0,0.58);
        }
        #txtCopyContent {
            height:20px;
            width:220px;
            padding-top:5px;
            border:0.8px solid #eee;
        }
        .addedStudent{
            width:100%;
            height:243px;
            position:absolute;
            left:0;
            top:65px;
            background:white;
            overflow-y:auto;
            max-height:243px;
        }
        .addedStudent input{
            width:330px;
            height:25px;
            line-height:25px;
            margin-top:9px;
            margin-left:25px;
            border:1px solid #eee;
            text-indent:10px;
            color: rgba(0,0,0,0.58);
        }

        .excelReturn,.excelSave{
            width:60px;
            height:30px;
            line-height:30px;
            border-radius: 8px;
            position:absolute;
            left:130px;
            top:320px;
            text-align:center;
            color: rgba(0,0,0,0.58);
            cursor:pointer;
            background:#ddd;
        }
        .excelSave{         
            left:230px;
        }

        #popDiv {
            background: #f5f5f5;
            box-shadow: 0px 0px 4px 0px rgba(0,0,0,0.12), 0px 4px 4px 0px rgba(0,0,0,0.24);
            border-radius: 8px;
            width: 585px;
            height: 526px;
            z-index: 99;
            left: 50%; /*FF IE7*/
            top: 50%; /*FF IE7*/
            margin-left: -292px !important; /*FF IE7 该值为本身宽的一半 */
            margin-top: -230px !important; /*FF IE7 该值为本身高的一半*/
            margin-top: 0px;
            position: fixed !important; /*FF IE7*/
            position: absolute; /*IE6*/
            _top: expression(eval(document.compatMode && document.compatMode=='CSS1Compat') ? documentElement.scrollTop + (document.documentElement.clientHeight-this.offsetHeight)/2 : /*IE6*/
            document.body.scrollTop + (document.body.clientHeight - this.clientHeight)/2);
        }

        #backgroundDiv {
            background-color: #ccc;
            width: 100%;
            height: 100%;
            left: 0;
            top: 0;
            background-color: rgba(0, 0, 0, 0.2);
            z-index: 1;
            position: fixed !important; /*FF IE7*/
            position: absolute; /*IE6*/
            _top: expression(eval(document.compatMode && document.compatMode=='CSS1Compat') ? documentElement.scrollTop + (document.documentElement.clientHeight-this.offsetHeight)/2 : /*IE6*/
            document.body.scrollTop + (document.body.clientHeight - this.clientHeight)/2); /*IE5 IE5.5*/
        }
    </style>

    <script type="text/javascript">
        function showRoll(uid, cid, sid) {
            if (sid == 0) {
                window.location.href = "Index.aspx?UID=" + uid + "&CID=" + cid;
            }
            else {
                window.location.href = "Index.aspx?UID=" + uid + "&CID=" + cid + "&SID=" + sid;
            }
        }

        function popExcel(type)
        {
            $("#action").val(type);
            $("#popExcel").fadeIn(600);
        }

        function closeExcel()
        {
            $("#addedStudent").html("");
            $("#popExcel").fadeOut(600);
        }

        function closeExcelAndGet() {
            var action = $("#action").val();
            var stuList = $("#hideStudent").val();

            if (stuList == "") {
                alert("请从Excel中粘贴学生列表！");
            }
            else {
                var create_by = "<%=userID%>";
                var user_name = "<%=teacherName%>";
                var class_id = $("#hideClassID").val();
                var class_name = $("#hideClassName").val();
                var url = "<%=postUrl%>" + "/v2/student/createBatchForPc/format/json";

                //判断是添加学生还是邀请家长
                if (action == "student") {
                    $.post(url, { class_id: class_id, create_by: create_by, class_name: class_name, user_name: user_name, students: stuList }, function (data) {
                        $("#hideStudent").val("");
                        $("#addedStudent").html("");
                        $("#popExcel").fadeOut(600);

                        $.post("Ajax/StudentList.ashx?op=list", { classID: class_id }, function (myData) {
                            $("#studentList").html(myData);
                        });
                    });
                }
                else {

                }
            }
        }

        function popDiv(classID, name) {
            $("#hideClassID").val(classID);
            $("#hideClassName").val(name);
            $("#className").html(name);

            $("#popDiv").fadeIn(600);
            $("#backgroundDiv").show();

            $.post("Ajax/StudentList.ashx?op=list", { classID: classID }, function (data) {
                $("#studentList").html(data);
            });
        }

        function closeDivAndGet()
        {
            $("#txtUsername").val("");
            $("#txtTel").val("");
            $("#addedStudent").html("");
            $("#popExcel").hide();
            $("#popDiv").fadeOut(600);
            $("#backgroundDiv").hide();
            var userID = "<%=userID%>";

            $.post("Ajax/CourseList.ashx", { userID: userID }, function (data) {
                $("#classList").html(data);
            });
        }

        function InputCheck(type) {
            if (type == "name") {
                var username = $("#txtUsername").val();

                if (username == "") {
                    $("#username").css("border-color", "#cccccc");
                    $("#enter").css("background-image", "url('Content/Images/enter.png')");
                }
                else {
                    $("#username").css("border-color", "#00C3F7");
                    $("#enter").css("background-image", "url('Content/Images/enterHigh.png')");
                }
            }
            else {
                var tel = $("#txtTel").val();

                if (tel == "") {
                    $("#tel").css("border-color", "#cccccc");
                }
                else {
                    $("#tel").css("border-color", "#00C3F7");
                }
            }
        }

        //添加学生
        function Enter() {
            var stuName = $("#txtUsername").val();
                
            if (stuName == "") {
                alert("请输入学生姓名！");
            }
            else {
                var tel = $("#txtTel").val();
                var create_by = "<%=userID%>";
                var user_name = "<%=teacherName%>";
                var class_id = $("#hideClassID").val();
                var class_name = $("#hideClassName").val();

                //随机一个图像
                var iconStr = "<%=iconStr%>";
                var iconArr = iconStr.split(",");
                var len = iconArr.length;
                var index = Math.floor(Math.random() * len);

                if (index == len) {
                    index = len - 1;
                }

                var icon = iconArr[index];
                var url = "<%=postUrl%>" + "/v2/student/createBatchForPc/format/json";
                
                var students = "[{\"icon_class\":\"" + icon + "\",\"name_class\":\"" + stuName + "\"}]";

                if (tel!="")
                {
                    students = "[{\"icon_class\":\"" + icon + "\",\"name_class\":\"" + stuName + "\",\"telephone\":\"" + tel + "\"}]";
                }

                $.post(url, { class_id: class_id, create_by: create_by, class_name: class_name, user_name: user_name, students: students}, function (data) {
                    $("#txtUsername").val("");
                    $("#txtTel").val("");
                    $("#username").css("border-color", "#cccccc");
                    $("#tel").css("border-color", "#cccccc");
                    $("#enter").css("background-image", "url('Content/Images/enter.png')");

                    $.post("Ajax/StudentList.ashx?op=list", { classID: class_id }, function (myData) {
                        $("#studentList").html(myData);
                    });
                });
            }
        }

        function copyExcel() {
            var copyContent = $("#txtCopyContent").val().replace(/^\s+|\s+$/g, "");

            if (copyContent != "") {
                var str = copyContent.replace(/(\r\n|\r|\n)/g, '*');
                
                var stuArr = str.split('*');
                var len = stuArr.length;
                var allInput = "";
                //学生列表
                var studentList = "";

                //随机一个图像
                var iconStr = "<%=iconStr%>";
                var iconArr = iconStr.split(",");
                var iconLen = iconArr.length;

                for (var i = 0; i < len; i++) {
                    if (stuArr[i] != "") {
                        var row = stuArr[i].replace(/^\s+|\s+$/g, "");

                        var rowArr = row.split('|');
                        var rowLen = rowArr.length;

                        var txtInput = "";
                        var oneStu = "";
                        var tel = "";
                        var stuName = "";
                        var icon = "";

                        for (var k = 0; k < rowLen; k++) {
                            if (k == 0) {
                                stuName = rowArr[k].replace(/^\s+|\s+$/g, "");
                                txtInput = txtInput + stuName;

                                var index = Math.floor(Math.random() * iconLen);

                                if (index == iconLen) {
                                    index = iconLen - 1;
                                }

                                icon = iconArr[index];
                            }
                            else {
                                if (rowArr[k] != "") {
                                    tel = rowArr[k].replace(/^\s+|\s+$/g, "");;
                                    txtInput = txtInput + " , " + tel;
                                    break;
                                }
                            }
                        }

                        if (tel == "") {
                            oneStu = "{\"icon_class\":\"" + icon + "\",\"name_class\":\"" + stuName + "\"}";
                        }
                        else {
                            oneStu = "{\"icon_class\":\"" + icon + "\",\"name_class\":\"" + stuName + "\",\"telephone\":\"" + tel + "\"}";
                        }

                        studentList = studentList + oneStu + ",";

                        var oneInput = "<input type=\"text\" value=\"" + txtInput + "\" readonly=\"true\"/>";
                        allInput = allInput + oneInput;
                    }
                }

                //去掉最后的","
                studentList = studentList.substring(0, studentList.length - 1);
                studentList = "[" + studentList + "]";
                
                $("#hideStudent").val(studentList);
                $("#addedStudent").html(allInput);
                $("#txtCopyContent").html("");
                $("#txtCopyContent").val("");
            }
        }

        function ShowExitDiv()
        {
            $("#exit_btn").show();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="c_main">
            <div class="c_header">
                <div class="c_title">校朋·光荣榜</div>
                <div class="c_image">
                    <asp:Image ID="imgSmall" runat="server" Style="width: 100%; height: 100%;" /></div>
                <div class="c_name">
                    <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label>
                </div>
                <div class="c_exit" onclick="ShowExitDiv()"></div>
            </div>
            <div class="c_middle">
                <div class="c_middle_image">
                    <asp:Image ID="imgBig" runat="server" Style="width: 100%; height: 100%;" />
                </div>
                <div class="c_middle_name">
                    <asp:Label ID="lblUsernameTwo" runat="server" Text=""></asp:Label>&nbsp;&nbsp;
                    <asp:Label ID="lblSex" runat="server" Text=""></asp:Label>
                </div>
                <div class="c_middle_role">教师</div>
                <div class="exit_btn" id="exit_btn">
                    <asp:Button ID="btnExit" runat="server" Text="退出登陆" OnClick="btnExit_Click" />
                </div>
            </div>
            <div class="c_bottom" id="classList" runat="server"></div>

            <div id="popDiv" style="display: none;">
                <div class="popTitle">
                    <div class="leftTitle">为 <span id="className">三年二班</span> 添加学生</div>
                    <div class="importIcon"></div>
                    <div class="rightTitle" onclick="popExcel('student')">导入学生名单添加</div>
                    <div class="importFatherIcon"></div>
                    <div class="rightFatherTitle" onclick="popExcel('parent')">邀请家长</div>
                </div>
                <div class="addContent">
                    <div class="addTitle">通过姓名添加</div>
                    <div class="userName" id="username">
                        <input type="text" id="txtUsername" placeholder='请输入姓名，如"小明"' onkeyup="InputCheck('name')"/>
                    </div>
                    <div class="tel" id="tel">
                        <input type="text" id="txtTel" placeholder="请输入学生家长手机号" onkeyup="InputCheck('tel')"/>
                    </div>
                    <div class="enter" id="enter" onclick="Enter()">确认&nbsp;</div>
                </div>
                <div class="addList">
                    <div class="contentBg" id="studentList"></div>
                </div>
                <div class="closeBtn">
                    <div class="returnBtn" onclick="closeDivAndGet()">返回</div>
                    <div class="saveBtn" onclick="closeDivAndGet()">学生添加完成</div>
                </div>
            </div>

            <div id="backgroundDiv" style="display: none;"></div>

            <div id="popExcel" style="display: none;">
                <div class="excelBg">
                    <div class="excelImport">粘贴Excel内容：<textarea rows="3" onkeyup ="copyExcel()" id="txtCopyContent"></textarea></div>
                    <div class="template"><a href="Utils/student.xlsx">下载模板</a></div>
                    <div class="addedStudent" id="addedStudent"></div>
                    <div class="excelReturn" onclick="closeExcel()">取消</div>
                    <div class="excelSave" onclick="closeExcelAndGet()">保存</div>
                </div>
            </div>

            <input type="hidden" id="hideStudent" value="" />
            <input type="hidden" id="hideClassID" value=""/>
            <input type="hidden" id="hideClassName" value="" />
            <input type="hidden" id="action" value="" />
        </div>
    </form>
</body>
</html>