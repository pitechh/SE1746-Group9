public class AddDelegateConferenceRoleDTO
{
    public int DelegateId { get; set; }
    public int ConferenceId { get; set; }
    public int RoleId { get; set; }
}

public class UpdateDelegateConferenceRoleDTO
{
    public int RoleId { get; set; }
}

public class DelegateConferenceRoleResponseDTO
{
    public int DelegateId { get; set; }
    public int ConferenceId { get; set; }
    public int RoleId { get; set; }
    public string DelegateName { get; set; } = null!;
    public string ConferenceName { get; set; } = null!;
    public string RoleName { get; set; } = null!;
}
