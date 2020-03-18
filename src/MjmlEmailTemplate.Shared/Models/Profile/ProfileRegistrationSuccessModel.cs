using MjmlEmailTemplate.Shared.Attributes;

namespace MjmlEmailTemplate.Shared.Models.Profile
{
    [TemplateRoute("profileRegistrationSuccess")]
    public class ProfileRegistrationSuccessModel : NotifyBaseModel
    {
        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string ConfirmationUrl { get; set; }
    }
}