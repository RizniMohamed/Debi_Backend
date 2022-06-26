using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Debi.Model
{
    public class User
    {
        private int userId;
        private string name;
        private string image;
        private int contact;
        private string address;
        private double nic;

        private int roleID;
        private int authID;

        private string email;
        private string password;

        private string role;

        public int UserId { get => userId; set => userId = value; }
        public string Name { get => name; set => name = value; }
        public string Image { get => image; set => image = value; }
        public int Contact { get => contact; set => contact = value; }
        public string Address { get => address; set => address = value; }
        public double Nic { get => nic; set => nic = value; }
        public int RoleID { get => roleID; set => roleID = value; }
        public int AuthID { get => authID; set => authID = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string Role { get => role; set => role = value; }
    }
}