using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Logistic Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Logistic_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class LogisticDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="mean">Mean (mu)</param>
    /// <param name="scale">Scale (s)</param>
    public LogisticDistribution(double mean, double scale) {
      if (!double.IsFinite(mean))
        throw new ArgumentOutOfRangeException(nameof(mean), "mean value must be finite");
      if (!double.IsFinite(scale))
        throw new ArgumentOutOfRangeException(nameof(scale), "scale value must be finite");

      if (scale <= 0)
        throw new ArgumentOutOfRangeException(nameof(scale), "mean value must be positive");

      m_Mean = mean;
      Scale = scale;
      m_Variance = Scale * Scale * Math.PI * Math.PI / 3.0;
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Scale
    /// </summary>
    public double Scale { get; }

    /// <summary>
    /// To String 
    /// </summary>
    public override string ToString() => $"Logistic Distribution with mean = {Mean}; scale = {Scale}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) {
      if (x < 0)
        throw new ArgumentOutOfRangeException(nameof(x), "value must be positive");
      if (double.IsNegativeInfinity(x))
        return 0.0;
      if (double.IsPositiveInfinity(x))
        return 1.0;

      return 1.0 / (1 + Math.Exp((Mean - x) / Scale));
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) {
      if (double.IsInfinity(x))
        return 0.0;

      double v = Math.Exp((Mean - x) / Scale);

      return v / Scale / (1 + v) / (1 + v);
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public override double Qdf(double x) {
      if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));
      if (x == 0)
        return double.NegativeInfinity;
      if (x == 1)
        return double.PositiveInfinity;

      return Mean + Scale * Math.Log(x / (1.0 - x));
    }

    #endregion IContinuousDistribution
  }

}
