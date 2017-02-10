$(function () {
    var parameters = {};

    //素材分类
    $.sync.ajax.post("/DataManage/GetCategorySubject", parameters, function (data) {
        if (data.State) {
            $("#ddlcs").setTemplateURL("/Scripts/template/manage/ddlcs.htm");
            $("#ddlcs").processTemplate(data.Result);
        }
    });

    //分页数据
    $.cellpagebar.init({
        total: $("#hdCount").val(),
        pagesize: 50,
        container: $("#divBar")
    }, function (pageindex) {
        parameters = {};
        parameters.pageIndex = pageindex;
        parameters.name = "";
        parameters.csid = $("#ddlcs").val();
        parameters.specialityid = $("#ddlSpe").val();
        parameters.typeid = $("#ddlType").val();
        $.sync.ajax.post("/DataManage/GetDelResourceList", parameters, function (data) {
            if (data.State) {
                $("#divResourceList").setTemplateURL("/Scripts/template/manage/auditresource.htm");
                $("#divResourceList").processTemplate(data.Result);
            }
        });
    });

    //搜索按钮
    $("#btnSearchResource").bind("click", function () {
        parameters = {};
        parameters.csid = $("#ddlcs").val();
        parameters.specialityid = $("#ddlSpe").val();
        parameters.typeid = $("#ddlType").val();
        parameters.name = $("#txtName").val();
        $.sync.ajax.post("/DataManage/GetDelResourceListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 50,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.csid = $("#ddlcs").val();
                    parameters.specialityid = $("#ddlSpe").val();
                    parameters.typeid = $("#ddlType").val();
                    parameters.name = $("#txtName").val();
                    $.sync.ajax.post("/DataManage/GetDelResourceList", parameters, function (data) {
                        if (data.State) {
                            $("#divResourceList").setTemplateURL("/Scripts/template/manage/auditresource.htm");
                            $("#divResourceList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });

    //分类改变选择素材专业
    $("#ddlcs").bind("change", function () {
        parameters = {};
        parameters.subid = $("#ddlcs").val();
        $.sync.ajax.post("/DataManage/GetSpeciality", parameters, function (data) {
            if (data.State) {
                $("#ddlSpe").setTemplateURL("/Scripts/template/manage/ddlstk.htm");
                $("#ddlSpe").processTemplate(data.Result);
            } else {
                $("#ddlSpe").empty();
                $("#ddlSpe").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });
        $("#btnSearchResource").click();
    });

    $("#ddlSpe,#ddlType").bind("change", function () {
        $("#btnSearchResource").click();
    });
    //全选、取消
    $("#ckAll").click(function () {
        var state = $(this).attr("checked");
        $("#divResourceList :checkbox").attr("checked", !!state);
    });

    //反选
    $("#ckReverse").click(function () {
        $("#divResourceList :checkbox").each(function () {
            $(this).attr("checked", !$(this).attr("checked"));
        });
    });

    //所有删除
    $(".del").die().live("click", function () {
        var $btnDel = $(this);
        parameters = {};
        parameters.guidlist = [];
        parameters.guidlist.push($btnDel.attr("dataid"));
        $.dialog({
            id: 'deleteResource',
            content: '是否彻底删除资源？',
            button: [
        {
            name: '确定',
            callback: function () {
                parameters.flag = "delete";
                $.sync.ajax.post("/ResourceManage/DeleteResource", parameters, function (data) {
                    if (data.State) {
                        $.dialog.tips("资源已经被删除!");
                        $("#btnSearchResource").click();
                    }
                });
            },
            focus: true
        }, {
            name: '取消'
        }
    ]
        });
    });

    //所有审核
    $(".audit").die().live("click", function () {
        var $btnAudit = $(this);
        parameters = {};
        parameters.guidlist = [];
        parameters.guidlist.push($btnAudit.attr("dataid"));
        $.dialog({
            id: 'Audit',
            content: '资源是否通过审核？',
            button: [
        {
            name: '确定',
            callback: function () {
                $.sync.ajax.post("/ResourceManage/Audit", parameters, function (data) {
                    if (data.State) {
                        $.dialog.tips("资源已经通过审核!");
                        $("#btnSearchResource").click();
                    }
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

    //通过审核
    $("#btnAudit").bind("click", function () {
        parameters = {};
        parameters.guidlist = [];
        $("#divResourceList").find(":checkbox:checked").each(function () {
            parameters.guidlist.push($(this).val());
        });
        if (parameters.guidlist.length > 0) {
            $.dialog({
                id: 'Audit',
                content: '资源是否通过审核？',
                button: [
        {
            name: '确定',
            callback: function () {
                $.sync.ajax.post("/ResourceManage/Audit", parameters, function (data) {
                    if (data.State) {
                        $("#ckAll").removeAttr("checked");
                        $("#ckReverse").removeAttr("checked");
                        $.dialog.tips("资源已经通过审核!");
                        $("#btnSearchResource").click();
                    }
                });
            },
            focus: true
        },
        {
            name: '取消'
        }
    ]
            });
        }
    });

    //删除选择
    $("#btnDelete").bind("click", function () {
        parameters = {};
        parameters.guidlist = [];
        $("#divResourceList").find(":checkbox:checked").each(function () {
            parameters.guidlist.push($(this).val());
        });
        if (parameters.guidlist.length > 0) {
            $.dialog({
                id: 'deleteResource',
                content: '是否彻底删除资源？',
                button: [
        {
            name: '确定',
            callback: function () {
                parameters.flag = "delete";
                $.sync.ajax.post("/ResourceManage/DeleteResource", parameters, function (data) {
                    if (data.State) {
                        $("#ckAll").removeAttr("checked");
                        $("#ckReverse").removeAttr("checked");
                        $.dialog.tips("资源已经被删除!");
                        $("#btnSearchResource").click();
                    }
                });
            },
            focus: true
        }, {
            name: '取消'
        }
    ]
            });
        }
    });


    //变色
    $("#divResourceList tr:gt(0)").live({
        mouseenter: function () {
            $(this).css("background", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).css("background", "");
        }
    });

    //预览
    $(".preview").die().live("click", function () {
        var id = $(this).attr("id");
        $("#hdResourceId").val(id);
        parameters = {};
        parameters.id = id;
        $.sync.ajax.post("/ResourceManage/Preview", parameters, function (data) {
            if (data.State) {
                preview(data.Result);
                $.dialog({
                    title: data.Result.Title,
                    content: document.getElementById('divPreview'),
                    id: 'EF893L',
                    fixed: true
                });
            }
        });
    });

    //拼接预览字符
    function preview(data) {
        var str = "";
        if (data.Flag == 0) {
            if (data.FileExt == ".jpg" || data.FileExt == ".gif" || data.FileExt == ".bmp" || data.FileExt == ".jpeg" || data.FileExt == ".png") {
                str = "<img src='" + data.PreviewFilepath + "' alt=''/>";
            } else if (data.FileExt == ".swf" || data.FileExt == ".ppt" || data.FileExt == ".doc") {
                str = "<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0'";
                str += " width='100%' height='100%'>";
                str += " <param name='movie' value='" + data.PreviewFilepath + "' />";
                str += " <param name='quality' value='high' />";
                str += " <embed src='" + data.PreviewFilepath + "' quality='high' pluginspage='http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash'";
                str += " type='application/x-shockwave-flash' width='800px' height='600px'>";
                str += " </embed>";
                str += " </object>";
            } else if (data.FileExt == ".flv") {
                var swf = new SWFObject("../../../Images/source/vcastr.swf", "single", "800", "600", "7");
                swf.addParam("allowfullscreen", "true");
                swf.addVariable("file", data.PreviewFilepath);
                swf.addVariable("image", data.PreviewFilepath);
                swf.addVariable("width", "800");
                swf.addVariable("height", "600");
                $("#tdPreview").empty();
                swf.write("tdPreview");
                return false;
            } else {
                str = "<img src='../../../Images/source/file_zanwu_big.jpg' alt=''  />";
            }
        } else {
            str = "<img src='../../../Images/source/file_zanwu_big.jpg' alt=''  />";
        }
        $("#tdPreview").empty();
        $("#tdPreview").append(str);
    }

    //预览中的通过
    $("#Audit").die().bind("click", function () {
        parameters = {};
        parameters.guidlist = [];
        parameters.guidlist.push($("#hdResourceId").val());
        $.sync.ajax.post("/ResourceManage/DeleteResource", parameters, function (data) {
            if (data.State) {
                $("#btnSearchResource").click();
                $.dialog.tips("资源通过审核!");
            }
        });
    });

    //预览中的删除
    $("#Delete").die().bind("click", function () {
        parameters = {};
        parameters.guidlist = [];
        parameters.guidlist.push($("#hdResourceId").val());
        $.dialog({
            id: 'deleteResource',
            content: '是否彻底删除资源？',
            button: [
        {
            name: '确定',
            callback: function () {
                parameters.flag = "delete";
                $.sync.ajax.post("/ResourceManage/DeleteResource", parameters, function (data) {
                    if (data.State) {
                        $("#btnSearchResource").click();
                        $.dialog.tips("资源已经被删除!");
                    }
                });
            },
            focus: true
        }, {
            name: '取消'
        }
    ]
        });
    });

});