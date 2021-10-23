using Simple.Statistics.Randoms;
using System;
using System.Collections.Generic;

namespace Simple.Statistics.Distributions.Samplers.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Monte Carlo Sampler
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------  

  public sealed class SamplerMonteCarlo : ISampler {
    #region Private Data

    private readonly IContinuousRandom m_Random;

    #endregion Private Data

    #region Create

    /// <summary>
    /// Sampling with Seed
    /// </summary>
    /// <param name="seed">Seed</param>
    public SamplerMonteCarlo(IContinuousRandom random) {
      m_Random = random ?? IContinuousRandom.Default;
    }

    /// <summary>
    /// Sampling 
    /// </summary>
    public SamplerMonteCarlo()
      : this(null) { }

    #endregion Create

    #region ISampler

    /// <summary>
    /// Generate [0..1] * [0..1] * ... * [0..1] samples 
    /// </summary>
    /// <param name="dimensions">Dimensions</param>
    /// <param name="count">Approximate number of samples</param>
    public IEnumerable<double[]> Generate(int dimensions, int count) {
      if (dimensions <= 0)
        throw new ArgumentOutOfRangeException(nameof(dimensions));
      else if (count < 0)
        throw new ArgumentOutOfRangeException(nameof(count));

      for (int i = 0; i < count; ++i) {
        double[] result = new double[dimensions];

        for (int c = result.Length - 1; c >= 0; --c)
          result[c] = m_Random.NextDouble();

        yield return result;
      }
    }

    #endregion ISampler
  }

}
