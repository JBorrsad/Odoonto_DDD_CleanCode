namespace WF.Mimetic.Domain.Models.Routes;

using System;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Core.Semaphores;
using WF.Mimetic.Domain.Interfaces.Routes;

public class CacheExpirationHandler : ICacheExpirationHandler
{
    private readonly ITaskSemaphore _semaphore;
    private DateTime _expirationDate;
    private TimeSpan _expirationTime;

    public CacheExpirationHandler()
    {
        _semaphore = new OnlyOneSemaphore();
        _expirationDate = DateTime.MinValue;
        _expirationTime = TimeSpan.FromMinutes(1);
    }


    public DateTime ExpirationDate => _expirationDate;
    public bool IsExpired => CheckAndUpdate();

    public void SetExpirationTime(TimeSpan time)
    {
        if(time < TimeSpan.FromSeconds(30))
        {
            throw new InvalidValueException("The expiration time cant be less than 30 seconds.");
        }

        _expirationTime = time;
    }

    private bool CheckAndUpdate()
    {
        if(ExpirationDate >= DateTime.Now)
        {
            return false;
        }

        return _semaphore.Run(() => 
        {
            if (ExpirationDate >= DateTime.Now)
            {
                return false;
            }

            _expirationDate = DateTime.Now.Add(_expirationTime);

            return true;
        });
    }
}
