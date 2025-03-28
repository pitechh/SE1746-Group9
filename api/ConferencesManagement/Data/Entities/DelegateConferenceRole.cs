using System;
using System.Collections.Generic;

namespace ConferencesManagementDAO.Data.Entities;

public partial class DelegateConferenceRole
{
    public int DelegateId { get; set; }

    public int ConferenceId { get; set; }

    public int RoleId { get; set; }

    public virtual Conference Conference { get; set; } = null!;

    public virtual Delegates Delegate { get; set; } = null!;

    public virtual ConferenceRole Role { get; set; } = null!;
}
