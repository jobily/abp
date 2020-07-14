namespace UnifiedMigration
{
    public class ProjectModel
    {
        public string Path { get; set; }

        public bool Recreate { get; set; }

        public string MigrationName { get; set; }
    }
}
