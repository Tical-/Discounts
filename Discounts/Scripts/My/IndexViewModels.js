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
        answ.always(function () {
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
//====================================================================//
Application.BrandsViewModel = function () {
    var self = this;
    self.Brands = ko.observableArray();
    function Brand(data) {
        var self = this;
        if (data != null) {
            self.Id = ko.observable(data.Id);
            self.Name = ko.observable(data.Name);
            self.ImageId = ko.observable(data.ImageId);
            self.Description = ko.observable(data.Description);
            self.File = ko.observable(data.File);
        }
        return self;
    }
    self.GetBrands = function () {
        self.Brands.removeAll();
        var answ = Application.POST("/api/AdminApi/GetBrands");
        answ.success(function (data) {
            $.each(data, function (index, value) {
                self.Brands.push(new Brand(value));
            });
        });
    };
    self.GetBrands();
    self.Insert = function (data) {
        var ImageId = 0;
        var filename = "";
        var formData = new FormData();
        formData.append('file', $('#File')[0].files[0]);
        var answ = Application.POST("/api/AdminApi/UploadFile/", formData, false, false);
        answ.success(function (data) {
            ImageId = data.ID;
            filename = data.file;
            var answ2 = Application.POST("/api/AdminApi/InsertBrand", ko.toJSON({ Name: $("#Name").val(), Description: $("#Description").val(), ImageId: ImageId, Id: 0, File: "" }));
            answ2.success(function (data) {
                var tempScrollTop = $(window).scrollTop();
                var brand = new Brand();
                brand.Id = ko.observable(data);
                brand.Name = ko.observable($("#Name").val());
                brand.Description = ko.observable($("#Description").val());
                brand.File = ko.observable(filename);
                $("#Name").val("");
                $("#Description").val("");
                self.Brands.push(brand);
                setTimeout(function () {
                    $('#All').masonry('destroy');
                    $('#All').masonry({
                        itemSelector: '.item',
                        isAnimated: true
                    });
                    $(window).scrollTop(tempScrollTop);
                }, 1500);
            });
        });
    }
    self.Delete = function (dt) {
        if (confirm("Удалить бренд?")) {
            var answ = Application.POST("/api/AdminApi/DeleteBrand/" + dt.Id());
            answ.success(function (data) {
                self.Brands.remove(dt);
                $('#All').masonry({
                    itemSelector: '.item',
                    isAnimated: true
                });
            });
        }
    }.bind(this);
}


//====================================================================//
Application.StoresViewModel = function () {
    var self = this;

}