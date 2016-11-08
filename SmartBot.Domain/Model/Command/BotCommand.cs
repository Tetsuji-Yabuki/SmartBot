using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;

namespace SmartBot.Domain.Model.Command
{
    public abstract class BotCommand
    {
        public abstract string CommandText { get; }

        public string[] Arguments { get; set; }

        protected virtual void Initialize()
        {

        }

        protected virtual void OnBeforeExecute()
        {

        }

        protected virtual CommandActionResult ExecuteCore()
        {
            return new CommandActionResult
            {
                EndCode = ActionResultCode.正常終了,
                Message = "対応するコマンドが実装されていません"
            };
        }

        protected virtual void OnAfterExecute()
        {

        }

        public virtual CommandActionResult Execute()
        {
            Initialize();

            OnBeforeExecute();

            var result = ExecuteCore();

            OnAfterExecute();

            return result;
        }
    }

    public class NotExistsBotCommand : BotCommand
    {
        public override string CommandText => "notexists";
    }

    public class ChatBotCommand : BotCommand
    {
        public override string CommandText => "chat";

        protected override CommandActionResult ExecuteCore()
        {
            var text = Arguments.Any() ? Arguments[0] : "";

            var response = new CommandActionResult();

            if (text.Contains("hello"))
                response.Message = "こんにちは！こんにちは！";

            return response;
        }
    }

    public class DeployBotCommand : BotCommand
    {
        public override string CommandText => "deploy";
    }

    public class BuildBotCommand : BotCommand
    {
        public override string CommandText => "build";
    }

}
