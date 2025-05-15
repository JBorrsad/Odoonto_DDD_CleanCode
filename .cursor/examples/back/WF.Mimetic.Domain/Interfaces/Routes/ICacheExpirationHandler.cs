namespace WF.Mimetic.Domain.Interfaces.Routes;

using System;

public interface ICacheExpirationHandler
{
    DateTime ExpirationDate { get; }
    bool IsExpired { get; }

    void SetExpirationTime(TimeSpan time);
}
