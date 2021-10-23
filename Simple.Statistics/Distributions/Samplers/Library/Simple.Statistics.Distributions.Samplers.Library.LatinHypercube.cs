using Simple.Statistics.Randoms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple.Statistics.Distributions.Samplers.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Latin Hypercube sampler 
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class SamplerLatinHypercube : ISampler {
    #region Private Data

    private readonly IContinuousRandom m_Random;

    #endregion Private Data

    #region Create

    /// <summary>
    /// Sampling with Seed
    /// </summary>
    /// <param name="seed">Seed</param>
    public SamplerLatinHypercube(IContinuousRandom random) {
      m_Random = random ?? IContinuousRandom.Default;
    }

    /// <summary>
    /// Sampling 
    /// </summary>
    public SamplerLatinHypercube()
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

      double h = 1.0 / count;

      List<int[]> ranges = Enumerable
        .Range(0, dimensions)
        .Select(dimension => Enumerable
          .Range(0, count)
          .OrderBy(x => m_Random.NextDouble())
          .ToArray())
        .ToList();

      for (int i = 0; i < count; ++i) {
        double[] result = new double[dimensions];

        for (int c = 0; c < result.Length; ++c) {
          int d = ranges[c][i];

          result[c] = h * d + m_Random.NextDouble() * h;
        }

        yield return result;
      }
    }

    #endregion ISampler
  }

}
