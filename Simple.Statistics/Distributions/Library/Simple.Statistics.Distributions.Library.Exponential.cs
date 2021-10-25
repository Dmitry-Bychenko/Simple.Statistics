using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Exponential Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Exponential_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class ExponentialDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="lambda">Rate</param>
    public ExponentialDistribution(double lambda) {
      if (lambda <= 0 || !double.IsFinite(lambda))
        throw new ArgumentOutOfRangeException(nameof(lambda), "lambda value must be positive");

      Lambda = lambda;
      m_Mean = 1 / lambda;
      m_Variance = 1 / lambda / lambda;
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Lambda parameter 
    /// </summary>
    public double Lambda { get; }

    /// <summary>
    /// To String 
    /// </summary>
    public override string ToString() => $"Exponential distribution with lambda = {Lambda}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public static double Cdf(double x, double lambda) {
      if (lambda <= 0 || !double.IsFinite(lambda))
        throw new ArgumentOutOfRangeException(nameof(lambda), "lambda value must be positive");

      return 1 - Math.Exp(-lambda * x);
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public static double Pdf(double x, double lambda) {
      if (lambda <= 0 || !double.IsFinite(lambda))
        throw new ArgumentOutOfRangeException(nameof(lambda), "lambda value must be positive");

      return lambda * Math.Exp(-lambda * x);
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public static double Qdf(double x, double lambda) {
      if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));

      if (lambda <= 0 || !double.IsFinite(lambda))
        throw new ArgumentOutOfRangeException(nameof(lambda), "lambda value must be positive");

      if (x == 0)
        return 0.0;
      if (x == 1)
        return double.PositiveInfinity;

      return -Math.Log(1 - x) / lambda;
    }

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) => 1 - Math.Exp(-Lambda * x);

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) => Lambda * Math.Exp(-Lambda * x);

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

      return -Math.Log(1 - x) / Lambda;
    }

    #endregion IContinuousDistribution
  }

}
