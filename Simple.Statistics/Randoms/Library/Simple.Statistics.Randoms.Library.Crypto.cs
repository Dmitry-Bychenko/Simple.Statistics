using System.Security.Cryptography;

namespace Simple.Statistics.Randoms.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Crypto Random based Continuous Random 
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class CryptoRandom : IContinuousRandom {
    #region Private Data

    private RNGCryptoServiceProvider m_Provider = new();

    #endregion Private Data

    #region Public

    /// <summary>
    /// To String 
    /// </summary>
    public override string ToString() => IsDisposed
      ? "Disposed Crypto Random"
      : "Crypto Random";

    #endregion Public

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
