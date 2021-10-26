using Simple.Statistics.Mathematics;
using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Maxwell Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Maxwell%E2%80%93Boltzmann_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class MaxwellDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="a">a parameter</param>
    public MaxwellDistribution(double a) {
      if (!double.IsFinite(a))
        throw new ArgumentOutOfRangeException(nameof(a), "a value must be finite");
      if (a <= 0)
        throw new ArgumentOutOfRangeException(nameof(a), "a value must be positive");

      A = a;

      m_Mean = 2 * a * Math.Sqrt(2 / Math.PI);
      m_Variance = a * a * (3 - 8 / Math.PI);
    }

    #endregion Create

    #region Public

    /// <summary>
    /// A parameter
    /// </summary>
    public double A { get; }

    /// <summary>
    /// To String (debug only)
    /// </summary>
    public override string ToString() => $"Maxwell Distribution with A = {A}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public static double Cdf(double x, double a) {
      if (!double.IsFinite(a))
        throw new ArgumentOutOfRangeException(nameof(a), "mean value must be finite");
      if (a <= 0)
        throw new ArgumentOutOfRangeException(nameof(a), "a value must be positive");

      if (x <= 0)
        return 0.0;

      return ProbabilityIntegral.Erf(x / Math.Sqrt(2) / a) -
             Math.Sqrt(2 / Math.PI) * x / a * Math.Exp(-x * x / 2 / a / a);
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public static double Pdf(double x, double a) {
      if (!double.IsFinite(a))
        throw new ArgumentOutOfRangeException(nameof(a), "mean value must be finite");
      if (a <= 0)
        throw new ArgumentOutOfRangeException(nameof(a), "a value must be positive");

      if (x <= 0)
        return 0.0;

      return Math.Sqrt(2 / Math.PI) * x * x / a / a / a * Math.Exp(-x * x / 2 / a / a);
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public static double Qdf(double x, double a) {
      if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));

      if (!double.IsFinite(a))
        throw new ArgumentOutOfRangeException(nameof(a), "mean value must be finite");
      if (a <= 0)
        throw new ArgumentOutOfRangeException(nameof(a), "a value must be positive");

      if (x == 0)
        return 0;
      if (x == 1)
        return double.PositiveInfinity;

      return Operators.Solve((v) => Cdf(v, a) - x, 0, double.PositiveInfinity);
    }

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) =>
      ProbabilityIntegral.Erf(x / Math.Sqrt(2) / A) - Math.Sqrt(2 / Math.PI) * x / A * Math.Exp(-x * x / 2 / A / A);

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) {
      if (x <= 0)
        return 0.0;

      return Math.Sqrt(2 / Math.PI) * x * x / A / A / A * Math.Exp(-x * x / 2 / A / A);
    }

    #endregion IContinuousDistribution
  }

}
