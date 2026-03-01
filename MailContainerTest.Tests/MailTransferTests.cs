using MailContainerTest.Data;
using MailContainerTest.Services;
using MailContainerTest.Types;
using Moq;
using Xunit;

namespace MailContainerTest.Tests
{
    public class MailTransferTests
    {

        private Mock<IMailContainerDataStore> _dataStore;
        private Mock<IMailContainerValidator> _validator;
        private MailTransferService _service;

        public MailTransferTests()
        {
            _dataStore = new Mock<IMailContainerDataStore>();
            _validator = new Mock<IMailContainerValidator>();
            _service = new MailTransferService(_dataStore.Object, _validator.Object);
        }


        [Fact]
        public void Validate_ShouldReturnFalse_WhenContainerIsNull()
        {
            var validator = new MailContainerValidator();

            var request = new MakeMailTransferRequest();
            request.MailType = MailType.StandardLetter;

            var result = validator.Validate(null, request);

            Assert.False(result.Success);
        }

        [Fact]
        public void Validate_ShouldReturnTrue_WhenTypesMatch()
        {
            var validator = new MailContainerValidator();

            var container = new MailContainer();
            container.AllowedMailType = AllowedMailType.StandardLetter;

            var request = new MakeMailTransferRequest();
            request.MailType = MailType.StandardLetter;

            var result = validator.Validate(container, request);

            Assert.True(result.Success);
        }

        [Fact]
        public void Validate_ShouldReturnFalse_WhenTypesDoNotMatch()
        {
            var validator = new MailContainerValidator();

            var container = new MailContainer();
            container.AllowedMailType = AllowedMailType.LargeLetter;

            var request = new MakeMailTransferRequest();
            request.MailType = MailType.StandardLetter;

            var result = validator.Validate(container, request);

            Assert.False(result.Success);
        }

        [Fact]
        public void Validate_ShouldFail_WhenNotEnoughCapacity()
        {
            var validator = new MailContainerValidator();

            var container = new MailContainer();
            container.AllowedMailType = AllowedMailType.LargeLetter;
            container.Capacity = 3;

            var request = new MakeMailTransferRequest();
            request.MailType = MailType.LargeLetter;
            request.NumberOfMailItems = 5;

            var result = validator.Validate(container, request);

            Assert.False(result.Success);
        }

        [Fact]
        public void Validate_ShouldFail_WhenContainerIsOutOfService()
        {
            var validator = new MailContainerValidator();

            var container = new MailContainer();
            container.AllowedMailType = AllowedMailType.SmallParcel;
            container.Status = MailContainerStatus.OutOfService;

            var request = new MakeMailTransferRequest();
            request.MailType = MailType.SmallParcel;

            var result = validator.Validate(container, request);

            Assert.False(result.Success);
        }

        [Fact]
        public void Validate_ShouldPass_WhenContainerIsOperational()
        {
            var validator = new MailContainerValidator();

            var container = new MailContainer();
            container.AllowedMailType = AllowedMailType.SmallParcel;
            container.Status = MailContainerStatus.Operational;

            var request = new MakeMailTransferRequest();
            request.MailType = MailType.SmallParcel;

            var result = validator.Validate(container, request);

            Assert.True(result.Success);
        }

        [Fact]
        public void MakeMailTransfer_ShouldNotUpdate_WhenValidationFails()
        {
            var service = new MailTransferService(_dataStore.Object, _validator.Object);

            var request = new MakeMailTransferRequest();
            request.SourceMailContainerNumber = "1";
            request.DestinationMailContainerNumber = "2";
            request.NumberOfMailItems = 5;
            request.MailType = MailType.StandardLetter;

            _dataStore.Setup(x => x.GetMailContainer("1")).Returns(new MailContainer());
            _dataStore.Setup(x => x.GetMailContainer("2")).Returns(new MailContainer());

            _validator.Setup(x => x.Validate(It.IsAny<MailContainer>(), request))
                     .Returns(new MakeMailTransferResult { Success = false });

            service.MakeMailTransfer(request);

            _dataStore.Verify(x => x.UpdateMailContainer(It.IsAny<MailContainer>()), Times.Never);
        }

        [Fact]
        public void MakeMailTransfer_ShouldReduceSourceCapacity()
        {
            var service = new MailTransferService(_dataStore.Object, _validator.Object);

            var source = new MailContainer();
            source.Capacity = 10;

            var request = new MakeMailTransferRequest();
            request.SourceMailContainerNumber = "1";
            request.DestinationMailContainerNumber = "2";
            request.NumberOfMailItems = 5;
            request.MailType = MailType.StandardLetter;

            _dataStore.Setup(x => x.GetMailContainer("1")).Returns(source);
            _dataStore.Setup(x => x.GetMailContainer("2")).Returns(new MailContainer());

            _validator.Setup(x => x.Validate(It.IsAny<MailContainer>(), request))
                     .Returns(new MakeMailTransferResult { Success = true });

            service.MakeMailTransfer(request);

            Assert.Equal(5, source.Capacity);
        }

        [Fact]
        public void MakeMailTransfer_ShouldIncreaseDestinationCapacity()
        {
            var service = new MailTransferService(_dataStore.Object, _validator.Object);

            var destination = new MailContainer();
            destination.Capacity = 10;

            var request = new MakeMailTransferRequest();
            request.SourceMailContainerNumber = "1";
            request.DestinationMailContainerNumber = "2";
            request.NumberOfMailItems = 5;
            request.MailType = MailType.StandardLetter;

            _dataStore.Setup(x => x.GetMailContainer("1")).Returns(new MailContainer());
            _dataStore.Setup(x => x.GetMailContainer("2")).Returns(destination);

            _validator.Setup(x => x.Validate(It.IsAny<MailContainer>(), request))
                     .Returns(new MakeMailTransferResult { Success = true });

            service.MakeMailTransfer(request);

            Assert.Equal(15, destination.Capacity);
        }
    }
}