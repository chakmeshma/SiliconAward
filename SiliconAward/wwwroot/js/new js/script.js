var bindLoginContainerResizeHandler = function() {


    var windowHeight = window.innerHeight;
    var windowWidth = window.innerWidth;
    var elementHeight = $(".loginContainer").height();
    var elementWidth = $(".loginContainer").width();
    var topMargin = (windowHeight - elementHeight) / 2;

    $(".loginContainer").css("marginTop", "" + topMargin + "px");
};

function init() {
    $(window).ready(bindLoginContainerResizeHandler);
    $(window).resize(bindLoginContainerResizeHandler);

}

function togglePasswordView() {
    var current = $("#passwordField").attr("type");

    if (current == "password")
        $("#passwordField").attr("type", "text")
    else
        $("#passwordField").attr("type", "password")
}

function initInputEvents() {
    $(".code-entry-input").click(function() {
        $(this).select();
    });
    $(".code-entry-input").keyup(function() {
        if (this.value.length == this.maxLength) {
            $(this).next('.code-entry-input').focus();
            $(this).next('.code-entry-input').select()
        }
    });


    $("#theForm").submit(function() {
        var codeStr = $("#codeInput1").val() +
                      $("#codeInput2").val() +
                      $("#codeInput3").val() +
                      $("#codeInput4").val() +
                      $("#codeInput5").val() +
                      $("#codeInput6").val();

        $("#confirmHiddenField").val(codeStr);
        return true;
    });

}