namespace Application.Dtos.Reviews;

public class ReviewInputDto
{
    public long UserId { get; set; }

    public short Rating { get; set; }

    public string Title { get; set; }

    public string Comment { get; set; }

    public DateTime CreatedAt { get; set; }
}