﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;
public class ProjectAddFormData
{
    //    public string? Image { get; set; }
    [Display(Name = "Project name", Prompt = "Enter project name")]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public string ClientId { get; set; } = null!;
    public string MemberId { get; set; } = null!;
    public int StatusId { get; set; }

}
