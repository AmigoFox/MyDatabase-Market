namespace API_DatabaseMarket.DTOs.Auth
{
    public record RegisterRequest(
        string Login,
        string Email,
        string Password,
        string? FullName,
        string? Phone
    );

}
