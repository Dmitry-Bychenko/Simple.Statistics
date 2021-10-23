using Simple.Statistics.Mathematics;
using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Binomial Probability Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Binomial_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class BinomialDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="n">Count (n)</param>
    /// <param name="p">Probability (p)</param>
    public BinomialDistribution(double n, double p) {
      if (p < 0 || p > 1 || !double.IsFinite(p))
        throw new ArgumentOutOfRangeException(nameof(p), "p value must be in [0..1] range");
      if (n < 0)
        throw new ArgumentOutOfRangeException(nameof(n), "n value must not be negative");
      if (!double.IsFinite(n))
        throw new ArgumentOutOfRangeException(nameof(n), "n value must be finite");

      P = p;
      N = n;

      m_Mean = p * n;
      m_Variance = n * p * (1 - p);
    }

    /// <summary>
    /// Standard constructor (p = 0.5)
    /// </summary>
    /// <param name="count">Count (n)</param>
    public BinomialDistribution(double count)
      : this(count, 0.5) { }

    #endregion Create

    #region Public

    /// <summary>
    /// Count (N)
    /// </summary>
    public double N { get; }

    /// <summary>
    /// Probability (P)
    /// </summary>
    public double P { get; }

    /// <summary>
    /// Probability (Q)
    /// </summary>
    public double Q => 1.0 - P;

    /// <summary>
    /// To String (debug only)
    /// </summary>
    public override string ToString() => $"Binomial Distribution with P = {P}; N = {N}";

    #endregion Public

    #region IContinuousProbabilityDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) {
      if (x <= 0)
        return 0.0;
      if (x >= N)
        return 1.0;

      return GammaFunctions.BetaIncomplete(Q, N - x, x + 1);
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    /// <seealso cref="https://proofwiki.org/wiki/Binomial_Coefficient_expressed_using_Beta_Function"/>
    public override double Pdf(double x) {
      if (x < 0.0 || x > N)
        return 0;

      double coef = 1.0 / (x + 1) / GammaFunctions.BetaFunc(x + 1, N - x + 1);

      return coef * Math.Pow(P, x) * Math.Pow(Q, N - x);
    }

    #endregion IContinuousProbabilityDistribution
  }

}
