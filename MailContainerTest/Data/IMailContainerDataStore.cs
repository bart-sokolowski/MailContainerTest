using MailContainerTest.Types;

namespace MailContainerTest.Data
{
    public interface IMailContainerDataStore
    {
        public MailContainer GetMailContainer(string mailContainerNumber);
        public void UpdateMailContainer(MailContainer mailContainer);
    }
}
