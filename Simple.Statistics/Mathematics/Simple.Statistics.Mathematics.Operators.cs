using System;

namespace Simple.Statistics.Mathematics {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Operators
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static partial class Operators {
    #region Algorithm

    private static double ToFinite(double value) {
      if (double.IsNaN(value))
        throw new ArgumentException("Argument must not be NaN", nameof(value));
      if (double.IsNegativeInfinity(value))
        return -1e200;//double.MinValue;
      if (double.IsPositiveInfinity(value))
        return 1e200;//double.MaxValue;
      else
        return value;
    }

    #endregion Algorithm

    #region Public

    /// <summary>
    /// Inversed function
    /// </summary>
    /// <param name="function"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static Func<double, double> Inverse(this Func<double, double> function,
                                               double from,
                                               double to) {
      if (double.IsNaN(from))
        throw new ArgumentException("Argument must not be NaN", nameof(from));
      if (double.IsNaN(to))
        throw new ArgumentException("Argument must not be NaN", nameof(to));

      return (toFind) => InverseAt(function, toFind, from, to);
    }

    /// <summary>
    /// Inversed function
    /// </summary>
    /// <param name="function"></param>
    /// <returns></returns>
    public static Func<double, double> Inverse(this Func<double, double> function) =>
      Inverse(function, double.MinValue, double.MaxValue);

    /// <summary>
    /// Inversed function
    /// </summary>
    public static double InverseAt(this Func<double, double> function,
                                        double toFind,
                                        double from,
                                        double to) {
      if (double.IsNaN(toFind))
        throw new ArgumentException("Argument must not be NaN", nameof(toFind));
      if (double.IsNaN(from))
        throw new ArgumentException("Argument must not be NaN", nameof(from));
      if (double.IsNaN(to))
        throw new ArgumentException("Argument must not be NaN", nameof(to));

      toFind = ToFinite(toFind);
      from = ToFinite(from);
      to = ToFinite(to);

      if (to < from)
        throw new ArgumentOutOfRangeException(nameof(to), "Empty interval.");

      double middle = (from + to) / 2.0;

      double fromValue = function(from);
      double toValue = function(to);
      double middleValue;

      if (((fromValue < toFind) && (toValue < toFind)) ||
          ((fromValue > toFind) && (toValue > toFind)))
        throw new ArgumentException("Insufficient or incorrect interval", nameof(to));

      // Main loop
      while (true) {
        middleValue = function(middle);

        if (middleValue > toFind)
          if (fromValue > toFind) {
            fromValue = middleValue;
            from = middle;
          }
          else
            to = middle;
        else if (fromValue >= toFind)
          to = middle;
        else {
          fromValue = middleValue;
          from = middle;
        }

        double newMiddle = (from + to) / 2.0;

        // No progess, quit
        if (newMiddle == middle)
          break;

        middle = newMiddle;
      }

      return middle;
    }

    /// <summary>
    /// Inversed function
    /// </summary>
    /// <param name="function"></param>
    /// <param name="toFind"></param>
    /// <returns></returns>
    public static double InverseAt(this Func<double, double> function, double toFind) =>
      InverseAt(function, toFind, double.MinValue, double.MaxValue);

    /// <summary>
    /// Solve function(x) = 0 on [from…to]
    /// </summary>
    /// <returns>Root</returns>
    public static double Solve(this Func<double, double> function,
                                    double from,
                                    double to) =>
      InverseAt(function, 0.0, from, to);

    /// <summary>
    /// Solve function(x) = 0 on [from…to]
    /// </summary>
    /// <param name="function">Function to solve</param>
    /// <returns>Root</returns>
    public static double Solve(this Func<double, double> function) =>
      InverseAt(function, 0.0, double.MinValue, double.MaxValue);

    #endregion Public
  }

}
