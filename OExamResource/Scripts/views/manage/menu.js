$(function () {
    var parameters = {};
    //页面数据
    $.sync.ajax.post("/DataManage/GetMenuList", parameters, function (data) {
        if (data.State) {
            $("#ulMenuList").setTemplateURL("/Scripts/template/manage/menu.htm");
            $("#ulMenuList").processTemplate(data.Result);
        }
    });
    //修改后左侧菜单刷新
    function getmenu() {
        $.sync.ajax.post("/DataManage/GetLeftMenu", parameters, function (data) {
            if (data.State) {
                $("#divMenu").setTemplateURL("/Scripts/template/manage/left.htm");
                $("#divMenu").processTemplate(data.Result);
            }
        });

    }
    //修改后刷新
    function refresh() {
        $.sync.ajax.post("/DataManage/GetMenuList", parameters, function (data) {
            if (data.State) {
                $("#ulMenuList").setTemplateURL("/Scripts/template/manage/menu.htm");
                $("#ulMenuList").processTemplate(data.Result);
            }
        });
    }
    //变色
    $(".uli,.uliuli").die().live({
        mouseover: function () {
            $(this).css("background", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).css("background", "");
        }
    });

    //隐藏
    $(".softdela").live("click", function () {
        parameters = {};
        parameters.id = $(this).attr("dataid");
        parameters.isdel = $(this).attr("flag");
        $.sync.ajax.post("/SystemManage/SoftDelMenu", parameters, function (data) {
            if (data.State) {
                getmenu();
                refresh();
            }
        });
    });
    //显示添加根节点
    $("#addRoot").live("click", function () {
        $.dialog({
            content: document.getElementById('divAddRoot'),
            id: 'EF8930',
            title: '添加根节点'
        });
    });
    //显示添加子节点
    $(".adda").live("click", function () {
        $.dialog({
            content: document.getElementById('divAddchild'),
            id: 'EF8931',
            title: '为“' + $("#sp" + $(this).attr("dataid")).text() + '”添加子节点'
        });
        $("#hdParentId").val($(this).attr("dataid"));
        $("#hdFlag").val($("#li" + $(this).attr("dataid")).attr("flag"));
    });
    //添加根节点
    $("#btnAddRoot").live("click", function () {
        parameters = {};
        parameters.type = $("#ddlNewLevel").val();
        if ($("#txtRootTitle").val().trim().length <= 0) {
            $.dialog.tips("节点标题不能为空！");
            return false;
        }
        parameters.title = $("#txtRootTitle").val().trim();
        if ($("#trNewModules").css("display") != "none") {
            parameters.module = $("#ddlNewModule").val();
        }
        $.sync.ajax.post("/SystemManage/AddRoot", parameters, function (data) {
            if (data.State) {
                getmenu();
                refresh();
            }
            $.dialog.tips(data.Msg);
            $.dialog.list["EF8930"].close();
            $("#txtRootTitle").val() = "";
        });
    });
    //编辑根节点
    $("#btnSureRoot").live("click", function () {
        parameters = {};
        parameters.id = $("#hdRCId").val();
        parameters.type = $("#ddlLevel").val();
        if ($("#txtRoot").val().trim().length <= 0) {
            $.dialog.tips("节点标题不能为空！");
            return false;
        }
        parameters.title = $("#txtRoot").val().trim();
        if ($("#trModules").css("display") != "none") {
            parameters.module = $("#ddlModule").val();
        }
        $.sync.ajax.post("/SystemManage/EditRoot", parameters, function (data) {
            if (data.State) {
                getmenu();
                refresh();
            }
            $.dialog.tips(data.Msg);
        });
    });
    //添加子节点
    $("#btnAddChild").live("click", function () {
        parameters = {};
        parameters.parentid = $("#hdParentId").val();
        if ($("#txtChildTitle").val().trim().length <= 0) {
            $.dialog.tips("节点标题不能为空！");
            return false;
        }
        parameters.title = $("#txtChildTitle").val().trim();
        if ($("#txtNewUrl").val().trim().length <= 0) {
            $.dialog.tips("页面地址不能为空！");
            return false;
        }
        parameters.url = $("#txtNewUrl").val().trim();
        parameters.type = $("#hdFlag").val();
        $.sync.ajax.post("/SystemManage/AddChild", parameters, function (data) {
            if (data.State) {
                getmenu();
                refresh();
            }
            $.dialog.tips(data.Msg);
            $.dialog.list["EF8931"].close();
            $("#txtChildTitle").val() = "";
            $("#txtNewUrl").val() = "";
        });
    });
    //编辑子节点
    $("#btnSureChild").live("click", function () {
        parameters = {};
        parameters.id = $("#hdRCId").val();
        if ($("#txtChild").val().trim().length <= 0) {
            $.dialog.tips("节点标题不能为空！");
            return false;
        }
        parameters.title = $("#txtChild").val().trim();
        if ($("#txtUrl").val().trim().length <= 0) {
            $.dialog.tips("页面地址不能为空！");
            return false;
        }
        parameters.url = $("#txtUrl").val().trim();
        $.sync.ajax.post("/SystemManage/EditChild", parameters, function (data) {
            if (data.State) {
                getmenu();
                refresh();
            }
            $.dialog.tips(data.Msg);
        });
    });
    //节点信息显示
    $(".uli,.uliuli").live("click", function () {
        var id = $(this).attr("dataid");
        var level = $(this).attr("class");
        var title = $("#sp" + id).text().trim();
        var flag = $(this).attr("flag");
        var module = $(this).attr("module");
        var url = $(this).attr("url");
        $("#hdRCId").val(id);
        if (level == "uli") {
            $("#divChild").hide();
            $("#divRoot").show();
            $("#txtRoot").val(title);
            $("#ddlLevel option[value='" + flag + "']").attr("selected", true);
            if (flag == "0") {
                $("#trModules").show();
                $("#ddlModule option[value='" + module + "']").attr("selected", true);
            } else {
                $("#trModules").hide();
            }
        } else if (level == "uliuli") {
            $("#divRoot").hide();
            $("#divChild").show();
            $("#txtChild").val(title);
            $("#txtUrl").val(url);
        }
    });

    $("#ddlNewLevel").live("change", function () {
        if ($(this).val() == "0") {
            $("#trNewModules").show();
        } else {
            $("#trNewModules").hide();
        }
    });

    $("#ddlLevel").live("change", function () {
        if ($(this).val() == "0") {
            $("#trModules").show();
        } else {
            $("#trModules").hide();
        }
    });
    //删除
    $(".dela").live("click", function () {
        parameters.id = $(this).attr("dataid");
        $.dialog({
            id: 'deleteMenu',
            content: '是否确认删除“' + $("#sp" + $(this).attr("dataid")).text() + '”及下级节点，删除后无法恢复？',
            button: [
            {
                name: '确定',
                callback: function () {
                    $.sync.ajax.post("/SystemManage/DelMenu", parameters, function (data) {
                        if (data.State) {
                            getmenu();
                            refresh();
                        }
                        $("#divRoot,#divChild").hide();
                    });
                },
                focus: true
            },
            {
                name: '取消'
            }
        ]
        });

    });
});