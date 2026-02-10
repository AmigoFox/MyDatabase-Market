namespace API_DatabaseMarket.DTOs.Auth
{
    public record MeResponse(
        int Id,
        string Login,
        string Email,
        string Phone,
        string FullName,

        string Role
    );
}
