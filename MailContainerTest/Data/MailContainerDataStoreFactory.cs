using System.Configuration;

namespace MailContainerTest.Data
{

    //created factory class to initialize the mail container based on the env var
    //it would be injected in the Program file as follows
    //services.AddScoped<IMailContainerDataStore>(sp => MailContainerDataStoreFactory.Create());
    public class MailContainerDataStoreFactory
    {
        public static IMailContainerDataStore Create()
        {
            var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

            if (dataStoreType == "Backup")
                return new BackupMailContainerDataStore();

            return new MailContainerDataStore();
        }
    }
}
