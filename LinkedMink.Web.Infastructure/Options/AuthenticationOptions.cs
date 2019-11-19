using Microsoft.AspNetCore.Identity;

namespace LinkedMink.Web.Infastructure.Options
{
    public class AuthenticationOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SigningKey { get; set; }
        public int DaysToExpire { get; set; }
        public IdentityOptions IdentityOptions { get; set; }
    }
}
