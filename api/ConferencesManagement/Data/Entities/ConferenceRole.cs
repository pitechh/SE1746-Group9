using System;
using System.Collections.Generic;

namespace ConferencesManagementDAO.Data.Entities;

public partial class ConferenceRole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DelegateConferenceRole> DelegateConferenceRoles { get; set; } = new List<DelegateConferenceRole>();
}
