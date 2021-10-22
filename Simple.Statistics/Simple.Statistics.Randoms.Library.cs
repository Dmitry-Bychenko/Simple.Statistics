using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Statistics {

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
        using (RNGCryptoServiceProvider provider = new()) {
          byte[] seedData = new byte[sizeof(int)];

          provider.GetBytes(seedData);

          return new Random(BitConverter.ToInt32(seedData, 0));
        }
      });
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Default Random
    /// </summary>
    public static IContinuousRandom Default { get; } = new ContinuousRandom();

    /// <summary>
    /// Seed
    /// </summary>
    public int? Seed { get; }

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
        if (m_Random is not null) {
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

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Crypto Random based Continuous Random 
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class CryptoRandom : IContinuousRandom, IDisposable {
    #region Private Data

    private RNGCryptoServiceProvider m_Provider = new();

    #endregion Private Data

    #region IContinuousRandom

    /// <summary>
    /// Next Double Random
    /// </summary>
    public double NextDouble() {
      byte[] seedData = new byte[7];

      m_Provider.GetBytes(seedData);

      double result = 0.0;

      for (int i = seedData.Length - 1; i >= 0; --i)
        result = result / 256.0 + seedData[i];

      return result / 256.0;
    }

    #endregion IContinuousRandom

    #region IDisposable 

    private void Dispose(bool disposing) {
      if (disposing) {
        if (m_Provider is not null) {
          m_Provider.Dispose();

          m_Provider = null;
        }
      }
    }

    /// <summary>
    /// Is Disposed
    /// </summary>
    public bool IsDisposed => m_Provider is null;

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose() => Dispose(true);

    #endregion IDisposable
  }

}
