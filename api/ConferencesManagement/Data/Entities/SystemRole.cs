using System;
using System.Collections.Generic;

namespace ConferencesManagementDAO.Data.Entities;

public partial class SystemRole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Delegates> Delegates { get; set; } = new List<Delegates>();
}
