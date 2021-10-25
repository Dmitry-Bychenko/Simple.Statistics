using Simple.Statistics.Mathematics;
using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Chi-Squared Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Chi-squared_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class ChiSquaredDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="degreeOfFreedom">Degree Of Freedom (n - 1)</param>
    public ChiSquaredDistribution(double degreeOfFreedom) {
      if (degreeOfFreedom <= 0 || !double.IsFinite(degreeOfFreedom))
        throw new ArgumentOutOfRangeException(nameof(degreeOfFreedom), "degreeOfFreedom value must be positive");

      DegreeOfFreedom = degreeOfFreedom;

      m_Mean = DegreeOfFreedom;
      m_Variance = 2 * DegreeOfFreedom;
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Degree Of Freedom
    /// </summary>
    public double DegreeOfFreedom { get; }

    /// <summary>
    /// To String
    /// </summary>
    public override string ToString() => $"Chi-Squared Distribution with df = {DegreeOfFreedom}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public static double Cdf(double x, double degreeOfFreedom) {
      if (degreeOfFreedom <= 0 || !double.IsFinite(degreeOfFreedom))
        throw new ArgumentOutOfRangeException(nameof(degreeOfFreedom), "degreeOfFreedom value must be positive");

      if (x <= 0)
        return 0.0;

      return GammaFunctions.GammaLowRegular(degreeOfFreedom / 2, x / 2);
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public static double Pdf(double x, double degreeOfFreedom) {
      if (x <= 0)
        throw new ArgumentOutOfRangeException(nameof(x), "value must be positive");
      if (degreeOfFreedom <= 0 || !double.IsFinite(degreeOfFreedom))
        throw new ArgumentOutOfRangeException(nameof(degreeOfFreedom), "degreeOfFreedom value must be positive");

      return Math.Pow(x, degreeOfFreedom / 2 - 1) * Math.Exp(-x / 2) / (Math.Pow(2, degreeOfFreedom / 2) * GammaFunctions.Gamma(degreeOfFreedom / 2));
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public static double Qdf(double x, double degreeOfFreedom) {
      if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));
      if (degreeOfFreedom <= 0)
        throw new ArgumentOutOfRangeException(nameof(degreeOfFreedom), "value must be positive");

      if (x == 0)
        return 0;
      if (x == 1)
        return double.PositiveInfinity;

      return Operators.Solve((v) => Cdf(v, degreeOfFreedom) - x, 0, double.PositiveInfinity);
    }

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) {
      if (x <= 0)
        throw new ArgumentOutOfRangeException(nameof(x), "value must be positive");

      return GammaFunctions.GammaLowRegular(DegreeOfFreedom / 2, x / 2);
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) {
      if (x <= 0)
        throw new ArgumentOutOfRangeException(nameof(x), "value must be positive");

      return Math.Pow(x, DegreeOfFreedom / 2 - 1) * Math.Exp(-x / 2) / (Math.Pow(2, DegreeOfFreedom / 2) * GammaFunctions.Gamma(DegreeOfFreedom / 2));
    }

    #endregion IContinuousDistribution
  }

}
