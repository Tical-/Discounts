ko.validation.rules['mustContrains'] = {
    validator: function (val, otherVal) {
        if (val == undefined)
            return false;
        return val.indexOf(otherVal) != -1;
    },
    message: 'The field must contrains {0}'
};

ko.validation.rules['PasswordEquals'] = {
    validator: function (val, otherVal) {
        if (val == undefined)
            return false;
        return val.indexOf(otherVal) != -1;
    },
    message: 'The field must contrains {0}'
};

ko.validation.registerExtenders();

ko.validation.locale('ru-RU');
ko.validation.init();

(function (window, undefined) {
    'use strict';
    var App = {};
    App.ViewModels = [];
    App.setAccessToken = function (accessToken) {
        sessionStorage.setItem("accessToken", accessToken);
    };
    App.Headers = function getSecurityHeaders() {
        var accessToken = localStorage["access_token"] || sessionStorage["access_token"];
        if (accessToken) {
            return { "Authorization": "Bearer " + accessToken };
        }
        return {};
    };
    App.POST = function (url, data, async = true, contentType = 'application/json; charset=utf-8') {
        var headers = App.Headers();
        return $.ajax(url, {
            async: async,
            type: 'POST',
            contentType: contentType,
            dataType: "json",
            processData: false,
            cache: false,
            headers: headers,
            data: data
        }).always(function () {
        }).error(function (dt) {
            console.log("PostError");
            console.log(dt);
            if (dt != null && dt.status != null && dt.status != undefined && dt.status === 401) {
                Logout();
            }
            if (dt != null && dt.status != null && dt.status != undefined && dt.status === 500) {
                console.log("%c"+dt.responseText, "background: black; color: white;");
            }
        });
    };
    App.GET = function (url) {
        var headers = App.Headers();
        console.log(headers);
        return $.ajax(url, {
            type: 'GET',
            cache: false,
            headers: headers
        }).always(function () {
        }).error(function (data) {
            if (data != null && data.status != null && data.status != undefined && data.status === 401) {
                Logout();
            }
        });
    };
    window.Application = App;
})(window);


