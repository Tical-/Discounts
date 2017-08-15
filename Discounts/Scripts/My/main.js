var UserRoleManagerViewModel = function () {
    var self = this;
    self.isLoggedIn = ko.observable("");
    self.userId = ko.observable("");
    self.roles = ko.observableArray([]);
    self.IsUserInRole = function (roleName) {
        for (var i = 0; i < self.roles().length; i++) {
            if (self.roles()[i] === roleName)
                return true;
        }
        return false;
    }

};

var UserRoleManager = new UserRoleManagerViewModel();