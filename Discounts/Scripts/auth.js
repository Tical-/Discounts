$(function () {
    AuthButtonsShowHide();
    $.ajaxSetup({
        beforeSend: function () {
            $("#Loading").show();
        },
        complete: function () {
            $("#Loading").hide();
        }
    });
});
function TestLoading() {
    $.ajax({
        type: 'POST',
        url: '/Home/Test'
    }).done(function(data) {

    });
}
function CloseLoginForm() {
    $("#LoginForm").hide();
}
function Logout() {
    sessionStorage.removeItem("access_token");
    AuthButtonsShowHide();
}
function AuthButtonsShowHide() {
    if (sessionStorage.getItem("access_token") != null) {
        $("#LoginButton").hide();
        $("#LogoutButton").show();
        $("#AccountButton").show();
    } else {
        $("#LoginButton").show();
        $("#LogoutButton").hide();
        $("#AccountButton").hide();
    }
}
function ShowLoginForm() {
    $("#LoginForm").show();
}
function Login() {
    var loginData = {
        grant_type: 'password',
        username: $("#login").val(),
        password: $("#password").val()
    };
    $.ajax({
        type: 'POST',
        url: '/Token',
        data: loginData
    }).done(function (data) {
        if (data != null && data.access_token != null && data.access_token != undefined) {
            sessionStorage.setItem('access_token', data.access_token);
            sessionStorage.setItem('expires_in', data.expires_in);
            $("#LoginForm").hide();
            AuthButtonsShowHide();
        }
    }).fail(function (data) {
        if (data.responseText ==
            "{\"error\":\"invalid_grant\",\"error_description\":\"The user name or password is incorrect.\"}"
        ) {
            alert('Неверная комбинация логин/пароль');
        } else {
            alert(data.responseText);
        }
    });
}