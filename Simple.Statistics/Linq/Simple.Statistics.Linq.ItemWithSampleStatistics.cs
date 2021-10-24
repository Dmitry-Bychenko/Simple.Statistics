using System;

namespace Simple.Statistics.Linq {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Item with Sample Statistics
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public struct ItemWithStatistics<T> {
    #region Create

    internal ItemWithStatistics(T current, SampleStatistics<T> statistics) {
      Current = current;
      Statistics = statistics ?? throw new ArgumentNullException(nameof(statistics));
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Current
    /// </summary>
    public T Current { get; }

    /// <summary>
    /// Statistics
    /// </summary>
    public SampleStatistics<T> Statistics { get; }

    /// <summary>
    /// To String
    /// </summary>
    public override string ToString() => Current?.ToString();

    /// <summary>
    /// Equals
    /// </summary>
    public override bool Equals(object obj) {
      if (obj is ItemWithStatistics<T> other)
        return object.Equals(Current, other.Current);
      if (obj is T otherCurrent)
        return object.Equals(Current, otherCurrent);

      return false;
    }

    /// <summary>
    /// Get Hash Code
    /// </summary>
    public override int GetHashCode() => Current is null ? -1 : Current.GetHashCode();

    #endregion Public

    #region Operators

    /// <summary>
    /// Equals
    /// </summary>
    public static bool operator ==(ItemWithStatistics<T> left, ItemWithStatistics<T> right) =>
      Equals(left.Current, right.Current);

    /// <summary>
    /// Not Equals
    /// </summary>
    public static bool operator !=(ItemWithStatistics<T> left, ItemWithStatistics<T> right) =>
      !Equals(left.Current, right.Current);

    /// <summary>
    /// Implicit (to Current)
    /// </summary>
    public static implicit operator T(ItemWithStatistics<T> value) => value.Current;

    #endregion Operators
  }

}
