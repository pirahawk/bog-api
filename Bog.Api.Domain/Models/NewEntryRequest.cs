﻿using System;

namespace Bog.Api.Domain.Models
{
    public class NewEntryRequest
    {
        public Guid BlogId { get; set; }
        public string Author { get; set; }
    }
}