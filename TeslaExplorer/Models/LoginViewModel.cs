using System;

namespace TeslaExplorer.Models
{
    public class LoginViewModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string SavePassword { get; set; }
        public bool IsSavePassword => SavePassword == "on";
    }
}