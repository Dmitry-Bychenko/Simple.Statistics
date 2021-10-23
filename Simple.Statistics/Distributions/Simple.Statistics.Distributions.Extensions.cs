using Simple.Statistics.Randoms;
using System;

namespace Simple.Statistics.Distributions {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Continuous Distribution Extensions
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class ContinuousDistributionExtensions {
    #region Public

    /// <summary>
    /// Rdf - Random Distributed Function
    /// </summary>
    public static double Rdf(this IContinuousDistribution distribution) {
      if (distribution is null)
        throw new ArgumentNullException(nameof(distribution));

      return distribution.Qdf(IContinuousRandom.Default.NextDouble());
    }

    #endregion Public
  }

}
