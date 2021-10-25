using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simple.Statistics.Distributions.Library;
using Simple.Statistics.Randoms;
using Simple.Statistics.Randoms.Library;

namespace Simple.Statistics.Tests {

  /// <summary>
  /// 
  /// </summary>
  [TestClass]
  public class ContinuousRandomsTest {

    [TestMethod]
    public void LessThenOneTesy() {
      using ContinuousRandom random = new();

      Assert.IsTrue(random.NextDouble() < 1.0);
    }

    [TestMethod]
    public void ConstantDtributionTest() {
      var distrib = new ConstantDistribution(3);

      using DistributedRandom random = new(distrib);

      var actual = random.NextDouble();

      Assert.IsTrue(actual == 3);
    }
  }

}
