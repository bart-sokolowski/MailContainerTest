using MailContainerTest.Types;

namespace MailContainerTest.Services
{
    public interface IMailContainerValidator
    {
        public MakeMailTransferResult Validate(MailContainer mailContainer, MakeMailTransferRequest request);
    }
}
