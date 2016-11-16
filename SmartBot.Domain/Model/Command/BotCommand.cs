using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Text;
using System.Web;
using Newtonsoft.Json;

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
            var text   = Arguments.Any() ? Arguments[0] : "";
            var result = new CommandActionResult();

            if (text.Contains("hello"))
                result.Message = "こんにちは！こんにちは！";

            return result;
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

    public class WikipediaBotCommand : BotCommand
    {
        public override string CommandText => "wkp";

        protected override CommandActionResult ExecuteCore()
        {
            var text         = Arguments.Any() ? Arguments[0] : "";
            var httpResponse = GetResponse(text);
            var result       = GetResult(httpResponse.Result);

            return result;
        }

        private async Task<string> GetResponse(string searchKey)
        {
            var encodeKey  = HttpUtility.UrlEncode(searchKey);
            var formatType = "json";
            var url        = $"http://ja.wikipedia.org/w/api.php?action=query&format={formatType}&prop=extracts&redirects=0&exchars=300&explaintext=1&titles={encodeKey}";

            using (var cl = new HttpClient())
                return await cl.GetStringAsync(url).ConfigureAwait(false);
        }

        private CommandActionResult GetResult(string response)
        {
            var result       = new CommandActionResult();
            var decodeResult = HttpUtility.UrlDecode(response);
            var json         = JsonConvert.DeserializeObject<WikipediaApiResult>(response);

            if (json.query.pages.Any())
                result.Message = json.query.pages.First().Value.extract;
            else
                result.Message = "そんなものねーよ！！";

            return result;
        }

        //Wikipedia apiの結果(json)

        public class WikipediaApiResult
        {
            public string batchcomplete { get; set; }
            public Query query { get; set; }
            public Limits limits { get; set; }
        }

        public class Query
        {
            public Dictionary<string, pageval> pages { get; set; }
        }

        public class Limits
        {
            public int extracts { get; set; }
        }

        public class pageval
        {
            public int pageid { get; set; }
            public int ns { get; set; }
            public string title { get; set; }
            public string extract { get; set; }
        }

    }

}
