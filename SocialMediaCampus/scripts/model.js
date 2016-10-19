/* #####################################################################
   #
   #   Project       : Modal Login with jQuery Effects
   #   Author        : Rodrigo Amarante (rodrigockamarante)
   #   Version       : 1.0
   #   Created       : 07/29/2015
   #   Last Change   : 08/04/2015
   #
   ##################################################################### */

$(function () {

    var $formLogin = $('#login-form');
    var $formLost = $('#lost-form');
    var $formRegister = $('#register-form');
    var $divForms = $('#div-forms');
    var $modalAnimateTime = 300;
    var $msgAnimateTime = 150;
    var $msgShowTime = 2000;

    $("form").submit(function () {
        switch (this.id) {
            case "login-form":
            
                var $lg_numara = $('#lg_numara').val();
                var $lg_password = $('#lg_password').val();
                var $lg_rememberme = false;

                if ($('#login_rememberme').val() == "on") {
                    $lg_rememberme = true;
                }

                $("#login_submit_btn").attr("disabled", "disabled");

                $.ajax({
                    method: "post",
                    url: "/SiteUsers/Login",
                    data: { login_numara: $lg_numara, login_password: $lg_password, login_rememberme: $lg_rememberme }
                }).done(function (res) {
                   
                    if (res.HasError) {
                        msgChange($('#div-login-msg'), $('#icon-login-msg'), $('#text-login-msg'), "error", "glyphicon-remove", res.Message);
                        
                        $('#lg_numara').val("");
                        $('#lg_password').val("");
                        $("#login_submit_btn").removeAttr("disabled");
                    }
                    else {
                        msgChange($('#div-login-msg'), $('#icon-login-msg'), $('#text-login-msg'), "success", "glyphicon-ok", res.Message);

                        $('#lg_numara').val("");
                        $('#lg_password').val("");
                        $("#login_submit_btn").removeAttr("disabled");
                        setTimeout(function () {
                            var url = "/MainPage/Shares";
                            window.location.href = url;
                        }, 1500);
                    }
                });
               
                return false;
                break;
            case "lost-form":
                var $ls_email = $('#lost_email').val();

                $.ajax({
                    method: "post",
                    url: "/SiteUsers/LostPassword",
                    data: { lost_email: $ls_email }
                }).done(function (res) {
                    if (res.HasError) {
                        msgChange($('#div-lost-msg'), $('#icon-lost-msg'), $('#text-lost-msg'), "error", "glyphicon-remove", res.Message);

                        $('#lost_email').val("");
                    }
                    else {
                        msgChange($('#div-lost-msg'), $('#icon-lost-msg'), $('#text-lost-msg'), "success", "glyphicon-ok", res.Message);
                    }
                });
                return false;
                break;
            case "register-form":

                var dataReuqest = {

                    firstname: $("#firstname").val(),
                    lastname: $('#lastname').val(),
                    number: $('#number').val(),
                    email: $('#email').val(),
                    password: $('#password').val()
                }

                $.ajax({
                    method: "POST",
                    url: "/SiteUsers/Register",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(dataReuqest)
                }).done(function (res) {
                    if (res.HasError) {
                        msgChange($('#div-register-msg'), $('#icon-register-msg'), $('#text-register-msg'), "error", "glyphicon-remove", res.Message);

                        $('#firstname').val("");
                        $('#lastname').val("");
                        $('#number').val("");
                        $('#email').val("");
                        $('#password').val("");
                    }
                    else {
                        msgChange($('#div-register-msg'), $('#icon-register-msg'), $('#text-register-msg'), "success", "glyphicon-ok", res.Message);

                        $("#register_submit_btn").attr("disabled", "disabled");

                        setTimeout(function () {
                            location.reload();
                        }, 2000);
                    }
                });

                return false;
                break;
            default:
                return false;
        }
        return false;
    });

    $('#login_register_btn').click(function () { modalAnimate($formLogin, $formRegister) });
    $('#register_login_btn').click(function () { modalAnimate($formRegister, $formLogin); });
    $('#login_lost_btn').click(function () { modalAnimate($formLogin, $formLost); });
    $('#lost_login_btn').click(function () { modalAnimate($formLost, $formLogin); });
    $('#lost_register_btn').click(function () { modalAnimate($formLost, $formRegister); });
    $('#register_lost_btn').click(function () { modalAnimate($formRegister, $formLost); });

    function modalAnimate($oldForm, $newForm) {
        var $oldH = $oldForm.height();
        var $newH = $newForm.height();
        $divForms.css("height", $oldH);
        $oldForm.fadeToggle($modalAnimateTime, function () {
            $divForms.animate({ height: $newH }, $modalAnimateTime, function () {
                $newForm.fadeToggle($modalAnimateTime);
            });
        });
    }

    function msgFade($msgId, $msgText) {
        $msgId.fadeOut($msgAnimateTime, function () {
            $(this).text($msgText).fadeIn($msgAnimateTime);
        });
    }

    function msgChange($divTag, $iconTag, $textTag, $divClass, $iconClass, $msgText) {
        var $msgOld = $divTag.text();
        msgFade($textTag, $msgText);
        $divTag.addClass($divClass);
        $iconTag.removeClass("glyphicon-chevron-right");
        $iconTag.addClass($iconClass + " " + $divClass);
        setTimeout(function () {
            msgFade($textTag, $msgOld);
            $divTag.removeClass($divClass);
            $iconTag.addClass("glyphicon-chevron-right");
            $iconTag.removeClass($iconClass + " " + $divClass);
        }, $msgShowTime);
    }
});