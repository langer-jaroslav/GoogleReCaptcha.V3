# GoogleReCaptcha.V3
Library for Google's ReCaptcha v3 backend
https://www.google.com/recaptcha

# Interface

    Task<bool> IsCaptchaPassedAsync(string token);
    Task<JObject> GetCaptchaResultData(string token);

# Setup guide
Install the package https://www.nuget.org/packages/GoogleReCaptcha.V3/ using nuget
Update your appsetting.json config file

    "googleReCaptcha:SiteKey": "_YOUR_SITE_KEY",
    "googleReCaptcha:SecretKey": "YOUR_SECRET_KEY"
    
You can get your keys here: https://www.google.com/recaptcha/admin

Update your middleware

    services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();

Update your Views

    @inject Microsoft.Extensions.Configuration.IConfiguration Configuration
    
    <form asp-action="FormPost">
        <input type="hidden" name="captcha" id="captchaInput" value=""/>
        ...
    </form>

    @section Scripts
    {
        <script src="https://www.google.com/recaptcha/api.js?render=@Configuration["googleReCaptcha:SiteKey"]"></script>
        <script>
        grecaptcha.ready(function() {
            grecaptcha.execute('@Configuration["googleReCaptcha:SiteKey"]', { action: 'contact' }).then(function (token) {
                $("#captchaInput").val(token);
            });
        });
        </script>
    }

Update your controller

inject ICaptchaValidator

    public class HomeController : Controller
    {
      private readonly ICaptchaValidator _captchaValidator;
      public HomeController(ICaptchaValidator captchaValidator)
      {
          _captchaValidator = captchaValidator;
      }
      [HttpPost]
      public async Task<IActionResult> FormPost(Model collection, string captcha){
        if (!await _captchaValidator.IsCaptchaPassedAsync(captcha))
        {
            ModelState.AddModelError("captcha","Captcha validation failed");
        }
        if(ModelState.IsValid){
            ...
        }
        return View(collection)
      }

# Additional info
You can use following method instead to get all response data
        
    async Task<JObject> GetCaptchaResultData(string token)
    
# Exceptions
Methods can throw 'HttpRequestException' when connection to the Google's service doesn't work or service doesn't response with HTTP 200
