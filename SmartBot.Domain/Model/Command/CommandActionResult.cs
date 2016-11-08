using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBot.Domain.Model.Command
{
    public enum ActionResultCode
    {
        正常終了,
        異常終了,
    }

    public class CommandActionResult
    {
        public ActionResultCode EndCode { get; set; }

        public string Message { get; set; }
    }
}
