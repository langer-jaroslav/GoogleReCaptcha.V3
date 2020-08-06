using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GoogleReCaptcha.V3.Interface
{
    public interface ICaptchaValidator
    {
        Task<bool> IsCaptchaPassedAsync(string token);
        Task<JObject> GetCaptchaResultDataAsync(string token);
        void UpdateSecretKey(string key);
    }
}
