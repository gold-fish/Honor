/*获取排行详情*/

$(function () {
    var objId = document.getElementById("hidClassId").value;
    var objType = document.getElementById("hidType").value;

    GetRollDeatailData(objId, objType, "class");

    $("#class").click(function () {
        objId = document.getElementById("hidClassId").value;
        objType = document.getElementById("hidType").value;
        $("#hidDetailType").val("class");

        GetRollDeatailData(objId, objType, "class");
        $("#rghm").removeClass("rightpPrsonal").removeClass("rightGroup").addClass("rightMiddle");
    });
    $("#person").click(function () {
        objId = document.getElementById("hidClassId").value;
        objType = document.getElementById("hidType").value;
        $("#hidDetailType").val("person");

        GetRollDeatailData(objId, objType, "personal");
        $("#rghm").removeClass("rightMiddle").removeClass("rightGroup").addClass("rightpPrsonal");
    });
    $("#group").click(function () {
        objId = document.getElementById("hidClassId").value;
        objType = document.getElementById("hidType").value;
        $("#hidDetailType").val("group");

        GetRollDeatailData(objId, objType, "group");
        $("#rghm").removeClass("rightMiddle").removeClass("rightpPrsonal").addClass("rightGroup");
    });
})

function GetRollDeatailData(objId, typeValue, item) {

    if (objId > 0 && item != "") {
        $.post("Ajax/HonorRollDetails.ashx", { action: item, Id: objId, typeId: typeValue }, function (data) { $("#rightRoll").html(data); });
    }
}