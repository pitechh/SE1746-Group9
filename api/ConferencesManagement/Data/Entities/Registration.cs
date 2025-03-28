using System;
using System.Collections.Generic;

namespace ConferencesManagementDAO.Data.Entities;

public partial class Registration
{
    public int Id { get; set; }

    public int DelegateId { get; set; }

    public int ConferenceId { get; set; }

    public string? Status { get; set; }

    public DateTime? RegisteredAt { get; set; }

    public virtual Conference Conference { get; set; } = null!;

    public virtual Delegates Delegate { get; set; } = null!;
}
