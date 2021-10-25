using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Bernoulli Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Bernoulli_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class BernoulliDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="p">Probability (p)</param>
    public BernoulliDistribution(double p) {
      if (p < 0 || p > 1 || !double.IsFinite(p))
        throw new ArgumentOutOfRangeException(nameof(p), "p value must be in [0..1] range");

      P = p;

      m_Mean = P;
      m_Variance = P * Q;
    }

    /// <summary>
    /// Standard constructor (p = 0.5)
    /// </summary>
    public BernoulliDistribution() :
      this(0.5) { }

    #endregion Create

    #region Public

    /// <summary>
    /// Probability (p)
    /// </summary>
    public double P { get; }

    /// <summary>
    /// Probability (q)
    /// </summary>
    public double Q => 1.0 - P;

    /// <summary>
    /// To String (Debug)
    /// </summary>
    public override string ToString() => $"Bernoulli Distribution with p = {P}; q = {Q}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public static double Cdf(double x, double p) {
      if (p < 0 || p > 1)
        throw new ArgumentOutOfRangeException(nameof(p));

      if (x < 0)
        return 0.0;
      if (x > 1)
        return 1.0;

      return 1.0 - p + p * x;
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public static double Pdf(double x, double p) {
      if (p < 0 || p > 1)
        throw new ArgumentOutOfRangeException(nameof(p));

      if (x < 0.0 || x > 1.0)
        return 0;

      return x * p + (1.0 - x) * (1.0 - p);
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public static double Qdf(double x, double p) {
      if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));
      if (p < 0 || p > 1)
        throw new ArgumentOutOfRangeException(nameof(p));

      if (x <= 1 - p)
        return 0.0;
      if (x == 1)
        return 1.0;

      return x / p + (1 - 1 / p);
    }

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) {
      if (x < 0)
        return 0.0;
      if (x > 1)
        return 1.0;

      return Q + P * x;
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) {
      if (x < 0.0 || x > 1.0)
        return 0;

      return x * P + (1.0 - x) * Q;
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public override double Qdf(double x) {
      if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));
      if (x <= Q)
        return 0.0;
      if (x == 1)
        return 1.0;

      return x / P + (1 - 1 / P);
    }

    #endregion IContinuousDistribution
  }

}
