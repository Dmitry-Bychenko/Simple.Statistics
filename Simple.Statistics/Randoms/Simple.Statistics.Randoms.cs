using Simple.Statistics.Randoms.Library;
using System;

namespace Simple.Statistics.Randoms {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// IRandom
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public interface IContinuousRandom : IDisposable {
    /// <summary>
    /// Next Random Double Value
    /// </summary>
    double NextDouble();

    /// <summary>
    /// Default Continuous Random
    /// </summary>
    public static IContinuousRandom Default { get; } = new ContinuousRandom();
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Continuous Random Extensions
  /// </summary>
  //
  //------------------------------------------------------------------------------------------------------------------

  public static class ContinuousRandomExtensions {
    #region Public

    /// <summary>
    /// Next
    /// </summary>
    public static int Next(this IContinuousRandom random, int maxValue) {
      if (random is null)
        throw new ArgumentNullException(nameof(random));

      if (maxValue < 0)
        throw new ArgumentOutOfRangeException(nameof(maxValue));

      return (int)(random.NextDouble() * maxValue);
    }

    /// <summary>
    /// Next
    /// </summary>
    public static int Next(this IContinuousRandom random, int minValue, int maxValue) {
      if (random is null)
        throw new ArgumentNullException(nameof(random));

      if (maxValue < minValue)
        throw new ArgumentOutOfRangeException(nameof(maxValue));

      return minValue + (int)(random.NextDouble() * ((long)maxValue - minValue));
    }

    #endregion Public
  }

}
