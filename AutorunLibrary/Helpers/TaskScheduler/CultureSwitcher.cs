using System;
using System.Threading;

namespace AutorunLibrary.Helpers.TaskScheduler
{
	internal class CultureSwitcher : IDisposable
	{
		private readonly System.Globalization.CultureInfo cur, curUI;

		public CultureSwitcher(System.Globalization.CultureInfo culture)
		{
			cur = Thread.CurrentThread.CurrentCulture;
			curUI = Thread.CurrentThread.CurrentUICulture;
			Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = culture;
		}

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          // TODO: dispose managed state (managed objects).
          Thread.CurrentThread.CurrentCulture = cur;
          Thread.CurrentThread.CurrentUICulture = curUI;
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

        disposedValue = true;
      }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~CultureSwitcher() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(true);
      // TODO: uncomment the following line if the finalizer is overridden above.
      // GC.SuppressFinalize(this);
    }
    #endregion

  }
}
