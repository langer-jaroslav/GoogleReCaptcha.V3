using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GoogleReCaptcha.V3.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace GoogleReCaptcha.V3
{
    public class GoogleReCaptchaValidator : ICaptchaValidator
    {

        private readonly HttpClient _httpClient;
        private const string RemoteAddress = "https://www.google.com/recaptcha/api/siteverify";
        private string _secretKey;
        private readonly double _minimumScore;

        public GoogleReCaptchaValidator(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            
            _secretKey = configuration["googleReCaptcha:SecretKey"];
            _minimumScore = configuration.GetValue<double>("googleReCaptcha:MinimumScore");
        }

        public async Task<bool> IsCaptchaPassedAsync(string token)
        {
            dynamic response = await GetCaptchaResultDataAsync(token);
            if (response.success == "true")
            {
                return System.Convert.ToDouble(response.score) >= _minimumScore;
            }
            return false;
        }

        public async Task<JObject> GetCaptchaResultDataAsync(string token)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", _secretKey),
                new KeyValuePair<string, string>("response", token)
            });
            var res = await _httpClient.PostAsync(RemoteAddress, content);
            if (res.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException(res.ReasonPhrase);
            }
            var jsonResult = await res.Content.ReadAsStringAsync();
            return JObject.Parse(jsonResult);
        }

        public void UpdateSecretKey(string key)
        {
            _secretKey = key;
        }
    }
}
