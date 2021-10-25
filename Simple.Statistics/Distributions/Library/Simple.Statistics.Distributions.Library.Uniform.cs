using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Uniform Distribution for [From..To) range
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Uniform_distribution_(continuous)"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class UniformDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="from">Range: from (inclusive)</param>
    /// <param name="to">Range: to (inclusive)</param>
    public UniformDistribution(double from, double to) {
      if (!double.IsFinite(from))
        throw new ArgumentOutOfRangeException(nameof(from), "value must be finite");
      if (!double.IsFinite(to))
        throw new ArgumentOutOfRangeException(nameof(to), "value must be finite");
      if (from > to)
        throw new ArgumentOutOfRangeException(nameof(to), "to must be less than from");

      From = from;
      To = to;

      m_Mean = (to + from) / 2.0;
      m_Variance = (to - from) / 12.0 * (to - from);
    }

    /// <summary>
    /// Standard constructor for [0..1) range
    /// </summary>
    public UniformDistribution()
      : this(0.0, 1.0) { }

    #endregion Create

    #region Public

    /// <summary>
    /// From (inclusive)
    /// </summary>
    public double From { get; }

    /// <summary>
    /// To (exclusive)
    /// </summary>
    public double To { get; }

    /// <summary>
    /// To String (debug only)
    /// </summary>
    public override string ToString() => (To == From)
      ? $"Constant Distribution (mean = {Mean})"
      : $"Uniform Distribution for [{From}..{To}) range";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public static double Cdf(double x, double from, double to) {
      if (!double.IsFinite(from))
        throw new ArgumentOutOfRangeException(nameof(from), "value must be finite");
      if (!double.IsFinite(to))
        throw new ArgumentOutOfRangeException(nameof(to), "value must be finite");
      if (from > to)
        throw new ArgumentOutOfRangeException(nameof(to), "to must be less than from");

      double mean = (to + from) / 2.0;

      if (to == from)
        return x < mean ? 0.0
                 : x == mean ? 0.5
                 : 1.0;

      return x < from ? 0.0
        : x >= to ? 1.0
        : (x - from) / (to - from);
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public static double Pdf(double x, double from, double to) {
      if (!double.IsFinite(from))
        throw new ArgumentOutOfRangeException(nameof(from), "value must be finite");
      if (!double.IsFinite(to))
        throw new ArgumentOutOfRangeException(nameof(to), "value must be finite");
      if (from > to)
        throw new ArgumentOutOfRangeException(nameof(to), "to must be less than from");

      return x < from || x > to ? 0.0 : 1.0 / (to - from);
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public static double Qdf(double x, double from, double to) {
      if (!double.IsFinite(from))
        throw new ArgumentOutOfRangeException(nameof(from), "value must be finite");
      if (!double.IsFinite(to))
        throw new ArgumentOutOfRangeException(nameof(to), "value must be finite");
      if (from > to)
        throw new ArgumentOutOfRangeException(nameof(to), "to must be less than from");

      if (to == from)
        return x >= 0 && x <= 1.0
          ? (to + from) / 2.0
          : throw new ArgumentOutOfRangeException(nameof(x));

      return x == 0 ? from
        : x == 1 ? to
        : x < 0 || x > 1 ? throw new ArgumentOutOfRangeException(nameof(x))
        : from + x * (to - from);
    }

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) {
      if (To == From)
        return x < Mean ? 0.0
                 : x == Mean ? 0.5
                 : 1.0;

      return x < From ? 0.0
        : x >= To ? 1.0
        : (x - From) / (To - From);
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) =>
      x < From || x > To ? 0.0 : 1.0 / (To - From);

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public override double Qdf(double x) {
      if (To == From)
        return x >= 0 && x <= 1.0
          ? Mean
          : throw new ArgumentOutOfRangeException(nameof(x));

      return x == 0 ? From
        : x == 1 ? To
        : x < 0 || x > 1 ? throw new ArgumentOutOfRangeException(nameof(x))
        : From + x * (To - From);
    }

    #endregion IContinuousDistribution
  }

}
