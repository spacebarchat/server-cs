using Newtonsoft.Json;

namespace Fosscord.DbModel.Classes;

public class RegisterData
{
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("invite")]
    public object Invite { get; set; }

    [JsonProperty("consent")]
    public bool Consent { get; set; }

    [JsonProperty("date_of_birth")]
    public string DateOfBirth { get; set; }

    [JsonProperty("gift_code_sku_id")]
    public object GiftCodeSkuId { get; set; }

    [JsonProperty("captcha_key")]
    public object CaptchaKey { get; set; }
}

public class LoginData
{
    [JsonProperty("login")]
    public string Login { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("undelete")]
    public bool Undelete { get; set; }

    [JsonProperty("captcha_key")]
    public object CaptchaKey { get; set; }

    [JsonProperty("login_source")]
    public object LoginSource { get; set; }

    [JsonProperty("gift_code_sku_id")]
    public object GiftCodeSkuId { get; set; }
}