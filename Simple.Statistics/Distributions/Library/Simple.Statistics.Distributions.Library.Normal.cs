using Simple.Statistics.Mathematics;
using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Normal Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Normal_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class NormalDistribution : ContinuousDistribution {
    #region Private Data

    private static readonly double factor = Math.Sqrt(Math.PI * 2);

    #endregion Private Data

    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="mean">Mean (mu)</param>
    /// <param name="sigma">Standard Error (sigma)</param>
    public NormalDistribution(double mean, double sigma) {
      if (!double.IsFinite(mean))
        throw new ArgumentOutOfRangeException(nameof(mean), "mean value must be finite");
      if (double.IsInfinity(sigma))
        throw new ArgumentOutOfRangeException(nameof(sigma), "sigma value must be finite");
      if (sigma <= 0)
        throw new ArgumentOutOfRangeException(nameof(sigma), "sigma value must be positive");

      m_Mean = mean;
      m_Variance = sigma * sigma;
    }

    /// <summary>
    /// Standard constructor for Mean (mu) = 0.0, Standard Error (sigma) = 1.0
    /// </summary>
    public NormalDistribution()
      : this(0.0, 1.0) { }

    #endregion Create

    #region Public

    /// <summary>
    /// To String (debug only)
    /// </summary>
    public override string ToString() => $"Normal Probability Distribution with mean = {Mean}; sd = {StandardError}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) =>
      0.5 + ProbabilityIntegral.Erf((x - Mean) / (Math.Sqrt(2) * StandardError)) / 2.0;

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) {
      double v = (x - Mean) / StandardError;

      return Math.Exp(-v * v / 2.0) / (StandardError * factor);
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public override double Qdf(double x) {
      if (x == 0)
        return double.NegativeInfinity;
      if (x == 1)
        return double.PositiveInfinity;
      if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));

      Func<double, double> erf = ProbabilityIntegral.Erf;

      return Mean + StandardError * Math.Sqrt(2) * erf.InverseAt(2.0 * x - 1, Mean - 10.0 * StandardError, Mean + 10.0 * StandardError);
    }

    #endregion IContinuousDistribution
  }

}
