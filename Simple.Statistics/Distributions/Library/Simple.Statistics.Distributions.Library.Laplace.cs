using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Laplace Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Laplace_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class LaplaceDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="b">Rate</param>
    public LaplaceDistribution(double mu, double b) {
      if (!double.IsFinite(mu))
        throw new ArgumentOutOfRangeException(nameof(mu), "mu must be finite");

      if (b <= 0 || !double.IsFinite(b))
        throw new ArgumentOutOfRangeException(nameof(b), "b value must be positive");

      Mu = mu;
      B = b;
      m_Mean = mu;
      m_Variance = 2 * b * b;
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Mu parameter 
    /// </summary>
    public double Mu { get; }

    /// <summary>
    /// B parameter 
    /// </summary>
    public double B { get; }

    /// <summary>
    /// To String 
    /// </summary>
    public override string ToString() => $"Laplace distribution with mu = {Mu}, b = {B}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public static double Cdf(double x, double mu, double b) {
      if (!double.IsFinite(mu))
        throw new ArgumentOutOfRangeException(nameof(mu), "mu must be finite");

      if (b <= 0 || !double.IsFinite(b))
        throw new ArgumentOutOfRangeException(nameof(b), "b value must be positive");

      return x <= mu
        ? Math.Exp((x - mu) / b) / 2
        : 1 - Math.Exp((mu - x) / b) / 2;
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public static double Pdf(double x, double mu, double b) {
      if (!double.IsFinite(mu))
        throw new ArgumentOutOfRangeException(nameof(mu), "mu must be finite");

      if (b <= 0 || !double.IsFinite(b))
        throw new ArgumentOutOfRangeException(nameof(b), "b value must be positive");

      return Math.Exp(-Math.Abs(x - mu) / b) / 2 / b;
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public static double Qdf(double x, double mu, double b) {
      if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));

      if (!double.IsFinite(mu))
        throw new ArgumentOutOfRangeException(nameof(mu), "mu must be finite");

      if (b <= 0 || !double.IsFinite(b))
        throw new ArgumentOutOfRangeException(nameof(b), "b value must be positive");

      if (x == 0)
        return double.NegativeInfinity;
      if (x == 1)
        return double.PositiveInfinity;

      return x <= 0.5
        ? mu + b * Math.Log(2 * x)
        : mu - b * Math.Log(2 - 2 * x);
    }

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) => x <= Mu
      ? Math.Exp((x - Mu) / B) / 2
      : 1 - Math.Exp((Mu - x) / B) / 2;

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) => Math.Exp(-Math.Abs(x - Mu) / B) / 2 / B;

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public override double Qdf(double x) {
      if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));
      if (x == 0)
        return double.NegativeInfinity;
      if (x == 1)
        return double.PositiveInfinity;

      return x <= 0.5
        ? Mu + B * Math.Log(2 * x)
        : Mu - B * Math.Log(2 - 2 * x);
    }

    #endregion IContinuousDistribution
  }

}
