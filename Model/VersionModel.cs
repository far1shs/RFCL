namespace RFCL.Model;

public class VersionModel
{
    public class VersionSourceList
    {
        public string Id { get; set; }
    }

    public class VersionPathSourceList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}