using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple.Statistics.Distributions.Samplers {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// ISampler Extensions
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class SamplerExtensions {
    #region Public

    /// <summary>
    /// Generate samples for the distributions provided
    /// </summary>
    /// <param name="sampler">Sampler</param>
    /// <param name="count">Number (approximate) of samples to create</param>
    /// <param name="distributions">Distributions</param>
    public static IEnumerable<double[]> GenerateSamples(this ISampler sampler,
      int count,
      IEnumerable<IContinuousDistribution> distributions) {

      if (sampler is null)
        throw new ArgumentNullException(nameof(sampler));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof(count));
      if (distributions is null)
        throw new ArgumentNullException(nameof(distributions));

      IContinuousDistribution[] dist = distributions.ToArray();

      if (dist.Length <= 0)
        throw new ArgumentOutOfRangeException(nameof(distributions));
      if (dist.Any(d => d is null))
        throw new ArgumentException("nulls are not allowed within distributions", nameof(distributions));

      foreach (double[] points in sampler.Generate(dist.Length, count)) {
        for (int i = 0; i < points.Length; ++i)
          points[i] = dist[i].Qdf(points[i]);

        yield return points;
      }
    }

    #endregion Public
  }

}
