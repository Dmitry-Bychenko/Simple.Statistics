using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Poisson Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Rayleigh_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class RayleighDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="sigma">Rate</param>
    public RayleighDistribution(double sigma) {
      if (sigma <= 0 || !double.IsFinite(sigma))
        throw new ArgumentOutOfRangeException(nameof(sigma), "sigma value must be positive");

      Sigma = sigma;
      m_Mean = sigma * Math.Sqrt(Math.PI / 2.0);
      m_Variance = (4 - Math.PI) / 2 * sigma * sigma;
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Lambda parameter 
    /// </summary>
    public double Sigma { get; }

    /// <summary>
    /// To String 
    /// </summary>
    public override string ToString() => $"Rayleigh distribution with sigma = {Sigma}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) =>
      1 - Math.Exp(-x * x / 2 / Sigma / Sigma);

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) =>
      x / Sigma / Sigma * Math.Exp(-x * x / 2 / Sigma / Sigma);

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public override double Qdf(double x) {
      if (x == 0)
        return 0.0;
      if (x == 1)
        return double.PositiveInfinity;
      if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));

      return Sigma * Math.Sqrt(-2 * Math.Log(1 - x));
    }

    #endregion IContinuousDistribution
  }

}
