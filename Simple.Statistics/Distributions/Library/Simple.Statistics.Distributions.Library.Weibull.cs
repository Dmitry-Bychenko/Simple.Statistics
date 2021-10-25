using Simple.Statistics.Mathematics;
using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Weibull Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Weibull_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------
  public sealed class WeibullDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    public WeibullDistribution(double scale, double shape) {
      Scale = scale > 0 ? scale : throw new ArgumentOutOfRangeException(nameof(scale));
      Shape = shape > 0 ? shape : throw new ArgumentOutOfRangeException(nameof(shape));

      m_Mean = Scale * GammaFunctions.Gamma(1 + 1 / Shape);
      m_Variance = Scale * Scale * (GammaFunctions.Gamma(1 + 2 / Shape) - Math.Pow(GammaFunctions.Gamma(1 + 1 / Shape), 2));
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Scale
    /// </summary>
    public double Scale { get; }

    /// <summary>
    /// Scale
    /// </summary>
    public double Shape { get; }

    /// <summary>
    /// To String (debug only)
    /// </summary>
    public override string ToString() => $"Weibull Distribution with shape = {Shape}; scale = {Scale}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public static double Cdf(double x, double scale, double shape) {
      scale = scale > 0 ? scale : throw new ArgumentOutOfRangeException(nameof(scale));
      shape = shape > 0 ? shape : throw new ArgumentOutOfRangeException(nameof(shape));

      return x < 0 ? 0.0 : 1 - Math.Exp(-Math.Pow(x / scale, shape));
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public static double Pdf(double x, double scale, double shape) {
      scale = scale > 0 ? scale : throw new ArgumentOutOfRangeException(nameof(scale));
      shape = shape > 0 ? shape : throw new ArgumentOutOfRangeException(nameof(shape));

      return x < 0
        ? 0.0
        : shape / scale * Math.Pow(x / scale, shape - 1) * Math.Exp(-Math.Pow(-x / scale, shape));
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public static double Qdf(double x, double scale, double shape) {
      scale = scale > 0 ? scale : throw new ArgumentOutOfRangeException(nameof(scale));
      shape = shape > 0 ? shape : throw new ArgumentOutOfRangeException(nameof(shape));

      if (x == 0)
        return 0;
      else if (x == 1)
        return double.PositiveInfinity;
      else if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));

      return scale * Math.Pow(Math.Abs(1 - x), 1 / shape);
    }

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) => x < 0
        ? 0.0
        : 1 - Math.Exp(-Math.Pow(x / Scale, Shape));

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) => x < 0
        ? 0.0
        : Shape / Scale * Math.Pow(x / Scale, Shape - 1) * Math.Exp(-Math.Pow(-x / Scale, Shape));

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public override double Qdf(double x) {
      if (x == 0)
        return 0;
      else if (x == 1)
        return double.PositiveInfinity;
      else if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));

      return Scale * Math.Pow(Math.Abs(1 - x), 1 / Shape);
    }

    #endregion IContinuousDistribution
  }

}
