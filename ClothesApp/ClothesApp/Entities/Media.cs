namespace ClothesApp.Entities;

public class Media
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string Url { get; set; }

    public string FileType { get; set; }

    public string FileName { get; set; }

    public virtual Product Product { get; set; }
}
