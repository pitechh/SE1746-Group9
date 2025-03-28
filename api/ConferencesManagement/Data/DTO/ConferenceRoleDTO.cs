namespace ConferencesManagementAPI.Data.DTO
{
    // DTO cho phản hồi thông tin vai trò hội thảo
    public class ConferenceRoleResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        
    }

    // DTO cho yêu cầu thêm mới vai trò hội thảo
    public class AddConferenceRoleRequestDTO
    {
        public string Name { get; set; } = null!;
    }

    // DTO cho yêu cầu cập nhật vai trò hội thảo
    public class UpdateConferenceRoleRequestDTO
    {
        public string Name { get; set; } = null!;
    }
}
