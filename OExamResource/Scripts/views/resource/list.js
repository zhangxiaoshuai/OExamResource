function nofindImg(e, ico) {
    e.onerror = null;
    e.src = ico;
}
$(function () {
    $('a.ban').live('click', function () {
        $.dialog.tips('正式版开放此资源');
    });
});