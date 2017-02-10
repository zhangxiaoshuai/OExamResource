$(function () {
    var parameters = {};

    //素材大类
    $.sync.ajax.post("/DataManage/GetCategory", parameters, function (data) {
        if (data.State) {
            $("#ddlCategory").setTemplateURL("/Scripts/template/manage/ddl.htm");
            $("#ddlCategory").processTemplate(data.Result);
        } else {
            $("#ddlCategory").empty();
        }
        $("#ddlCategory").get(0).selectedIndex = 0;
        $("#hdCategory").val($("#ddlCategory").val());
    });
    //素材分类
    parameters = {};
    parameters.cateid = $("#ddlCategory").val();
    if (parameters.cateid.length > 0) {
        $.sync.ajax.post("/DataManage/GetSubject", parameters, function (data) {
            if (data.State) {
                $("#ddlSubject").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlSubject").processTemplate(data.Result);
            } else {
                $("#ddlSubject").empty();
            }
            $("#lbCategory").text($("#ddlCategory :selected").text());
            $("#ddlSubject").get(0).selectedIndex = 0;
            $("#hdSubject").val($("#ddlSubject").val());
        });
    }

    //素材专业
    parameters = {};
    parameters.subid = $("#ddlSubject").val();
    if (parameters.subid.length > 0) {
        $.sync.ajax.post("/DataManage/GetSpeciality", parameters, function (data) {
            if (data.State) {
                $("#ddlSpeciality").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlSpeciality").processTemplate(data.Result);
            } else {
                $("#ddlSpeciality").empty();
            }
            $("#lbSubject").text($("#ddlSubject :selected").text());
            $("#ddlSpeciality").get(0).selectedIndex = 0;
            $("#hdSpeciality").val($("#ddlSpeciality").val());
        });
    }

    //素材知识点
    parameters = {};
    parameters.speid = $("#ddlSpeciality").val();
    if (parameters.speid.length > 0) {
        $.sync.ajax.post("/DataManage/GetKnowledge", parameters, function (data) {
            if (data.State) {
                $("#ddlKnowledge").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlKnowledge").processTemplate(data.Result);
            } else {
                $("#ddlKnowledge").empty();
            }
            $("#lbSpeciality").text($("#ddlSpeciality :selected").text());
            $("#ddlKnowledge").get(0).selectedIndex = 0;
            $("#hdKnowledge").val($("#ddlKnowledge").val());
        });
    }

    //大类改变 素材分类
    $("#ddlCategory").bind("change", function () {
        parameters = {};
        parameters.cateid = $("#ddlCategory").val();
        $.sync.ajax.post("/DataManage/GetSubject", parameters, function (data) {
            if (data.State) {
                $("#ddlSubject").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlSubject").processTemplate(data.Result);
            } else {
                $("#ddlSubject").empty();
            }
            $("#lbCategory").text($("#ddlCategory :selected").text());
            $("#ddlSubject").get(0).selectedIndex = 0;
            $("#hdCategory").val($("#ddlCategory").val());
        });
        $("#btnEscCategory").click();
        $("#btnEscSubject").click();
        $("#btnEscSpeciality").click();
        $("#btnEscKnowledge").click();
    });


    //分类改变 素材专业
    $("#ddlSubject").bind("change", function () {
        parameters = {};
        parameters.subid = $("#ddlSubject").val();
        $.sync.ajax.post("/DataManage/GetSpeciality", parameters, function (data) {
            if (data.State) {
                $("#ddlSpeciality").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlSpeciality").processTemplate(data.Result);
            } else {
                $("#ddlSpeciality").empty();
            }
            $("#lbSubject").text($("#ddlSubject :selected").text());
            $("#ddlSpeciality").get(0).selectedIndex = 0;
            $("#hdSubject").val($("#ddlSubject").val());
        });
        $("#btnEscCategory").click();
        $("#btnEscSubject").click();
        $("#btnEscSpeciality").click();
        $("#btnEscKnowledge").click();
    });

    //专业改变 知识点
    $("#ddlSpeciality").bind("change", function () {
        parameters = {};
        parameters.speid = $("#ddlSpeciality").val();
        $.sync.ajax.post("/DataManage/GetKnowledge", parameters, function (data) {
            if (data.State) {
                $("#ddlKnowledge").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlKnowledge").processTemplate(data.Result);
            } else {
                $("#ddlKnowledge").empty();
            }
            $("#lbSpeciality").text($("#ddlSpeciality :selected").text());
            $("#ddlKnowledge").get(0).selectedIndex = 0;
            $("#hdSpeciality").val($("#ddlSpeciality").val());
        });
        $("#btnEscCategory").click();
        $("#btnEscSubject").click();
        $("#btnEscSpeciality").click();
        $("#btnEscKnowledge").click();
    });

    $("#ddlKnowledge").bind("change", function () {
        $("#btnEscCategory").click();
        $("#btnEscSubject").click();
        $("#btnEscSpeciality").click();
        $("#btnEscKnowledge").click();
    });
    //添加Category
    $("#btnAddCategory").bind("click", function () {
        $("#txtCategory").show();
        $("#btnCategory").show();
        $("#btnEscCategory").show();
        $("#btnCategory").val($("#btnAddCategory").val());
        $("#txtCategory").val("");
        $("#btnCategory").attr("flag", "add");
    });
    //修改Category
    $("#btnEditCategory").bind("click", function () {
        if ($("#ddlCategory").val() != null) {
            $("#txtCategory").show();
            $("#btnCategory").show();
            $("#btnEscCategory").show();
            $("#btnCategory").val($("#btnEditCategory").val());
            $("#txtCategory").val($("#ddlCategory :selected").text());
            $("#btnCategory").attr("flag", "edit");
        } else {
            $.dialog.tips("无选项，请添加或选中！");
        }
    });
    //取消
    $("#btnEscCategory").bind("click", function () {
        $("#txtCategory").hide();
        $("#btnCategory").hide();
        $("#btnEscCategory").hide();
    });
    //确定添加/修改Category
    $("#btnCategory").bind("click", function () {
        parameters = {};
        parameters.cateid = $("#ddlCategory").val();
        parameters.flag = $("#btnCategory").attr("flag");
        parameters.name = $("#txtCategory").val();
        if (parameters.name.length > 0) {
            $.sync.ajax.post("/ResourceManage/Category", parameters, function (data) {
                if (data.State) {
                    if (parameters.flag == "add") {
                        $("#ddlCategory").append("<option value=" + data.Result + ">" + parameters.name + "</option>");
                    } else if (parameters.flag == "edit") {
                        $("#ddlCategory").find("option:selected").text(parameters.name);
                    }
                }
                $.dialog.tips(data.Msg);

            });
        } else {
            $.dialog.tips("为空或未选中！");
        }
    });
    //删除Category
    $("#btnDelCategory").bind("click", function () {
        $.artDialog.confirm("确定删除吗？", function () {
            parameters = {};
            parameters.flag = "del";
            parameters.cateid = $("#ddlCategory").val();
            parameters.name = $("#ddlCategory :selected").text();
            $.sync.ajax.post("/ResourceManage/Category", parameters, function (data) {
                if (data.State) {
                    $("#ddlCategory option[value='" + parameters.cateid + "']").remove();
                    if ($("#ddlCategory").val() == null) {
                        $("#btnCategory").val($("#btnAddCategory").val());
                        $("#txtCategory").val("");
                        $("#btnCategory").attr("flag", "add");
                    }
                }
                $.dialog.tips(data.Msg);
            });
        });
    });

    //添加Subject
    $("#btnAddSubject").bind("click", function () {
        $("#txtSubject").show();
        $("#btnSubject").show();
        $("#btnEscSubject").show();
        $("#btnSubject").val($("#btnAddSubject").val());
        $("#txtSubject").val("");
        $("#btnSubject").attr("flag", "add");
    });
    //修改Subject
    $("#btnEditSubject").bind("click", function () {
        if ($("#ddlSubject").val() != null) {
            $("#txtSubject").show();
            $("#btnSubject").show();
            $("#btnEscSubject").show();
            $("#btnSubject").val($("#btnEditSubject").val());
            $("#txtSubject").val($("#ddlSubject :selected").text());
            $("#btnSubject").attr("flag", "edit");
        } else {
            $.dialog.tips("无选项，请添加或选中！");
        }
    });
    //取消
    $("#btnEscSubject").bind("click", function () {
        $("#txtSubject").hide();
        $("#btnSubject").hide();
        $("#btnEscSubject").hide();
    });
    //确定添加/修改Subject
    $("#btnSubject").bind("click", function () {
        parameters = {};
        parameters.subid = $("#ddlSubject").val();
        parameters.flag = $("#btnSubject").attr("flag");
        parameters.cateid = $("#hdCategory").val();
        parameters.name = $("#txtSubject").val();
        if (parameters.name.length > 0) {
            $.sync.ajax.post("/ResourceManage/Subject", parameters, function (data) {
                if (data.State) {
                    if (parameters.flag == "add") {
                        $("#ddlSubject").append("<option value=" + data.Result + ">" + parameters.name + "</option>");
                    } else if (parameters.flag == "edit") {
                        $("#ddlSubject").find("option:selected").text(parameters.name);
                    }
                }
                $.dialog.tips(data.Msg);
            });
        } else {
            $.dialog.tips("为空或未选中！");
        }

    });
    //删除Subject
    $("#btnDelSubject").bind("click", function () {
        $.artDialog.confirm("确定删除吗？", function () {
            parameters = {};
            parameters.flag = "del";
            parameters.subid = $("#ddlSubject").val();
            parameters.name = $("#ddlSubject :selected").text();
            $.sync.ajax.post("/ResourceManage/Subject", parameters, function (data) {
                if (data.State) {
                    $("#ddlSubject option[value='" + parameters.subid + "']").remove();
                    $("#ddlSubject").get(0).selectedIndex = 1;
                    if ($("#ddlSubject").val() == null) {
                        $("#btnSubject").val($("#btnAddSubject").val());
                        $("#txtSubject").val("");
                        $("#btnSubject").attr("flag", "add");
                    }
                }
                $.dialog.tips(data.Msg);
            });
        });
    });

    //添加Speciality
    $("#btnAddSpeciality").bind("click", function () {
        $("#txtSpeciality").show();
        $("#btnSpeciality").show();
        $("#btnEscSpeciality").show();
        $("#btnSpeciality").val($("#btnAddSpeciality").val());
        $("#txtSpeciality").val("");
        $("#btnSpeciality").attr("flag", "add");
    });
    //修改Speciality
    $("#btnEditSpeciality").bind("click", function () {
        if ($("#ddlSpeciality").val() != null) {
            $("#txtSpeciality").show();
            $("#btnSpeciality").show();
            $("#btnEscSpeciality").show();
            $("#btnSpeciality").val($("#btnEditSpeciality").val());
            $("#txtSpeciality").val($("#ddlSpeciality :selected").text());
            $("#btnSpeciality").attr("flag", "edit");
        } else {
            $.dialog.tips("无选项，请添加或选中！");
        }
    });
    //取消
    $("#btnEscSpeciality").bind("click", function () {
        $("#txtSpeciality").hide();
        $("#btnSpeciality").hide();
        $("#btnEscSpeciality").hide();
    });
    //确定添加/修改Speciality
    $("#btnSpeciality").bind("click", function () {
        parameters = {};
        parameters.speid = $("#ddlSpeciality").val();
        parameters.flag = $("#btnSpeciality").attr("flag");
        parameters.subid = $("#hdSubject").val();
        parameters.name = $("#txtSpeciality").val();
        if (parameters.name.length > 0) {
            $.sync.ajax.post("/ResourceManage/Speciality", parameters, function (data) {
                if (data.State) {
                    if (parameters.flag == "add") {
                        $("#ddlSpeciality").append("<option value=" + data.Result + ">" + parameters.name + "</option>");
                    } else if (parameters.flag == "edit") {
                        $("#ddlSpeciality").find("option:selected").text(parameters.name);
                    }
                }
                $.dialog.tips(data.Msg);
            });
        } else {
            $.dialog.tips("为空或未选中！");
        }
    });
    //删除Speciality
    $("#btnDelSpeciality").bind("click", function () {
        $.artDialog.confirm("确定删除吗？", function () {
            parameters = {};
            parameters.flag = "del";
            parameters.speid = $("#ddlSpeciality").val();
            parameters.name = $("#ddlSpeciality :selected").text();
            $.sync.ajax.post("/ResourceManage/Speciality", parameters, function (data) {
                if (data.State) {
                    $("#ddlSpeciality option[value='" + parameters.speid + "']").remove();
                    if ($("#ddlSpeciality").val() == null) {
                        $("#btnSpeciality").val($("#btnAddSpeciality").val());
                        $("#txtSpeciality").val("");
                        $("#btnSpeciality").attr("flag", "add");
                    }
                }
                $.dialog.tips(data.Msg);
            });
        });
    });

    //添加Knowledge
    $("#btnAddKnowledge").bind("click", function () {
        $("#txtKnowledge").show();
        $("#btnKnowledge").show();
        $("#btnEscKnowledge").show();
        $("#btnKnowledge").val($("#btnAddKnowledge").val());
        $("#txtKnowledge").val("");
        $("#btnKnowledge").attr("flag", "add");
    });
    //修改Knowledge
    $("#btnEditKnowledge").bind("click", function () {
        if ($("#ddlKnowledge").val() != null) {
            $("#txtKnowledge").show();
            $("#btnKnowledge").show();
            $("#btnEscKnowledge").show();
            $("#btnKnowledge").val($("#btnEditKnowledge").val());
            $("#txtKnowledge").val($("#ddlKnowledge :selected").text());
            $("#btnKnowledge").attr("flag", "edit");
        } else {
            $.dialog.tips("无选项，请添加或选中！");
        }
    });
    //取消
    $("#btnEscKnowledge").bind("click", function () {
        $("#txtKnowledge").hide();
        $("#btnKnowledge").hide();
        $("#btnEscKnowledge").hide();
    });
    //确定添加/修改Knowledge
    $("#btnKnowledge").bind("click", function () {
        parameters = {};
        parameters.knowid = $("#ddlKnowledge").val();
        parameters.flag = $("#btnKnowledge").attr("flag");
        parameters.speid = $("#hdSpeciality").val();
        parameters.name = $("#txtKnowledge").val();
        if (parameters.name.length > 0) {
            $.sync.ajax.post("/ResourceManage/Knowledge", parameters, function (data) {
                if (data.State) {
                    if (parameters.flag == "add") {
                        $("#ddlKnowledge").append("<option value=" + data.Result + ">" + parameters.name + "</option>");
                    } else if (parameters.flag == "edit") {
                        $("#ddlKnowledge").find("option:selected").text(parameters.name);
                    }
                }
                $.dialog.tips(data.Msg);
            });
        } else {
            $.dialog.tips("为空或未选中！");
        }
    });
    //删除Knowledge
    $("#btnDelKnowledge").bind("click", function () {
        $.artDialog.confirm("确定删除吗？", function () {
            parameters = {};
            parameters.flag = "del";
            parameters.knowid = $("#ddlKnowledge").val();
            parameters.name = $("#ddlKnowledge :selected").text();
            $.sync.ajax.post("/ResourceManage/Knowledge", parameters, function (data) {
                if (data.State) {
                    $("#ddlKnowledge option[value='" + parameters.knowid + "']").remove();
                    if ($("#ddlKnowledge").val() == null) {
                        $("#btnKnowledge").val($("#btnAddKnowledge").val());
                        $("#txtKnowledge").val("");
                        $("#btnKnowledge").attr("flag", "add");
                    }
                }
                $.dialog.tips(data.Msg);
            });
        });
    });
});