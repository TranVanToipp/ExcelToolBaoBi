using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelTool.Models;

public class ConfigModel
{
    public int Id { get; set; }

    public string Theme { get; set; } = "Light";

    public string ExcelFolder { get; set; } = "";

    public string ExportFolder { get; set; } = "";

    public bool AutoBackup { get; set; }

    public DateTime UpdatedAt { get; set; }
}
