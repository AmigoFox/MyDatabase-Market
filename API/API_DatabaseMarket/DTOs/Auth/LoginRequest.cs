namespace API_DatabaseMarket.DTOs.Auth
{
    public record LoginRequest(
        string Login,
        string Password
    );

}
