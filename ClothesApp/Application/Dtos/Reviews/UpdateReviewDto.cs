namespace Application.Dtos.Reviews;

public class UpdateReviewDto
{
    public short Rating { get; set; }

    public string Title { get; set; }

    public string Comment { get; set; }
}