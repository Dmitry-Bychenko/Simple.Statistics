using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Statistics.Linq {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// 
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class SampleStatistics<T> : IEnumerable<ItemWithStatistics<T>> {
    #region Private Data

    private readonly IEnumerable<T> m_Source;

    private readonly List<ISampleStatisticsExecutor<T>> m_Executors;

    #endregion Private Data

    #region Algorithm

    private IEnumerable<ItemWithStatistics<T>> Generator() {
      foreach (var item in m_Source) {
        foreach (var executor in m_Executors) 
          executor.Append(item);
        
        yield return new ItemWithStatistics<T>(item, this);
      }
    }

    #endregion Algorithm

    #region Create

    /// <summary>
    /// Standard 
    /// </summary>
    public SampleStatistics(IEnumerable<T> source, IEnumerable<ISampleStatisticsExecutor<T>> executors) {
      m_Source = source ?? throw new ArgumentNullException(nameof(source));

      if (executors is null)
        throw new ArgumentNullException(nameof(executors));

      m_Executors = executors
        .Where(item => item is not null)
        .Distinct()
        .ToList();
    }

    /// <summary>
    /// Standard 
    /// </summary>
    public SampleStatistics(IEnumerable<T> source, params ISampleStatisticsExecutor<T>[] executors)
      : this(source, (IEnumerable<ISampleStatisticsExecutor<T>>)executors) { }

    #endregion Create

    #region Public

    /// <summary>
    /// Statistics
    /// </summary>
    public S Statistics<S>() where S : ISampleStatisticsExecutor<T> => m_Executors
      .FirstOrDefault(item => typeof(S).IsAssignableFrom(item.GetType())) is S result ? result : default;

    #endregion Public

    #region IEnumerable<T>

    /// <summary>
    /// Typed Enumerator
    /// </summary>
    public IEnumerator<ItemWithStatistics<T>> GetEnumerator() => Generator().GetEnumerator();

    /// <summary>
    /// Typeless enumerator
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    #endregion IEnumerable<T>
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// 
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static partial class EnumerableExtensions {
    #region Public

    /// <summary>
    /// To Sample Statistics 
    /// </summary>
    public static SampleStatistics<T> ToSampleStatistics<T>(this IEnumerable<T> source) =>
      new (source);
    
    #endregion Public
  }

}
