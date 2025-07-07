using System.Text.Json.Serialization;

namespace Spacebar.DbModel.Classes;

public class RegisterData
{
    [JsonPropertyName("email")] public string Email { get; set; }

    [JsonPropertyName("username")] public string Username { get; set; }

    [JsonPropertyName("password")] public string Password { get; set; }

    [JsonPropertyName("invite")] public object Invite { get; set; }

    [JsonPropertyName("consent")] public bool Consent { get; set; }

    [JsonPropertyName("date_of_birth")] public string DateOfBirth { get; set; }

    [JsonPropertyName("gift_code_sku_id")] public object GiftCodeSkuId { get; set; }

    [JsonPropertyName("captcha_key")] public object CaptchaKey { get; set; }
}

public class LoginData
{
    [JsonPropertyName("login")] public string Login { get; set; }

    [JsonPropertyName("password")] public string Password { get; set; }

    [JsonPropertyName("undelete")] public bool Undelete { get; set; }

    [JsonPropertyName("captcha_key")] public object CaptchaKey { get; set; }

    [JsonPropertyName("login_source")] public object LoginSource { get; set; }

    [JsonPropertyName("gift_code_sku_id")] public object GiftCodeSkuId { get; set; }
}