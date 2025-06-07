using API.Business;
using Xunit;

namespace API.Tests.Business
{
    public class SessionTests
    {
        [Fact]
        public void GenerateSessionID_ReturnsUnique30CharString()
        {
            var session = new Session();
            var first = session.GenerateSessionID();
            var second = session.GenerateSessionID();

            Assert.Equal(30, first.Length);
            Assert.Equal(30, second.Length);
            Assert.NotEqual(first, second);
        }
    }
}
