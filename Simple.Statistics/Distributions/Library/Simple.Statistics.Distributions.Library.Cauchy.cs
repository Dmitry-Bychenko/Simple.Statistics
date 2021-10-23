using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Normal Probability Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Cauchy_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------
  public sealed class CauchyDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="offset">Offset</param>
    /// <param name="Gamma">Gamma parameter</param>
    public CauchyDistribution(double offset, double gamma) {
      if (!double.IsFinite(offset))
        throw new ArgumentOutOfRangeException(nameof(offset), "offset parameter must be positive");
      if (gamma <= 0 || !double.IsFinite(gamma))
        throw new ArgumentOutOfRangeException(nameof(gamma), "gamma parameter must be positive");

      Offset = offset;
      Gamma = gamma;

      m_Mean = double.PositiveInfinity;
      m_Variance = double.PositiveInfinity;
    }

    /// <summary>
    /// Standard constructor for Offset = 0.0, Gamma = 1.0
    /// </summary>
    public CauchyDistribution()
      : this(0.0, 1.0) { }

    #endregion Create

    #region Public

    /// <summary>
    /// Offset
    /// </summary>
    public double Offset { get; }

    /// <summary>
    /// Gamma
    /// </summary>
    public double Gamma { get; }

    /// <summary>
    /// To String
    /// </summary>
    public override string ToString() =>
      $"Cauchy Distribution with offset = {Offset}; gamma = {Gamma}";

    #endregion Public

    #region IContinuousProbabilityDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) => 0.5 + Math.Atan((x - Offset) / Gamma) / Math.PI;

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) => 1.0 / (Math.PI * Gamma * (1 + (x - Offset) * (x - Offset) / Gamma / Gamma));

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

      return Offset + Gamma * Math.Tan(Math.PI * (x - 0.5));
    }

    #endregion IContinuousProbabilityDistribution
  }

}
