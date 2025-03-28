namespace ConferencesManagementAPI.Data.DTO
{
    public class AuthRequestDTO
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class AuthResponseDTO
    {
        public string Token { get; set; } = "";
        public DateTime Expiration { get; set; }
    }


    public class ChangePasswordDTO
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;

    }

    public class RegisterDelegatesDTO
    {
        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;

        public string? Organization { get; set; }

        public string? Position { get; set; }

        public string? Gender { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Nationality { get; set; }

        public string? Address { get; set; }

        public string? PassportNumber { get; set; }

        public string? AvatarUrl { get; set; }

        public string? Biography { get; set; }
    }
}
