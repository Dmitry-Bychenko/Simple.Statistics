using Simple.Statistics.Mathematics;
using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Pert Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/PERT_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class PertDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="count">Count (n)</param>
    /// <param name="p">Probability (p)</param>
    public PertDistribution(double a, double b, double c) {
      if (!double.IsFinite(a))
        throw new ArgumentOutOfRangeException(nameof(a), "a value must be finite");
      if (!double.IsFinite(b))
        throw new ArgumentOutOfRangeException(nameof(b), "b value must be finite");
      if (!double.IsFinite(c))
        throw new ArgumentOutOfRangeException(nameof(c), "c value must be finite");

      if (b <= a)
        throw new ArgumentOutOfRangeException(nameof(b), "b must be bigger than a");
      if (c <= b)
        throw new ArgumentOutOfRangeException(nameof(c), "c must be bigger than b");

      A = a;
      B = b;
      C = c;

      m_Mean = (A + 4.0 * B + C) / 6.0;
      m_Variance = (Mean - A) * (C - Mean) / 7.0;
    }

    #endregion Create

    #region Public

    /// <summary>
    /// A
    /// </summary>
    public double A { get; }

    /// <summary>
    /// B
    /// </summary>
    public double B { get; }

    /// <summary>
    /// C
    /// </summary>
    public double C { get; }

    /// <summary>
    /// Alpha
    /// </summary>
    public double Alpha => 1.0 + 4.0 * (B - A) / (C - A);

    /// <summary>
    /// Beta
    /// </summary>
    public double Beta => 1.0 + 4.0 * (C - B) / (C - A);

    /// <summary>
    /// To String (debug only)
    /// </summary>
    public override string ToString() => $"Pert Distribution with A = {A}; B = {B}; C = {C}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) {
      if (x <= A)
        return 0.0;
      if (x >= C)
        return 1.0;

      return GammaFunctions.BetaIncomplete((x - A) / (C - A), Alpha, Beta);
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    /// <seealso cref="https://proofwiki.org/wiki/Binomial_Coefficient_expressed_using_Beta_Function"/>
    public override double Pdf(double x) {
      if (x <= A)
        return 0.0;
      if (x >= C)
        return 0.0;

      return Math.Pow(x - A, Alpha - 1) *
             Math.Pow(C - x, Beta - 1) /
             GammaFunctions.BetaFunc(Alpha, Beta) /
             Math.Pow(C - A, Alpha + Beta - 1);
    }

    #endregion IContinuousDistribution
  }

}
