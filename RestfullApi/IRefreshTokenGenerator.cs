namespace RestfullApi
{
    public interface IRefreshTokenGenerator
    {
        string GenerateToken(string username);
    }
}
