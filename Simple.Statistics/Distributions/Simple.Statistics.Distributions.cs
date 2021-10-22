using System;

using Simple.Statistics.Randoms;

namespace Simple.Statistics.Distributions {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Continuous Probability Distribution (like Normal, LogNormal, Triangle etc.)
  /// Thread Safe
  /// </summary>
  /// <see cref="https://en.wikipedia.org/wiki/Probability_distribution#Continuous_probability_distribution"/>
  /// <threadsafety static="true" instance="true"/>
  // 
  //-------------------------------------------------------------------------------------------------------------------

  public interface IContinuousDistribution {
    /// <summary>
    /// Probability Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    double Pdf(double x);

    /// <summary>
    /// Cumulative Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    double Cdf(double x);

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    double Qdf(double x);
  }

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
