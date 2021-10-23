using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Triangular Distribution
  /// </summary>
  /// <seealso cref="https://en.wikipedia.org/wiki/Triangular_distribution"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class TriangularDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="from">Leftmost point (a)</param>
    /// <param name="to">Rightmost point (b)</param>
    /// <param name="mode">Mode (c)</param>
    public TriangularDistribution(double from, double to, double mode) {
      if (!double.IsFinite(from))
        throw new ArgumentOutOfRangeException(nameof(from), "from value must be finite");
      if (!double.IsFinite(to))
        throw new ArgumentOutOfRangeException(nameof(to), "to value must be finite");
      if (!double.IsFinite(mode))
        throw new ArgumentOutOfRangeException(nameof(mode), "mode value must be finite");

      if (from >= to)
        throw new ArgumentOutOfRangeException(nameof(to), "empty [from..to) interval");
      if (from > mode)
        throw new ArgumentOutOfRangeException(nameof(mode), "wrong mode location (mode < from)");
      if (mode > to)
        throw new ArgumentOutOfRangeException(nameof(mode), "wrong mode location (mode > to)");

      From = from;
      To = to;
      Mode = mode;

      m_Mean = (From + To + Mode) / 3.0;
      m_Variance = (From * From + To * To + Mode * Mode - From * To - From * Mode - To * Mode) / 18.0;
    }

    /// <summary>
    /// Standard constructor (isosceles triangle)
    /// </summary>
    /// <param name="from">Leftmost point (a)</param>
    /// <param name="to">Rightmost point (b)</param>
    public TriangularDistribution(double from, double to)
      : this(from, to, (from + to) / 2.0) { }

    /// <summary>
    /// Standard constructor (isosceles triangle with 0..1 range)
    /// </summary>
    public TriangularDistribution()
      : this(0.0, 1.0, 0.5) { }

    #endregion Create

    #region Public

    /// <summary>
    /// Leftmost point
    /// </summary>
    public double From { get; }

    /// <summary>
    /// Rightmost point
    /// </summary>
    public double To { get; }

    /// <summary>
    /// Mode
    /// </summary>
    public double Mode { get; }

    /// <summary>
    /// To String (debug)
    /// </summary>
    public override string ToString() => $"Triangular Distribution with left = {From}, right = {To}, mode {Mode}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) =>
          x <= From ? 0.0
        : x < Mode ? (x - From) * (x - From) / (To - From) / (Mode - From)
        : x == Mode ? (Mode - From) / (To - From)
        : x < To ? 1.0 - (To - x) * (To - x) / (To - From) / (To - Mode)
        : 1.0;

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) =>
          x < From ? 0.0
        : x < Mode ? 2.0 * (x - From) / (To - From) / (Mode - From)
        : x == Mode ? 2.0 / (To - From)
        : x < To ? 2 * (To - x) / (To - From) / (To - Mode)
        : 0.0;

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    /// <seealso cref="https://www.boost.org/doc/libs/1_68_0/libs/math/doc/html/math_toolkit/dist_ref/dists/triangular_dist.html"/>
    public override double Qdf(double x) {
      if (x == 0)
        return From;
      if (x == 1)
        return To;
      if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));

      double a = From;
      double b = To;
      double c = Mode;

      double p0 = (c - a) / (b - a);

      if (x < p0)
        return Math.Sqrt((b - a) * (c - a) * x) + a;
      if (x == p0)
        return c;

      return b - Math.Sqrt((b - a) * (b - c) * (1.0 - x));
    }

    #endregion IContinuousDistribution
  }

}
