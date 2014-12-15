$(document).ready(function () {

    $('button[data-set]').click(function () {
        $(this).addClass("active").siblings().removeClass("active");
        submit($(this));
        return false;
    });
    function submit(source) {
        var form = source.parents('form');
        setIntervalSelectedOptions(form);
        form.submit();
    };

    function setIntervalSelectedOptions(form) {
        $('button[data-set].active').each(function () {
            var button = $(this);
            var set = button.attr('data-set');
            var value = button.val();
            var input = '<input name="{0}" value="{1}" type="hidden">'.formatEx(set, value);
            $(input).appendTo(form);
        });
    }

});