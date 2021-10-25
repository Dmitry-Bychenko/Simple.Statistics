using System;

namespace Simple.Statistics.Distributions.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Log Triangular Distribution
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class LogTriangularDistribution : ContinuousDistribution {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="from">Leftmost point (a)</param>
    /// <param name="to">Rightmost point (b)</param>
    /// <param name="mode">Mode (c)</param>
    public LogTriangularDistribution(double from, double to, double mode) {
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

      //m_Mean = (From + To + Mode) / 3.0;
      //m_Variance = (From * From + To * To + Mode * Mode - From * To - From * Mode - To * Mode) / 18.0;
    }

    /// <summary>
    /// Standard constructor (isosceles triangle)
    /// </summary>
    /// <param name="from">Leftmost point (a)</param>
    /// <param name="to">Rightmost point (b)</param>
    public LogTriangularDistribution(double from, double to)
      : this(from, to, (from + to) / 2.0) { }

    /// <summary>
    /// Standard constructor (isosceles triangle with 0..1 range)
    /// </summary>
    public LogTriangularDistribution()
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
    /// To String 
    /// </summary>
    public override string ToString() => $"Log Triangular Distribution with left = {From}, right = {To}, mode {Mode}";

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public static double Cdf(double x, double from, double to, double mode) {
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

      if (x <= Math.Exp(from))
        return 0.0;
      if (x <= Math.Exp(mode))
        return (Math.Log(x) - from) * (Math.Log(x) - from) / (to - from) / (mode - from);
      if (x < Math.Exp(to))
        return (Math.Log(x) - to) * (Math.Log(x) - to) / (to - from) / (to - mode);

      return 1.0;
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public static double Pdf(double x, double from, double to, double mode) {
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

      if (x < Math.Exp(from))
        return 0.0;
      if (x < Math.Exp(mode))
        return 2 * (Math.Log(x) - from) / x / (to - from) / (mode - from);
      if (x == Math.Exp(mode))
        return 2 / x / (to - from);
      if (x <= Math.Exp(to))
        return 2 * (to - Math.Log(x)) / x / (to - from) / (to - mode);

      return 0.0;
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    /// <seealso cref="https://www.boost.org/doc/libs/1_68_0/libs/math/doc/html/math_toolkit/dist_ref/dists/triangular_dist.html"/>
    public static double Qdf(double x, double from, double to, double mode) {
      if (x < 0 || x > 1 || !double.IsFinite(x))
        throw new ArgumentOutOfRangeException(nameof(x), "x must be in [0..1] range");

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

      if (x == 0.0)
        return Math.Exp(from);
      if (x <= (mode - from) / (to - from))
        return Math.Exp(from + Math.Sqrt(x * (to - from) * (mode - from)));
      if (x < 1)
        return Math.Exp(to - Math.Sqrt((1 - x) * (to - from) * (to - mode)));

      return Math.Exp(to);
    }

    /// <summary>
    /// Cumulative Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public override double Cdf(double x) {
      if (x <= Math.Exp(From))
        return 0.0;
      if (x <= Math.Exp(Mode))
        return (Math.Log(x) - From) * (Math.Log(x) - From) / (To - From) / (Mode - From);
      if (x < Math.Exp(To))
        return (Math.Log(x) - To) * (Math.Log(x) - To) / (To - From) / (To - Mode);

      return 1.0;
    }

    /// <summary>
    /// Probability Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public override double Pdf(double x) {
      if (x < Math.Exp(From))
        return 0.0;
      if (x < Math.Exp(Mode))
        return 2 * (Math.Log(x) - From) / x / (To - From) / (Mode - From);
      if (x == Math.Exp(Mode))
        return 2 / x / (To - From);
      if (x <= Math.Exp(To))
        return 2 * (To - Math.Log(x)) / x / (To - From) / (To - Mode);

      return 0.0;
    }

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    /// <seealso cref="https://www.boost.org/doc/libs/1_68_0/libs/math/doc/html/math_toolkit/dist_ref/dists/triangular_dist.html"/>
    public override double Qdf(double x) {
      if (x < 0 || x > 1 || !double.IsFinite(x))
        throw new ArgumentOutOfRangeException(nameof(x), "x must be in [0..1] range");

      if (x == 0.0)
        return Math.Exp(From);
      if (x <= (Mode - From) / (To - From))
        return Math.Exp(From + Math.Sqrt(x * (To - From) * (Mode - From)));
      if (x < 1)
        return Math.Exp(To - Math.Sqrt((1 - x) * (To - From) * (To - Mode)));

      return Math.Exp(To);
    }

    #endregion IContinuousDistribution
  }

}
