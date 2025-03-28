namespace ConferencesManagementAPI.Data.DTO
{
    public class GeneralResponseDTO
    {
        public bool isSuccess { get; set; }

        public string Message { get; set; } = "";

        public object? data { get; set; }

    }
}
