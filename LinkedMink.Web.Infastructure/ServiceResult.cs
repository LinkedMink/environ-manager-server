using System.Reflection;
using LinkedMink.Web.Infastructure.Resources;

namespace LinkedMink.Web.Infastructure
{
    public enum ServiceResultCode
    {
        // General
        Success = 0,
        Failed = 1,
        NotFound = 2,
        ValidationError = 3,

        // Authentication
        IsLockedOut = 100,
        IsInactive = 101,

        RegistrationCodeIncorrect = 1000,
    }

    public class ServiceResult<TData>
    {
        public ServiceResult()
        {
            Code = ServiceResultCode.Success;
            Description = null;
            Errors = null;
        }

        public ServiceResultCode Code
        {
            get => _code;
            set
            {
                var descriptionField = typeof(ServiceResultCodeDescriptions).GetProperty(
                    value.ToString(), 
                    BindingFlags.NonPublic | BindingFlags.Static);

                if (descriptionField != null)
                {
                    Description = (string)descriptionField.GetValue(null);
                }

                _code = value;
            }
        }

        public string Description { get; set; }

        public TData Data { get; set; }

        public object Errors { get; set; }

        private ServiceResultCode _code;
    }
}
