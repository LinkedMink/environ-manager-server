using MailKit.Security;

namespace LinkedMink.Net.Message
{
    public class EmailOptions
    {
        public class EmailServerOptions
        {
            public string Address { get; set; }
            public int Port { get; set; }
            public SecureSocketOptions Authentication { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class EmailAddress
        {
            public string Name { get; set; }
            public string Address { get; set; }
        }

        public EmailServerOptions MailServer { get; set; }
        public string ClientDomain { get; set; }
        public EmailAddress FromAddress { get; set; }
        public EmailAddress AdministratorAddress { get; set; }
        public int TimeoutMilliseconds { get; set; }
    }
}
