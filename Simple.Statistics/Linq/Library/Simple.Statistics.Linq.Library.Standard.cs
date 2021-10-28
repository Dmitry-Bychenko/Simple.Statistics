using Simple.Statistics.Distributions.Library;

using System;
using System.Collections.Generic;

namespace Simple.Statistics.Linq.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Standard Sample Statistics
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class StandardSampleStatistics<T> : ISampleStatisticsExecutor<T> {
    #region Private Data

    private readonly IComparer<T> m_Comparer;

    private double m_SumR;

    private double m_SumLog;

    #endregion Private Data

    #region Algorithm

    private static double StandardSelector(T item) => (double)Convert.ToDouble(item);

    #endregion Algorithm

    #region Create

    /// <summary>
    /// Standard Constructor
    /// </summary>
    internal StandardSampleStatistics(Func<T, double> selector, IComparer<T> comparer) {
      if (selector is null) {
        if (typeof(double).IsAssignableFrom(typeof(T)))
          Selector = StandardSelector;
      }

      Selector = selector ?? throw new ArgumentNullException(nameof(selector));

      if (comparer is null) {
        comparer = Comparer<T>.Default;

        if (comparer is null)
          comparer = Comparer<T>.Create((left, right) => Selector(left).CompareTo(Selector(right)));
      }

      m_Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
    }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    public StandardSampleStatistics(IEnumerable<T> source, Func<T, double> selector, IComparer<T> comparer)
      : this(selector, comparer) {

      if (source is not null)
        foreach (var item in source)
          ((ISampleStatisticsExecutor<T>)this).Append(item);
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Selector
    /// </summary>
    public Func<T, double> Selector { get; }

    /// <summary>
    /// Count
    /// </summary>
    public long Count { get; private set; }

    /// <summary>
    /// Sum
    /// </summary>
    public double Sum { get; private set; }

    /// <summary>
    /// Sum
    /// </summary>
    public double Sum2 { get; private set; }

    /// <summary>
    /// Sum
    /// </summary>
    public double Sum3 { get; private set; }

    /// <summary>
    /// Sum
    /// </summary>
    public double Sum4 { get; private set; }

    /// <summary>
    /// Mean
    /// </summary>
    public double Mean => Sum / Count;

    /// <summary>
    /// Quadratic Mean
    /// </summary>
    public double QuadraticMean => Math.Sqrt(Sum2 / Count);

    /// <summary>
    /// Harmonic Mean
    /// </summary>
    public double HarmonicMean => Count / m_SumR;

    /// <summary>
    /// Geometric Mean
    /// </summary>
    public double GeometricMean => Parity * Math.Exp(m_SumLog / Count);

    /// <summary>
    /// Variance
    /// </summary>
    public double Variance => Sum2 / Count - Sum * Sum / Count / Count;

    /// <summary>
    /// Standard Error
    /// </summary>
    public double StandardError => Math.Sqrt(Variance);

    /// <summary>
    /// Skew
    /// </summary>
    public double Skew {
      get {
        if (Count <= 0)
          return double.NaN;

        double m1 = Sum / Count;
        double m2 = Sum2 / Count;
        double m3 = Sum3 / Count;

        double s = m3 - 3 * m2 * m1 + 2 * m1 * m1 * m1;
        double d = m2 - m1 * m1;

        if (d == 0)
          if (s < 0)
            return double.NegativeInfinity;
          else
            return double.PositiveInfinity;
        else
          return s / Math.Sqrt(d * d * d);
      }
    }

    /// <summary>
    /// Kurtosis
    /// </summary>
    public double Kurtosis {
      get {
        if (Count <= 0)
          return double.NaN;

        double m1 = Sum / Count;
        double m2 = Sum2 / Count;
        double m3 = Sum3 / Count;
        double m4 = Sum4 / Count;

        double s = m4 - 4 * m3 * m1 + 6 * m2 * m1 * m1 - 3 * m1 * m1 * m1 * m1;
        double d = m2 - m1 * m1;

        if (d == 0)
          if (s < 0)
            return double.NegativeInfinity;
          else
            return double.PositiveInfinity;
        else
          return s / d * d - 3.0;
      }
    }

    /// <summary>
    /// Max
    /// </summary>
    public T Max { get; private set; }

    /// <summary>
    /// Max Index
    /// </summary>
    public long MaxIndex { get; private set; } = -1;

    /// <summary>
    /// Min
    /// </summary>
    public T Min { get; private set; }

    /// <summary>
    /// Min Index
    /// </summary>
    public long MinIndex { get; private set; } = -1;

    /// <summary>
    /// Student Qdf 
    /// </summary>
    public double Student(double x) => StudentDistribution.Qdf(Math.Abs(x - Mean) / StandardError, Count - 1);

    /// <summary>
    /// Confidence Interval
    /// </summary>
    /// <param name="p">level in [0..1] range</param>
    public (double from, double to) ConfidenceInterval(double p) {
      if (p < 0 || p > 1)
        throw new ArgumentOutOfRangeException(nameof(p));

      var mean = Mean;

      if (0 == p)
        return (mean, mean);
      if (1 == p)
        return Variance == 0 ? (mean, mean) : (double.NegativeInfinity, double.PositiveInfinity);

      double d = StudentDistribution.Cdf(p, Count - 1) * StandardError;

      return (mean - d, mean + d);
    }

    /// <summary>
    /// Parity
    /// </summary>
    public int Parity { get; private set; } = 1;

    /// <summary>
    /// To String
    /// </summary>
    public override string ToString() =>
      $"N = {Count}; Min = {Min}, Max = {Max}; Mean = {Mean}, StdErr = {StandardError}";

    #endregion Public

    #region ISampleStatisticsExecutor<T>

    // Append
    void ISampleStatisticsExecutor<T>.Append(T item) {
      if (Count == 0 || m_Comparer.Compare(item, Min) > 0) {
        Min = item;
        MinIndex = Count;
      }

      if (Count == 0 || m_Comparer.Compare(item, Max) > 0) {
        Max = item;
        MaxIndex = Count;
      }

      Count += 1;

      double value = Selector(item);

      Sum += value;
      Sum2 += value * value;
      Sum3 += value * value * value;
      Sum4 += value * value * value * value;

      m_SumR += 1.0 / value;

      if (value == 0)
        Parity = 0;
      else if (value < 0)
        Parity = -Parity;

      if (value > 0)
        m_SumLog += Math.Log(Math.Abs(value));
    }
        
    #endregion ISampleStatisticsExecutor<T>
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Enumerable Extensions
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static partial class EnumerableExtensions {
    #region Public

    /// <summary>
    /// To Standard Statistics
    /// </summary>
    public static StandardSampleStatistics<T> ToStandardStatistics<T>(this IEnumerable<T> source,
                                                                           Func<T, double> selector,
                                                                           IComparer<T> comparer) {
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      StandardSampleStatistics<T> result = new(selector, comparer);

      foreach (var item in source)
        ((ISampleStatisticsExecutor<T>)result).Append(item);

      return result;
    }

    /// <summary>
    /// To Standard Statistics
    /// </summary>
    public static StandardSampleStatistics<T> ToStandardStatistics<T>(this IEnumerable<T> source,
                                                                           Func<T, double> selector)
      where T : IComparable<T> => ToStandardStatistics<T>(source, selector, Comparer<T>.Default);

    /// <summary>
    /// To Standard Statistics
    /// </summary>
    public static StandardSampleStatistics<T> ToStandardStatistics<T>(this IEnumerable<T> source)
      where T : IComparable<T> => ToStandardStatistics<T>(source, null, Comparer<T>.Default);

    /// <summary>
    /// To Standard Statistics
    /// </summary>
    public static StandardSampleStatistics<T> ToStandardStatistics<T>(this IEnumerable<T> source,
                                                                           IComparer<T> comparer) =>
      ToStandardStatistics<T>(source, null, comparer);

    #endregion Public
  }

}
