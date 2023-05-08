﻿namespace ClothesApp.Entities;

public class Section
{
    public long Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    
    public virtual ICollection<SectionCategory> SectionCategories { get; } = new List<SectionCategory>();
}
