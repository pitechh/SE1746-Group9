namespace ConferencesManagementAPI.Data.DTO
{
    public class ConferenceHostingRegistrationDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Location { get; set; } = null!;

        public DateTime? CreateAt { get; set; }

        public string? Description { get; set; }

        public string? RegisterName { get; set; }

        public string? RegisterId { get; set; }

        public string Status { get; set; } = "Pending";
    }

    public class CreateConferenceHostingRegistrationDTO
    {
        public string Name { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Location { get; set; } = null!;

        public DateTime? CreateAt { get; set; }

        public string? Description { get; set; }

        public int RegisterId { get; set; }

        public string Status { get; set; } = "Pending";
    }

}
