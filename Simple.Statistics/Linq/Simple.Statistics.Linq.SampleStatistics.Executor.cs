using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
