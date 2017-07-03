﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NET_FlatFileToDB.Models
{
    public class FlatFileViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public int ContentLength { get; set; }
    }
}