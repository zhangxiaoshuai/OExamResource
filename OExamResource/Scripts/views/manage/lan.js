$(function () {
    var parameters = {};
    parameters.Id = "ce79087f-0637-4512-b2b6-a2abaff48c52";
    $("#ce79087f-0637-4512-b2b6-a2abaff48c52").attr("class", "clickon");
    $.sync.ajax.post("/DataManage/GetLeftMenu", parameters, function (data) {
        if (data.State) {
            $("#divMenu").setTemplateURL("/Scripts/template/manage/left.htm");
            $("#divMenu").processTemplate(data.Result);
        }
    });

    ///分类管理///////////////////////////////////////////
    parameters = {};
    $("#Lan_Type").click(function () {
        $.sync.ajax.post("/LanManage/Lan_Type", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/Lan_Type.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        readlist();
    });
    parameters = {};
    //读取列表
    function readlist() {
        parameters = {};
        parameters.schoolid = $("#ddlSchool").val();
        parameters.type = $("#ddlTypes").val();
        $.sync.ajax.post("/LanManage/GetLanTypes", parameters, function (data) {
            if (data.State) {
                if ($("#ddlTypes").val() == 2) {
                    if ($("#hdShow").val() == 1) {
                        $("#tdSchool").show();
                    } else {
                        $("#tdSchool").hide();
                    }
                    $("#divTypes").setTemplateURL("/Scripts/template/manage/lantype2.htm");
                }
                else {
                    $("#tdSchool").hide();
                    $("#divTypes").setTemplateURL("/Scripts/template/manage/lantype.htm");
                }
                $("#divTypes").processTemplate(data.Result);
            }
        });
    }
    //加载列表 
    //学校改变，读取相应列表
    $("#ddlSchool").die().live("change", function () {
        readlist();
    });

    //分类类型改变，读取相应列表
    $("#ddlTypes").die().live("change", function () {
        readlist();
    });
    //添加
    $("#btnAddType").die().live("click", function () {
        parameters = {};
        if ($("#txtName").val().trim().length == 0) {
            $.dialog.tips("请输入名称");
            return false;
        }
        parameters.name = $("#txtName").val();
        parameters.type = $("#ddlTypes").val();
        parameters.schoolid = $("#ddlSchool").val();
        $.sync.ajax.post("/LanManage/AddType", parameters, function (data) {
            if (data.State) {
                readlist();
                $.dialog.tips(data.Msg);
            } else {
                $.dialog.alert(data.Msg);
            }
        });
    });

    //所有删除
    $(".dellan").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/LanManage/DeleteType", parameters, function (data) {
                if (data.State) {
                    readlist();
                    $.dialog.tips(data.Msg);
                } else {
                    $.dialog.alert(data.Msg);
                }
            });
        });
    });

    //所有修改
    $(".edit").die().live("click", function () {
        var $edit = $(this);
        var id = $edit.attr("dataid");
        $.dialog({
            title: "更改名称",
            content: document.getElementById('divEdit'),
            id: 'EF8930'
        });
        $("#hdTypeId").val(id);
        $("#txtNewName").val($("#p" + id).text().trim());
    });

    //确定修改
    $("#btnEditType").die().live("click", function () {
        parameters = {};
        parameters.id = $("#hdTypeId").val();
        parameters.name = $("#txtNewName").val().trim();
        $.sync.ajax.post("/LanManage/EditType", parameters, function (data) {
            if (data.State) {
                $.dialog["EF8930"].close();
                readlist();
                $.dialog.tips(data.Msg);
            } else {
                $.dialog.alert(data.Msg);
            }
        });
    });
    //变色
    $("#divTypes tr:gt(0)").die().live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });
    ///信息管理///////////////////////////////////////////
    parameters = {};
    $("#Lan_EditInfo").click(function () {
        $.sync.ajax.post("/LanManage/Lan_EditInfo", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/lan_editinfo.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        //下拉
        parameters = {};
        parameters.schoolid = $("#hdId").val();
        $.sync.ajax.post("/LanManage/GetSFD", parameters, function (data) {
            if (data.State) {
                $("#ddlSFD").setTemplateURL("/Scripts/template/manage/sfd.htm");
                $("#ddlSFD").processTemplate(data.Result);
            }
        });
    });

    $("#ddlSFD").die().live("change", function () {
        parameters = {};
        parameters.id = $(this).val();
        $.sync.ajax.post("/LanManage/Lan_EditInfo", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/lan_editinfo.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        //下拉
        parameters = {};
        parameters.schoolid = $("#hdId").val();
        $.sync.ajax.post("/LanManage/GetSFD", parameters, function (data) {
            if (data.State) {
                $("#ddlSFD").setTemplateURL("/Scripts/template/manage/sfd.htm");
                $("#ddlSFD").processTemplate(data.Result);
            }
        });
    });

    //修改名称
    $("#btnEditName").die().live("click", function () {
        if ($(this).val() == "修改名称") {
            $(this).val("确定");
            $("#shName").hide();
            $("#txtName").show();
        } else {
            parameters.schoolid = $("#hdId").val();
            parameters.name = $("#txtName").val();
            if ($("#txtName").val().length == 0) {
                $.dialog.tips("名称不能为空！");
            } else {
                $.sync.ajax.post("/LanManage/EditName", parameters, function (data) {
                    if (data.State) {
                        $.dialog.tips(data.Msg);
                    }
                });
                $("#shName").text($("#txtName").val());
                $(this).val("修改名称");
                $("#shName").show();
                $("#txtName").hide();
            }
        }
    });

    //修改介绍
    var remark = $("#remark").val();
    $("#btnEditRemark").die().live("click", function () {
        if ($("#remark").attr("disabled") != "disabled") {
            parameters.schoolid = $("#hdId").val();
            parameters.remark = $("#remark").val();
            $.sync.ajax.post("/LanManage/EditRemark", parameters, function (data) {
                if (data.State) {
                    $("#btnEdit").click();
                    $.dialog.tips(data.Msg);
                }
            });
        } else {
            return false;
        }
    });
    $("#btnEdit").die().live("click", function () {
        if ($(this).val() == "取消") {
            $(this).val("修改");
            $("#remark").val($("#remark").val());
            $("#remark").attr("disabled", "disabled");
        } else {
            $(this).val("取消");
            $("#remark").removeAttr("disabled");
        }
    });

    //图片
    $("#divBannerImgs img").die().live({
        mouseover: function () {
            $(this).addClass("hover");
        },
        mouseleave: function () {
            $(this).removeClass("hover");
        }
    });

    //删除图片
    $(".delImg").die().live("click", function () {
        var id = $(this).attr("dataid");
        parameters = {};
        parameters.schoolid = $("#hdId").val();
        parameters.img = $("#img" + id).attr("src");
        $.sync.ajax.post("/LanManage/DelBannerImg", parameters, function (data) {
            if (data.State) {
                $("#" + id).remove();
            }
        });
    });

    //logo
    $("#btn_show_logo").die().live("click", function () {
        $.dialog({
            title: "更改学校Logo",
            content: document.getElementById('form_Logo'),
            id: 'EF8931'
        });
        var options = {
            success: function (data, statusText, xhr, $form) {
                data = JSON.parse(data);
                var picPath = data.Result.Pic;
                if (!data.State) {
                    $.dialog.tips(data.Msg);
                } else {
                    $.dialog.list['EF8931'].close();
                    $("#imgLogo").attr("src", picPath);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.write(XMLHttpRequest.responseText);
                console.log(textStatus);
                console.log(errorThrown);
            }
        };
        $("#form_Logo").ajaxForm(options);
    });

    $("#delLogoImg").die().live("click", function () {
        parameters = {};
        parameters.schoolid = $("#hdId").val();
        $.sync.ajax.post("/LanManage/DelLogoImg", parameters, function (data) {
            if (!!data.State) {
                $("#imgLogo").remove();
                var img = '<img id=\"imgName\" style=\"width: 104px;height: 100px; border: 1px solid #ffffff\" />';
                $("#delLogoImg").before(img);
            }
        });
    });
    //name
    $("#btn_show_name").die().live("click", function () {
        $.dialog({
            title: "更改学校名称图片",
            content: document.getElementById('form_name'),
            id: 'EF8930'
        });
        var options = {
            success: function (data, statusText, xhr, $form) {
                data = JSON.parse(data);
                var picPath = data.Result.Pic;
                if (!data.State) {
                    $.dialog.tips(data.Msg);
                } else {
                    $.dialog.list['EF8930'].close();
                    $("#imgName").attr("src", picPath);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.write(XMLHttpRequest.responseText);
                console.log(textStatus);
                console.log(errorThrown);
            }
        };
        $("#form_name").ajaxForm(options);
    });

    $("#delNameImg").die().live("click", function () {
        parameters = {};
        parameters.schoolid = $("#hdId").val();
        $.sync.ajax.post("/LanManage/DelNameImg", parameters, function (data) {
            if (!!data.State) {
                $("#imgName").remove();
                var img = '<img id=\"imgName\" style=\"width: 400px;height: 100px; border: 1px solid #ffffff\" />';
                $("#delNameImg").before(img);
            }
        });
    });
    //banner
    $("#btn_show_banner").die().live("click", function () {
        $.dialog({
            title: "更改学校标语",
            content: document.getElementById('form_banner'),
            id: 'EF8920'
        });
        var options = {
            success: function (data, statusText, xhr, $form) {
                data = JSON.parse(data);
                var picPath = data.Result.Pic;
                if (!data.State) {
                    $.dialog.tips(data.Msg);
                } else {
                    $.dialog.list['EF8920'].close();
                    var html = "<li id='" + data.Result.Id + "'><img id='img" + data.Result.Id + "', src='" + picPath + "' alt='' /><a class='delImg' style='vertical-align:top' href='javascript:void(0);' title='删除' dataid='" + data.Result.Id + "'>×</a></li>";
                    $("#app").before(html);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.write(XMLHttpRequest.responseText);
                console.log(textStatus);
                console.log(errorThrown);
            }
        };
        $("#form_banner").ajaxForm(options);
    });
   //资源列表///////////////////////////////////////////
        parameters = {};
        $("#Lan_Resource").click(function () {
            $.sync.ajax.post("/LanManage/Lan_Resource", parameters, function (data) {
                if (data.State) {
                    $("#divdata").setTemplateURL("/Scripts/template/manage/Lan_Resource.htm");
                    $("#divdata").processTemplate(data.Result);
                }
            });
            //学校院系
            parameters.schoolid = $("#hdSchoolId").val();
            $.sync.ajax.post("/LanManage/GetSFD", parameters, function (data) {
                if (data.State) {
                    $("#ddlSFD").setTemplateURL("/Scripts/template/manage/sfd.htm");
                    $("#ddlSFD").processTemplate(data.Result);
                }
            });

            //基本资源分类
            parameters = {};
            $.sync.ajax.post("/LanManage/GetBase", parameters, function (data) {
                if (data.State) {
                    $("#opt0").setTemplateURL("/Scripts/template/manage/ddl.htm");
                    $("#opt0").processTemplate(data.Result);
                }
            });
            //扩展资源分类
            parameters = {};
            $.sync.ajax.post("/LanManage/GetExt", parameters, function (data) {
                if (data.State) {
                    $("#opt1").setTemplateURL("/Scripts/template/manage/ddl.htm");
                    $("#opt1").processTemplate(data.Result);
                }
            });
            //学校自定义资源分类 
            parameters = {};
            parameters.schoolid = $("#ddlSFD").val();
            $.sync.ajax.post("/LanManage/GetCustom", parameters, function (data) {
                if (data.State) {
                    $("#opt2").setTemplateURL("/Scripts/template/manage/ddl.htm");
                    $("#opt2").processTemplate(data.Result);
                }
            });
            $("#btnSearchLanResource").click();
        });
         
        $("#ddlSFD").die().live("change", function () {
            parameters = {};
            parameters.sfd = $("#ddlSFD").val();
            $.sync.ajax.post("/LanManage/GetCourse", parameters, function (data) {
                if (data.State) {
                    $("#ddlCourse").setTemplateURL("/Scripts/template/manage/ddl.htm");
                    $("#ddlCourse").processTemplate(data.Result);
                }
            });
            parameters = {};
            parameters.schoolid = $("#ddlSFD").val();
            $.sync.ajax.post("/LanManage/GetCustom", parameters, function (data) {
                if (data.State) {
                    $("#opt2").setTemplateURL("/Scripts/template/manage/ddl.htm");
                    $("#opt2").processTemplate(data.Result);
                }
            });
        });

        $("#ddlSFD,#ddlCourse,#ddlType").die().live("change", function () {
            $("#btnSearchLanResource").click();
        });

        //分页数据
        $.cellpagebar.init({
            total: $("#hdCount").val(),
            pagesize: 50,
            container: $("#divBar")
        }, function (pageindex) {
            //schoolId, courseId, typeId, key
            parameters = {};
            parameters.pageIndex = pageindex;
            parameters.key = "";
            parameters.schoolId = $("#ddlSFD").val();
            parameters.courseId = $("#ddlCourse").val();
            parameters.typeId = $("#ddlType").val();
            $.sync.ajax.post("/DataManage/GetLanResourceList", parameters, function (data) {
                if (data.State) {
                    $("#divLanResourceList").setTemplateURL("/Scripts/template/manage/lanresource.htm");
                    $("#divLanResourceList").processTemplate(data.Result);
                }
            });
        });

        //搜索按钮
        $("#btnSearchLanResource").die().live("click", function () {
            parameters = {};
            parameters.key = $("#txtLanResource").val();
            parameters.schoolId = $("#ddlSFD").val();
            parameters.courseId = $("#ddlCourse").val();
            parameters.typeId = $("#ddlType").val();
            $.sync.ajax.post("/DataManage/GetLanResourceListCount", parameters, function (data) {
                if (data.State) {
                    $.cellpagebar.init({
                        total: data.Result,
                        pagesize: 50,
                        container: $("#divBar")
                    }, function (pageindex) {
                        $("#sCount").text(data.Result);
                        parameters = {};
                        parameters.pageIndex = pageindex;
                        parameters.key = $("#txtLanResource").val();
                        parameters.schoolId = $("#ddlSFD").val();
                        parameters.courseId = $("#ddlCourse").val();
                        parameters.typeId = $("#ddlType").val();
                        $.sync.ajax.post("/DataManage/GetLanResourceList", parameters, function (data) {
                            if (data.State) {
                                $("#divLanResourceList").setTemplateURL("/Scripts/template/manage/lanresource.htm");
                                $("#divLanResourceList").processTemplate(data.Result);
                            }
                        });
                    });
                }
            });
        });

        //全选、取消
        $("#ckAll").die().live("click",function () {
            var state = $(this).attr("checked");
            $("#divLanResourceList :checkbox").attr("checked", !!state);
        });
 

        //所有删除
        $(".del").die().live("click", function () {
            var $btnDel = $(this);
            $.dialog({
                id: 'deleteResource',
                title: '删除资源',
                content: '是否确认删除？',
                button: [{
                    name: '确定',
                    callback: function () {
                        parameters = {};
                        parameters.guidlist = [];
                        parameters.guidlist.push($btnDel.attr("dataid"));
                        $.sync.ajax.post("/LanManage/DeleteResource", parameters, function (data) {
                            if (data.State) {
                                $.dialog.list["deleteResource"].close();
                                $("#btnSearchLanResource").click();
                                $.dialog.tips(data.Msg);
                            }
                        });
                        return false;
                    },
                    focus: true
                },
        {
            name: '取消'
        }]
            });
        });

        //删除
$("#btnDelete").die().live("click", function () {
            parameters = {};
            parameters.guidlist = [];
            $("#divLanResourceList").find(":checkbox:checked").each(function () {
                parameters.guidlist.push($(this).val());
            });
            if (parameters.guidlist.length <= 0) {
                $.dialog.tips("没有选中资源！");
                return false;
            };
            $.dialog({
                id: 'deleteResource',
                title: '删除资源',
                content: '是否确认删除？',
                button: [{
                    name: '确定',
                    callback: function () {
                        $.sync.ajax.post("/LanManage/DeleteResource", parameters, function (data) {
                            if (data.State) {
                                $.dialog.list["deleteResource"].close();
                                $("#btnSearchLanResource").click();
                                $.dialog.tips(data.Msg);
                            }
                        });
                        return false;
                    },
                    focus: true
                },
        {
            name: '取消'
        }]
            });
        });

        //变色
        $("#divLanResourceList tr:gt(0)").die().live({
            mouseenter: function () {
                $(this).attr("bgcolor", "#D6DFF7");
            },
            mouseleave: function () {
                $(this).removeAttr("bgcolor");
            }
        });
});