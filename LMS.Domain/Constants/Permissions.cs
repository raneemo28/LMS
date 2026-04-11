namespace LMS.Domain.Constants;

public static class Permissions
{
    public static class System
    {
        public const string ManageUsers = "Permissions.System.Users.Manage";
        public const string ViewLogs = "Permissions.System.Logs.View";
    }

    public static class Metadata
    {
        public const string ManageVocabularies = "Permissions.Metadata.Vocabularies.Manage";
        public const string ManageTemplates = "Permissions.Metadata.Templates.Manage";
    }

    public static class Catalog
    {
        public const string CreateItem = "Permissions.Catalog.Items.Create";
        public const string EditItem = "Permissions.Catalog.Items.Edit";
        public const string DeleteItem = "Permissions.Catalog.Items.Delete";
        public const string ManageSets = "Permissions.Catalog.Sets.Manage";
        public const string ManageMedia = "Permissions.Catalog.Media.Manage";
    }
}