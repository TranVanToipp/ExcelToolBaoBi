using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelTool.Models;

public class HistoryModel
{
    public int Id { get; set; }

    public string Action { get; set; } = "";

    public string FileName { get; set; } = "";

    public DateTime CreatedAt { get; set; }
}