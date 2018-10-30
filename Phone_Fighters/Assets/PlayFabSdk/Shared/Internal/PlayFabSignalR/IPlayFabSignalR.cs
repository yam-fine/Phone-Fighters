using System;

namespace PlayFab.Internal
{
    public interface IPlayFabSignalR
    {
        event Action<string> OnReceived;
        event System.Action OnReconnected;
        event System.Action OnDisconnected;
        event Action<Exception> OnError;

        void Start(string url, string hubName);
        void Stop();

        void Update();
        
        void Invoke(string api, Action resultCallback, params object[] args);
        void Subscribe(string onInvoked, Action<object[]> resultCallback);
    }

}

