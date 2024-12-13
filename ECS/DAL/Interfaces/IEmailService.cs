﻿namespace ECS.DAL.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipientEmail, string subject, string body);
    }
}
