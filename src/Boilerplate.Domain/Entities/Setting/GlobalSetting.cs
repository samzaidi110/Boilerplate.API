﻿using Boilerplate.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Entities.Configuration;

public class GlobalSetting : Entity
{
    public required string Key { get; set; }
    public required string Value { get; set; }
    public string? Description { get; set; }
}