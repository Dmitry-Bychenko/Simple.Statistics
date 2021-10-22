using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simple.Statistics;
using Simple.Statistics.Randoms;

namespace Simple.Statistics.Tests {

  /// <summary>
  /// 
  /// </summary>
  [TestClass]
  public class ContinuousRandomsTest {

    [TestMethod]
    public void LessThenOne() {
      using ContinuousRandom random = new ();

      Assert.IsTrue(random.NextDouble() < 1.0);
    }
  }
}
