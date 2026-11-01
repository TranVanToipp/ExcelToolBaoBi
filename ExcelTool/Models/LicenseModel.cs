using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelTool.Models;

public class LicenseModel
{
    public int Id { get; set; }

    public string LicenseKey { get; set; } = "";

    public string LicenseType { get; set; } = "";

    public DateTime ExpiredAt { get; set; }

    public bool Activated { get; set; }
}