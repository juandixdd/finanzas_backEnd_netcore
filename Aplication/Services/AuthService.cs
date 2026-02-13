using BaseBackend.Domain.Entities;
using BaseBackend.Domain.Interfaces;
using BaseBackend.Application.Common.Exceptions;

namespace BaseBackend.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    // ✅ REGISTRO
    public async Task RegisterAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ValidationException("Email is required");

        if (!email.Contains("@"))
            throw new ValidationException("Invalid email format");

        if (string.IsNullOrWhiteSpace(password))
            throw new ValidationException("Password is required");

        if (password.Length < 6)
            throw new ValidationException("Password must be at least 6 characters");

        var existingUser = await _userRepository.GetByEmailAsync(email);

        if (existingUser != null)
            throw new ValidationException("User already exists");

        var passwordHash = _passwordHasher.Hash(password);

        var user = new User(email, passwordHash);

        await _userRepository.AddAsync(user);
    }

    // ✅ LOGIN
    public async Task<string> LoginAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            throw new ValidationException("Email and password are required");

        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
            throw new UnauthorizedException("Invalid email or password");

        var isValid = _passwordHasher.Verify(password, user.PasswordHash);

        if (!isValid)
            throw new UnauthorizedException("Invalid email or password");

        return _jwtTokenGenerator.GenerateToken(user);
    }
}
