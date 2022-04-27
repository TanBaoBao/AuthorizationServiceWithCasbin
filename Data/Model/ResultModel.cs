using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class ResultModel
    {
        public bool IsSuccess { get; set; }
        public object ResponseSuccess { get; set; }
        public object ResponseFailed { get; set; }
        public int Code { get; set; }
    }
}
