﻿namespace QLDatVeMayBay.Services
{
    public class EmailSettings
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
