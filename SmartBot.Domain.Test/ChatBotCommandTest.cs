
using SmartBot.Domain.Model.Command;
using Xunit;

namespace SmartBot.Domain.Test
{
    public class ChatBotCommandTest
    {
        [Fact]
        public void ExecuteTest()
        {
            var cmd = CommandFactory.Create("chat:test");

            var result = cmd.Execute();

            Assert.Equal(0, (int)result.EndCode);
        }
    }
}
