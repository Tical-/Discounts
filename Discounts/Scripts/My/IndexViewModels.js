Application.UsersViewModel = function () {
    var self = this;
    self.Users = ko.observableArray();
    function User(data) {
        var self = this;
        if (data != null) {
            self.Id = ko.observable(data.Id);
            self.Email = ko.observable(data.Email);
            self.UserName = ko.observable(data.UserName);
            self.IsAdmin = ko.observable(data.IsAdmin);
        }
        return self;
    }
    self.GetUsers = function () {
        self.Users.removeAll();
        var answ = Application.POST("/api/AdminApi/GetUsers");
        answ.success(function (data) {
            $.each(data, function (index, value) {
                self.Users.push(new User(value));
            });
        });
    };
    self.Delete = function (data) {
        self.Users.remove(data);
    }.bind(this);
    self.Save = function (data) {
        var temp = ko.toJSON(ko.toJS(this));
        var answ = Application.POST("/api/AdminApi/SaveUsers", temp);
        answ.always(function() {
            self.GetUsers();
        });
    }
    self.ShowAddForm = function (data) {
        $("#ShowAdd").hide();
        $("#AddForm").show();
    }
    self.Insert = function (data) {
        var exists = false;
        $.each(this.Users(), function (index, value) {
            if (value.Email() == $("#Email").val()) {
                exists = true;
            }
        });
        if (!exists) {
            if ($("#Email").val())
                var user = new User();
            user.Id = ko.observable("-");
            user.Email = ko.observable($("#Email").val());
            user.IsAdmin = ko.observable($("#IsAdmin").val());
            user.UserName = ko.observable($("#UserName").val());
            self.Users.push(user);
            $("#ShowAdd").show();
            $("#AddForm").hide();
            $("#Email").val("");
            $("#IsAdmin").val("");
            $("#UserName").val("");
        } else {
            alert('Пользователь с данным Email уже существует');
        }
    }
    self.GetUsers();
}