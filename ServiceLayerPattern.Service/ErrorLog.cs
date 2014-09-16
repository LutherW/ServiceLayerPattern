using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayerPattern.Service
{
    public class ErrorLog
    {
        public static string GenerateErrorRefMessageAndLog(Exception ex) 
        {
            // 在这里记录错误以及唯一的引用ID
            return string.Format("错误的引用标识:{0}", Guid.NewGuid().ToString());
        }
    }
}
