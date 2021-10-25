using Simple.Statistics.Mathematics;
using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Beta Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Beta_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class BetaDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="alpha">Alpha parameter</param>
    /// <param name="beta">Beta parameter</param>
    public BetaDistribution(double alpha, double beta) {
      if (!double.IsFinite(alpha))
        throw new ArgumentOutOfRangeException(nameof(alpha), "alpha value must be finite");
      if (!double.IsFinite(beta))
        throw new ArgumentOutOfRangeException(nameof(beta), "beta value must be finite");
      if (alpha <= 0)
        throw new ArgumentOutOfRangeException(nameof(alpha), "alpha value must be positive");
      if (beta <= 0)
        throw new ArgumentOutOfRangeException(nameof(beta), "beta value must be positive");

      Alpha = alpha;
      Beta = beta;

      m_Mean = Alpha / (Alpha + Beta);
      m_Variance = Alpha * Beta / (Alpha + Beta + 1.0) / (Alpha + Beta) / (Alpha + Beta);
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Alpha parameter
    /// </summary>
    public double Alpha { get; }

    /// <summary>
    /// Beta parameter
    /// </summary>
    public double Beta { get; }

    /// <summary>
    /// To String 
    /// </summary>
    public override string ToString() => $"Beta Distribution with aplha = {Alpha}; beta = {Beta}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    /// <seealso cref="https://en.wikipedia.org/wiki/Beta_function#Incomplete_beta_function"/>
    public static double Cdf(double x, double alpha, double beta) {
      if (!double.IsFinite(alpha))
        throw new ArgumentOutOfRangeException(nameof(alpha), "alpha value must be finite");
      if (!double.IsFinite(beta))
        throw new ArgumentOutOfRangeException(nameof(beta), "beta value must be finite");
      if (alpha <= 0)
        throw new ArgumentOutOfRangeException(nameof(alpha), "alpha value must be positive");
      if (beta <= 0)
        throw new ArgumentOutOfRangeException(nameof(beta), "beta value must be positive");

      if (x <= 0)
        return 0.0;
      if (x >= 1)
        return 1.0;

      return GammaFunctions.BetaIncompleteRegular(x, alpha, beta);
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public static double Pdf(double x, double alpha, double beta) {
      if (!double.IsFinite(alpha))
        throw new ArgumentOutOfRangeException(nameof(alpha), "alpha value must be finite");
      if (!double.IsFinite(beta))
        throw new ArgumentOutOfRangeException(nameof(beta), "beta value must be finite");
      if (alpha <= 0)
        throw new ArgumentOutOfRangeException(nameof(alpha), "alpha value must be positive");
      if (beta <= 0)
        throw new ArgumentOutOfRangeException(nameof(beta), "beta value must be positive");

      if (x < 0 || x > 1.0)
        throw new ArgumentOutOfRangeException(nameof(x), "value must be in [0..1] range");

      if (alpha == beta && alpha == 1)
        return 1.0;

      if (x == 0)
        return alpha <= 1 ? double.PositiveInfinity : 0.0;
      if (x == 1)
        return beta <= 1 ? double.PositiveInfinity : 0.0;

      return Math.Pow(x, alpha - 1.0) * Math.Pow(1.0 - x, beta - 1.0) / GammaFunctions.BetaFunc(alpha, beta);
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public static double Qdf(double x, double alpha, double beta) {
      if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));

      if (!double.IsFinite(alpha))
        throw new ArgumentOutOfRangeException(nameof(alpha), "alpha value must be finite");
      if (!double.IsFinite(beta))
        throw new ArgumentOutOfRangeException(nameof(beta), "beta value must be finite");
      if (alpha <= 0)
        throw new ArgumentOutOfRangeException(nameof(alpha), "alpha value must be positive");
      if (beta <= 0)
        throw new ArgumentOutOfRangeException(nameof(beta), "beta value must be positive");

      if (x == 0)
        return 0;
      if (x == 1)
        return double.PositiveInfinity;

      return Operators.Solve((v) => Cdf(v, alpha, beta) - x, 0, 1);
    }

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    /// <seealso cref="https://en.wikipedia.org/wiki/Beta_function#Incomplete_beta_function"/>
    public override double Cdf(double x) {
      if (x <= 0)
        return 0.0;
      if (x >= 1)
        return 1.0;

      if (!double.IsFinite(x))
        throw new ArgumentOutOfRangeException(nameof(x), "value must be in [0..1] range");

      return GammaFunctions.BetaIncompleteRegular(x, Alpha, Beta);
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) {
      if (x < 0 || x > 1.0)
        throw new ArgumentOutOfRangeException(nameof(x), "value must be in [0..1] range");

      if (Alpha == Beta && Alpha == 1)
        return 1.0;

      if (x == 0)
        return Alpha <= 1 ? double.PositiveInfinity : 0.0;
      if (x == 1)
        return Beta <= 1 ? double.PositiveInfinity : 0.0;

      return Math.Pow(x, Alpha - 1.0) * Math.Pow(1.0 - x, Beta - 1.0) / GammaFunctions.BetaFunc(Alpha, Beta);
    }

    #endregion IContinuousDistribution
  }

}
