$(function () {
    var parameters = {};
    parameters.Id = "59eb93dd-2711-4f34-9fca-adf9df89f22e";
    $("#59eb93dd-2711-4f34-9fca-adf9df89f22e").attr("class", "clickon");
    $.sync.ajax.post("/DataManage/GetLeftMenu", parameters, function (data) {
        if (data.State) {
            $("#divMenu").setTemplateURL("/Scripts/template/manage/left.htm");
            $("#divMenu").processTemplate(data.Result);
        }
    });

    ///素材列表----开始------------------

    parameters = {};
    $("#Resource_list").click(function () {
        $.sync.ajax.post("/ResourceManage/Resource_list", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/resource_list.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        var parameters = {};
        //素材分类
        $.sync.ajax.post("/DataManage/GetCategorySubject", parameters, function (data) {
            if (data.State) {
                $("#ddlcs_resourcelist").setTemplateURL("/Scripts/template/manage/ddlcs.htm");
                $("#ddlcs_resourcelist").processTemplate(data.Result);
            }
        });
        $("#btnSearchResource").click();
    });
    //上传
    $("#btnUploadResource").die().live("click", function () {
        var _url = "/ResourceManage/ResourceMaterialsUpload?BaseType=" + $(this).attr("basetype");
        $("#divdata").load(encodeURI(_url));
    });
    //返回
    $("#btnupload_back").die().live("click", function () {
        var back = $("#BaseType").val();
        if (back == "0") {
            $("#Resource_list").click();
        }
        else {
            $("#Course_list").click();
        };
    });
    //分类改变选择素材专业
    $("#ddlcs_resourcelist").die().live("change", function () {
        parameters = {};
        parameters.subid = $("#ddlcs_resourcelist").val();
        if (parameters.subid == null || parameters.subid == undefined || parameters.subid == "") {
            $("#ddlSpe_resourcelist").empty();
            $("#ddlSpe_resourcelist").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
        }
        else {
            $.sync.ajax.post("/DataManage/GetSpeciality", parameters, function (data) {
                if (data.State) {
                    $("#ddlSpe_resourcelist").setTemplateURL("/Scripts/template/manage/ddlstk.htm");
                    $("#ddlSpe_resourcelist").processTemplate(data.Result);
                } else {
                    $("#ddlSpe_resourcelist").empty();
                    $("#ddlSpe_resourcelist").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
                }
            });
        }
    });

    $("#ddlSpe_resourcelist,#ddlType_resourcelist").die().live("change", function () {
        $("#btnSearchResource").click();
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
        parameters.csid = $("#ddlcs_resourcelist").val();
        parameters.specialityid = $("#ddlSpe_resourcelist").val();
        parameters.typeid = $("#ddlType_resourcelist").val();
        $.sync.ajax.post("/DataManage/GetResourceList", parameters, function (data) {
            if (data.State) {
                $("#divResourceList").setTemplateURL("/Scripts/template/manage/resource.htm");
                $("#divResourceList").processTemplate(data.Result);
            }
        });
    });

    //搜索按钮
    $("#btnSearchResource").die().live("click", function () {
        parameters = {};
        parameters.csid = $("#ddlcs_resourcelist").val();
        parameters.specialityid = $("#ddlSpe_resourcelist").val();
        parameters.typeid = $("#ddlType_resourcelist").val();
        parameters.name = $("#txtName").val();
        $.sync.ajax.post("/DataManage/GetResourceListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 50,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.csid = $("#ddlcs_resourcelist").val();
                    parameters.specialityid = $("#ddlSpe_resourcelist").val();
                    parameters.typeid = $("#ddlType_resourcelist").val();
                    parameters.name = $("#txtName").val();
                    $.sync.ajax.post("/DataManage/GetResourceList", parameters, function (data) {
                        if (data.State) {
                            $("#divResourceList").setTemplateURL("/Scripts/template/manage/resource.htm");
                            $("#divResourceList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });

    //全选、取消
    $("#ckAll").die().live("click", function () {
        var state = $(this).attr("checked");
        $("#divResourceList :checkbox").attr("checked", !!state);
    });

    //所有删除
    $(".del_resourcelist").die().live("click", function () {
        var $btnDel = $(this);
        parameters = {};
        parameters.guidlist = [];
        parameters.guidlist.push($btnDel.attr("dataid"));
        $.dialog({
            id: 'deleteResource',
            content: '是否将资源放入素材审核？',
            button: [
            {
                name: '确定',
                callback: function () {
                    parameters.flag = "softdelete";
                    $.sync.ajax.post("/ResourceManage/DeleteResource", parameters, function (data) {
                        if (data.State) {
                            $.dialog.tips("资源已经放入素材审核!");
                            $.dialog.list["deleteResource"].close();
                            $("#btnSearchResource").click();
                        }
                    });
                },
                focus: true
            },
            {
                name: '直接删除',
                callback: function () {
                    if (confirm("是否彻底删除资源?")) {
                        parameters.flag = "delete";
                        $.sync.ajax.post("/ResourceManage/DeleteResource", parameters, function (data) {
                            if (data.State) {
                                $.dialog.tips("资源已经被删除!");
                                $.dialog.list["deleteResource"].close();
                                $("#btnSearchResource").click();
                            }
                        });
                    }
                }
            },
            {
                name: '取消'
            }
        ]
        });
    });

    //删除选择
    $("#btnDelete_resourcelist").die().live("click", function () {
        parameters = {};
        parameters.guidlist = [];
        $("#divResourceList").find(":checkbox:checked").each(function () {
            parameters.guidlist.push($(this).val());
        });
        if (parameters.guidlist.length <= 0) {
            $.dialog.tips("没有选中素材！");
            return false;
        };
        $.dialog({
            id: 'deleteResources',
            content: '是否将资源放入素材审核？',
            button: [
            {
                name: '确定',
                callback: function () {
                    parameters.flag = "softdelete";
                    $.sync.ajax.post("/ResourceManage/DeleteResource", parameters, function (data) {
                        if (data.State) {
                            $.dialog.tips("资源已经放入素材审核!");
                            $.dialog.list["deleteResources"].close();
                            $("#btnSearchResource").click();
                        }
                    });
                },
                focus: true
            },
            {
                name: '直接删除',
                callback: function () {
                    if (confirm("是否彻底删除资源?")) {
                        parameters.flag = "delete";
                        $.sync.ajax.post("/ResourceManage/DeleteResource", parameters, function (data) {
                            if (data.State) {
                                $.dialog.tips("资源已经被删除!");
                                $.dialog.list["deleteResources"].close();
                                $("#btnSearchResource").click();
                            }
                        });
                    }
                }
            },
            {
                name: '取消'
            }
        ]
        });

    });

    //    //批量修改
    //    $("#btnEditSelected").bind("click", function () {
    //        var data = [];
    //        $("#divResourceList").find(":checkbox:checked").each(function () {
    //            data.push($(this).val());
    //        });
    //        if (data.length > 0) {
    //            $("#hdData").val(JSON.stringify(data));
    //            $("#form1").submit();
    //        } else {
    //            $.dialog.tips("没有选中素材！");
    //        }
    //    });

    //        //预览
    //        $(".preview").die().live("click", function () {
    //            var data = [];
    //            var id = $(this).attr("id");
    //            $("#divResourceList :checkbox[value='" + id + "']").attr("checked", "checked");
    //            $("#divResourceList").find(":checkbox:checked").each(function () {
    //                data.push($(this).val());
    //            });
    //            $("#hdData2").val(JSON.stringify(data));
    //            $("#form2").submit();

    //        });
    //变色
    $("#divResourceList tr:gt(0)").die().live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });
    ///素材列表----修改------------------
    $(".edit_resourcelist").die().live("click", function () {
        parameters = {};
        parameters.id = $(this).attr("dataid");
        $.sync.ajax.post("/ResourceManage/Resource_EditResource", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/resource_EditResource.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        //        parameters = {}
        //        parameters.resourceid = $("#hdId").val();
        //        $.sync.ajax.post("/DataManage/GetResCategorySubject", parameters, function (data) {
        //            if (data.State) {
        //                $("#ddlcs_Editresource").setTemplateURL("/Scripts/template/manage/ddlcsc.htm");
        //                $("#ddlcs_Editresource").processTemplate(data.Result);
        //            }
        //        });
    });
    //素材分类 

    //分类改变选择素材专业
    $("#ddlcs_Editresource").die().live("change", function () {
        parameters = {};
        parameters.subid = $("#ddlcs_Editresource").val();
        $.sync.ajax.post("/DataManage/GetSpeciality", parameters, function (data) {
            if (data.State) {
                $("#ddlSpe_Editresource").setTemplateURL("/Scripts/template/manage/ddlstkc.htm");
                $("#ddlSpe_Editresource").processTemplate(data.Result);
                $("#ddlSpe_Editresource").get(0).selectedIndex = 0;
                parameters = {};
                parameters.speid = $("#ddlSpe_Editresource").val();
                $.sync.ajax.post("/DataManage/GetKnowledge", parameters, function (data) {
                    if (data.State) {
                        $("#ddlKnow_Editresource").setTemplateURL("/Scripts/template/manage/ddlstkc.htm");
                        $("#ddlKnow_Editresource").processTemplate(data.Result);
                        $("#ddlKnow_Editresource").get(0).selectedIndex = 0;
                    } else {
                        $("#ddlKnow_Editresource").empty();
                        $("#ddlKnow_Editresource").append("<option value='00000000-0000-0000-0000-000000000000' selected='selected'>请选择</option>");
                    }
                });
            } else {
                $("#ddlSpe_Editresource").empty();
                $("#ddlSpe_Editresource").append("<option value='00000000-0000-0000-0000-000000000000' selected='selected'>请选择</option>");
            }
        });
    });

    //专业改变选择素材知识点
    $("#ddlSpe_Editresource").die().live("change", function () {
        parameters = {};
        parameters.speid = $("#ddlSpe_Editresource").val();
        $.sync.ajax.post("/DataManage/GetKnowledge", parameters, function (data) {
            if (data.State) {
                $("#ddlKnow_Editresource").setTemplateURL("/Scripts/template/manage/ddlstkc.htm");
                $("#ddlKnow_Editresource").processTemplate(data.Result);
                $("#ddlKnow_Editresource").get(0).selectedIndex = 0;
            } else {
                $("#ddlKnow_Editresource").empty();
                $("#ddlKnow_Editresource").append("<option value='00000000-0000-0000-0000-000000000000' selected='selected'>请选择</option>");
            }
        });
    });

    //保存按钮
    $("#btnSure_Editresource").die().live("click", function () {
        parameters = {};
        parameters.resid = $("#hdId").val();
        parameters.name = $("#txtResourceName").val();
        // parameters.cateid = $("#ddlcs_Editresource option:selected").parent("optgroup").attr("id");
        parameters.subid = $("#ddlcs_Editresource").val();
        parameters.speid = $("#ddlSpe_Editresource").val();
        parameters.typeid = $("#ddlType_Editresource").val();
        // parameters.jp = $("#ddlBoutiqueGrade_Editresource").val();
        parameters.fescribe = $("#txtFescribe").val();
        parameters.keys = $("#txtKey").val();
        if (parameters.name.trim().length <= 0) {
            $.dialog.tips("素材名称不能为空！");
            return false;
        }
        $.sync.ajax.post("/ResourceManage/EditRes", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
                // $("#btnEsc_Editresource").click();
            }
        });
    });

    //返回按钮
    $("#btnEsc_Editresource").die().live("click", function () {
        $("#Resource_list").click();
    });

    ///素材列表----结束------------------



    ///素材审核----开始------------------
    $("#AuditResource").click(function () {
        $.sync.ajax.post("/ResourceManage/Resource_AuditResource", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/resource_AuditResource.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        parameters = {};
        //素材分类
        $.sync.ajax.post("/DataManage/GetCategorySubject", parameters, function (data) {
            if (data.State) {
                $("#ddlcs_auditresource").setTemplateURL("/Scripts/template/manage/ddlcs.htm");
                $("#ddlcs_auditresource").processTemplate(data.Result);
            }
        });
        $("#btnSearchResource_audit").click();
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
        parameters.csid = $("#ddlcs_auditresource").val();
        parameters.specialityid = $("#ddlSpe_auditresource").val();
        parameters.typeid = $("#ddlType_auditresource").val();
        $.sync.ajax.post("/DataManage/GetDelResourceList", parameters, function (data) {
            if (data.State) {
                $("#divResourceList").setTemplateURL("/Scripts/template/manage/auditresource.htm");
                $("#divResourceList").processTemplate(data.Result);
            }
        });
    });

    //搜索按钮
    $("#btnSearchResource_audit").die().live("click", function () {
        parameters = {};
        parameters.csid = $("#ddlcs_auditresource").val();
        parameters.specialityid = $("#ddlSpe_auditresource").val();
        parameters.typeid = $("#ddlType_auditresource").val();
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
                    parameters.csid = $("#ddlcs_auditresource").val();
                    parameters.specialityid = $("#ddlSpe_auditresource").val();
                    parameters.typeid = $("#ddlType_auditresource").val();
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
    $("#ddlcs_auditresource").die().live("change", function () {
        parameters = {};
        parameters.subid = $("#ddlcs_auditresource").val();
        $.sync.ajax.post("/DataManage/GetSpeciality", parameters, function (data) {
            if (data.State) {
                $("#ddlSpe_auditresource").setTemplateURL("/Scripts/template/manage/ddlstk.htm");
                $("#ddlSpe_auditresource").processTemplate(data.Result);
            } else {
                $("#ddlSpe_auditresource").empty();
                $("#ddlSpe_auditresource").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });
        //$("#btnSearchResource_audit").click();
    });

    $("#ddlSpe_auditresource,#ddlType_auditresource").die().live("change", function () {
        $("#btnSearchResource_audit").click();
    });
    //全选、取消
    $("#ckAll_auditresource").die().live("click", function () {
        var state = $(this).attr("checked");
        $("#divResourceList :checkbox").attr("checked", !!state);
    });

    //所有删除
    $(".del_auditresource").die().live("click", function () {
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
    $(".audit_auditresource").die().live("click", function () {
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
                        $("#btnSearchResource_audit").click();
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
    $("#btnAudit_auditresource").die().live("click", function () {
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
                        $("#ckAll_auditresource").removeAttr("checked");
                        $("#ckReverse_auditresource").removeAttr("checked");
                        $.dialog.tips("资源已经通过审核!");
                        $("#btnSearchResource_audit").click();
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
    $("#btnDelete_auditresource").die().live("click", function () {
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
                        $("#ckAll_auditresource").removeAttr("checked");
                        $("#ckReverse_auditresource").removeAttr("checked");
                        $.dialog.tips("资源已经被删除!");
                        $("#btnSearchResource_audit").click();
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
    $("#divResourceList tr:gt(0)").die().live({
        mouseenter: function () {
            $(this).css("background", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).css("background", "");
        }
    });

    //预览
    $(".preview_auditresource").die().live("click", function () {
        var id = $(this).attr("id");
        $("#hdResourceId").attr("value", id);
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
                str += " type='application/x-shockwave-flash' width='640px' height='480px'>";
                str += " </embed>";
                str += " </object>";
            } else if (data.FileExt == ".flv") {
                //                str = "<object type='application/x-shockwave-flash' width='400' height='300'  style='margin-left:130px;' wmode='transparent'";
                //                str = " data='../../../Images/source/flvplayer.swf?file={$T[0].PreviewFilepath}'>";
                //                str = " <param name='movie' value='../../../Images/source/flvplayer.swf?file={$T[0].PreviewFilepath}' />";
                //                str = "<param name='wmode' value='transparent' /></object>";

                var swf = new SWFObject("../../../Images/source/flvplayer.swf", "single", "800", "600", "7");
                swf.addParam("allowfullscreen", "true");
                swf.addVariable("file", data.PreviewFilepath);
                swf.addVariable("image", data.PreviewFilepath);
                swf.addVariable("width", "640");
                swf.addVariable("height", "480");
                $("#tdPreview").empty();
                $("#tdPreview").append(swf);
                //swf.write("tdPreview");
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
    $("#Audit_auditresource").die().live("click", function () {
        parameters = {};
        parameters.guidlist = [];
        parameters.guidlist.push($("#hdResourceId").val());
        $.sync.ajax.post("/ResourceManage/Audit", parameters, function (data) {
            if (data.State) {
                $.dialog.tips("资源通过审核!");
                $.dialog.list["EF893L"].close();
                $("#btnSearchResource_audit").click();
            }
        });
    });

    //预览中的删除
    $("#Delete_auditresource").die().live("click", function () {
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
                        $.dialog.tips("资源已经被删除!");
                        $.dialog.list["EF893L"].close();
                        $("#btnSearchResource_audit").click();

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

    ///素材审核----结束------------------


    ///素材更新----开始------------------

    $("#resource_Update").click(function () {
        $.sync.ajax.post("/ResourceManage/resource_Update", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/resource_Update.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });
    var parameters = {};
    $("#btnImport_upresource").die().live("click", function () {
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

    ///素材更新----结束------------------


    ///素材统计----开始------------------
    $("#Resource_Stats").click(function () {
        $.sync.ajax.post("/ResourceManage/Resource_Stats", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/Resource_Stats.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        parameters = {};
        $.sync.ajax.post("/DataManage/GetStatistics", parameters, function (data) {
            if (data.State) {
                $("#stats_resource").setTemplateURL("/Scripts/template/manage/stats.htm");
                $("#stats_resource").processTemplate(data.Result);
            }
        });
    });
    ///素材统计----结束------------------


    ///分类管理----开始------------------
    $("#Resource_SortManage").click(function () {
        $.sync.ajax.post("/ResourceManage/Resource_SortManage", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/Resource_SortManage.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        //素材大类
        $.sync.ajax.post("/DataManage/GetCategory", parameters, function (data) {
            if (data.State) {
                $("#ddlCategory_resource").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlCategory_resource").processTemplate(data.Result);
            } else {
                $("#ddlCategory_resource").empty();
            }
            $("#ddlCategory_resource").get(0).selectedIndex = 0;
            $("#hdCategory_resource").val($("#ddlCategory_resource").val());
        });
        //素材分类
        parameters = {};
        parameters.cateid = $("#ddlCategory_resource").val();
        if (parameters.cateid.length > 0) {
            $.sync.ajax.post("/DataManage/GetSubject", parameters, function (data) {
                if (data.State) {
                    $("#ddlSubject_resource").setTemplateURL("/Scripts/template/manage/ddl.htm");
                    $("#ddlSubject_resource").processTemplate(data.Result);
                } else {
                    $("#ddlSubject_resource").empty();
                }
                $("#lbCategory_resource").text($("#ddlCategory :selected").text());
                $("#ddlSubject_resource").get(0).selectedIndex = 0;
                $("#hdSubject_resource").val($("#ddlSubject_resource").val());
            });
        }
        //素材专业
        parameters = {};
        parameters.subid = $("#ddlSubject_resource").val();
        if (parameters.subid.length > 0) {
            $.sync.ajax.post("/DataManage/GetSpeciality", parameters, function (data) {
                if (data.State) {
                    $("#ddlSpeciality_resource").setTemplateURL("/Scripts/template/manage/ddl.htm");
                    $("#ddlSpeciality_resource").processTemplate(data.Result);
                } else {
                    $("#ddlSpeciality_resource").empty();
                }
                $("#lbSubject_resource").text($("#ddlSubject_resource :selected").text());
                $("#ddlSpeciality_resource").get(0).selectedIndex = 0;
                $("#hdSpeciality_resource").val($("#ddlSpeciality_resource").val());
            });
        }
        //素材知识点
        //        parameters = {};
        //        parameters.speid = $("#ddlSpeciality_resource").val();
        //        if (parameters.speid.length > 0) {
        //            $.sync.ajax.post("/DataManage/GetKnowledge", parameters, function (data) {
        //                if (data.State) {
        //                    $("#ddlKnowledge_resource").setTemplateURL("/Scripts/template/manage/ddl.htm");
        //                    $("#ddlKnowledge_resource").processTemplate(data.Result);
        //                } else {
        //                    $("#ddlKnowledge_resource").empty();
        //                }
        //                $("#lbSpeciality_resource").text($("#ddlSpeciality_resource :selected").text());
        //                $("#ddlKnowledge_resource").get(0).selectedIndex = 0;
        //                $("#hdKnowledge_resource").val($("#ddlKnowledge_resource").val());
        //            });
        //        }
    });
    var parameters = {};


    //大类改变 素材分类
    $("#ddlCategory_resource").die().live("change", function () {
        parameters = {};
        parameters.cateid = $("#ddlCategory_resource").val();
        $.sync.ajax.post("/DataManage/GetSubject", parameters, function (data) {
            if (data.State) {
                $("#ddlSubject_resource").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlSubject_resource").processTemplate(data.Result);
                $("#ddlSubject_resource").get(0).selectedIndex = 0;
                $("#ddlSubject_resource").change();
            } else {
                $("#ddlSubject_resource").empty();
                $("#ddlSpeciality_resource").empty();
                $("#lbSubject_resource").text('');
            }
            $("#lbCategory_resource").text($("#ddlCategory_resource :selected").text());
            $("#ddlSubject_resource").get(0).selectedIndex = 0;
            $("#hdCategory_resource").val($("#ddlCategory_resource").val());
        });
        $("#btnEscCategory_resource").click();
        $("#btnEscSubject_resource").click();
        $("#btnEscSpeciality_resource").click();
        $("#btnEscKnowledge_resource").click();
    });


    //分类改变 素材专业
    $("#ddlSubject_resource").die().live("change", function () {
        parameters = {};
        parameters.subid = $("#ddlSubject_resource").val();
        $.sync.ajax.post("/DataManage/GetSpeciality", parameters, function (data) {
            if (data.State) {
                $("#ddlSpeciality_resource").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlSpeciality_resource").processTemplate(data.Result);
            } else {
                $("#ddlSpeciality_resource").empty();
            }
            $("#lbSubject_resource").text($("#ddlSubject_resource :selected").text());
            $("#ddlSpeciality_resource").get(0).selectedIndex = 0;
            $("#hdSubject_resource").val($("#ddlSubject_resource").val());
        });
        $("#btnEscCategory_resource").click();
        $("#btnEscSubject_resource").click();
        $("#btnEscSpeciality_resource").click();
        $("#btnEscKnowledge_resource").click();
    });

    //专业改变 知识点
    $("#ddlSpeciality_resource").bind("change", function () {
        parameters = {};
        parameters.speid = $("#ddlSpeciality_resource").val();
        $.sync.ajax.post("/DataManage/GetKnowledge", parameters, function (data) {
            if (data.State) {
                $("#ddlKnowledge_resource").setTemplateURL("/Scripts/template/manage/ddl.htm");
                $("#ddlKnowledge_resource").processTemplate(data.Result);
            } else {
                $("#ddlKnowledge_resource").empty();
            }
            $("#lbSpeciality_resource").text($("#ddlSpeciality_resource :selected").text());
            $("#ddlKnowledge_resource").get(0).selectedIndex = 0;
            $("#hdSpeciality_resource").val($("#ddlSpeciality_resource").val());
        });
        $("#btnEscCategory_resource").click();
        $("#btnEscSubject_resource").click();
        $("#btnEscSpeciality_resource").click();
        $("#btnEscKnowledge_resource").click();
    });

    $("#ddlKnowledge_resource").die().live("change", function () {
        $("#btnEscCategory_resource").click();
        $("#btnEscSubject_resource").click();
        $("#btnEscSpeciality_resource").click();
        $("#btnEscKnowledge_resource").click();
    });
    //添加Category
    $("#btnAddCategory_resource").die().live("click", function () {
        $("#txtCategory_resource").show();
        $("#btnCategory_resource").show();
        $("#btnEscCategory_resource").show();
        $("#btnCategory_resource").val($("#btnAddCategory_resource").val());
        $("#txtCategory_resource").val("");
        $("#btnCategory_resource").attr("flag", "add");
    });
    //修改Category
    $("#btnEditCategory_resource").die().live("click", function () {
        if ($("#ddlCategory_resource").val() != null) {
            $("#txtCategory_resource").show();
            $("#btnCategory_resource").show();
            $("#btnEscCategory_resource").show();
            $("#btnCategory_resource").val($("#btnEditCategory_resource").val());
            $("#txtCategory_resource").val($("#ddlCategory_resource :selected").text());
            $("#btnCategory_resource").attr("flag", "edit");
        } else {
            $.dialog.tips("无选项，请添加或选中！");
        }
    });
    //取消
    $("#btnEscCategory_resource").die().live("click", function () {
        $("#txtCategory_resource").hide();
        $("#btnCategory_resource").hide();
        $("#btnEscCategory_resource").hide();
    });
    //确定添加/修改Category
    $("#btnCategory_resource").die().live("click", function () {
        parameters = {};
        parameters.cateid = $("#ddlCategory_resource").val();
        parameters.flag = $("#btnCategory_resource").attr("flag");
        parameters.name = $("#txtCategory_resource").val();
        parameters.tier = 0;
        if (parameters.name.length > 0) {
            $.sync.ajax.post("/ResourceManage/OperationCategory", parameters, function (data) {
                if (data.State) {
                    if (parameters.flag == "add") {
                        $("#ddlCategory_resource").append("<option value=" + data.Result + ">" + parameters.name + "</option>");
                    } else if (parameters.flag == "edit") {
                        $("#ddlCategory_resource").find("option:selected").text(parameters.name);
                    }
                }
                $("#btnEscCategory_resource").click();
                $("#btnEscSubject_resource").click();
                $("#btnEscSpeciality_resource").click();
                $("#btnEscKnowledge_resource").click();
                $.dialog.tips(data.Msg);

            });
        } else {
            $.dialog.tips("为空或未选中！");
        }
    });
    //删除Category
    $("#btnDelCategory_resource").die().live("click", function () {
        $.artDialog.confirm("确定删除吗？", function () {
            parameters = {};
            parameters.flag = "del";
            parameters.id = $("#ddlCategory_resource").val();
            parameters.name = $("#ddlCategory_resource :selected").text();
            parameters.tier = 0;
            $.sync.ajax.post("/ResourceManage/OperationCategory", parameters, function (data) {
                if (data.State) {
                    $("#ddlCategory_resource option[value='" + parameters.id + "']").remove();
                    if ($("#ddlCategory_resource").val() == null) {
                        $("#btnCategory_resource").val($("#btnAddCategory_resource").val());
                        $("#txtCategory_resource").val("");
                        $("#btnCategory_resource").attr("flag", "add");
                    }
                }
                $("#btnEscCategory_resource").click();
                $("#btnEscSubject_resource").click();
                $("#btnEscSpeciality_resource").click();
                $("#btnEscKnowledge_resource").click();
                $.dialog.tips(data.Msg);
            });
        });
    });

    //添加Subject
    $("#btnAddSubject_resource").die().live("click", function () {
        $("#txtSubject_resource").show();
        $("#btnSubject_resource").show();
        $("#btnEscSubject_resource").show();
        $("#btnSubject_resource").val($("#btnAddSubject_resource").val());
        $("#txtSubject_resource").val("");
        $("#btnSubject_resource").attr("flag", "add");
    });
    //修改Subject
    $("#btnEditSubject_resource").die().live("click", function () {
        if ($("#ddlSubject_resource").val() != null) {
            $("#txtSubject_resource").show();
            $("#btnSubject_resource").show();
            $("#btnEscSubject_resource").show();
            $("#btnSubject_resource").val($("#btnEditSubject_resource").val());
            $("#txtSubject_resource").val($("#ddlSubject_resource :selected").text());
            $("#btnSubject_resource").attr("flag", "edit");
        } else {
            $.dialog.tips("无选项，请添加或选中！");
        }
    });
    //取消
    $("#btnEscSubject_resource").die().live("click", function () {
        $("#txtSubject_resource").hide();
        $("#btnSubject_resource").hide();
        $("#btnEscSubject_resource").hide();
    });
    //确定添加/修改Subject
    $("#btnSubject_resource").die().live("click", function () {
        parameters = {};
        parameters.id = $("#ddlSubject_resource").val();
        parameters.flag = $("#btnSubject_resource").attr("flag");
        parameters.parentId = $("#hdCategory_resource").val();
        parameters.name = $("#txtSubject_resource").val();
        parameters.tier = 1;
        if (parameters.name.length > 0) {
            $.sync.ajax.post("/ResourceManage/OperationCategory", parameters, function (data) {
                if (data.State) {
                    if (parameters.flag == "add") {
                        $("#ddlSubject_resource").append("<option value=" + data.Result + ">" + parameters.name + "</option>");
                    } else if (parameters.flag == "edit") {
                        $("#ddlSubject_resource").find("option:selected").text(parameters.name);
                    }
                }
                $("#btnEscCategory_resource").click();
                $("#btnEscSubject_resource").click();
                $("#btnEscSpeciality_resource").click();
                $("#btnEscKnowledge_resource").click();
                $.dialog.tips(data.Msg);
            });
        } else {
            $.dialog.tips("为空或未选中！");
        }

    });
    //删除Subject
    $("#btnDelSubject_resource").die().live("click", function () {
        $.artDialog.confirm("确定删除吗？", function () {
            parameters = {};
            parameters.flag = "del";
            parameters.id = $("#ddlSubject_resource").val();
            parameters.name = $("#ddlSubject_resource :selected").text();
            parameters.tier = 1;
            $.sync.ajax.post("/ResourceManage/OperationCategory", parameters, function (data) {
                if (data.State) {
                    $("#ddlSubject_resource option[value='" + parameters.id + "']").remove();
                    $("#ddlSubject_resource").get(0).selectedIndex = 1;
                    if ($("#ddlSubject_resource").val() == null) {
                        $("#btnSubject_resource").val($("#btnAddSubject_resource").val());
                        $("#txtSubject_resource").val("");
                        $("#btnSubject_resource").attr("flag", "add");
                    }
                }
                $("#btnEscCategory_resource").click();
                $("#btnEscSubject_resource").click();
                $("#btnEscSpeciality_resource").click();
                $("#btnEscKnowledge_resource").click();
                $.dialog.tips(data.Msg);
            });
        });
    });

    //添加Speciality
    $("#btnAddSpeciality_resource").die().live("click", function () {
        $("#txtSpeciality_resource").show();
        $("#btnSpeciality_resource").show();
        $("#btnEscSpeciality_resource").show();
        $("#btnSpeciality_resource").val($("#btnAddSpeciality_resource").val());
        $("#txtSpeciality_resource").val("");
        $("#btnSpeciality_resource").attr("flag", "add");
    });
    //修改Speciality
    $("#btnEditSpeciality_resource").die().live("click", function () {
        if ($("#ddlSpeciality_resource").val() != null) {
            $("#txtSpeciality_resource").show();
            $("#btnSpeciality_resource").show();
            $("#btnEscSpeciality_resource").show();
            $("#btnSpeciality_resource").val($("#btnEditSpeciality_resource").val());
            $("#txtSpeciality_resource").val($("#ddlSpeciality_resource :selected").text());
            $("#btnSpeciality_resource").attr("flag", "edit");
        } else {
            $.dialog.tips("无选项，请添加或选中！");
        }
    });
    //取消
    $("#btnEscSpeciality_resource").die().live("click", function () {
        $("#txtSpeciality_resource").hide();
        $("#btnSpeciality_resource").hide();
        $("#btnEscSpeciality_resource").hide();
    });
    //确定添加/修改Speciality
    $("#btnSpeciality_resource").die().live("click", function () {
        parameters = {};
        parameters.id = $("#ddlSpeciality_resource").val();
        parameters.flag = $("#btnSpeciality_resource").attr("flag");
        parameters.parentId = $("#hdSubject_resource").val();
        parameters.name = $("#txtSpeciality_resource").val();
        parameters.tier = 2;
        if (parameters.name.length > 0) {
            $.sync.ajax.post("/ResourceManage/OperationCategory", parameters, function (data) {
                if (data.State) {
                    if (parameters.flag == "add") {
                        $("#ddlSpeciality_resource").append("<option value=" + data.Result + ">" + parameters.name + "</option>");
                    } else if (parameters.flag == "edit") {
                        $("#ddlSpeciality_resource").find("option:selected").text(parameters.name);
                    }
                }
                $("#btnEscCategory_resource").click();
                $("#btnEscSubject_resource").click();
                $("#btnEscSpeciality_resource").click();
                $("#btnEscKnowledge_resource").click();
                $.dialog.tips(data.Msg);
            });
        } else {
            $.dialog.tips("为空或未选中！");
        }
    });
    //删除Speciality
    $("#btnDelSpeciality_resource").die().live("click", function () {
        $.artDialog.confirm("确定删除吗？", function () {
            parameters = {};
            parameters.flag = "del";
            parameters.id = $("#ddlSpeciality_resource").val();
            parameters.name = $("#ddlSpeciality_resource :selected").text();
            parameters.tier = 2;
            $.sync.ajax.post("/ResourceManage/OperationCategory", parameters, function (data) {
                if (data.State) {
                    $("#ddlSpeciality_resource option[value='" + parameters.id + "']").remove();
                    if ($("#ddlSpeciality_resource").val() == null) {
                        $("#btnSpeciality_resource").val($("#btnAddSpeciality_resource").val());
                        $("#txtSpeciality_resource").val("");
                        $("#btnSpeciality_resource").attr("flag", "add");
                    }
                }
                $("#btnEscCategory_resource").click();
                $("#btnEscSubject_resource").click();
                $("#btnEscSpeciality_resource").click();
                $("#btnEscKnowledge_resource").click();
                $.dialog.tips(data.Msg);
            });
        });
    });

    //添加Knowledge
    $("#btnAddKnowledge_resource").die().live("click", function () {
        $("#txtKnowledge_resource").show();
        $("#btnKnowledge_resource").show();
        $("#btnEscKnowledge_resource").show();
        $("#btnKnowledge_resource").val($("#btnAddKnowledge_resource").val());
        $("#txtKnowledge_resource").val("");
        $("#btnKnowledge_resource").attr("flag", "add");
    });
    //修改Knowledge
    $("#btnEditKnowledge_resource").die().live("click", function () {
        if ($("#ddlKnowledge_resource").val() != null) {
            $("#txtKnowledge_resource").show();
            $("#btnKnowledge_resource").show();
            $("#btnEscKnowledge_resource").show();
            $("#btnKnowledge_resource").val($("#btnEditKnowledge_resource").val());
            $("#txtKnowledge_resource").val($("#ddlKnowledge_resource :selected").text());
            $("#btnKnowledge_resource").attr("flag", "edit");
        } else {
            $.dialog.tips("无选项，请添加或选中！");
        }
    });
    //取消
    $("#btnEscKnowledge_resource").die().live("click", function () {
        $("#txtKnowledge_resource").hide();
        $("#btnKnowledge_resource").hide();
        $("#btnEscKnowledge_resource").hide();
    });
    //确定添加/修改Knowledge
    $("#btnKnowledge_resource").bind("click", function () {
        parameters = {};
        parameters.knowid = $("#ddlKnowledge_resource").val();
        parameters.flag = $("#btnKnowledge_resource").attr("flag");
        parameters.speid = $("#hdSpeciality_resource").val();
        parameters.name = $("#txtKnowledge_resource").val();
        if (parameters.name.length > 0) {
            $.sync.ajax.post("/ResourceManage/Knowledge", parameters, function (data) {
                if (data.State) {
                    if (parameters.flag == "add") {
                        $("#ddlKnowledge_resource").append("<option value=" + data.Result + ">" + parameters.name + "</option>");
                    } else if (parameters.flag == "edit") {
                        $("#ddlKnowledge_resource").find("option:selected").text(parameters.name);
                    }
                }
                $.dialog.tips(data.Msg);
            });
        } else {
            $.dialog.tips("为空或未选中！");
        }
    });
    //删除Knowledge
    $("#btnDelKnowledge_resource").die().live("click", function () {
        $.artDialog.confirm("确定删除吗？", function () {
            parameters = {};
            parameters.flag = "del";
            parameters.knowid = $("#ddlKnowledge_resource").val();
            parameters.name = $("#ddlKnowledge_resource :selected").text();
            $.sync.ajax.post("/ResourceManage/Knowledge", parameters, function (data) {
                if (data.State) {
                    $("#ddlKnowledge_resource option[value='" + parameters.knowid + "']").remove();
                    if ($("#ddlKnowledge_resource").val() == null) {
                        $("#btnKnowledge_resource").val($("#btnAddKnowledge_resource").val());
                        $("#txtKnowledge_resource").val("");
                        $("#btnKnowledge_resource").attr("flag", "add");
                    }
                }
                $.dialog.tips(data.Msg);
            });
        });
    });
    ///分类管理----结束------------------

    ///课件列表---开始---------------------
    parameters = {};
    $("#Course_list").click(function () {
        $.sync.ajax.post("/ResourceManage/Course_list", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/Course_list.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        var parameters = {};
        //素材分类
        $.sync.ajax.post("/DataManage/GetCategorySubject", parameters, function (data) {
            if (data.State) {
                $("#ddlcs_Courselist").setTemplateURL("/Scripts/template/manage/ddlcs.htm");
                $("#ddlcs_Courselist").processTemplate(data.Result);
            }
        });
        // $("#btnSearchResource").click(); //分页数据
        $.cellpagebar.init({
            total: $("#hdCount").val(),
            pagesize: 50,
            container: $("#divBar")
        }, function (pageindex) {
            parameters = {};
            parameters.pageIndex = pageindex;
            parameters.name = "";
            parameters.csid = $("#ddlcs_Courselist").val();
            parameters.specialityid = $("#ddlSpe_Courselist").val();
            parameters.typeid = $("#ddlType_Courselist").val();
            $.sync.ajax.post("/DataManage/GetCourseList", parameters, function (data) {
                if (data.State) {
                    $("#divCourseList").setTemplateURL("/Scripts/template/manage/coursesource.htm");
                    $("#divCourseList").processTemplate(data.Result);
                }
            });
        });
    });

    //分类改变选择课件专业
    $("#ddlcs_Courselist").die().live("change", function () {
        parameters = {};
        parameters.subid = $("#ddlcs_Courselist").val();
        $.sync.ajax.post("/DataManage/GetSpeciality", parameters, function (data) {
            if (data.State) {
                $("#ddlSpe_Courselist").setTemplateURL("/Scripts/template/manage/ddlstk.htm");
                $("#ddlSpe_Courselist").processTemplate(data.Result);
            } else {
                $("#ddlSpe_Courselist").empty();
                $("#ddlSpe_Courselist").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });
        // $("#btnSearchCourse").click();
    });

    $("#ddlSpe_Courselist,#ddlType_Courselist").die().live("change", function () {
        $("#btnSearchCourse").click();
    });



    $("#btnSearchCourse").die().live("click", function () {
        parameters = {};
        parameters.csid = $("#ddlcs_Courselist").val();
        parameters.specialityid = $("#ddlSpe_Courselist").val();
        parameters.typeid = $("#ddlType_Courselist").val();
        parameters.name = $("#txtName").val();
        $.sync.ajax.post("/DataManage/GetCourseListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 50,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.csid = $("#ddlcs_Courselist").val();
                    parameters.specialityid = $("#ddlSpe_Courselist").val();
                    parameters.typeid = $("#ddlType_Courselist").val();
                    parameters.name = $("#txtName").val();
                    $.sync.ajax.post("/DataManage/GetCourseList", parameters, function (data) {
                        if (data.State) {
                            $("#divCourseList").setTemplateURL("/Scripts/template/manage/coursesource.htm");
                            $("#divCourseList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });
    //全选、取消
    $("#CourseckAll").die().live("click", function () {
        var state = $(this).attr("checked");
        $("#divCourseList :checkbox").attr("checked", !!state);
    });
    //删除选择
    $("#btnDelete_Courselist").die().live("click", function () {
        parameters = {};
        parameters.guidlist = [];
        $("#divCourseList").find(":checkbox:checked").each(function () {
            parameters.guidlist.push($(this).val());
        });
        if (parameters.guidlist.length <= 0) {
            $.dialog.tips("没有选中素材！");
            return false;
        };
        $.dialog({
            id: 'deleteCourses',
            content: '是否将资源放入资源审核？',
            button: [
            {
                name: '确定',
                callback: function () {
                    parameters.flag = "softdelete";
                    $.sync.ajax.post("/ResourceManage/DeleteCourse", parameters, function (data) {
                        if (data.State) {
                            $.dialog.tips("资源已经放入资源审核!");
                            $.dialog.list["deleteCourses"].close();
                            $("#btnSearchCourse").click();
                        }
                    });
                },
                focus: true
            },
            {
                name: '直接删除',
                callback: function () {
                    if (confirm("是否彻底删除资源?")) {
                        parameters.flag = "delete";
                        $.sync.ajax.post("/ResourceManage/DeleteCourse", parameters, function (data) {
                            if (data.State) {
                                $.dialog.tips("资源已经被删除!");
                                $.dialog.list["deleteCourses"].close();
                                $("#btnSearchCourse").click();
                            }
                        });
                    }
                }
            },
            {
                name: '取消'
            }
        ]
        });

    });
    //所有删除
    $(".del_Courselist").die().live("click", function () {
        var $btnDel = $(this);
        parameters = {};
        parameters.guidlist = [];
        parameters.guidlist.push($btnDel.attr("dataid"));
        $.dialog({
            id: 'deleteCourse',
            content: '是否将资源放入资源审核？',
            button: [
            {
                name: '确定',
                callback: function () {
                    parameters.flag = "softdelete";
                    $.sync.ajax.post("/ResourceManage/DeleteCourse", parameters, function (data) {
                        if (data.State) {
                            $.dialog.tips("资源已经放入资源审核!");
                            $.dialog.list["deleteCourse"].close();
                            $("#btnSearchCourse").click();
                        }
                    });
                },
                focus: true
            },
            {
                name: '直接删除',
                callback: function () {
                    if (confirm("是否彻底删除资源?")) {
                        parameters.flag = "delete";
                        $.sync.ajax.post("/ResourceManage/DeleteCourse", parameters, function (data) {
                            if (data.State) {
                                $.dialog.tips("资源已经被删除!");
                                $.dialog.list["deleteCourse"].close();
                                $("#btnSearchCourse").click();
                            }
                        });
                    }
                }
            },
            {
                name: '取消'
            }
        ]
        });
    });

    //变色
    $("#divCourseList tr:gt(0)").die().live({
        mouseenter: function () {
            $(this).attr("bgcolor", "#D6DFF7");
        },
        mouseleave: function () {
            $(this).removeAttr("bgcolor");
        }
    });
    ///课件列表----修改------------------
    $(".edit_Courselist").die().live("click", function () {
        parameters = {};
        parameters.id = $(this).attr("dataid");
        $.sync.ajax.post("/ResourceManage/Course_Edit", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/Course_Edit.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });

    //分类改变选择素材专业
    $("#ddlcs_EditCourse").die().live("change", function () {
        parameters = {};
        parameters.subid = $("#ddlcs_EditCourse").val();
        $.sync.ajax.post("/DataManage/GetSpeciality", parameters, function (data) {
            if (data.State) {
                $("#ddlSpe_EditCourse").setTemplateURL("/Scripts/template/manage/ddlstkc.htm");
                $("#ddlSpe_EditCourse").processTemplate(data.Result);
                $("#ddlSpe_EditCourse").get(0).selectedIndex = 0;
            } else {
                $("#ddlSpe_EditCourse").empty();
                $("#ddlSpe_EditCourse").append("<option value='00000000-0000-0000-0000-000000000000' selected='selected'>请选择</option>");
            }
        });
    });
    //专业改变选择素材知识点
    $("#ddlSpe_EditCourse").die().live("change", function () {
        parameters = {};
        parameters.speid = $("#ddlSpe_EditCourse").val();
        $.sync.ajax.post("/DataManage/GetKnowledge", parameters, function (data) {
            if (data.State) {
                $("#ddlKnow_EditCourse").setTemplateURL("/Scripts/template/manage/ddlstkc.htm");
                $("#ddlKnow_EditCourse").processTemplate(data.Result);
                $("#ddlKnow_EditCourse").get(0).selectedIndex = 0;
            } else {
                $("#ddlKnow_EditCourse").empty();
                $("#ddlKnow_EditCourse").append("<option value='00000000-0000-0000-0000-000000000000' selected='selected'>请选择</option>");
            }
        });
    });
    //保存按钮
    $("#btnSure_EditCourse").die().live("click", function () {
        parameters = {};
        parameters.resid = $("#hdId").val();
        parameters.name = $("#txtResourceName").val();
        // parameters.cateid = $("#ddlcs_Editresource option:selected").parent("optgroup").attr("id");
        parameters.subid = $("#ddlcs_EditCourse").val();
        parameters.speid = $("#ddlSpe_EditCourse").val();
        parameters.typeid = $("#ddlType_EditCourse").val();
        // parameters.jp = $("#ddlBoutiqueGrade_Editresource").val();
        parameters.fescribe = $("#txtFescribe").val();
        parameters.keys = $("#txtKey").val();
        if (parameters.name.trim().length <= 0) {
            $.dialog.tips("素材名称不能为空！");
            return false;
        }
        $.sync.ajax.post("/ResourceManage/Course_EditRes", parameters, function (data) {
            if (data.State) {
                $.dialog.tips(data.Msg);
                // $("#btnEsc_Editresource").click();
            }
        });
    });
    //返回按钮
    $("#btnEsc_EditCourse").die().live("click", function () {
        $("#Course_list").click();
    });
    ///课件列表---结束---------------------

    ///课件审核---开始---------------------
    $("#Course_Audit").click(function () {
        $.sync.ajax.post("/ResourceManage/Course_Audit", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/Course_Audit.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        parameters = {};
        //素材分类
        $.sync.ajax.post("/DataManage/GetCategorySubject", parameters, function (data) {
            if (data.State) {
                $("#ddlcs_auditCourse").setTemplateURL("/Scripts/template/manage/ddlcs.htm");
                $("#ddlcs_auditCourse").processTemplate(data.Result);
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
            parameters.csid = $("#ddlcs_auditCourse").val();
            parameters.specialityid = $("#ddlSpe_auditCourse").val();
            parameters.typeid = $("#ddlType_auditCourse").val();
            $.sync.ajax.post("/DataManage/GetDelCourseList", parameters, function (data) {
                if (data.State) {
                    $("#divCourseList").setTemplateURL("/Scripts/template/manage/Course_Auditlist.htm");
                    $("#divCourseList").processTemplate(data.Result);
                }
            });
        });
    });
    //搜索按钮
    $("#btnSearchCourse_audit").die().live("click", function () {
        parameters = {};
        parameters.csid = $("#ddlcs_auditCourse").val();
        parameters.specialityid = $("#ddlSpe_auditCourse").val();
        parameters.typeid = $("#ddlType_auditCourse").val();
        parameters.name = $("#txtName").val();
        $.sync.ajax.post("/DataManage/GetDelCourseListCount", parameters, function (data) {
            if (data.State) {
                $.cellpagebar.init({
                    total: data.Result,
                    pagesize: 50,
                    container: $("#divBar")
                }, function (pageindex) {
                    $("#sCount").text(data.Result);
                    parameters = {};
                    parameters.pageIndex = pageindex;
                    parameters.csid = $("#ddlcs_auditCourse").val();
                    parameters.specialityid = $("#ddlSpe_auditCourse").val();
                    parameters.typeid = $("#ddlType_auditCourse").val();
                    parameters.name = $("#txtName").val();
                    $.sync.ajax.post("/DataManage/GetDelCourseList", parameters, function (data) {
                        if (data.State) {
                            $("#divCourseList").setTemplateURL("/Scripts/template/manage/Course_Auditlist.htm");
                            $("#divCourseList").processTemplate(data.Result);
                        }
                    });
                });
            }
        });
    });
    //分类改变选择素材专业
    $("#ddlcs_auditCourse").die().live("change", function () {
        parameters = {};
        parameters.subid = $("#ddlcs_auditCourse").val();
        $.sync.ajax.post("/DataManage/GetSpeciality", parameters, function (data) {
            if (data.State) {
                $("#ddlSpe_auditCourse").setTemplateURL("/Scripts/template/manage/ddlstk.htm");
                $("#ddlSpe_auditCourse").processTemplate(data.Result);
            } else {
                $("#ddlSpe_auditCourse").empty();
                $("#ddlSpe_auditCourse").append("<option value='00000000-0000-0000-0000-000000000000'>请选择</option>");
            }
        });
        $("#btnSearchCourse_audit").click();
    });

    $("#ddlSpe_auditCourse,#ddlType_auditCourse").die().live("change", function () {
        $("#btnSearchCourse_audit").click();
    });

    //全选、取消
    $("#ckAll_auditCourse").die().live("click", function () {
        var state = $(this).attr("checked");
        $("#divCourseList :checkbox").attr("checked", !!state);
    });
    //所有删除
    $(".del_auditCourse").die().live("click", function () {
        var $btnDel = $(this);
        parameters = {};
        parameters.guidlist = [];
        parameters.guidlist.push($btnDel.attr("dataid"));
        $.dialog({
            id: 'deleteCourse',
            content: '是否彻底删除资源？',
            button: [
        {
            name: '确定',
            callback: function () {
                parameters.flag = "delete";
                $.sync.ajax.post("/ResourceManage/DeleteCourse", parameters, function (data) {
                    if (data.State) {
                        $.dialog.tips("资源已经被删除!");
                        $("#btnSearchCourse_audit").click();
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
    $(".audit_auditCourse").die().live("click", function () {
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
                $.sync.ajax.post("/ResourceManage/CourseAudit", parameters, function (data) {
                    if (data.State) {
                        $.dialog.tips("资源已经通过审核!");
                        $("#btnSearchCourse_audit").click();
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
    $("#btnAudit_auditCourse").die().live("click", function () {
        parameters = {};
        parameters.guidlist = [];
        $("#divCourseList").find(":checkbox:checked").each(function () {
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
                $.sync.ajax.post("/ResourceManage/CourseAudit", parameters, function (data) {
                    if (data.State) {
                        $("#ckAll_auditCourse").removeAttr("checked");
                        $("#ckReverse_auditCourse").removeAttr("checked");
                        $.dialog.tips("资源已经通过审核!");
                        $("#btnSearchCourse_audit").click();
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
    $("#btnDelete_auditCourse").die().live("click", function () {
        parameters = {};
        parameters.guidlist = [];
        $("#divCourseList").find(":checkbox:checked").each(function () {
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
                $.sync.ajax.post("/ResourceManage/DeleteCourse", parameters, function (data) {
                    if (data.State) {
                        $("#ckAll_auditCourse").removeAttr("checked");
                        $("#ckReverse_auditCourse").removeAttr("checked");
                        $.dialog.tips("资源已经被删除!");
                        $("#btnSearchCourse_audit").click();
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


    //预览
    $(".preview_auditCourse").die().live("click", function () {
        var id = $(this).attr("id");
        $("#hdResourceId").attr("value", id);
        parameters = {};
        parameters.id = id;
        $.sync.ajax.post("/ResourceManage/CoursePreview", parameters, function (data) {
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
                str = "<img src='" + data.PreviewFilepath + "' alt='' width='640px' height='480px'/>";
            } else if (data.FileExt == ".swf" || data.FileExt == ".ppt" || data.FileExt == ".doc") {
                str = "<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0'";
                str += " width='100%' height='100%'>";
                str += " <param name='movie' value='" + data.PreviewFilepath + "' />";
                str += " <param name='quality' value='high' />";
                str += " <embed src='" + data.PreviewFilepath + "' quality='high' pluginspage='http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash'";
                str += " type='application/x-shockwave-flash' width='640px' height='480px'>";
                str += " </embed>";
                str += " </object>";
            } else if (data.FileExt == ".flv") {
                //                str = "<object type='application/x-shockwave-flash' width='400' height='300'  style='margin-left:130px;' wmode='transparent'";
                //                str = " data='../../../Images/source/flvplayer.swf?file={$T[0].PreviewFilepath}'>";
                //                str = " <param name='movie' value='../../../Images/source/flvplayer.swf?file={$T[0].PreviewFilepath}' />";
                //                str = "<param name='wmode' value='transparent' /></object>";

                var swf = new SWFObject("../../../Images/source/flvplayer.swf", "single", "800", "600", "7");
                swf.addParam("allowfullscreen", "true");
                swf.addVariable("file", data.PreviewFilepath);
                swf.addVariable("image", data.PreviewFilepath);
                swf.addVariable("width", "640");
                swf.addVariable("height", "480");
                $("#tdPreview").empty();
                $("#tdPreview").append(swf);
                //swf.write("tdPreview");
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
    $("#Audit_auditCourse").die().live("click", function () {
        parameters = {};
        parameters.guidlist = [];
        parameters.guidlist.push($("#hdResourceId").val());
        $.sync.ajax.post("/ResourceManage/CourseAudit", parameters, function (data) {
            if (data.State) {
                $.dialog.tips("资源通过审核!");
                $.dialog.list["EF893L"].close();
                $("#btnSearchCourse_audit").click();
            }
        });
    });

    //预览中的删除
    $("#Delete_auditCourse").die().live("click", function () {
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
                $.sync.ajax.post("/ResourceManage/DeleteCourse", parameters, function (data) {
                    if (data.State) {
                        $.dialog.tips("资源已经被删除!");
                        $.dialog.list["EF893L"].close();
                        $("#btnSearchCourse_audit").click();
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

    ///课件审核---结束---------------------

    ///课件更新----开始------------------
    $("#Course_update").click(function () {
        $.sync.ajax.post("/ResourceManage/Course_Update", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/Course_Update.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
    });
    var parameters = {};
    $("#btnImport_upCourse").die().live("click", function () {
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

    ///课件更新----结束------------------

    ///资源统计----开始------------------
    $("#Course_stats").click(function () {
        $.sync.ajax.post("/ResourceManage/Course_Stats", parameters, function (data) {
            if (data.State) {
                $("#divdata").setTemplateURL("/Scripts/template/manage/Course_Stats.htm");
                $("#divdata").processTemplate(data.Result);
            }
        });
        parameters = {};
        $.sync.ajax.post("/DataManage/GetCourseStatistics", parameters, function (data) {
            if (data.State) {
                $("#stats_Course").setTemplateURL("/Scripts/template/manage/Course_statslist.htm");
                $("#stats_Course").processTemplate(data.Result);
            }
        });
    });
    ///资源统计----结束------------------

});