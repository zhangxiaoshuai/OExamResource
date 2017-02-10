$(function () {
    if (!('placeholder' in document.createElement('input'))) {
        $.getScript("/Scripts/plugin/placeholder/placeholder.js", function () {
            $('input,textarea').placeholder();
        });
    }
});