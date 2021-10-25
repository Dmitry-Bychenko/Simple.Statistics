using Simple.Statistics.Mathematics;
using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Poisson Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Poisson_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class PoissonDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="lambda">Rate (lambda, a)</param>
    public PoissonDistribution(double lambda) {
      if (lambda <= 0)
        throw new ArgumentOutOfRangeException(nameof(lambda), "lambda value must be positive");

      Lambda = lambda;
      m_Mean = lambda;
      m_Variance = lambda;
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
    public override string ToString() => $"Poisson Distribution with lambda = {Lambda}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public static double Cdf(double x, double lambda) {
      if (lambda <= 0)
        throw new ArgumentOutOfRangeException(nameof(lambda), "lambda value must be positive");

      if (x <= 0)
        return 0.0;
      if (double.IsPositiveInfinity(x))
        return 1.0;

      return GammaFunctions.GammaHigh(x + 1, lambda) / GammaFunctions.Factorial(x);
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public static double Pdf(double x, double lambda) {
      if (x < 0)
        throw new ArgumentOutOfRangeException(nameof(x), "value must be non negative");
      if (lambda <= 0)
        throw new ArgumentOutOfRangeException(nameof(lambda), "lambda value must be positive");

      if (x == 0)
        return 0.0;

      return Math.Pow(lambda, x) * Math.Exp(-lambda) / GammaFunctions.Factorial(x);
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public static double Qdf(double x, double lambda) {
      if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));
      if (lambda <= 0)
        throw new ArgumentOutOfRangeException(nameof(lambda), "lambda value must be positive");

      if (x == 0)
        return 0;
      if (x == 1)
        return double.PositiveInfinity;

      return Operators.Solve((v) => Cdf(v, lambda) - x, 0, double.PositiveInfinity);
    }

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) {
      if (x <= 0)
        return 0.0;
      if (double.IsPositiveInfinity(x))
        return 1.0;

      return GammaFunctions.GammaHigh(x + 1, Lambda) / GammaFunctions.Factorial(x);
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) {
      if (x < 0)
        throw new ArgumentOutOfRangeException(nameof(x), "value must be non negative");
      if (x == 0)
        return 0.0;

      return Math.Pow(Lambda, x) * Math.Exp(-Lambda) / GammaFunctions.Factorial(x);
    }

    #endregion IContinuousDistribution
  }

}
