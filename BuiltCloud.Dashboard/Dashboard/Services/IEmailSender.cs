#region Using

using System.Threading.Tasks;

#endregion

namespace BuiltCloud.Dashboard.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
