using MailContainerTest.Data;
using MailContainerTest.Types;

namespace MailContainerTest.Services
{
    public class MailTransferService : IMailTransferService
    {
        private readonly IMailContainerDataStore _dataStore;
        private readonly IMailContainerValidator _validator;

        public MailTransferService(IMailContainerDataStore dataStore, IMailContainerValidator validator)
        {
            _dataStore = dataStore;
            _validator = validator;
        }

        public MakeMailTransferResult MakeMailTransfer(MakeMailTransferRequest request)
        {
            var sourceContainer = _dataStore.GetMailContainer(request.SourceMailContainerNumber);
            var destinationContainer = _dataStore.GetMailContainer(request.DestinationMailContainerNumber);

            var result = _validator.Validate(sourceContainer, request);

            if (result.Success)
            {
                sourceContainer.Capacity -= request.NumberOfMailItems;
                destinationContainer.Capacity += request.NumberOfMailItems;

                _dataStore.UpdateMailContainer(sourceContainer);
                _dataStore.UpdateMailContainer(destinationContainer);
            }

            return result;
        }
    }
}
