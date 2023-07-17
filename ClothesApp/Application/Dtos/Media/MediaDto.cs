namespace Application.Dtos.Media;

public struct MediaDto
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string FileType { get; set; }

    public string FileName { get; set; }
}