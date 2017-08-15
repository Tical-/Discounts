using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Discounts.Models
{
    public class UsersModel
    {
        public List<AdmUser> Users { get; set; }
        public class AdmUser
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }
            public bool IsAdmin { get; set; }
        }
    }
}