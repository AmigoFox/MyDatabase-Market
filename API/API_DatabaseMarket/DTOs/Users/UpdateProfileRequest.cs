namespace API_DatabaseMarket.DTOs.Users
{
    public record UpdateProfileRequest(
        string? FullName,
        string? Phone,
        string? Email,
        string? NewPassword
    );
}
