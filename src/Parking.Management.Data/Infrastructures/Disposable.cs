﻿namespace Parking.Management.Data.Infrastructures;

public class Disposable: IDisposable
{
    private bool _isDisposed;
    
    ~Disposable()
    {
        Dispose(false);
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    private void Dispose(bool disposing)
    {
        if (!_isDisposed && disposing)
            DisposeCore();

        _isDisposed = true;
    }

    protected virtual void DisposeCore()
    {
    }
}