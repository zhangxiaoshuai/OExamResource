/*
cellpagebar
author：xiejiabin
date：2012-06-06
*/
$.extend({
    cellpagebar: {}
});
$.cellpagebar = {
    init: function (data, func) {
        data.text = jQuery.ajax({
            url: data.template || "/Scripts/plugin/cellpagebar/template.htm",
            async: false
        }).responseText;
        data.arr = data.text.match(/\{#(.+)#\}/g);
        data.strcell = data.arr[0].replace("{#", "").replace("#}", "");
        data.pagecount = Math.ceil(data.total / data.pagesize);
        if (data.pagecount <= 0) {
            data.container.html("Sync没有检测到数据。");
            func(-1);
            return false;
        }
        data.barcount = data.barcount || 9;
        data.container = data.container;
        data.strcells = "";
        for (var i = 1; i <= Math.min(data.barcount, data.pagecount); i++) {
            data.strcells += data.strcell.format(i);
        }
        data.text = data.text.replace(/\{#.+#\}/g, data.strcells);
        data.text = String.format(data.text, data.pagecount);
        data.container.html(data.text);
        /*页码按钮*/
        data.container.children(".pagecell").off().on("click", function () {
            $.cellpagebar.pageClick($(this).attr("data"), data, func);
        });
        /*首页按钮*/
        data.container.children("#pagefirst").unbind().bind("click", function () {
            $.cellpagebar.pageClick(1, data, func);
        });
        /*尾页按钮*/
        data.container.children("#pagelast").unbind().bind("click", function () {
            $.cellpagebar.pageClick(data.pagecount, data, func);
        });
        /*上一页按钮*/
        data.container.children("#pageprev").unbind().bind("click", function () {
            var j = parseInt(data.container.children(".pagecurrent").attr("data")) - 1;
            if (j < 1)
                return false;
            data.container.children(".pagecell[data=" + j + "]").click();
        });
        /*下一页按钮*/
        data.container.children("#pagenext").unbind().bind("click", function () {
            var j = parseInt(data.container.children(".pagecurrent").attr("data")) + 1;
            if (j > data.pagecount)
                return false;
            data.container.children(".pagecell[data=" + j + "]").click();
        });
        //跳转
        data.container.children("#pagego").unbind().bind("click", function () {
            var j = data.container.children("#pageindex").val();
            if (isNaN(j)) {
                j = data.container.children(".pagecurrent").attr("data");
            } else if (j < 1) {
                j = 1;
            } else if (j > data.pagecount) {
                j = data.pagecount;
            }
            data.container.children("#pageindex").val(j)
            $.cellpagebar.pageClick(j, data, func);
        });
        /*初始化*/
        if (!data.pageindex || isNaN(data.pageindex)) {
            data.container.children(".pagecell:first").click();
        } else {
            if (data.pageindex < 1) {
                data.pageindex = 1;
            } else if (data.pageindex > data.pagecount) {
                data.pageindex = data.pagecount;
            }
            data.container.children("#pageindex").val(data.pageindex)
            $.cellpagebar.pageClick(data.pageindex, data, func);
        }
    },
    pageClick: function (num, data, func) {
        var that = data.container.children(".pagecell[data=" + num + "]");
        if (that.hasClass("pagecurrent"))
            return false;
        var _start = 0;
        var _end = 0;
        if (num - Math.ceil(data.barcount / 2) + 1 >= 1 && num - Math.ceil(data.barcount / 2) + data.barcount <= data.pagecount) {
            _start = num - Math.ceil(data.barcount / 2) + 1;
            _end = num - Math.ceil(data.barcount / 2) + data.barcount;
        } else if (num < Math.ceil(data.barcount / 2)) {
            _start = 1;
            _end = (data.barcount > data.pagecount ? data.pagecount : data.barcount);
        } else {
            _start = ((data.pagecount - data.barcount) < 1 ? 1 : (data.pagecount - data.barcount + 1));
            _end = data.pagecount;
        }
        var _pagecells = data.container.children(".pagecell");
        var j = 0;
        for (var i = _start; i <= _end; i++) {
            _pagecells.eq(j).replaceWith(data.strcell.format(i));
            j++;
        }
        data.container.children(".pagecell").unbind().bind("click", function () {
            $.cellpagebar.pageClick($(this).attr("data"), data, func);
        });
        data.container.children("#pageindex").val(num);
        data.container.children(".pagecell[data=" + num + "]").addClass("pagecurrent").siblings().removeClass("pagecurrent");
        func(num);
    }
};
