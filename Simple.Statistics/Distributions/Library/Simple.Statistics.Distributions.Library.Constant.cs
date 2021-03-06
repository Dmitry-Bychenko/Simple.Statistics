using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Constant Distribution. 
  /// Random value can have a single constant value only 
  /// PDF is Dirac Delta Function
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Dirac_delta_function"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class ConstantDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="mean">Mean</param>
    public ConstantDistribution(double mean) {
      if (!double.IsFinite(mean))
        throw new ArgumentOutOfRangeException(nameof(mean), "Mean value must be finite");

      m_Mean = mean;
      m_Variance = 0;
    }

    /// <summary>
    /// Standard constructor (Mean = 0.0)
    /// </summary>
    public ConstantDistribution()
      : this(0) { }

    #endregion Create

    #region Public

    /// <summary>
    /// To String (debug only)
    /// </summary>
    public override string ToString() => $"Constant Distribution (mean = {Mean})";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public static double Cdf(double x, double mean) =>
        x < mean ? 0.0
      : x == mean ? 0.5
      : 1.0;

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public static double Pdf(double x, double mean) => x == mean ? double.PositiveInfinity : 0.0;

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public static double Qdf(double x, double mean) => x >= 0 && x <= 1.0
      ? mean
      : throw new ArgumentOutOfRangeException(nameof(x));

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) =>
        x < Mean ? 0.0
      : x == Mean ? 0.5
      : 1.0;

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) => x == Mean ? double.PositiveInfinity : 0.0;

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public override double Qdf(double x) => x >= 0 && x <= 1.0
      ? Mean
      : throw new ArgumentOutOfRangeException(nameof(x));

    #endregion IContinuousDistribution
  }

}
