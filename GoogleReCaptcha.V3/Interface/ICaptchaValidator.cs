using System.Threading.Tasks;

namespace GoogleReCaptcha.V3.Interface
{
    public interface ICaptchaValidator
    {
        Task<bool> IsCaptchaPassedAsync(string gRecaptchaResponse);
    }
}
