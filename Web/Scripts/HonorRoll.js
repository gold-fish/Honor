/*
描述:光荣榜操作脚本
*/

$(function () {
    var bw = document.getElementById("barStudent").clientWidth - 26;
    document.getElementById("btnStudent").style.left = bw + "px";
    document.getElementById("btnTeam").style.left = bw + "px";

    slide(100, bw + 2, "btnStudent");
    slide(100, bw + 2, "btnTeam");

    honorRollSlide('btnStudent', 'barStudent')
    honorRollSlide("btnTeam", "barTeam");
})

/*btnID:滑动按钮ID,
 *barID:滑动区域ID,
 *objId:班级ID/科目ID
 *objType:(0:班级,1:科目)
 *rollType:(0:日,1:月,2:周排行)
*/
function honorRollSlide(btnID, barID) {
    var f = this, g = document, b = window, m = Math;
    document.getElementById(btnID).onmousedown = function (e) {
        var x = (e || b.event).clientX;
        var l = this.offsetLeft;
        var max = document.getElementById(barID).offsetWidth - this.offsetWidth;
        g.onmousemove = function (e) {
            var thisX = (e || b.event).clientX;
            var to = m.min(max, m.max(-2, l + (thisX - x)));
            document.getElementById(btnID).style.left = to + 'px';
            f.slide((m.round(m.max(0, to / max) * 100)), to, btnID);
            b.getSelection ? b.getSelection().removeAllRanges() : g.selection.empty();
        };
        g.onmouseup = new Function('this.onmousemove=null');
    }
}

function slide(pos, x, typeID) {
    var objId = document.getElementById("hidClassId").value;
    var objType = document.getElementById("hidType").value;
    var bjNum;
    var teamNum;
    if (typeID == "btnStudent") {
        document.getElementById("student").style.width = Math.max(0, x + 2) + "px";
        bjNum = Math.round(pos / 6.7);
        $.post("Ajax/HonorRoll.ashx", { num: bjNum, type: "student", cId: objId, objType: objType, rollType: 0 }, function (data) {
            document.getElementById("studentHonorRoll").innerHTML = data;
        });
    }
    else {
        document.getElementById("team").style.width = Math.max(0, x + 2) + "px";
        teamNum = Math.round(pos / 16.7);
        $.post("Ajax/HonorRoll.ashx", { num: teamNum, type: "team", cId: objId, objType: objType, rollType: 0 }, function (data) {
            document.getElementById("teamRoll").innerHTML = data;
        });
    }
}

//objId:班级ID,科目ID
//type:(0:班级,1:科目)
//rollItem:(0:日,1:月,2:周排行)
function loadHonorRollData() {
    var bw = document.getElementById("barStudent").clientWidth - 26;
    document.getElementById("btnStudent").style.left = bw + "px";
    document.getElementById("btnTeam").style.left = bw + "px";

    var objId = document.getElementById("hidClassId").value;
    var objType = document.getElementById("hidType").value;

    document.getElementById("student").style.width = Math.max(0, bw + 2) + "px";

    $.post("Ajax/HonorRoll.ashx", { num: 15, type: "student", cId: objId, objType: objType, rollType: 0 }, function (data) {
        document.getElementById("studentHonorRoll").innerHTML = data;
    });

    document.getElementById("team").style.width = Math.max(0, bw + 2) + "px";
    $.post("Ajax/HonorRoll.ashx", { num: 6, type: "team", cId: objId, objType: objType, rollType: 0 }, function (data) {
        document.getElementById("teamRoll").innerHTML = data;
    });
}