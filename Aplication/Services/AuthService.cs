using BaseBackend.Domain.Entities;
using BaseBackend.Domain.Interfaces;
using BaseBackend.Application.Common.Exceptions;
using BaseBackend.Application.DTOs;

namespace BaseBackend.Application.Services
{
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

        // ✅ REGISTRO CORRECTO
        public async Task RegisterAsync(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ValidationException("Email is required");

            if (!request.Email.Contains("@"))
                throw new ValidationException("Invalid email format");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ValidationException("Password is required");

            if (request.Password.Length < 6)
                throw new ValidationException("Password must be at least 6 characters");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ValidationException("Name is required");

            if (string.IsNullOrWhiteSpace(request.LastName))
                throw new ValidationException("Last name is required");

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
                throw new ValidationException("User already exists");

            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = new User(
                email: request.Email,
                passwordHash: passwordHash,
                name: request.Name.Trim(),
                lastName: request.LastName.Trim(),
                currency: request.Currency,
                birthDate: request.BirthDate
            );

            await _userRepository.AddAsync(user);
        }

        // ✅ LOGIN (correcto)
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
}
