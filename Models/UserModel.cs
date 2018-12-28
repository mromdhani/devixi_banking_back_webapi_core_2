using System;

namespace MonProjetBanking_Back.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfJoing { get; set; }
        public string  Pouvoir{ get; set; }
    }
}