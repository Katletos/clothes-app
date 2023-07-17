namespace Application.Dtos.Reviews;

public struct UpdateReviewDto
{
    public short Rating { get; set; }

    public string Title { get; set; }

    public string Comment { get; set; }
}