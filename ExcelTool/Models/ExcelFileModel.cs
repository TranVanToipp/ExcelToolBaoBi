using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelTool.Models;

public class ExcelFileModel
{
    public int Id { get; set; }

    public string FileName { get; set; } = "";

    public string FilePath { get; set; } = "";

    public string Status { get; set; } = "";

    public DateTime CreatedAt { get; set; }
}
