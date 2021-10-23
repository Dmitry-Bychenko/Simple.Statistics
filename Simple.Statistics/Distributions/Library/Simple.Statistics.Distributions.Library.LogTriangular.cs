using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Log Triangular Distribution
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class LogTriangularDistribution : ContinuousDistribution {
    #region Public

    /// <summary>
    /// Leftmost point
    /// </summary>
    public double From { get; }

    /// <summary>
    /// Rightmost point
    /// </summary>
    public double To { get; }

    /// <summary>
    /// Mode
    /// </summary>
    public double Mode { get; }

    /// <summary>
    /// To String 
    /// </summary>
    public override string ToString() => $"Log Triangular Distribution with left = {From}, right = {To}, mode {Mode}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) {
      if (x <= Math.Exp(From))
        return 0.0;
      if (x <= Math.Exp(Mode))
        return (Math.Log(x) - From) * (Math.Log(x) - From) / (To - From) / (Mode - From);
      if (x < Math.Exp(To))
        return (Math.Log(x) - To) * (Math.Log(x) - To) / (To - From) / (To - Mode);

      return 1.0;
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) {
      if (x < Math.Exp(From))
        return 0.0;
      if (x < Math.Exp(Mode))
        return 2 * (Math.Log(x) - From) / x / (To - From) / (Mode - From);
      if (x == Math.Exp(Mode))
        return 2 / x / (To - From);
      if (x <= Math.Exp(To))
        return 2 * (To - Math.Log(x)) / x / (To - From) / (To - Mode);

      return 0.0;
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    /// <seealso cref="https://www.boost.org/doc/libs/1_68_0/libs/math/doc/html/math_toolkit/dist_ref/dists/triangular_dist.html"/>
    public override double Qdf(double x) {
      if (x < 0 || x > 1 || !double.IsFinite(x))
        throw new ArgumentOutOfRangeException(nameof(x), "x must be in [0..1] range");

      if (x == 0.0)
        return Math.Exp(From);
      if (x <= (Mode - From) / (To - From))
        return Math.Exp(From + Math.Sqrt(x * (To - From) * (Mode - From)));
      if (x < 1)
        return Math.Exp(To - Math.Sqrt((1 - x) * (To - From) * (To - Mode)));

      return Math.Exp(To);
    }

    #endregion IContinuousDistribution
  }

}
