using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlmCore.ThreadDowner.FileInfos
{
    public class FileHelper
    {
        public int Id { get; set; }
        public bool IsComplate { get; set; } = false;
        public byte[] Sbytes { get; set; }
        public string PartialName { get;  set; }
    }
}
