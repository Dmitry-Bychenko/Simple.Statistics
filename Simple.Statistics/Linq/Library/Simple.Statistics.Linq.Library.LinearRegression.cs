using System;
using System.Collections.Generic;

namespace Simple.Statistics.Linq.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// 
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class LinearRegression<T> : ISampleStatisticsExecutor<T> {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="xSelector">X selector</param>
    /// <param name="ySelector">Y selector</param>
    public LinearRegression(Func<T, double> xSelector, Func<T, double> ySelector) {
      SelectorX = xSelector ?? throw new ArgumentNullException(nameof(xSelector));
      SelectorY = ySelector ?? throw new ArgumentNullException(nameof(ySelector));
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Selector to get X
    /// </summary>
    public Func<T, double> SelectorX { get; }

    /// <summary>
    /// Selector to get Y
    /// </summary>
    public Func<T, double> SelectorY { get; }

    /// <summary>
    /// N (count)
    /// </summary>
    public long N { get; private set; }

    /// <summary>
    /// Sum of x
    /// </summary>
    public double Sx { get; private set; }

    /// <summary>
    /// Sum of x**2
    /// </summary>
    public double Sxx { get; private set; }

    /// <summary>
    /// Sum of y
    /// </summary>
    public double Sy { get; private set; }

    /// <summary>
    /// Sum of y**2
    /// </summary>
    public double Syy { get; private set; }

    /// <summary>
    /// Sum of x * y
    /// </summary>
    public double Sxy { get; private set; }

    /// <summary>
    /// Mean X
    /// </summary>
    public double MeanX => Sx / N;

    /// <summary>
    /// Mean Y
    /// </summary>
    public double MeanY => Sy / N;

    /// <summary>
    /// Variance X
    /// </summary>
    public double VarianceX => Sxx / N - MeanX * MeanX;

    /// <summary>
    /// Variance Y
    /// </summary>
    public double VarianceY => Syy / N - MeanY * MeanY;

    /// <summary>
    /// Standard Error X
    /// </summary>
    public double StandardErrorX => Math.Sqrt(VarianceX);

    /// <summary>
    /// Standard Error Y
    /// </summary>
    public double StandardErrorY => Math.Sqrt(VarianceY);

    /// <summary>
    /// Covariance
    /// </summary>
    public double Covariance => Sxy / N - MeanX * MeanY;

    /// <summary>
    /// A coefficient
    /// </summary>
    public double A => (N * Sxy - Sx * Sy) / (N * Sxx - Sx * Sx);

    /// <summary>
    /// A variance
    /// </summary>
    public double VarianceA {
      get {
        double dx = Sxx - Sx * Sx / N;
        double dy = Syy - Sy * Sy / N;

        if (dx == 0.0)
          return 0.0;

        return (dy / dx - A * A) / N;
      }
    }

    /// <summary>
    /// A Standard Error
    /// </summary>
    public double StandardErrorA => Math.Sqrt(VarianceA);

    /// <summary>
    /// B coefficient
    /// </summary>
    public double B => (Sy - A * Sx) / N;

    /// <summary>
    /// B Variance
    /// </summary>
    public double VarianceB {
      get {
        double dx = Sxx - Sx * Sx / N;

        return VarianceA * dx;
      }
    }

    /// <summary>
    /// B Standard Error
    /// </summary>
    public double StandardErrorB => Math.Sqrt(VarianceB);

    /// <summary>
    /// TSS
    /// </summary>
    public double TSS => Syy - Sy * Sy / N;

    /// <summary>
    /// ESS
    /// </summary>
    public double ESS => A * A * (Sxx - Sx * Sx / N);

    /// <summary>
    /// RSS
    /// </summary>
    public double RSS => TSS - ESS;

    /// <summary>
    /// Correlation
    /// </summary>
    public double R => (Sxy / N - MeanX * MeanY) / Math.Sqrt(VarianceX) / Math.Sqrt(VarianceY);

    /// <summary>
    /// Data Degree Of Freedom
    /// </summary>
    public long DataDegreeOfFreedom => N - 2;

    /// <summary>
    /// Variable Degree Of Freedom
    /// </summary>
    public long VariableDegreeOfFreedom => 1;

    /// <summary>
    /// Empirical Fisher Coefficient F(DataDegree, VariableDegree)
    /// </summary>
    public double F => R * R / (1 - R * R) * DataDegreeOfFreedom / VariableDegreeOfFreedom;

    /// <summary>
    /// Predictor
    /// </summary>
    public Func<double, double> Predictor => (x) => A * x + B;

    #endregion Public

    #region ISampleStatisticsExecutor<T>

    void ISampleStatisticsExecutor<T>.Append(T item) {
      N += 1;

      double x = SelectorX(item);
      double y = SelectorY(item);

      Sx += x;
      Sy += y;
      Sxx += x * x;
      Syy += y * y;
      Sxy += x * y;
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
    /// To Linear Regression
    /// </summary>
    public static LinearRegression<T> ToLinearRegression<T>(this IEnumerable<T> source,
                                                                 Func<T, double> xSelector,
                                                                 Func<T, double> ySelector) {
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      LinearRegression<T> result = new(xSelector, ySelector);

      foreach (var item in source)
        ((ISampleStatisticsExecutor<T>)result).Append(item);

      return result;
    }

    #endregion Public
  }

}
