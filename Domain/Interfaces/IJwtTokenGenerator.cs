namespace BaseBackend.Domain.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Entities.User user);
}