using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

namespace Simple.Statistics.Randoms.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Iron Table Random Generator
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class IronTableRandom : IContinuousRandom {
    #region Private Data

    private ThreadLocal<int> m_Index = new();

    private List<int> m_Items;

    #endregion Private Data

    #region Algorithm

    private void CreateTable(int count) {
      double fi = (Math.Sqrt(5) - 1.0) / 2.0;

      m_Items = Enumerable
        .Range(1, count)
        .Select(x => x)
        .OrderBy(x => (x * fi) % 1.0)
        .ToList();
    }

    #endregion Algorithm

    #region Create

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="count">Count</param>
    /// <param name="seed">Seed</param>
    public IronTableRandom(int count, int seed) {
      if (count <= 0)
        throw new ArgumentOutOfRangeException(nameof(count));

      Seed = seed;

      CreateTable(count);

      m_Index.Value = (seed % count + count) % count;
    }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="count">Count</param>
    public IronTableRandom(int count) {
      if (count <= 0)
        throw new ArgumentOutOfRangeException(nameof(count));

      CreateTable(count);

      Seed = null;

      using RNGCryptoServiceProvider provider = new();

      unchecked {
        byte[] data = new byte[sizeof(int)];
        provider.GetBytes(data);

        m_Index.Value = (BitConverter.ToInt32(data) % count + count) % count;
      }
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Seed
    /// </summary>
    public int? Seed { get; }

    /// <summary>
    /// Items
    /// </summary>
    public IReadOnlyList<int> Items => m_Items;

    /// <summary>
    /// Current Index
    /// </summary>
    public int Index => m_Index.Value;

    /// <summary>
    /// Count (Length of the Iron Table)
    /// </summary>
    public int Count => m_Items.Count;

    #endregion Public

    #region IContinuousRandom

    /// <summary>
    /// Next Double Random
    /// </summary>
    public double NextDouble() {
      if (m_Index is null)
        throw new ObjectDisposedException("this");

      int index = (m_Index.Value + 1) % m_Items.Count;

      m_Index.Value = index;

      return (m_Items[index] - 1.0) / m_Items.Count;
    }

    #endregion IContinuousRandom

    #region IDisposable

    private void Dispose(bool disposing) {
      if (disposing && m_Index is not null) {
        m_Index.Dispose();
        m_Index = null;
      }
    }

    /// <summary>
    /// Is Disposed
    /// </summary>
    public bool IsDisposed => m_Index is null;

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose() => Dispose(true);

    #endregion IDisposable
  }

}
