using BaseBackend.Application.Common.Exceptions;

namespace BaseBackend.Application.DTOs
{
    public class RegisterRequest
    {
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;

        public string Name { get; init; } = null!;
        public string LastName { get; init; } = null!;

        public string Currency { get; init; } = "COP";
        public DateTime BirthDate { get; init; }

        // ✅ Validación simple de Application Layer
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Email))
                throw new ValidationException("Email es obligatorio");

            if (!Email.Contains("@"))
                throw new ValidationException("Formato de email inválido");

            if (string.IsNullOrWhiteSpace(Password))
                throw new ValidationException("Password es obligatorio");

            if (Password.Length < 6)
                throw new ValidationException("Password debe tener mínimo 6 caracteres");

            if (string.IsNullOrWhiteSpace(Name))
                throw new ValidationException("Nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(LastName))
                throw new ValidationException("Apellido es obligatorio");

            if (BirthDate == default)
                throw new ValidationException("Fecha de nacimiento inválida");

            if (BirthDate > DateTime.UtcNow)
                throw new ValidationException("Fecha de nacimiento no puede ser futura");
        }
    }
}