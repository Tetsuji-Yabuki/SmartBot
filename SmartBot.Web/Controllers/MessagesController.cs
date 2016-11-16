using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using SmartBot.Domain.Model.Command;

namespace SmartBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var response = new HttpResponseMessage();
            try
            {
                if (ActivityTypes.Message.Equals(activity.Type))
                {
                    var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                    var command = CommandFactory.Create(activity.Text);

                    var result = command.Execute();

                    //var message = new StringBuilder();
                    //message.AppendLine($"Command[{command.CommandText}]");
                    //message.AppendLine($"Args[{string.Join(",", command.Arguments)}]");
                    //message.AppendLine($"Result[{nameof(result.EndCode)}]");
                    //message.AppendLine($"Message[{result.Message}]");

                    var reply = activity.CreateReply(result.Message);

                    await connector.Conversations.ReplyToActivityAsync(reply);
                }

                response = Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
    }
}