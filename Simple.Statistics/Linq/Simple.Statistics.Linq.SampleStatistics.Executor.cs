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


}
