using System.Collections.Generic;

namespace Simple.Statistics.Distributions.Samplers {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Uniform Distribution Sampler
  /// </summary>
  /// <see cref="https://en.wikipedia.org/wiki/Latin_hypercube_sampling"/>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public interface ISampler {
    /// <summary>
    /// Generate sample for [0..1] * [0..1] * ... * [0..1] (Dimension times) range
    /// </summary>
    /// <param name="dimensions">Dimensions</param>
    /// <param name="count">Approximate number of samples</param>
    IEnumerable<double[]> Generate(int dimensions, int count);
  }

}
