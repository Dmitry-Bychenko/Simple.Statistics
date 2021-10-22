using System;
using System.Threading;

using Simple.Statistics.Mathematics;

namespace Simple.Statistics.Distributions {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Continuous Probability Distribution (like Normal, LogNormal, Triangle etc.)
  /// Thread Safe
  /// </summary>
  /// <see cref="https://en.wikipedia.org/wiki/Probability_distribution#Continuous_probability_distribution"/>
  /// <threadsafety static="true" instance="true"/>
  // 
  //-------------------------------------------------------------------------------------------------------------------

  public interface IContinuousDistribution {
    /// <summary>
    /// Probability Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    double Pdf(double x);

    /// <summary>
    /// Cumulative Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    double Cdf(double x);

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    double Qdf(double x);
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Continuous Distribution
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public abstract class ContinuousDistribution : IContinuousDistribution {
    #region Private Data

    private const double Tolerance = 1.0e-7;

    internal double m_Mean = double.NaN;

    internal double m_Variance = double.NaN;

    #endregion Private Data

    #region Algorithm

    /// <summary>
    /// Mean Computation
    /// </summary>
    protected virtual double CoreMean() {
      double left = Qdf(Tolerance);
      double right = Qdf(1 - Tolerance);

      return Integrals.SimpsonAt((x) => x * Pdf(x), left, right);
    }

    /// <summary>
    /// Variance Computation
    /// </summary>
    protected virtual double CoreVariance() {
      double left = Qdf(Tolerance);
      double right = Qdf(1 - Tolerance);

      return Integrals.SimpsonAt((x) => x * x * Pdf(x), left, right) - Mean * Mean;
    }

    #endregion Algorithm

    #region Public

    /// <summary>
    /// Mean
    /// </summary>
    public double Mean {
      get {
        if (double.IsNaN(m_Mean))
          Interlocked.Exchange(ref m_Mean, CoreMean());

        return m_Mean;
      }
    }

    /// <summary>
    /// Variance
    /// </summary>
    public double Variance {
      get {
        if (double.IsNaN(m_Variance))
          Interlocked.Exchange(ref m_Variance, CoreVariance());

        return m_Mean;
      }
    }

    /// <summary>
    /// Standard Error
    /// </summary>
    public double StandardError => Math.Sqrt(Variance);

    /// <summary>
    /// To String 
    /// </summary>
    public abstract override string ToString(); // <- We force developers to implement it

    #endregion Public

    #region IContinuousDistribution

    /// <summary>
    /// Probability Density Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Probability_density_function"/>
    public abstract double Pdf(double x);

    /// <summary>
    /// Cumulative Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Cumulative_distribution_function"/>
    public abstract double Cdf(double x);

    /// <summary>
    /// Quantile Distribution Function
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Quantile_function"/>
    public virtual double Qdf(double x) {
      if (x == 0)
        return double.NegativeInfinity;
      if (x == 1)
        return double.PositiveInfinity;
      if (x < 0 || x > 1)
        throw new ArgumentOutOfRangeException(nameof(x));

      Func<double, double> cdf = Cdf;

      return cdf.Inverse(double.MinValue, double.MaxValue)(x);
    }

    #endregion IContinuousDistribution
  }
}
