namespace ConferencesManagementAPI.Data.DTO
{
    public class DelegatesDTO
    {
        public class UpdateDelegatesRequestDTO
        {
            public int Id { get; set; }

            public string FullName { get; set; } = null!;

            public string Phone { get; set; } = null!;

            public string Password { get; set; } = "";

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

        public class AddDelegatesRequestDTO
        {
            public string FullName { get; set; } = null!;

            public string Email { get; set; } = null!;

            public string Phone { get; set; } = null!;

            public string Password { get; set; } = "";

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

        public class GetDelegatesResponseDTO
        {
            public int Id { get; set; }

            public string FullName { get; set; } = null!;

            public string Email { get; set; } = null!;

            public string Phone { get; set; } = null!;

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
}
