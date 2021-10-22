using System;
using System.Security.Cryptography;
using System.Threading;

namespace Simple.Statistics.Randoms {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Random based Continuous Random 
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class ContinuousRandom : IContinuousRandom, IDisposable {
    #region Private Data

    private ThreadLocal<Random> m_Random;

    #endregion Private Data

    #region Create

    /// <summary>
    /// Simple Random, standard constructor
    /// </summary>
    /// <param name="seed">Seed</param>
    public ContinuousRandom(int seed) {
      Seed = seed;

      m_Random = new ThreadLocal<Random>(() => new Random(seed));
    }

    /// <summary>
    /// Simple Random, standard constructor
    /// </summary>
    public ContinuousRandom() {
      m_Random = new ThreadLocal<Random>(() => {
        using RNGCryptoServiceProvider provider = new();

        byte[] seedData = new byte[sizeof(int)];
        provider.GetBytes(seedData);

        return new Random(BitConverter.ToInt32(seedData, 0));
      });
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Seed
    /// </summary>
    public int? Seed { get; }

    /// <summary>
    /// To String 
    /// </summary>
    public override string ToString() {
      if (IsDisposed)
        return "Disposed Continuous Random";

      if (ReferenceEquals(this, IContinuousRandom.Default))
        return "Default Random";

      return Seed.HasValue
        ? $"Continuous Random (Seed = {Seed})"
        : $"Continuous Random";
    }

    #endregion Public

    #region IContinuousRandom

    /// <summary>
    /// Next Double Random
    /// </summary>
    public double NextDouble() => m_Random is not null
      ? m_Random.Value.NextDouble()
      : throw new ObjectDisposedException("this");

    #endregion IContinuousRandom

    #region IDisposable

    /// <summary>
    /// Is Disposed
    /// </summary>
    public bool IsDisposed => m_Random is null;

    private void Dispose(bool disposing) {
      if (disposing) {
        if (m_Random is not null && !ReferenceEquals(this, IContinuousRandom.Default)) {
          m_Random.Dispose();
          m_Random = null;
        }
      }
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose() => Dispose(true);

    #endregion IDisposable
  }

}
