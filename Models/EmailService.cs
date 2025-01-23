using System.Net;
using System.Net.Mail;
using TestGp.Models;

public class EmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _fromEmail;
    private readonly string _senderDisplayName;
    private readonly string _noReplyEmailAddress;

    public EmailService(IConfiguration configuration)
    {
        _smtpServer = configuration["EmailSettings:SmtpServer"];
        _smtpPort = configuration.GetValue<int>("EmailSettings:SmtpPort");
        _smtpUsername = configuration["EmailSettings:SmtpUsername"];
        _smtpPassword = configuration["EmailSettings:SmtpPassword"];
        _fromEmail = configuration["EmailSettings:FromEmail"];
        _senderDisplayName = configuration["EmailSettings:SenderDisplayName"];
        _noReplyEmailAddress = configuration["EmailSettings:NoReplyEmailAddress"];
    }

    public void SendBookingConfirmationEmail(string toEmail, Register r, string username, Booking b, int BookingID, int Tnum)
    {
        try
        {
            using (SmtpClient smtpClient = new SmtpClient(_smtpServer))
            {
                smtpClient.Port = _smtpPort;
                smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                smtpClient.EnableSsl = true;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_noReplyEmailAddress, _senderDisplayName);
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = "Your Booking have been confirmed ";
                mailMessage.Body = $"Thank you,{username} for your booking. Your Booking number is: {BookingID} , number of tickets is: {Tnum}.";

                smtpClient.Send(mailMessage);
            }
        }
        catch (Exception ex)
        {
            // Handle or log the exception
            Console.WriteLine($"Error sending email: {ex}");
            throw; // Re-throw the exception to propagate it up the call stack if needed
        }
    }
}