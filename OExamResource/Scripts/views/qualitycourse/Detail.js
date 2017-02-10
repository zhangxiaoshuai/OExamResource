$(function () {
    var par = {};
    //导航
    par = {};
    par.id = $("#hdId").val();
    $.sync.ajax.post("/QualityCourse/Getnav", par, function (data) {
        $("#navs").setTemplateURL("/Scripts/template/qualitycourse/nav.htm");
        $("#navs").processTemplate(data.Result);
    });
    //连接状态
    par = {};
    par.Address = $("#hdAddress").val();
    if (par.Address != null) {
        $.sync.ajax.post("/QualityCourse/GetUrlIsExists", par, function (data) {
            if (data.State) {
                $("#Urlimg").attr("src", "/Content/image/QualityCourse/QualityCourse_ture.jpg");
            } else {
                $("#Urlimg").attr("src", "/Content/image/QualityCourse/QualityCourse_false.jpg");
            }
            $("#labstate").attr("style", "display:none");
        }, true);
    }

    par = {};
    par.id = $("#hdId").val();
    $.sync.ajax.post("/QualityCourse/GetDetail", par, function (data) {
        $("#Details").setTemplateURL("/Scripts/template/qualitycourse/Details.htm", null, { filter_data: false });
        $("#Details").processTemplate(data.Result);
    });
    $(".aclick").click(function () {
        var fine = $(this).attr("values");
        //内容概要
        if (fine == "c_team") {
            $("#f_d_team").show();
            $("#f_team").hide();
            //            $("#f_d_team").attr("style", "display:block");
            //            $("#f_team").attr("style", "display:none");
        }
        if (fine == "d_team") {
            $("#f_d_team").hide();
            $("#f_team").show();
        }
        //课程简介
        if (fine == "js_team") {
            $("#d_j_s_team").show();
            $("#j_s_team").hide();
        }
        if (fine == "d_js_team") {
            $("#d_j_s_team").hide();
            $("#j_s_team").show();
        }
        //教师团队
        if (fine == "km_team") {
            $("#d_k_m_team").show();
            $("#k_m_team").hide();
        }
        if (fine == "d_km_team") {
            $("#d_k_m_team").hide();
            $("#k_m_team").show();
        }
        //建设规划
        if (fine == "jg_team") {
            $("#d_j_g_team").show();
            $("#j_g_team").hide();
        }
        if (fine == "d_jg_team") {
            $("#d_j_g_team").hide();
            $("#j_g_team").show();
        }
        //政策措施
        if (fine == "zc_team") {
            $("#d_z_c_team").show();
            $("#z_c_team").hide();
        }
        if (fine == "d_zc_team") {
            $("#d_z_c_team").hide();
            $("#z_c_team").show();
        }
    });
});