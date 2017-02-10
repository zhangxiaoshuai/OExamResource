/*
author:xiejiabin@163.com
*/
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
        function (m, i) {
            return args[i];
        });
}
String.format = function () {
    if (arguments.length == 0)
        return null;
    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
}
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
String.prototype.ltrim = function () {
    return this.replace(/(^\s*)/g, "");
}
String.prototype.rtrim = function () {
    return this.replace(/(\s*$)/g, "");
}
String.prototype.replaceAll = function (c1,c2) {
    return this.replace(new RegExp(c1, "gm"), c2);
}
String.prototype.cint = function () {
    var _array = this.split("");
    var _this = "";
    for (var i = 0; i < _array.length; i++)
        _this += (isNaN(_array[i]) ? _array[i] : ["零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十"][_array[i]]);
    return _this;
}
Date.prototype.format = function (format) {
    format = format || "yy-MM-dd hh:mm:ss";
    var o =
    {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}
String.prototype.toDate = function (format) {
    var date = new Date(parseInt(this.replace("/Date(", "").replace(")/", ""), 10));
    return date.format(format);
};
function GetRandomNum(Min, Max) {
    var Range = Max - Min;
    var Rand = Math.random();
    return (Min + Math.round(Rand * Range));
}   