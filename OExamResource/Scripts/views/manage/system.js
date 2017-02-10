$(function () {
    var parameters = {};
    parameters.Id = "05dca932-1371-4485-9e46-ee0bea634a55";
    $("#59eb93dd-2711-4f34-9fca-adf9df89f22e").attr("class", "clickon");
    $.sync.ajax.post("/DataManage/GetLeftMenu", parameters, function (data) {
        if (data.State) {
            $("#divMenu").setTemplateURL("/Scripts/template/manage/left.htm");
            $("#divMenu").processTemplate(data.Result);
        }
    });
    //下载异常监控开始
    $("#downMonitor").click(function () {
        $("#divdata").setTemplateURL("/Scripts/template/manage/downMonitor.htm");
        $("#divdata").processTemplate("1");
        $("#btnSearchDownMonitor").click();
    });

    //搜索学校按钮
    $("#btnSearchDownMonitor").die().live("click", function () {
        var parameters = {};
        parameters.name = $("#txtName").val();
        parameters.ip = $("#txtIp").val();
        parameters.Startime = $("#txt_startime").val();
        parameters.Endtime = $("#txt_endtime").val();
        parameters.teacherName = $("#TextName").val();
        $.sync.ajax.post("/DataManage/GetDownSchoolIpListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 10,
                    container: $("#divBar")
                }, function (pageindex) {
                    parameters.pageIndex = pageindex;
                    $.sync.ajax.post("/DataManage/GetDownSchoolIpList", parameters, function (data) {
                        if (data.State) {
                            $("#divDownSchoolList").setTemplateURL("/Scripts/template/manage/down_school.htm");
                            $("#divDownSchoolList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });
   //启用学校下载按钮
    $(".schoolDownOk").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认启用？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/SystemManage/StartSchool", parameters, function (data) {
                if (data.State) {
                    $.dialog.tips(data.Msg);
                    $("#btnSearchSchool").click();
                } else {
                    $.dialog.alert(data.Msg);
                }
            });
        });
    });
    //停用学校下载按钮
    $(".schoolDownOkNo").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认停用？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/SystemManage/StopSchool", parameters, function (data) {
                if (data.State) {
                    $.dialog.tips(data.Msg);
                    $("#btnSearchSchool").click();
                } else {
                    $.dialog.alert(data.Msg);
                }
            });
        });
    });




    //---------修改密码---开始--------------------------
    //显示——修改密码
    $("#EditPwd").click(function () {
        $.sync.ajax.post("/SystemManage/EditPwd", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_pwd.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });

    //修改密码
    $("#btnPwd").die().live("click", function () {
        parameters = {};
        parameters.oldpwd = $("#txtOldPwd").val();
        parameters.newpwd = $("#txtNewPwd").val();
        var newPwdSure = $("#txtNewPwdSure").val();
        if (parameters.oldpwd.length <= 0) {
            $("#lb_old").text("原密码不能为空！");
            return false;
        }
        if (parameters.newpwd.length <= 0) {
            $("#lb_new").text("新密码不能为空！");
            return false;
        }
        if (parameters.newpwd != newPwdSure) {
            $("#lb_sure").text("两次密码不一致！");
            return false;
        }
        $.sync.ajax.post("/SystemManage/EditUserPwd", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg, 2);
                $.sync.ajax.post("/SystemManage/PerZL", parameters, function (data) {
                    if (data.State) {
                        $("#divdata").setTemplateURL("/Scripts/template/manage/system_perzl.htm");
                        $("#divdata").processTemplate(data.Result);
                    }
                });
            } else {
                $.dialog.tips(data.Msg);
            }
        });

    });

    //---------修改密码---结束--------------------------



    //---------个人信息---开始--------------------------
    //显示——个人信息
    $("#PerZL").click(function () {
        $.sync.ajax.post("/SystemManage/PerZL", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_perzl.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });
    //个人信息
    $("#ddlFaculty").die().live("change", function () {
        parameters = {};
        parameters.facid = $("#ddlFaculty").val();
        $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
            if (data.State) {
                $("#ddlDepartment").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlDepartment").processTemplate(data.Result);
            } else {
                $("#ddlDepartment").empty();
                $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });
    });
    //修改信息
    $("#btnInfoSure").die().live("click", function () {
        parameters = {};
        parameters.id = $("#hdId").val();
        parameters.name = $("#txtName").val();
        if (parameters.name.trim().length <= 0) {
            $("#lb_name").text("真实姓名不能为空！");
            return false;
        }
        parameters.departmentid = $("#ddlDepartment").val();
        parameters.birthday = $("#txtBirthday").val();
        parameters.facultyid = $("#ddlFaculty").val();
        parameters.email = $("#txtEmail").val();
        parameters.phone = $("#txtPhone").val();
        parameters.sex = $(":radio[name='rSex']:checked").next("label").text().trim();
        parameters.job = $("#ddlJob").val();
        parameters.post = $("#ddlPost").val();
        parameters.profession = $("#ddlProfession").val();
        $.sync.ajax.post("/SystemManage/EditInfo", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
            };
        });
    });

    $("#btnPWord").die().live("click", function () {
        $.sync.ajax.post("/SystemManage/EditPwd", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_pwd.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });


    //---------个人信息---结束--------------------------

    //---------系统信息---开始--------------------------
    //显示——系统信息
    $("#xinxi").click(function () {
        $.sync.ajax.post("/SystemManage/xinxi", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_xinxi.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        var parameters = {};
        //获取 学校开通模块
        parameters.id = $("#hdSchoolId").val();
        $.sync.ajax.post("/DataManage/GetSysModules", parameters, function (data) {
            if (data.State) {
                $("#listModules").setTemplateURL("/Scripts/template/manage/module.htm");
                $("#listModules").processTemplate(data.Result);
            }
        });
    });

    //显示修改名称
    $("#btnName").die().live("click", function () {
        $.dialog({
            content: document.getElementById('divEdit'),
            id: 'EF893L',
            title: '修改学校名称'
        });
    });

    //确认修改
    $("#btnEdit").die().live("click", function () {
        parameters = {};
        parameters.id = $("#hdSchoolId").val();
        parameters.name = $("#txtName").val();
        $.sync.ajax.post("/SystemManage/EditSchoolName", parameters, function (data) {
            if (data.State) {
                $.dialog.list["EF893L"].close();
                $.dialog.tips(data.Msg);
            }
            $("#xinxi").click();
        });
    });

    $("#ckIsReg").die().live("change", function () {
        parameters = {};
        parameters.reg = $(this).val();
        $.sync.ajax.post("/SystemManage/EditReg", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
            }
        });
    });

    $("#ckIsValid").die().live("change", function () {
        parameters = {};
        parameters.val = $(this).val();
        $.sync.ajax.post("/SystemManage/EditValid", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
            }
        });
    });

    $(".ckModules").die().live("change", function () {
        parameters = {};
        parameters.ids = [];
        $(".ckModules").each(function () {
            var _this = $(this);
            if (_this.attr("disabled") != "disabled") {
                if (_this.attr("checked") == "checked") {
                    parameters.ids.push(_this.val());
                }
            }
        });
        if (parameters.ids.length == 0) {
            parameters.ids = [];
        }
        $.sync.ajax.post("/SystemManage/EditModules", parameters, function (data) {
        });
    });
    $(".cktrial").die().live("change", function () {
        var _this = $(this);
        var $that = $(this).parent();
        var v_id = $(this).attr('id');
        if (_this.attr("checked") == "checked") {
            $that.append("<input type='checkbox' name='ckTrialTag'  value='0' class='" + v_id + "' /><label for='ckTrialTag' class='" + v_id + "'>远程镜像</label>");
        }
        else {
            $("." + v_id).remove();
        }
    });
    ///////查看详细使用记录
    $("#Asystem_schoollog").die().live("click", function () {
        parameters.id = $(this).attr("schoolid");
        $.sync.ajax.post("/SystemManage/SchoolLog", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_schoollog.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });

        //----------------素材库
        //获取素材Ip段下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "0";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbRIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbRIp").processTemplate(data.Result);
            }
        });

        //获取素材分类下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "0";
        $.sync.ajax.post("/SystemManage/GetSubDetailsR", parameters, function (data) {
            if (data.State) {
                $("#lbRSub").setTemplateURL("/Scripts/template/manage/detailsR.htm");
                $("#lbRSub").processTemplate(data.Result);
            }
        });

        //获取素材月份下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "0";
        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
            if (data.State) {
                $("#lbRMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbRMonthCount").processTemplate(data.Result);
            }
        });

        //-------------医学库        
        //获取医学Ip段下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "1";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbMIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbMIp").processTemplate(data.Result);
            }
        });

        //获取医学分类下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "1";
        $.sync.ajax.post("/SystemManage/GetSubDetailsM", parameters, function (data) {
            if (data.State) {
                $("#lbMSub").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbMSub").processTemplate(data.Result);
            }
        });

        //获取医学月份下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "1";
        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
            if (data.State) {
                $("#lbMMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbMMonthCount").processTemplate(data.Result);
            }
        });

        //--------------------试题库
        //获取试题库Ip段浏览详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "3";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbTIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbTIp").processTemplate(data.Result);
            }
        });

        //获取试题月份下载详情
        //        parameters = {};
        //        parameters.id = $(this).attr("schoolid");
        //        parameters.tag = "3";
        //        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
        //            if (data.State) {
        //                $("#lbTMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
        //                $("#lbTMonthCount").processTemplate(data.Result);

        //            }
        //               });
        //试题按学科门数统计
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "3";
        $.sync.ajax.post("/SystemManage/GetTestSubjectDetails", parameters, function (data) {
            if (data.State) {
                $("#lbTMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbTMonthCount").processTemplate(data.Result);
            }
        });


        //---------------------模考库
        //获取模考库Ip段下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "4";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbEIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbEIp").processTemplate(data.Result);
            }
        });

        //获取模考分类详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "4";
        $.sync.ajax.post("/SystemManage/GetSubDetailsE", parameters, function (data) {
            if (data.State) {
                $("#lbESub").setTemplateURL("/Scripts/template/manage/detailsR.htm");
                $("#lbESub").processTemplate(data.Result);
            }
        });

        //获取模考月份下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "4";
        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
            if (data.State) {
                $("#lbEMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbEMonthCount").processTemplate(data.Result);
            }
        });

        //-------------精品课程库
        //获取精品课程库Ip段详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "2";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbQIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbQIp").processTemplate(data.Result);
            }
        });

        //获取精品课程分类详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "2";
        $.sync.ajax.post("/SystemManage/GetSubDetailsQ", parameters, function (data) {
            if (data.State) {
                $("#lbQSub").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbQSub").processTemplate(data.Result);
            }
        });

        //获取精品课程月份详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "2";
        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
            if (data.State) {
                $("#lbQMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbQMonthCount").processTemplate(data.Result);
            }
        });
    });
    //---------系统信息---结束--------------------------

    //---------ip---开始--------------------------
    $("#Ip").click(function () {
        var parameters = {};
        $.sync.ajax.post("/SystemManage/Ip", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_ip.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });
    var parameters = {};

    //刷新
    $("#btnRe").die().live("click", function () {
        parameters = {};
        parameters.schoolid = $("#hdSchoolId").val();
        $.sync.ajax.post("/DataManage/GetIpList", parameters, function (data) {
            if (data.State) {
                $("#txtIP").val(data.Result);
            }
        });
    });


    $("#btnIpEdit").die().live("click", function () {
        if ($(this).val() == "取消") {
            $(this).val("修改");
            $("#btnRe").click();
            $("#txtIP").attr("disabled", "disabled");
        } else {
            $(this).val("取消");
            $("#txtIP").removeAttr("disabled");
        }
    });

    $("#btnSure").die().live("click", function () {
        if ($("#txtIP").attr("disabled") != "disabled") {
            parameters = {};
            parameters.id = $("#hdSchoolId").val();
            parameters.ips = $("#txtIP").val().replace("，", ",");
            $.sync.ajax.post("/SystemManage/EditIp", parameters, function (data) {
                if (data.State) {
                    $.dialog.tips(data.Msg);
                    $("#txtIP").attr("disabled", "disabled");
                    $("#btnIpEdit").click();
                } else {
                    $.dialog.alert(data.Msg);
                }
                $("#btnRe").click();
            });
        } else {
            return false;
        }
    });
    //---------ip---结束--------------------------

    //---------院系管理---开始--------------------------
    $("#Department").click(function () {
        var parameters = {};
        $.sync.ajax.post("/SystemManage/Department", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_Department.htm");
                $("#divdata").processTemplate(data.Result);
                $("#btnSearchDep").click();
            }
        });
    });
    var parameters = {};

    //分页数据
    //用户信息列表
    $.cellpagebar.init({
        total: $("#hdCount").val(),
        pagesize: 10,
        container: $("#divBar")
    }, function (pageindex) {
        parameters = {};
        parameters.pageIndex = pageindex;
        parameters.schoolid = $("#hdSchoolId").val();
        parameters.name = "";
        $.sync.ajax.post("/DataManage/GetDepList", parameters, function (data) {
            if (data.State) {
                $("#divDepList").setTemplateURL("/Scripts/template/manage/dep.htm");
                $("#divDepList").processTemplate(data.Result);
            }
        });
    });

    //搜索院系按钮
    $("#btnSearchDep").die().live("click", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool_yx").val();
        parameters.name = $("#txtName").val();
        $.sync.ajax.post("/DataManage/GetDepListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 10,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.schoolid = $("#ddlSchool_yx").val();
                    parameters.name = $("#txtName").val();
                    $.sync.ajax.post("/DataManage/GetDepList", parameters, function (data) {
                        if (data.State) {
                            $("#divDepList").setTemplateURL("/Scripts/template/manage/dep.htm");
                            $("#divDepList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });

    $("#ddlSchool_yx").die().live("change", function () {
        $("#btnSearchDep").click();
    });

    //所有删除
    $(".del_yxgl").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/SystemManage/DeleteFacDep", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchDep").click();
                }
                $("#btnSearchDep").click();
                $.dialog.tips(data.Msg);

            });

        });
    });

    //所有修改
    $(".edit").die().live("click", function () {
        $.dialog({
            content: document.getElementById('divEditDepFac'),
            id: 'EF89EE',
            title: '修改院系名称'
        });
        $("#txtEditDepFacName").val($(this).parent("td").prev("td").text().trim());
        $("#hdDepFacId").val($(this).attr("dataid"));
        $("#hdDepFacName").val($("#txtEditName").val());
    });

    //确认修改
    $("#btnEditDepFac").die().live("click", function () {
        parameters = {};
        parameters.id = $("#hdDepFacId").val();
        parameters.name = $("#txtEditDepFacName").val();
        if (parameters.name == $("#hdDepName").val()) {
            $.dialog.tips("院系名称没有更改！");
            return false;
        }
        if (parameters.name.length != 0) {
            $.sync.ajax.post("/SystemManage/EditFacDep", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchDep").click();
                }
                $.dialog.tips(data.Msg);
                var list = art.dialog.list;
                for (var i in list) {
                    list[i].close();
                };
            });
        } else {
            $.dialog.tips("院系名称不能为空！");
        };

    });

    //添加学院按钮
    $("#btnAddFac").die().live("click", function () {
        $.dialog({
            content: document.getElementById('divAddFac'),
            id: 'EF8931',
            title: $("#ddlSchool :selected").text()
        });
    });
    //添加系别按钮
    $("#btnAddDep").die().live("click", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool_yx").val();
        $.sync.ajax.post("/DataManage/GetFacDep", parameters, function (data) {
            if (data.State) {
                $("#ddlFac").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlFac").processTemplate(data.Result);
            } else {
                $("#ddlFac").empty();
                $("#ddlFac").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });
        $.dialog({
            content: document.getElementById('divAddDep'),
            id: 'EF893L',
            title: $("#ddlSchool :selected").text()
        });
    });

    //确认添加学院
    $("#btnSureFac").die().live("click", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool_yx").val();
        parameters.name = $("#txtNewFacName").val();
        if (parameters.name.length == 0) {
            $.dialog.tips("学院名称不能为空！");
            return false;
        } else {
            $.sync.ajax.post("/SystemManage/AddFac", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchDep").click();
                    $.dialog.list["EF8931"].close();
                }
                $.dialog.tips(data.Msg);

            });
        }
    });

    //确认添加系别
    $("#btnSureDep").die().live("click", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool_yx").val();
        parameters.facultyid = $("#ddlFac").val();
        parameters.name = $("#txtNewDepName").val();
        if (parameters.name.length == 0) {
            $.dialog.tips("学院名称不能为空！");
            return false;
        } else if (parameters.facultyid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加学院！");
            return false;
        } else {
            $.sync.ajax.post("/SystemManage/AddDep", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchDep").click();
                    $.dialog.list["EF893L"].close();
                }
                $.dialog.tips(data.Msg);

            });
        }
    });
    //---------院系管理---结束--------------------------

    //---------管理员---开始--------------------------
    $("#ManagerList").click(function () {
        var parameters = {};
        $.sync.ajax.post("/SystemManage/ManagerList", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_ManagerList.htm");
                $("#divdata").processTemplate(data.Result);
                $("#btnSearchManager").click();
            }
        });
    });

    var parameters = {};

    //分页数据
    //用户信息列表
    $.cellpagebar.init({
        total: $("#hdCount").val(),
        pagesize: 30,
        container: $("#divBar")
    }, function (pageindex) {
        parameters = {};
        parameters.pageIndex = pageindex;
        parameters.state = -1;
        parameters.name = "";
        $.sync.ajax.post("/DataManage/GetManagerList", parameters, function (data) {
            if (data.State) {
                $("#divUserList").setTemplateURL("/Scripts/template/manage/manager.htm");
                $("#divUserList").processTemplate(data.Result);
            }
        });
    });

    //搜索用户按钮
    $("#btnSearchManager").die().live("click", function () {
        parameters = {};
        parameters.state = $("#ddlState").val();
        parameters.name = $("#txtName").val();
        $.sync.ajax.post("/DataManage/GetManagerListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 30,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.state = $("#ddlState").val();
                    parameters.name = $("#txtName").val();
                    $.sync.ajax.post("/DataManage/GetManagerList", parameters, function (data) {
                        if (data.State) {
                            $("#divUserList").setTemplateURL("/Scripts/template/manage/manager.htm");
                            $("#divUserList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });

    $("#ddlState").die().live("change", function () {
        $("#btnSearchManager").click();
    });

    //变色
    $("#divUserList tr:gt(0)").die().live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });

    //全选、取消
    $("#ckAll").die().live("click", function () {
        var state = $(this).attr("checked");
        $("#divUserList :checkbox").attr("checked", !!state);
    });
    //所有启用
    $(".start").die().live("click", function () {
        if ($(this).attr("state") == "已启用") {
            $.dialog.tips("已启用");
            return false;
        }
        var $btnStart = $(this);
        $.dialog.confirm("是否确认启用？", function () {
            parameters = {};
            parameters.id = $btnStart.attr("dataid");
            $.sync.ajax.post("/SystemManage/StartTeacher", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchManager").click();
                    $.dialog.tips("操作成功");
                }
            });
        });
    });

    //所有停用
    $(".stop").die().live("click", function () {
        if ($(this).attr("state") == "已停用") {
            $.dialog.tips("已停用");
            return false;
        }
        var $btnStop = $(this);
        $.dialog.confirm("是否确认停用？", function () {
            parameters = {};
            parameters.id = $btnStop.attr("dataid");
            $.sync.ajax.post("/SystemManage/StopTeacher", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchManager").click();
                    $.dialog.tips("操作成功");
                } else {
                    $.dialog.alert(data.Msg);
                }
            });
        });
    });

    //所有删除
    $(".del").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/SystemManage/DeleteTeacher", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchManager").click();
                    $.dialog.tips("操作成功");
                } else {
                    $.dialog.alert(data.Msg);
                }
            });
        });
    });

    //单选操作
    $("#btnAffirm").die().live("click", function () {
        parameters = {};
        parameters.ids = [];
        $("#divUserList").find(":checkbox:checked").each(function () {
            parameters.ids.push($(this).val());
        });
        parameters.aff = $("input[name='groupAff']:checked").val();
        if (parameters.ids.length > 0) {
            $.sync.ajax.post("/SystemManage/Affirm", parameters, function (data) {
                $("#ckAll").removeAttr("checked");
                $("#ckReverse").removeAttr("checked");
                $("#btnSearchManager").click();
                $.dialog.tips(data.Msg);
            });
        };
    });
    ///修改----------------开始----------------------
    $("#up_manager").die().live("click", function () {
        parameters = {};
        parameters.id = $(this).attr("dataid");
        $.sync.ajax.post("/SystemManage/EditManager", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_EditManager.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });


    });

    //学校改变
    $("#ddlSchool_maa").die().live("change", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool_maa").val();
        $.sync.ajax.post("/DataManage/GetFaculty", parameters, function (data) {
            if (data.State) {
                $("#ddlFaculty_maa").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlFaculty_maa").processTemplate(data.Result);
                parameters.facid = $("#ddlFaculty_maa").val();
                $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
                    if (data.State) {
                        $("#ddlDepartment_maa").setTemplateURL("/Scripts/template/manage/ddl.htm");
                        $("#ddlDepartment_maa").processTemplate(data.Result);
                    } else {
                        $("#ddlDepartment_maa").empty();
                        $("#ddlDepartment_maa").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                    }
                });
            } else {
                $("#ddlFaculty_maa").empty();
                $("#ddlFaculty_maa").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                $("#ddlDepartment_maa").empty();
                $("#ddlDepartment_maa").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });
    });

    //学院改变
    $("#ddlFaculty_maa").die().live("change", function () {
        parameters = {};
        parameters.facid = $("#ddlFaculty_maa").val();
        $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
            if (data.State) {
                $("#ddlDepartment_maa").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlDepartment_maa").processTemplate(data.Result);
            } else {
                $("#ddlDepartment_maa").empty();
                $("#ddlDepartment_maa").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });

    });


    //初始化密码
    $("#btnInitialize_maa").die().live("click", function () {
        parameters = {};
        var list = [];
        list.push($("#btnInfoSure_maa").attr("userid"));
        parameters.guidlist = JSON.stringify(list);
        $.sync.ajax.post("/TeacherManage/Initialize", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
            }
        });
    });

    //修改信息
    $("#btnInfoSure_maa").die().live("click", function () {
        parameters = {};
        parameters.id = $(this).attr("userid");
        parameters.name = $("#txtName").val();
        if (parameters.name.trim().length <= 0) {
            $("#lb_name").text("真实姓名不能为空！");
            return false;
        }
        parameters.schoolid = $("#ddlSchool_maa").val();
        parameters.birthday = $("#txtBirthday_maa").val();
        parameters.facultyid = $("#ddlFaculty_maa").val();
        parameters.departmentid = $("#ddlDepartment_maa").val();
        if (parameters.facultyid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加学院！");
            return false;
        } else if (parameters.departmentid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加系别！");
            return false;
        }
        parameters.email = $("#txtEmail").val();
        parameters.phone = $("#txtPhone").val();
        parameters.sex = $(":radio[name='rSex']:checked").next("label").text().trim();
        parameters.role = $("#ddlRole_maa").val();
        if (parameters.role == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加角色！");
            return false;
        }
        parameters.job = $("#ddlJob_maa").val();
        parameters.post = $("#ddlPost_maa").val();
        parameters.profession = $("#ddlProfession_maa").val();
        $.sync.ajax.post("/SystemManage/EditManagerInfo", parameters, function (data) {
            if (data.State) {
                $("#lb_name").text("");
                $.dialog.tips(data.Msg);
                $("#btnEsc_maa").click();
            };
        });
    });

    //取消
    $("#btnEsc_maa").die().live("click", function () {
        $("#ManagerList").click();
    });

    ///修改-----------------结束------------------------

    //批量修改
    $("#btnEditSelected").die().live("click", function () {
        var data = [];
        $("#divUserList").find(":checkbox:checked").each(function () {
            data.push($(this).val());
        });
        if (data.length > 0) {
            $("#hdData").val(JSON.stringify(data));
            $("form").submit();
        }
    });

    //添加按钮
    $("#btnAddManager").die().live("click", function () {
        $.dialog({
            content: document.getElementById('divAddManager'),
            id: 'EF893L'
        });
    });

    //学校改变
    $("#ddlSchool").die().live("change", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool").val();
        $.sync.ajax.post("/DataManage/GetFaculty", parameters, function (data) {
            if (data.State) {
                $("#ddlFaculty").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlFaculty").processTemplate(data.Result);
                parameters.facid = $("#ddlFaculty").val();
                $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
                    if (data.State) {
                        $("#ddlDepartment").setTemplateURL("/Scripts/template/manage/ddl.htm");
                        $("#ddlDepartment").processTemplate(data.Result);
                    } else {
                        $("#ddlDepartment").empty();
                        $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                    }
                });
            } else {
                $("#ddlFaculty").empty();
                $("#ddlFaculty").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                $("#ddlDepartment").empty();
                $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });

        $.sync.ajax.post("/DataManage/GetRole", parameters, function (data) {
            if (data.State) {
                $("#ddlRole").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlRole").processTemplate(data.Result);
            } else {
                $("#ddlRole").empty();
                $("#ddlRole").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });

    });

    //学院改变
    $("#ddlFaculty").die().live("change", function () {
        parameters = {};
        parameters.facid = $("#ddlFaculty").val();
        $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
            if (data.State) {
                $("#ddlDepartment").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlDepartment").processTemplate(data.Result);
            } else {
                $("#ddlDepartment").empty();
                $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });

    });

    //确认添加
    $("#btnAdd").die().live("click", function () {
        parameters = {};
        var pwd = $("#txtSurePwd").val();
        parameters.name = $("#txtTrueName").val();
        parameters.loginName = $("#txtLoginName").val();
        parameters.loginPwd = $("#txtLoginPwd").val();
        parameters.schoolid = $("#ddlSchool").val();
        parameters.facultyid = $("#ddlFaculty").val();
        parameters.departmentid = $("#ddlDepartment").val();
        parameters.role = $("#ddlRole").val();
        if (parameters.name.length == 0) {
            $.dialog.tips("真实姓名不能为空！");
            return false;
        } else if (parameters.loginPwd == 0) {
            $.dialog.tips("登录密码不能为空！");
            return false;
        } else if (parameters.loginName == 0) {
            $.dialog.tips("登录名称不能为空！");
            return false;
        } else if (parameters.loginPwd != pwd) {
            $.dialog.tips("两次密码不一致！");
            return false;
        } else if (parameters.facultyid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加学院！");
            return false;
        } else if (parameters.role == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加角色！");
            return false;
        } else {
            $.sync.ajax.post("/SystemManage/AddManager", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchManager").click();
                    $.dialog.list["EF893L"].close();
                    $.dialog.tips(data.Msg);
                };
            });
        }
    });
    ///---------管理员---结束--------------------------
    ///---------教师列表---开始--------------------------

    $("#TeacherList").click(function () {
        $.sync.ajax.post("/TeacherManage/TeacherList", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_TeacherList.htm");
                $("#divdata").processTemplate(data.Result);
                $("#btnSearchUser").click();
            }
        });
    });
    var parameters = {};
    //分页数据
    //用户信息列表
    $.cellpagebar.init({
        total: $("#hdCount").val(),
        pagesize: 30,
        container: $("#divBar")
    }, function (pageindex) {
        parameters = {};
        parameters.pageIndex = pageindex;
        parameters.state = -1;
        parameters.name = "";
        $.sync.ajax.post("/DataManage/GetUserList", parameters, function (data) {
            if (data.State) {
                $("#divUserList").setTemplateURL("/Scripts/template/manage/user.htm");
                $("#divUserList").processTemplate(data.Result);
            }
        });
    });

    //搜索用户按钮
    $("#btnSearchUser").die().live("click", function () {
        parameters = {};
        parameters.state = $("#ddlState").val();
        parameters.name = $("#txtName").val();
        $.sync.ajax.post("/DataManage/GetUserListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 30,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.state = $("#ddlState").val();
                    parameters.name = $("#txtName").val();
                    $.sync.ajax.post("/DataManage/GetUserList", parameters, function (data) {
                        if (data.State) {
                            $("#divUserList").setTemplateURL("/Scripts/template/manage/user.htm");
                            $("#divUserList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });

    $("#ddlState").die().live("change", function () {
        $("#btnSearchUser").click();
    });

    //全选、取消
    $("#ckAll_teacherlist").die().live("click", function () {
        var state = $(this).attr("checked");
        $("#divUserList :checkbox").attr("checked", !!state);
    });
    //所有启用
    $(".start_teacherlist").die().live("click", function () {
        if ($(this).attr("state") == "已启用") {
            return false;
        }
        var $btnStart = $(this);
        $.dialog.confirm("是否确认启用？", function () {
            parameters = {};
            parameters.id = $btnStart.attr("dataid");
            $.sync.ajax.post("/TeacherManage/StartTeacher", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchUser").click();
                }
            });
        });
    });

    //所有停用
    $(".stop_teacherlist").die().live("click", function () {
        if ($(this).attr("state") == "已停用") {
            return false;
        }
        var $btnStop = $(this);
        $.dialog.confirm("是否确认停用？", function () {
            parameters = {};
            parameters.id = $btnStop.attr("dataid");
            $.sync.ajax.post("/TeacherManage/StopTeacher", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchUser").click();
                }
            });
        });
    });

    //所有删除
    $(".del_teacherlist").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/TeacherManage/DeleteTeacher", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchUser").click();
                }
            });
        });
    });

    //单选操作
    $("#btnAffirm").die().live("click", function () {
        parameters = {};
        parameters.ids = [];
        $("#divUserList").find(":checkbox:checked").each(function () {
            parameters.ids.push($(this).val());
        });
        parameters.aff = $("input[name='groupAff']:checked").val();
        if (parameters.ids.length > 0) {
            $.sync.ajax.post("/TeacherManage/Affirm", parameters, function (data) {
                $("#ckAll").removeAttr("checked");
                $("#ckReverse").removeAttr("checked");
                $.dialog.tips(data.Msg);
                $("#btnSearchUser").click();
            });
        };
    });
    //变色
    $("#divUserList tr:gt(0)").die().live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });
    //教师列表--修改
    $("#update_teacherlist").die().live("click", function () {
        parameters = {};
        parameters.id = $(this).attr("dataid");
        $.sync.ajax.post("/TeacherManage/EditTeacher", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_EditTeacher.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });
    //学校改变
    $("#ddlSchool_tl").die().live("change", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool_tl").val();
        $.sync.ajax.post("/DataManage/GetFaculty", parameters, function (data) {
            if (data.State) {
                $("#ddlFaculty").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlFaculty").processTemplate(data.Result);
                parameters.facid = $("#ddlFaculty").val();
                $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
                    if (data.State) {
                        $("#ddlDepartment").setTemplateURL("/Scripts/template/manage/ddl.htm");
                        $("#ddlDepartment").processTemplate(data.Result);
                    } else {
                        $("#ddlDepartment").empty();
                        $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                    }
                });
            } else {
                $("#ddlFaculty").empty();
                $("#ddlFaculty").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                $("#ddlDepartment").empty();
                $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });

    });

    //学院改变
    $("#ddlFaculty").die().live("change", function () {
        parameters = {};
        parameters.facid = $("#ddlFaculty").val();
        $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
            if (data.State) {
                $("#ddlDepartment").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlDepartment").processTemplate(data.Result);
            } else {
                $("#ddlDepartment").empty();
                $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });

    });


    //初始化密码
    $("#btnInitialize").die().live("click", function () {
        parameters = {};
        var list = [];
        list.push($("#btnInitialize").attr("teaid"));
        parameters.guidlist = JSON.stringify(list);
        $.sync.ajax.post("/TeacherManage/Initialize", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
            }
        });
    });

    //修改信息
    $("#btnInfoSure_tea").die().live("click", function () {
        parameters = {};
        parameters.id = $(this).attr("teaid");
        parameters.name = $("#txtName").val();
        if (parameters.name.trim().length <= 0) {
            $("#lb_name").text("真实姓名不能为空！");
            return false;
        }
        parameters.schoolid = $("#ddlSchool_tl").val();
        parameters.birthday = $("#txtBirthday").val();
        parameters.facultyid = $("#ddlFaculty").val();
        parameters.departmentid = $("#ddlDepartment").val();
        if (parameters.facultyid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加学院！");
            return false;
        } else if (parameters.departmentid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加系别！");
            return false;
        }
        parameters.email = $("#txtEmail").val();
        parameters.phone = $("#txtPhone").val();
        parameters.sex = $(":radio[name='rSex']:checked").next("label").text().trim();
        parameters.job = $("#ddlJob").val();
        parameters.post = $("#ddlPost").val();
        parameters.profession = $("#ddlProfession").val();
        parameters.YNManager = $("input[name='YNManager']:checked").val();
        $.sync.ajax.post("/TeacherManage/EditInfo", parameters, function (data) {
            if (data.State) {
                $("#lbFacultyName").attr("dataid", $("#ddlFaculty").val());
                $("#lbFacultyName").text($("#ddlFaculty :selected").text());
                $("#lb_name").text("");
                $.dialog.tips(data.Msg);
                $("#btnEsc").click();
            };
        });
    });

    //取消
    $("#btnEsc").die().live("click", function () {
        $("#TeacherList").click();
    });
    $("#btnEscTeacher").die().live("click", function () {
        $("#TeacherList").click();
    });
    ///---------教师列表---结束--------------------------


    ///---------批量导入---开始--------------------------

    //    $("#TeacherAddXls").click(function () {
    //        parameters = {};
    //        $.sync.ajax.post("/TeacherManage/TeacherAddXls", parameters, function (data) {
    //            if (data.State) {
    //                $("#divdata").setTemplateURL("/Scripts/template/manage/system_TeacherAddXls.htm");
    //                $("#divdata").processTemplate(data.Result);
    //            }
    //        });
    //    });
    $("#TeacherImport").die().live("click", function () {
        parameters = {};
        $.sync.ajax.post("/TeacherManage/TeacherAddXls", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_TeacherAddXls.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });
    $("#btnImport_TeacherXls").die().live("click", function () {
        if ($("#txtSheet").val().trim().length <= 0) {
            $.dialog.alert("请正确填写！");
            return false;
        }
        if ($("#upXls").val().trim().length <= 0) {
            $.dialog.alert("请正确选择文件！");
            return false;
        }
        var options = {
            success: function (data, statusText, xhr, $form) {
                data = JSON.parse(data);
                $.dialog.alert(data.Msg);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.write(XMLHttpRequest.responseText);
                console.log(textStatus);
                console.log(errorThrown);
            }
        };
        $("#form_update").ajaxForm(options);
    });
    $("#btnBackTeacher").die().live("click", function () {
        $("#TeacherList").click();
    });

    //导入院系

    $("#btnAddDepXls").die().live("click", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool_yx").val();
        $.sync.ajax.post("/SystemManage/DepAddXls", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_DepartmentXls.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });

    $("#btnImport_DepartmentXls").die().live("click", function () {
        if ($("#txtSheet").val().trim().length <= 0) {
            $.dialog.alert("请正确填写！");
            return false;
        }
        if ($("#upXls").val().trim().length <= 0) {
            $.dialog.alert("请正确选择文件！");
            return false;
        }
        var options = {
            success: function (data, statusText, xhr, $form) {
                data = JSON.parse(data);
                $.dialog.alert(data.Msg);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                document.write(XMLHttpRequest.responseText);
                console.log(textStatus);
                console.log(errorThrown);
            }
        };
        $("#form_depupdate").ajaxForm(options);
    });

    $("#btnBackDepartment").die().live("click", function () {
        $("#Department").click();
    });
    ///---------批量导入---结束--------------------------

    ///---------批量生成---开始--------------------------
    $("#TeacherAddMore").click(function () {
        parameters = {};
        $.sync.ajax.post("/TeacherManage/TeacherAddMore", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_TeacherAddMore.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });
    //    var parameters = {};
    //   
    //    parameters.facid = $("#ddlFaculty_teachermore").val();
    //    $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
    //        if (data.State) {
    //            $("#ddlDepartment_teachermore").setTemplateURL("/Scripts/template/manage/ddl.htm");
    //            $("#ddlDepartment_teachermore").processTemplate(data.Result);
    //        } else {
    //            $("#ddlDepartment_teachermore").empty();
    //            $("#ddlDepartment_teachermore").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
    //        }
    //    });

    //学校改变
    $("#ddlSchool_teachermore").die().live("change", function () {
        parameters.schoolid = $("#ddlSchool_teachermore").val();
        $.sync.ajax.post("/DataManage/GetFaculty", parameters, function (data) {
            if (data.State) {
                $("#ddlFaculty_teachermore").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlFaculty_teachermore").processTemplate(data.Result);
                parameters.facid = $("#ddlFaculty_teachermore").val();
                $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
                    if (data.State) {
                        $("#ddlDepartment_teachermore").setTemplateURL("/Scripts/template/manage/ddl.htm");
                        $("#ddlDepartment_teachermore").processTemplate(data.Result);
                    } else {
                        $("#ddlDepartment_teachermore").empty();
                        $("#ddlDepartment_teachermore").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                    }
                });
            } else {
                $("#ddlFaculty").empty();
                $("#ddlFaculty").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                $("#ddlDepartment").empty();
                $("#ddlDepartment").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });
    });
    //院系改变
    $("#ddlFaculty_teachermore").die().live("change", function () {
        parameters = {};
        parameters.facid = $("#ddlFaculty_teachermore").val();
        $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
            if (data.State) {
                $("#ddlDepartment_teachermore").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlDepartment_teachermore").processTemplate(data.Result);
            } else {
                $("#ddlDepartment_teachermore").empty();
                $("#ddlDepartment_teachermore").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });
    });

    //生成
    $("#btnCreate_TeacherMore").die().live("click", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool_teachermore").val();
        parameters.facultyid = $("#ddlFaculty_teachermore").val();
        parameters.departmentid = $("#ddlDepartment_teachermore").val();
        if (parameters.facultyid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加学院！");
            return false;
        } else if (parameters.departmentid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加系别！");
            return false;
        }
        var prefix = $("#txtPrefix").val().trim();
        var count = $("#txtCount").val().trim();
        var regP = /^[A-Za-z0-9_]+$/;
        var regC = /^[1-9][0-9]*$/;
        if (regP.test(prefix) && prefix.length > 0) {
            parameters.prefix = prefix;
            $("#lbPreMsg").text("");
        } else {
            $("#lbPreMsg").text("错误字符！");
        }
        if (regC.test(count) && count.length > 0) {
            if (count > 200) {
                $.dialog.alert("输入200以内数字！");
                return false;
            } else {
                parameters.count = count;
                $("#lbCouMsg").text("");
            }
        } else {
            $("#lbCouMsg").text("错误数字！");
            return false;
        }
        $.sync.ajax.post("/TeacherManage/AddMoreTeacher", parameters, function (data) {
            if (data.State) {
                art.dialog({
                    id: 'testID',
                    content: data.Msg,
                    button: [
        {
            name: '确定',
            callback: function () {
                // window.location = "/TeacherManage/TeacherList";
            },
            focus: true
        }
    ]
                });
            };
        });
    });
    ///---------批量生成---结束--------------------------


    ///---------添加类别---开始--------------------------
    $("#KindAdd").click(function () {
        parameters = {};
        $.sync.ajax.post("/TeacherManage/KindAdd", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_KindAdd.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });
    var parameters = {};
    //确定
    $("#btnAdd_TeacherKindd").die().live("click", function () {
        parameters = {};
        parameters.kind = $("input[name='rKind']:checked").val();
        parameters.name = $("#txtName").val().trim();
        if (parameters.name.length <= 0) {
            return false;
        } else {
            $.sync.ajax.post("/TeacherManage/TeaKindAdd", parameters, function (data) {
                if (data.State) {
                    $.dialog.tips(data.Msg);
                } else {
                    $("#lbError").text(data.Msg);
                }
            });
        }
    });
    //取消
    $("#btnEsc_TeacherKindd").die().live("click", function () {
        $("#KindAdd").click();
    });

    $("#btnAddNewKind").die().live("click", function () {
        $.dialog({
            content: document.getElementById('divAddPostDiv'),
            id: 'EF07677',
            title: "添加职务"
        });
    });

    //确认添加职务
    $("#btnNewPost").die().live("click", function () {
        parameters = {};
        parameters.kind = "Post";
        parameters.name = $("#TxtNewPostName").val();
        if (parameters.name.length == 0) {
            $.dialog.tips("职务名称不能为空！");
            return false;
        } else {
            $.sync.ajax.post("/TeacherManage/TeaKindAdd", parameters, function (data) {
                if (data.State) {
                    $.dialog.list["EF07677"].close();
                    $.dialog.tips(data.Msg);
                    $("#KindList").click();
                }
            });
        }

    });
    ///---------添加类别---结束--------------------------



    ///---------类别列表---开始--------------------------
    $("#KindList").click(function () {
        parameters = {};
        $.sync.ajax.post("/TeacherManage/KindList", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_KindList.htm");
                $("#divdata").processTemplate(data.Result);

            }
        });
        //        $.sync.ajax.post("/DataManage/GetJob", parameters, function (data) {
        //            if (data.State) {
        //                $("#divKind").setTemplateURL("/Scripts/template/manage/job.htm");
        //                $("#divKind").processTemplate(data.Result);
        //            }
        //        });

        $.sync.ajax.post("/DataManage/GetPost", parameters, function (data) {
            if (data.State) {
                $("#divKind").setTemplateURL("/Scripts/template/manage/post.htm");
                $("#divKind").processTemplate(data.Result);
            }
        });

    });
    var parameters = {};

    //单选改变，读取相应列表
    $("input[name='rKind']").die().live("click", function () {
        var kind = $("input[name='rKind']:checked").val();
        if (kind == "Job") {
            $.sync.ajax.post("/DataManage/GetJob", parameters, function (data) {
                if (data.State) {
                    $("#divKind").setTemplateURL("/Scripts/template/manage/job.htm");
                    $("#divKind").processTemplate(data.Result);
                }
            });
        } else if (kind == "Pro") {
            $.sync.ajax.post("/DataManage/GetPro", parameters, function (data) {
                if (data.State) {
                    $("#divKind").setTemplateURL("/Scripts/template/manage/pro.htm");
                    $("#divKind").processTemplate(data.Result);
                }
            });
        } else if (kind == "Post") {
            $.sync.ajax.post("/DataManage/GetPost", parameters, function (data) {
                if (data.State) {
                    $("#divKind").setTemplateURL("/Scripts/template/manage/post.htm");
                    $("#divKind").processTemplate(data.Result);
                }
            });
        }
    });

    //所有Job删除
    $(".delJob").die().die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/TeacherManage/DeleteJob", parameters, function (data) {
                if (data.State) {
                    $("#rJob").click();
                }
            });
        });
    });

    //所有Pro删除
    $(".delPro").die().die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/TeacherManage/DeletePro", parameters, function (data) {
                if (data.State) {
                    $("#rPro").click();
                }
            });
        });
    });

    //所有Post删除
    $(".delPost").die().die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/TeacherManage/DeletePost", parameters, function (data) {
                if (data.State) {
                    $("#KindList").click();
                }
            });
        });
    });

    //变色
    $("#divKind tr:gt(0)").die().live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });

    //点击修改
    $(".edit_lb").die().live("click", function () {
        if ($(this).attr("typeid") == "job") {
            $("#u_job").val($(this).attr("jobname"));
            $("#job_id").attr("jobid", $(this).attr("jobid"));
            $.dialog({
                content: document.getElementById('job_add_s'),
                id: 'EF0002',
                title: "职称类别"
            });
        } else if ($(this).attr("typeid") == "pro") {
            $("#u_pro").val($(this).attr("proname"));
            $("#pro_id").attr("proid", $(this).attr("proid"));
            $.dialog({
                content: document.getElementById('pro_add_s'),
                id: 'EF0003',
                title: "职业类别"
            });
        } else {
            $("#u_post").val($(this).attr("postname"));
            $("#post_id").attr("postid", $(this).attr("postid"));
            $.dialog({
                content: document.getElementById('post_add_s'),
                id: 'EF0004',
                title: "职务类别"
            });
        }
    });
    $(".btn_ok").die().live("click", function () {
        if ($(this).attr("typeid") == "job") {
            parameters = {};
            parameters.id = $(this).attr("jobid");
            parameters.name = $("#u_job").val().trim();
            if (parameters.name.length <= 0) {
                return false;
            } else {
                $.sync.ajax.post("/TeacherManage/EditJob", parameters, function (data) {
                    if (data.State) {
                        $.dialog.tips(data.Msg);
                        $.dialog.list["EF0002"].close();
                        parameters = {};
                        $.sync.ajax.post("/DataManage/GetJob", parameters, function (data) {
                            if (data.State) {
                                $("#divKind").setTemplateURL("/Scripts/template/manage/job.htm");
                                $("#divKind").processTemplate(data.Result);
                            }
                        });
                    }
                });
            }
        } else if ($(this).attr("typeid") == "pro") {
            parameters = {};
            parameters.id = $(this).attr("proid");
            parameters.name = $("#u_pro").val().trim();
            if (parameters.name.length <= 0) {
                return false;
            } else {
                $.sync.ajax.post("/TeacherManage/EditProfession", parameters, function (data) {
                    if (data.State) {
                        $.dialog.tips(data.Msg);
                        $.dialog.list["EF0003"].close();
                        $.sync.ajax.post("/DataManage/GetPro", parameters, function (data) {
                            if (data.State) {
                                $("#divKind").setTemplateURL("/Scripts/template/manage/pro.htm");
                                $("#divKind").processTemplate(data.Result);
                            }
                        });
                    }
                });
            }
        } else {
            parameters = {};
            parameters.id = $(this).attr("postid");
            parameters.name = $("#u_post").val().trim();
            if (parameters.name.length <= 0) {
                return false;
            } else {
                $.sync.ajax.post("/TeacherManage/EditPost", parameters, function (data) {
                    if (data.State) {
                        $.dialog.tips(data.Msg);
                        $.dialog.list["EF0004"].close();
                        $.sync.ajax.post("/DataManage/GetPost", parameters, function (data) {
                            if (data.State) {
                                $("#divKind").setTemplateURL("/Scripts/template/manage/post.htm");
                                $("#divKind").processTemplate(data.Result);
                            }
                        });
                    }
                });
            }
        }

    });
    ///---------类别列表---结束--------------------------


    ///---------添加教师---开始--------------------------


    $("#TeacherAdd").die().live("click", function () {
        parameters = {};
        $.sync.ajax.post("/TeacherManage/TeacherAdd", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_TeacherAdd.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        parameters.facid = $("#ddlFaculty_addTeacher").val();
        $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
            if (data.State) {
                $("#ddlDepartment_addTeacher").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlDepartment_addTeacher").processTemplate(data.Result);    
            } else {
                $("#ddlDepartment_addTeacher").empty();
                $("#ddlDepartment_addTeacher").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });
    });


    //学校改变
    $("#ddlSchool_addTeacher").die().live("change", function () {
        parameters.schoolid = $("#ddlSchool_addTeacher").val();
        $.sync.ajax.post("/DataManage/GetFaculty", parameters, function (data) {
            if (data.State) {
                $("#ddlFaculty_addTeacher").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlFaculty_addTeacher").processTemplate(data.Result);
                parameters = {};
                parameters.facid = $("#ddlFaculty_addTeacher").val();
                $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
                    if (data.State) {
                        $("#ddlDepartment_addTeacher").setTemplateURL("/Scripts/template/manage/ddl.htm");
                        $("#ddlDepartment_addTeacher").processTemplate(data.Result);
                    } else {
                        $("#ddlDepartment_addTeacher").empty();
                        $("#ddlDepartment_addTeacher").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                    };
                });
            } else {
                $("#ddlFaculty_addTeacher").empty();
                $("#ddlFaculty_addTeacher").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                $("#ddlDepartment_addTeacher").empty();
                $("#ddlDepartment_addTeacher").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            };
        });
    });

    //学院改变
    $("#ddlFaculty_addTeacher").die().live("change", function () {
        parameters.facid = $("#ddlFaculty_addTeacher").val();
        $.sync.ajax.post("/DataManage/GetDepartment", parameters, function (data) {
            if (data.State) {
                $("#ddlDepartment_addTeacher").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlDepartment_addTeacher").processTemplate(data.Result);
            } else {
                $("#ddlDepartment_addTeacher").empty();
                $("#ddlDepartment_addTeacher").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            };
        });
    });

    //添加教师
    $("#btnAdd_addTeacher").die().live("click", function () {
        parameters = {};
        parameters.schoolid = $("#ddlSchool_addTeacher").val();
        parameters.loginname = $("#txtLoginName").val();
        if (parameters.loginname.trim().length <= 0) {
            $.dialog.tips("登录名不能为空！");
            return false;
        }
        if ($("#txtLoginPwd").val() != $("#txtSurePwd").val()) {
            $.dialog.tips("两次密码不一致！");
            return false;
        }
        parameters.pwd = $("#txtLoginPwd").val();
        if (parameters.pwd.trim().length <= 0) {
            $.dialog.tips("密码不能为空！");
            return false;
        }
        parameters.name = $("#txtName").val();
        if (parameters.name.trim().length <= 0) {
            $.dialog.tips("真实姓名不能为空！");
            return false;
        }
        parameters.birthday = $("#txtBirthday").val();
        parameters.facultyid = $("#ddlFaculty_addTeacher").val();
        parameters.departmentid = $("#ddlDepartment_addTeacher").val();
        if (parameters.facultyid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加学院！");
            return false;
        } else if (parameters.departmentid == "00000000-0000-0000-0000-000000000000") {
            $.dialog.tips("请选择或添加系别！");
            return false;
        }
        parameters.email = $("#txtEmail").val();
        parameters.phone = $("#txtPhone").val();
        parameters.sex = $("input[name='rSex']:checked").next("label").text().trim();
        parameters.jobid = $("#ddlJob_addTeacher").val();
        parameters.postid = $("#ddlPost_addTeacher").val();
        parameters.professionid = $("#ddlProfession_addTeacher").val();
        parameters.YNManager = $("input[name='YNManager']:checked").val();
        $.sync.ajax.post("/TeacherManage/AddTeacher", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
                $.sync.ajax.post("/TeacherManage/TeacherList", parameters, function (data) {
                    if (data.State) {
                        $("#divdata").setTemplateURL("/Scripts/template/manage/system_TeacherList.htm");
                        $("#divdata").processTemplate(data.Result);
                        $("#btnSearchUser").click();
                    }
                });

                // window.location.href = "/TeacherManage/TeacherList";
            };
        });
    });

    ///---------添加教师---结束--------------------------





    ///---------试用管理---开始--------------------------
    $("#SchoolList").click(function () {
        parameters = {};
        $.sync.ajax.post("/SystemManage/SchoolList", parameters, function (data) {
            if (data.State) {
                if (data.Identity != 5 && data.Identity != 6) {
                    $("#divdata").setTemplateURL("/Scripts/template/manage/system_SchoolList.htm");
                } else {
                    $("#divdata").setTemplateURL("/Scripts/template/manage/system_SchoolListTwo.htm");
                }
                $("#divdata").processTemplate(data.Result);
            }
        });
        //模块列表
        $.sync.ajax.post("/DataManage/GetModuleList", parameters, function (data) {
            if (data.State) {
                $("#divModules").setTemplateURL("/Scripts/template/manage/module1.htm");
                $("#divModules").processTemplate(data.Result);
            }
        });

        $("#btnSearchSchool").click();
    });




    //分页数据
    //学校信息列表
    $.cellpagebar.init({
        total: $("#hdCount").val(),
        pagesize: 10,
        container: $("#divBar")
    }, function (pageindex) {
        parameters = {};
        parameters.pageIndex = pageindex;
        parameters.sellerid = "00000000-0000-0000-0000-000000000000";
        parameters.name = "";
        $.sync.ajax.post("/DataManage/GetSchoolInfoList", parameters, function (data) {
            if (data.State) {
                $("#divSchoolList").setTemplateURL("/Scripts/template/manage/system_school.htm");
                $("#divSchoolList").processTemplate(data.Result);
            }
        });
    });

    $("#sellID").die().live("change", function () {
        $("#btnSearchSchool").click();
    });

    //添加按钮
    $("#btnAdd_school").die().live("click", function () {
        art.dialog({
            title: "添加试用学校",
            content: document.getElementById('divAddSchool'),
            id: 'EF893L'
        });
    });



    //确认添加学校按钮
    $("#btnAddSchool").die().live("click", function () {
        var parameters = {};
        if ($("#txtSchoolName").val().length == 0) {
            $.dialog.tips("学校名称不能为空！");
            return false;
        }
        if ($("#txtEndTime").val().length == 0) {
            $.dialog.tips("到期时间不能为空！");
            return false;
        }
        parameters.schoolname = $("#txtSchoolName").val();
        parameters.ids = [];
        $("#divModules").find("input[name=schoolmodule]:checked").each(function () {
            var a = {};
            var $tr = $(this).parent();
            a.ModuleId = $(this).val();
            var isck = $tr.find("input[name=ckTrialTag]").attr("checked");
            if (isck == "checked") {
                a.TrialTag = 1;
            }
            else {
                a.TrialTag = 0;
            }
            //a.TrialTag = $tr.find("input[name=ckTrialTag]").val();
            parameters.ids.push(a);
        });

        //        $("#divModules").find(":checkbox:checked").each(function () {
        //            parameters.ids.push($(this).val());
        //        });


        parameters.ips = $("#txtIP").val().replace("，", ",");
        parameters.end = $("#txtEndTime").val();
        parameters.remind = $("#txtRemind").val();
        parameters.tier = $("#ddlTier").val();
        parameters.sell = $("#ddlSeller_schoollist").val();
        parameters.trial = $("#ddlTrial_schoollist").val();
        parameters.province = $("#SelectProvince").val();

        if (parameters.schoolname.length > 0) {
            $.sync.ajax.post("/SystemManage/AddSchool", parameters, function (data) {
                if (!!data.State) {
                    $.dialog.tips(data.Msg);
                    $("#res").click();
                    $.dialog.list["EF893L"].close();
                    $("#btnSearchSchool").click();
                    $("#sSchoolName").text("").removeCss("color");
                } else {
                    $("#txtIP").val(data.Result);
                    $.dialog.alert(data.Msg);
                }
            });

        } else {
            $("#sSchoolName").text("不能为空！").css("color", "red");
        };
    });


    //删除学校按钮
    $(".del_school").die().live("click", function () {
        var $btnDel = $(this);
        $.dialog.confirm("是否确认删除？", function () {
            parameters = {};
            parameters.id = $btnDel.attr("dataid");
            $.sync.ajax.post("/SystemManage/DeleteSchool", parameters, function (data) {
                if (data.State) {
                    $.dialog.tips(data.Msg);
                    $("#btnSearchSchool").click();
                } else {
                    $.dialog.alert(data.Msg);
                }
            });
        });
    });


    //搜索学校按钮
    $("#btnSearchSchool").die().live("click", function () {
        var parameters = {};
        parameters.sellerid = $("#sellID option:selected").val();
        parameters.privinceID = $("#PrivinceID option:selected").val();
        parameters.name = $("#txtName").val();
        parameters.TrialTag = $("#Select_trialTag").val();
        parameters.Startime = $("#txt_startime").val();
        parameters.Endtime = $("#txt_endtime").val();
        $.sync.ajax.post("/DataManage/GetSchoolInfoListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 10,
                    container: $("#divBar")
                }, function (pageindex) {
                    //                    parameters = {};
                    parameters.pageIndex = pageindex;
                    //                    parameters.sellerid = $("#ddlSeller_schoollist").val();
                    //                    parameters.name = $("#txtName").val();
                    $.sync.ajax.post("/DataManage/GetSchoolInfoList", parameters, function (data) {
                        if (data.State) {
                            $("#divSchoolList").setTemplateURL("/Scripts/template/manage/system_school.htm");
                            $("#divSchoolList").processTemplate(data.Result);
                            if (data.Identity == 5 || data.Identity == 6) {
                                $("#divSchoolList [could=c]").hide();
                            }
                        }
                    });
                });
            }
        });
    });

    //变色
    $("#divSchoolList tr:gt(0)").die().live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });
    ////修改试用信息---------------------------
    $(".Edit_system_school").die().live("click", function () {
        parameters = {};
        parameters.id = $(this).attr("dataid");
        $.sync.ajax.post("/SystemManage/EditSchool", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_EditSchool.htm");
                $("#divdata").processTemplate(data.Result);
                // $("#hdSchoolId").val(parameters.id);
            }
        });
        //模块列表
        parameters = {};
        parameters.id = $(this).attr("dataid");
        $.sync.ajax.post("/DataManage/GetModules", parameters, function (data) {
            if (data.State) {
                $("#divModules").setTemplateURL("/Scripts/template/manage/module1.htm");
                $("#divModules").processTemplate(data.Result);
            }
        });
        //学校IP段
        parameters = {};
        parameters.schoolid = $(this).attr("dataid");
        $.sync.ajax.post("/DataManage/GetIpList", parameters, function (data) {
            if (data.State) {
                $("#txtIP").val(data.Result);
            }
        });
    });

    //确认
    $("#btnEditSchool").die().live("click", function () {
        var parameters = {};
        if ($("#txtSchoolName").val().length == 0) {
            $.dialog.tips("学校名称不能为空！");
            return false;
        }
        if ($("#txtEndTime").val().length == 0) {
            $.dialog.tips("到期时间不能为空！");
            return false;
        }
        parameters.id = $("#hdsId").val();
        parameters.schoolname = $("#txtSchoolName").val();
        parameters.ids = [];

        $("#divModules").find("input[name=schoolmodule]:checked").each(function () {
            var a = {};
            var $tr = $(this).parent();
            a.ModuleId = $(this).val();
            var isck = $tr.find("input[name=ckTrialTag]").attr("checked");
            if (isck == "checked") {
                a.TrialTag = 1;
            }
            else {
                a.TrialTag = 0;
            }
            parameters.ids.push(a);
        });




        //        $("#divModules").find(":checkbox:checked").each(function () {
        //            parameters.ids.push($(this).val());
        //        });

        parameters.ips = $("#txtIP").val().replace("，", ",");
        parameters.end = $("#txtEndTime").val();
        parameters.remind = $("#txtRemind").val();
        parameters.sellid = $("#ddlSeller_EditSchool").val();
        parameters.trial = $("#ddlTrial_EditSchool").val();
        parameters.province = $("#SelectProvince").val();

        $.sync.ajax.post("/SystemManage/EditSchools", parameters, function (data) {
            if (!!data.State) {
                $.dialog.tips(data.Msg);
                $("#btnEsc_sl").click();
            } else {
                parameters = {};
                parameters.schoolid = $("#hdId").val();
                $.sync.ajax.post("/DataManage/GetIpList", parameters, function (data) {
                    if (data.State) {
                        $("#txtIP").val(data.Result);
                    }
                });
                $.dialog.alert(data.Msg);
                $("#btnEsc_sl").click();
            }

        });
    });

    //取消
    $("#btnEsc_sl").die().live("click", function () {
        $("#SchoolList").click();
    });
    ///---------试用管理---结束--------------------------

    ///---------试用提醒---开始--------------------------
    //1
    $("#SchoolReminder").click(function () {
        parameters = {};
        // parameters.id = $(this).attr("schoolid");
        $.sync.ajax.post("/SystemManage/SchoolReminder", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/SchoolReminder_htm.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        $("#btnSearchReminder").click();
        //        parameters.Tier = -1;
        //        $.sync.ajax.post("/SystemManage/Reminder", parameters, function (data) {
        //            if (data.State) {
        //                $("#ReminderList").setTemplateURL("/Scripts/template/manage/Reminder_htm.htm");
        //                $("#ReminderList").processTemplate(data.Result);
        //            }
        //        });
    });
    $("#txt_endtime,#txt_startime").die().live("blur", function () {
        var date1 = $("#txt_endtime").val().replace("-", "/");
        var date2 = $("#txt_startime").val().replace("-", "/");
        var dt1 = new Date(date1);
        var dt2 = new Date(date2);
        if (dt1 < new Date() || dt2 > new Date()) {
            $("#Reminder_tier").val("");
            $("#Reminder_tier").attr("disabled", true);
        } else {
            $("#Reminder_tier").attr("disabled", false);
        }
    });
    //新搜索试用提醒
    $("#btnSearchReminder").die().live("click", function () {
        var date1 = $("#txt_endtime").val().replace("-", "/");
        var date2 = $("#txt_startime").val().replace("-", "/");
        var dt1 = new Date(date1);
        var dt2 = new Date(date2);
        if (dt1 < new Date() || dt2 > new Date()) {
            $("#Reminder_tier").val("");
            $("#Reminder_tier").attr("disabled", true);
        } else {
            $("#Reminder_tier").attr("disabled", false);
        }
        parameters = {};
        parameters.Tier = $("#Reminder_tier").val();
        parameters.TrialTag = $("#Select_trialTag").val();
        parameters.Startime = $("#txt_startime").val();
        parameters.Endtime = $("#txt_endtime").val();
        $.sync.ajax.post("/SystemManage/GetReminderCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 20,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.Tier = $("#Reminder_tier").val();
                    parameters.TrialTag = $("#Select_trialTag").val();
                    parameters.Startime = $("#txt_startime").val();
                    parameters.Endtime = $("#txt_endtime").val();
                    $.sync.ajax.post("/SystemManage/Reminder", parameters, function (data) {
                        if (data.State) {
                            $("#ReminderList").setTemplateURL("/Scripts/template/manage/Reminder_htm.htm");
                            $("#ReminderList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });
    //全选、取消
    $("#ckAll_school").die().live("click", function () {
        var state = $(this).attr("checked");
        $("#ReminderList :checkbox").attr("checked", !!state);
    });
    //所有启用
    $(".start_Reminder").die().live("click", function () {
        if ($(this).attr("state") == "已启用") {
            $.dialog.tips("已启用");
            return false;
        }
        var $btnStart = $(this);
        $.dialog.confirm("是否确认启用？", function () {
            parameters = {};
            parameters.id = $btnStart.attr("dataid");
            $.sync.ajax.post("/SystemManage/StartSchool", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchReminder").click();
                    $.dialog.tips("操作成功");
                }
            });
        });
    });

    //所有停用
    $(".stop_Reminder").die().live("click", function () {
        if ($(this).attr("state") == "已停用") {
            $.dialog.tips("已停用");
            return false;
        }
        var $btnStop = $(this);
        $.dialog.confirm("是否确认停用？", function () {
            parameters = {};
            parameters.id = $btnStop.attr("dataid");
            $.sync.ajax.post("/SystemManage/StopSchool", parameters, function (data) {
                if (data.State) {
                    $("#btnSearchReminder").click();
                    $.dialog.tips("操作成功");
                } else {
                    $.dialog.alert(data.Msg);
                }
            });
        });
    });
    //单选操作
    $("#btn_confirm").die().live("click", function () {
        parameters = {};
        parameters.ids = [];
        $("#ReminderList").find(":checkbox:checked").each(function () {
            parameters.ids.push($(this).val());
        });
        parameters.aff = $("input[name='groupAff']:checked").val();
        if (parameters.ids.length > 0) {
            $.sync.ajax.post("/SystemManage/AffirmSchool", parameters, function (data) {
                $("#ckAll").removeAttr("checked");
                $("#ckReverse").removeAttr("checked");
                $("#btnSearchReminder").click();
                $.dialog.tips(data.Msg);
            });
        };
    });
    ///---------试用提醒---结束--------------------------

    ///---------下载异常---开始--------------------------

    $("#DownException").click(function () {
        parameters = {};
        // parameters.id = $(this).attr("schoolid");
        $.sync.ajax.post("/SystemManage/DownException", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/DownException_htm.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        parameters.Tier = -1;
        $.sync.ajax.post("/SystemManage/Exception", parameters, function (data) {
            if (data.State) {
                $("#ExceptionList").setTemplateURL("/Scripts/template/manage/Exception_htm.htm");
                $("#ExceptionList").processTemplate(data.Result);
            }
        });
    });

    $("#Exception_tier").die().live("change", function () {
        parameters = {};
        parameters.Tier = $(this).val();
        $.sync.ajax.post("/SystemManage/Exception", parameters, function (data) {
            if (data.State) {
                $("#ExceptionList").setTemplateURL("/Scripts/template/manage/Exception_htm.htm");
                $("#ExceptionList").processTemplate(data.Result);
            }
        });
    });



    ///---------下载异常---结束--------------------------


    ///--------学校试用统计--------------开始-------------
    $("#ASchoollog").click(function () {
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        $.sync.ajax.post("/SystemManage/SchoolLog", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_schoollog.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });

    });
    $("#AschoolFH").click(function () {

        parameters = {};
        parameters.id = $(this).attr("schoolid");
        $.sync.ajax.post("/SystemManage/SchoolLog", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_schoollog.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });

        //----------------素材库
        //获取素材Ip段下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "0";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbRIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbRIp").processTemplate(data.Result);
            }
        });

        //获取素材分类下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "0";
        $.sync.ajax.post("/SystemManage/GetSubDetailsR", parameters, function (data) {
            if (data.State) {
                $("#lbRSub").setTemplateURL("/Scripts/template/manage/detailsR.htm");
                $("#lbRSub").processTemplate(data.Result);
            }
        });

        //获取素材月份下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "0";
        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
            if (data.State) {
                $("#lbRMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbRMonthCount").processTemplate(data.Result);
            }
        });

        //-------------医学库        
        //获取医学Ip段下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "1";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbMIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbMIp").processTemplate(data.Result);
            }
        });

        //获取医学分类下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "1";
        $.sync.ajax.post("/SystemManage/GetSubDetailsM", parameters, function (data) {
            if (data.State) {
                $("#lbMSub").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbMSub").processTemplate(data.Result);
            }
        });

        //获取医学月份下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "1";
        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
            if (data.State) {
                $("#lbMMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbMMonthCount").processTemplate(data.Result);
            }
        });

        //--------------------试题库
        //获取试题库Ip段下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "3";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbTIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbTIp").processTemplate(data.Result);
            }
        });

        //获取试题月份下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "3";
        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
            if (data.State) {
                $("#lbTMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbTMonthCount").processTemplate(data.Result);
            }
        });
        //---------------------模考库
        //获取模考库Ip段下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "4";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbEIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbEIp").processTemplate(data.Result);
            }
        });

        //获取模考分类详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "4";
        $.sync.ajax.post("/SystemManage/GetSubDetailsE", parameters, function (data) {
            if (data.State) {
                $("#lbESub").setTemplateURL("/Scripts/template/manage/detailsR.htm");
                $("#lbESub").processTemplate(data.Result);
            }
        });

        //获取模考月份下载详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "4";
        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
            if (data.State) {
                $("#lbEMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbEMonthCount").processTemplate(data.Result);
            }
        });

        //-------------精品课程库
        //获取精品课程库Ip段详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "2";
        $.sync.ajax.post("/SystemManage/GetIpDetails", parameters, function (data) {
            if (data.State) {
                $("#lbQIp").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbQIp").processTemplate(data.Result);
            }
        });

        //获取精品课程分类详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "2";
        $.sync.ajax.post("/SystemManage/GetSubDetailsQ", parameters, function (data) {
            if (data.State) {
                $("#lbQSub").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbQSub").processTemplate(data.Result);
            }
        });

        //获取精品课程月份详情
        parameters = {};
        parameters.id = $(this).attr("schoolid");
        parameters.tag = "2";
        $.sync.ajax.post("/SystemManage/GetMonthDetails", parameters, function (data) {
            if (data.State) {
                $("#lbQMonthCount").setTemplateURL("/Scripts/template/manage/details.htm");
                $("#lbQMonthCount").processTemplate(data.Result);
            }
        });
    });



    ///--------学校试用统计--------------开始-------------

    ///-------------角色管理--------------开始-----------
    $("#role").click(function () {
        parameters = {};
        $.sync.ajax.post("/SystemManage/RoleList", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_rolelist.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });
    //添加角色
    $("#btnAddRole").die().live("click", function () {
        $.dialog({
            content: document.getElementById('divAddroleDiv'),
            id: 'EF7677',
            title: "添加角色"
        });
    });

    //确认添加角色
    $("#btnSurerole").die().live("click", function () {
        parameters = {};
        parameters.rolename = $("#TxtRoleName").val();
        if (parameters.rolename.length == 0) {
            $.dialog.tips("角色名称不能为空！");
            return false;
        } else {
            $.sync.ajax.post("/SystemManage/Addrole", parameters, function (data) {
                if (data.State) {
                    $.dialog.list["EF7677"].close();
                    $.dialog.tips(data.Msg);
                    $("#role").click();
                }
            });
        }

    });
    //删除角色
    $(".del_role").die().live("click", function () {
        parameters = {};
        parameters.roleid = $(this).attr("dataid");
        $.dialog.confirm("是否确认删除？", function () {
            $.sync.ajax.post("/SystemManage/delrole", parameters, function (data) {
                if (data.State) {
                    $.dialog.tips(data.Msg);
                    $("#role").click();
                }
            });
        });
    });

    //修改角色
    $(".edit_role").die().live("click", function () {
        $.dialog({
            content: document.getElementById('DivUpRoleDiv'),
            id: 'EF7177',
            title: "修改角色"
        });
        $("#btnRoleUP").attr("dataid", $(this).attr("dataid"));
    });

    //确认修改角色
    $("#btnRoleUP").die().live("click", function () {
        parameters = {};
        parameters.rolename = $("#TxtRoleNameUp").val();
        parameters.roleid = $(this).attr("dataid");
        if (parameters.rolename.length == 0) {
            $.dialog.tips("角色名称不能为空！");
            return false;
        } else {
            $.sync.ajax.post("/SystemManage/Uprole", parameters, function (data) {
                if (data.State) {
                    $.dialog.list["EF7177"].close();
                    $.dialog.tips(data.Msg);
                    $("#role").click();
                }
            });
        }

    });
    ///------------角色管理---------------结束------------

    ////------------------销售管理---------------开始--------------- 
    $("#Sell").click(function () {
        parameters = {};
        $.sync.ajax.post("/SystemManage/sellList", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_selllist.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });
    //添加销售
    $("#btnAddSell").die().live("click", function () {
        $.dialog({
            content: document.getElementById('divAddsellDiv'),
            id: 'EF1677',
            title: "添加销售"
        });
    });

    //确认添加销售
    $("#btnSuresell").die().live("click", function () {
        parameters = {};
        parameters.sellname = $("#TxtSellName").val();
        parameters.sellloginname = $("#zhCreate").val();
        parameters.sellloginpwd = $("#mmCreate").val();
        if (parameters.sellname.length == 0 || parameters.sellloginname.length == 0 || parameters.sellloginpwd.length == 0) {
            $.dialog.tips("销售名称不能为空！");
            return false;
        } else {
            $.sync.ajax.post("/SystemManage/Addsell", parameters, function (data) {
                if (data.State) {
                    $.dialog.list["EF1677"].close();
                    $.dialog.tips(data.Msg);
                    $("#Sell").click();
                }
            });
        }

    });
    //删除销售
    $(".del_sell").die().live("click", function () {
        parameters = {};
        parameters.sellid = $(this).attr("dataid");
        $.dialog.confirm("是否确认删除？", function () {
            $.sync.ajax.post("/SystemManage/delsell", parameters, function (data) {
                if (data.State) {
                    $.dialog.tips(data.Msg);
                    $("#Sell").click();
                }
            });
        });
    });

    //修改销售
    $(".edit_sell").die().live("click", function () {
        $.dialog({
            content: document.getElementById('DivUpSellDiv'),
            id: 'EF1177',
            title: "修改销售"
        });
        $("#btnSellUP").attr("dataid", $(this).attr("dataid"));
    });

    //确认修改销售
    $("#btnSellUP").die().live("click", function () {
        parameters = {};
        parameters.sellname = $("#TxtSellNameUp").val();
        parameters.zhUpdate = $("#zhUpdate").val();
        parameters.mmUpdate = $("#mmUpdate").val();
        parameters.sellid = $(this).attr("dataid");
        if (parameters.sellname.length == 0 || parameters.zhUpdate.length == 0 || parameters.mmUpdate.length == 0) {
            $.dialog.tips("销售名称不能为空！");
            return false;
        } else {
            $.sync.ajax.post("/SystemManage/Upsell", parameters, function (data) {
                if (data.State) {
                    $.dialog.list["EF1177"].close();
                    $.dialog.tips(data.Msg);
                    $("#Sell").click();
                }
            });
        }

    });
    ////------------------销售管理--------------结束--------------


    ////------------------菜单管理---------------开始--------------- 
    $("#Menu").click(function () {
        parameters = {};
        $.sync.ajax.post("/SystemManage/menuList", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/system_menulist.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });
    //添加菜单
    $("#btnAddMenu").die().live("click", function () {
        $.dialog({
            content: document.getElementById('divAddmenuDiv'),
            id: 'EF1672',
            title: "添加菜单"
        });
    });

    //确认添加菜单
    $("#btnSuremenu").die().live("click", function () {
        parameters = {};
        parameters.menuname = $("#TxtMenuName").val();
        if (parameters.menuname.length == 0) {
            $.dialog.tips("菜单名称不能为空！");
            return false;
        } else {
            $.sync.ajax.post("/SystemManage/Addmenu", parameters, function (data) {
                if (data.State) {
                    $.dialog.list["EF1672"].close();
                    $.dialog.tips(data.Msg);
                    $("#Menu").click();
                }
            });
        }
    });
    //删除菜单
    $(".del_menu").die().live("click", function () {
        parameters = {};
        parameters.menuid = $(this).attr("dataid");
        $.dialog.confirm("是否确认删除？", function () {
            $.sync.ajax.post("/SystemManage/delmenu", parameters, function (data) {
                if (data.State) {
                    $.dialog.tips(data.Msg);
                    $("#Menu").click();
                }
            });
        });
    });

    //修改菜单
    $(".edit_menu").die().live("click", function () {
        $.dialog({
            content: document.getElementById('DivUpMenuDiv'),
            id: 'EF1171',
            title: "修改菜单"
        });
        $("#btnMenuUP").attr("dataid", $(this).attr("dataid"));
    });

    //确认修改菜单
    $("#btMenuUP").die().live("click", function () {
        parameters = {};
        parameters.menuname = $("#TxtMenuNameUp").val();
        parameters.menuid = $(this).attr("dataid");
        if (parameters.menuname.length == 0) {
            $.dialog.tips("菜单名称不能为空！");
            return false;
        } else {
            $.sync.ajax.post("/SystemManage/Upmenu", parameters, function (data) {
                if (data.State) {
                    $.dialog.list["EF1171"].close();
                    $.dialog.tips(data.Msg);
                    $("#Menu").click();
                }
            });
        }
    });
    ////------------------菜单管理--------------结束--------------


    //试用报告
    $("#Trialreport").die().live("click", function () {
        par = {};
        par.schoolid = $(this).attr("schoolid");
        window.location.href = "/SystemManage/Trialreport?schoolid=" + par.schoolid;
        //        $.sync.ajax.post("/SystemManage/Trialreport", parameters, function (data) {
        //            //            if (data.State) { 
        //            //                $.dialog.tips(data.Msg); 
        //            //            }
        //        });
    });
    //试用医学报告
    $("#TrialreportMedical").die().live("click", function () {
        par = {};
        par.schoolid = $(this).attr("schoolid");
        window.location.href = "/SystemManage/TrialreportMedical?schoolid=" + par.schoolid;
    });
    //试用试题库报告
    $("#TrialreporTest").die().live("click", function () {
        par = {};
        par.schoolid = $(this).attr("schoolid");
        window.location.href = "/SystemManage/TrialreportTest?schoolid=" + par.schoolid;
    });
    //试用模拟考报告
    $("#TrialreporExam").die().live("click", function () {
        par = {};
        par.schoolid = $(this).attr("schoolid");
        window.location.href = "/SystemManage/TrialreportExam?schoolid=" + par.schoolid;
    });
    //试用精品课堂报告
    $("#TrialreporQualityCourse").die().live("click", function () {
        par = {};
        par.schoolid = $(this).attr("schoolid");
        window.location.href = "/SystemManage/TrialreportQualityCourse?schoolid=" + par.schoolid;
    });

});