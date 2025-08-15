namespace SimpleDataEntryForm.Models
{
    public class DataEntry
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Title { get; set; } = "";
        public int? Age { get; set; }
        public string? Hometown { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
