﻿namespace Domain.Entities;

public class Section
{
    public long Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Category> Categories { get; set; }

    public virtual ICollection<SectionCategory> SectionsCategories { get; set; }
}