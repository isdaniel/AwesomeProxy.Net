using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeProxySample_net60
{
    /// <summary>
    /// 使用AND運算,來計算權限
    /// </summary>
    public enum AuthType : int
    {
        PM = 1,
        RD = 2,
        SD = 4,
        SA = 8
    }
}