using Simple.Statistics.Randoms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple.Statistics.Distributions.Samplers.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Orthogonal sampler 
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class SamplerOrthogonal : ISampler {
    #region Private Data

    private readonly IContinuousRandom m_Random;

    #endregion Private Data

    #region Algorithm

    public static IEnumerable<T[]> OrderedWithReplacement<T>(IEnumerable<T> source, int size) {
      if (source is null)
        throw new ArgumentNullException(nameof(source));
      else if (size < 0)
        throw new ArgumentOutOfRangeException(nameof(size));

      T[] alphabet = source.ToArray();

      if (alphabet.Length <= 0)
        yield break;
      else if (size == 0)
        yield break;

      int[] indexes = new int[size];

      do {
        yield return indexes
          .Select(i => alphabet[i])
          .ToArray();

        for (int i = indexes.Length - 1; i >= 0; --i)
          if (indexes[i] >= alphabet.Length - 1)
            indexes[i] = 0;
          else {
            indexes[i] += 1;

            break;
          }
      }
      while (!indexes.All(index => index == 0));
    }

    #endregion Algorithm

    #region Create

    /// <summary>
    /// Sampling with Seed
    /// </summary>
    /// <param name="seed">Seed</param>
    public SamplerOrthogonal(IContinuousRandom random) {
      m_Random = random ?? IContinuousRandom.Default;
    }

    /// <summary>
    /// Sampling 
    /// </summary>
    public SamplerOrthogonal()
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

      double root = Math.Pow(count, 1.0 / dimensions);

      int parts = (int)root + (root % 1 == 0 ? 0 : 1);
      double h = 1.0 / parts;

      int[] shifts = Enumerable
        .Range(0, parts)
        .Select(i => i)
        .ToArray();

      foreach (int[] record in OrderedWithReplacement(shifts, dimensions)) {
        double[] result = record
          .Select(i => i * h + m_Random.NextDouble() * h)
          .ToArray();

        yield return result;
      }
    }

    #endregion ISampler
  }

}
