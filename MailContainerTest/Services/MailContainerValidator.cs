using MailContainerTest.Types;

namespace MailContainerTest.Services
{

    //decoupled the validator from the mail transfer service to make it easier to test and maintain
    public class MailContainerValidator : IMailContainerValidator
    {
        public MakeMailTransferResult Validate(MailContainer mailContainer, MakeMailTransferRequest request) 
        {
            var result = new MakeMailTransferResult();

            if (mailContainer == null)
            {
                result.Success = false;
                return result;
            }

            switch (request.MailType)
            {
                case MailType.StandardLetter:
                    
                    if (!mailContainer.AllowedMailType.HasFlag(AllowedMailType.StandardLetter))
                    {
                        result.Success = false;
                    }
                    break;

                case MailType.LargeLetter:
                    if (!mailContainer.AllowedMailType.HasFlag(AllowedMailType.LargeLetter))
                    {
                        result.Success = false;
                    }
                    else if (mailContainer.Capacity < request.NumberOfMailItems)
                    {
                        result.Success = false;
                    }
                    break;

                case MailType.SmallParcel:
                    if (!mailContainer.AllowedMailType.HasFlag(AllowedMailType.SmallParcel))
                    {
                        result.Success = false;

                    }
                    else if (mailContainer.Status != MailContainerStatus.Operational)
                    {
                        result.Success = false;
                    }
                    break;

                default:
                    result.Success = false;
                    break;
            }

            return result;
        }
    }
}
