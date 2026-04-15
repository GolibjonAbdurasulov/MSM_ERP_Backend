namespace Services.ViewModels.TokenViewModels;

public class TokenRequest
{
    public string GrantType { get; set; }  
    public string Email { get; set; }    // grant_type=password
    public string Password { get; set; }    // grant_type=password
    public string ClientId { get; set; }    // grant_type=client_credentials
    public string ClientSecret { get; set; }// grant_type=client_credentials
    public string Token { get; set; }       // grant_type=refresh_token
}