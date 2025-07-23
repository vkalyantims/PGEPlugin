using System;

public interface IConnectionHandler
{
    event Action<IGetTotalScenes,bool, ITerminateSession> OnConnected;
}