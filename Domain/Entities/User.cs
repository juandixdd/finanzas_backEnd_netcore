using System;

namespace BaseBackend.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }   // Autoincremental por EF Core
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        
        public string Name { get; private set; }
        public string LastName { get; private set; }
        
        public string Currency { get; private set; }
        
        public DateTime BirthDate { get; private set; }
        
        public DateTime CreatedAt { get; private set; }
        
        public DateTime UpdatedAt { get; private set; }

        // Constructor privado para EF Core
        private User() { }

        // Constructor completo
        public User(string email, string passwordHash, string name, string lastName, string currency, DateTime birthDate)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Currency = currency ?? "COP";
            BirthDate = birthDate;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}