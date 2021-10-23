using Simple.Statistics.Mathematics;
using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Student Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Student%27s_t-distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class StudentDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="degreeOfFreedom">Degree Of Freedom (n - 1)</param>
    public StudentDistribution(double degreeOfFreedom) {
      if (degreeOfFreedom <= 0 || !double.IsFinite(degreeOfFreedom))
        throw new ArgumentOutOfRangeException(nameof(degreeOfFreedom), "value must be positive");

      DegreeOfFreedom = degreeOfFreedom;

      m_Mean = DegreeOfFreedom < 1 ? double.NaN : 0.0;
      m_Variance =
          DegreeOfFreedom > 2 ? DegreeOfFreedom / (DegreeOfFreedom - 2.0)
        : DegreeOfFreedom > 1 ? double.PositiveInfinity
        : double.NaN;
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Degree Of Freedom
    /// </summary>
    public double DegreeOfFreedom { get; }

    /// <summary>
    /// To String (debug only)
    /// </summary>
    public override string ToString() => $"Student Distribution with df = {DegreeOfFreedom}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) {
      if (x < 0)
        throw new ArgumentOutOfRangeException(nameof(x), "value must not be negative");

      return 1.0 - GammaFunctions.BetaIncompleteRegular(DegreeOfFreedom / (x * x + DegreeOfFreedom), DegreeOfFreedom / 2, 0.5) / 2;
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) {
      return GammaFunctions.Gamma((DegreeOfFreedom + 1) / 2) / (Math.Sqrt(DegreeOfFreedom * Math.PI) * GammaFunctions.Gamma(DegreeOfFreedom / 2)) *
              Math.Pow(1 + x * x / DegreeOfFreedom, -(DegreeOfFreedom + 1) / 2);
    }

    #endregion IContinuousDistribution
  }

}
