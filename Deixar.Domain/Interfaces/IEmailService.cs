using Deixar.DTOs;

namespace Deixar.Domain.Interfaces;

public interface IEmailService
{
    void SendMail(Message message);
}
