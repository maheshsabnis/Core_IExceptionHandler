﻿using System;
using System.Collections.Generic;

namespace Core_IExceptionHandler.Models;

public partial class Department
{
    public int DeptNo { get; set; }

    public string DeptName { get; set; } = null!;

    public string Location { get; set; } = null!;

    public int Capacity { get; set; }
}
