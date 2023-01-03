using IslamiTexts.DocumentModel;

namespace Tests.DocumentModel
{
    [TestClass]
    public class BuckwalterTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            string testString = @"بِسْمِ اللَّهِ الرَّحْمَٰنِ الرَّحِيمِ";
            string convertedString = new IslamiTexts.DocumentModel.Buckwalter().ConvertToBuckwalter(testString);
            Assert.AreEqual("abc", convertedString, "Strings don't match");
        }
    }
}