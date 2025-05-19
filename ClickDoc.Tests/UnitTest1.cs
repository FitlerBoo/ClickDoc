using Xunit;

namespace ClickDoc.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void DotsTest()
        {
            var dots = DateFormatter.DayInDotsMMMMYYYYFormat(new(2025, 05, 13));
            Assert.Equals("\"13\" мая 2025 года", dots);
        }
    }
}
