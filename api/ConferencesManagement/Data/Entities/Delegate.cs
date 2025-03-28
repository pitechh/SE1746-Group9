using System;
using System.Collections.Generic;

namespace ConferencesManagementDAO.Data.Entities;

public partial class Delegates
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Organization { get; set; }

    public string? Position { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Nationality { get; set; }

    public string? Address { get; set; }

    public string? PassportNumber { get; set; }

    public string? AvatarUrl { get; set; }

    public string? Biography { get; set; }

    public bool? IsConfirmed { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ConferenceHostingRegistration> ConferenceHostingRegistrations { get; set; } = new List<ConferenceHostingRegistration>();

    public virtual ICollection<DelegateConferenceRole> DelegateConferenceRoles { get; set; } = new List<DelegateConferenceRole>();

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual ICollection<Conference> Conferences { get; set; } = new List<Conference>();

    public virtual ICollection<SystemRole> Roles { get; set; } = new List<SystemRole>();
}
