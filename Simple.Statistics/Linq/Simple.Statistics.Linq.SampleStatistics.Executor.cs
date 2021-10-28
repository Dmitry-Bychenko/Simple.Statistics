namespace Simple.Statistics.Linq {

  //-------------------------------------------------------------------------------------------------------------------
  //  
  /// <summary>
  /// Sample Statistics Executor
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public interface ISampleStatisticsExecutor<T> {
    /// <summary>
    /// Append
    /// </summary>
    void Append(T item);
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Windowed Statistics Executor
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public interface IWindowedStatisticsExecutor<T> : ISampleStatisticsExecutor<T> {
    /// <summary>
    /// Remove
    /// </summary>
    void Remove(T item);
  }

}
