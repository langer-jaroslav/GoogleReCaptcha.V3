# GoogleReCaptcha.V3
Library for Google's ReCaptcha v3 backend
https://www.google.com/recaptcha

Target platform: .NET Core 3.1+

When targeting Core 3.0 use package version 1.1.2

# Interface

    Task<bool> IsCaptchaPassedAsync(string token);
    Task<JObject> GetCaptchaResultDataAsync(string token);
    void UpdateSecretKey(string key);

# Setup guide
Install the package https://www.nuget.org/packages/GoogleReCaptcha.V3/ using nuget
Update your appsetting.json config file

    "googleReCaptcha:SiteKey": "YOUR_SITE_KEY",
    "googleReCaptcha:SecretKey": "YOUR_SECRET_KEY"
    "googleReCaptcha:MinimumScore": "YOUR_MINIMUM_ACCEPTABLE_SCORE"
    
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
        
    async Task<JObject> GetCaptchaResultDataAsync(string token)
    
To update secret key there is method.

    void UpdateSecretKey(string key);
    
Method must be called jsut before the validation await _captchaValidator.IsCaptchaPassedAsync(captcha) after the form has been sent like this:

    [HttpPost]
    public async Task<IActionResult> Register(SampleViewModel collection, string captcha)
    {
        _captchaValidator.UpdateSecretKey("NEW SECRET KEY");
        if (!await _captchaValidator.IsCaptchaPassedAsync(captcha))
        {
            ModelState.AddModelError("captcha","Captcha validation failed");
        }
        if(ModelState.IsValid)
        {
            ViewData["Message"] = "Success";
        }
        return View(collection);
    }
    
# Exceptions
Methods can throw 'HttpRequestException' when connection to the Google's service doesn't work or service doesn't response with HTTP 200

# Contact
Contact me in case you find any errors or problem via email langerjara@gmail.com
