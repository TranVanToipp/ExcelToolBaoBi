using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelTool.Models;

public class TemplateModel
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string FilePath { get; set; } = "";

    public DateTime CreatedAt { get; set; }
}