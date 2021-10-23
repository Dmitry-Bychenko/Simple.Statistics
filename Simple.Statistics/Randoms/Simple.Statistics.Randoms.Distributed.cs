using Simple.Statistics.Distributions;
using System;

namespace Simple.Statistics.Randoms {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Random Value with given Continuous Distribution
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class DistributedRandom : IContinuousRandom, IDisposable {
    #region Private Data

    private IContinuousRandom m_Random;

    #endregion Private Data

    #region Create

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="distribution">Distribution to use</param>
    /// <param name="random">Random to use; null for deafult generator</param>
    public DistributedRandom(IContinuousDistribution distribution, IContinuousRandom random) {
      Distribution = distribution ?? throw new ArgumentNullException(nameof(distribution));
      m_Random = random ?? IContinuousRandom.Default;
    }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="distribution">Distribution to use</param>
    public DistributedRandom(IContinuousDistribution distribution)
      : this(distribution, null) { }

    #endregion Create

    #region Public

    /// <summary>
    /// Distribution
    /// </summary>
    public IContinuousDistribution Distribution { get; }

    /// <summary>
    /// To String
    /// </summary>
    public override string ToString() {
      if (m_Random is null)
        return $"Disposed DistributedRandom";

      return $"{Distribution} over {m_Random} generator";
    }

    #endregion Public

    #region IContinuousRandom

    /// <summary>
    /// Next Double 
    /// </summary>
    public double NextDouble() => m_Random is not null
      ? Distribution.Qdf(m_Random.NextDouble())
      : throw new ObjectDisposedException("this");

    #endregion IContinuousRandom

    #region IDisposable

    private void Dispose(bool disposing) {
      if (disposing && m_Random is not null) {
        if (!ReferenceEquals(m_Random, IContinuousRandom.Default) && m_Random is IDisposable disposable)
          disposable.Dispose();

        m_Random = null;
      }
    }

    /// <summary>
    /// Is Disposed
    /// </summary>
    public bool IsDisposed => m_Random is null;

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose() => Dispose(true);

    #endregion IDisposable
  }

}
