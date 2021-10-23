using Simple.Statistics.Mathematics;
using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Fisher Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/F-distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class FisherDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="d1">Degree Of Freedom 1</param>
    /// <param name="d2">Degree Of Freedom 2</param>
    public FisherDistribution(double d1, double d2) {
      if (d1 <= 0 || !double.IsFinite(d1))
        throw new ArgumentOutOfRangeException(nameof(d1), "d1 value must be positive");
      if (d2 <= 0 || !double.IsFinite(d2))
        throw new ArgumentOutOfRangeException(nameof(d2), "d2 value must be positive");

      D1 = d1;
      D2 = d2;

      m_Mean = D2 > 2.0
        ? D2 / (D2 - 2.0)
        : double.PositiveInfinity;

      m_Variance = D2 > 4.0
        ? 2 * D2 * D2 * (D1 + D2 - 2) / D1 / (D2 - 2) / (D2 - 2) / (D2 - 4)
        : double.PositiveInfinity;
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Degree Of Freedom 1
    /// </summary>
    public double D1 { get; }

    /// <summary>
    /// Degree Of Freedom 2
    /// </summary>
    public double D2 { get; }

    /// <summary>
    /// To String (debug only)
    /// </summary>
    public override string ToString() => $"Fisher Distribution with df1 = {D1}; df2 = {D2}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) {
      if (x <= 0)
        throw new ArgumentOutOfRangeException(nameof(x), "value must be positive");

      return GammaFunctions.BetaIncompleteRegular(D1 * x / (D1 * x + D2),
                                                  D1 / 2,
                                                  D2 / 2);
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) {
      if (x <= 0)
        throw new ArgumentOutOfRangeException(nameof(x), "value must be positive");

      return Math.Sqrt(Math.Pow(D1 * x, D1) *
                       Math.Pow(D2, D2) /
                       Math.Pow(D1 * x + D2, D1 + D2)) /
             (x * GammaFunctions.BetaFunc(D1 / 2, D2 / 2));
    }

    #endregion IContinuousDistribution
  }

}
