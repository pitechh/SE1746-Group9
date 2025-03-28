namespace ConferencesManagementAPI.Constants
{
    public static class RegistrationStatusConstants
    {
        public const string STATUS_Pending = "Pending";
        public const string STATUS_Confirmed = "Confirmed";
        public const string STATUS_Cancelled = "Cancelled";

        public static readonly HashSet<string> ValidStatuses = new() { STATUS_Pending, STATUS_Confirmed, STATUS_Cancelled };

        public static bool IsValidStatus(string status, out string errorMessage)
        {
            try
            {
                if (ValidStatuses.Contains(status))
                {
                    errorMessage = string.Empty;
                    return true;
                }
                errorMessage = "Invalid registration status.";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error validating status: {ex.Message}";
                return false;
            }
        }

        public static class ConfHostingRegistrationStatusConstants
        {
            public const string STATUS_PENDING = "Pending";
            public const string STATUS_APPROVED = "Approved";
            public const string STATUS_DENIED = "Denied";
        }

    }
}
