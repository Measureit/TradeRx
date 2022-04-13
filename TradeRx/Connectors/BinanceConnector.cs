using System.Net.WebSockets;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using TradeRx.Responses;

namespace TradeRx.Connectors
{
    internal class BinanceConnector : IConnector
    {
        private readonly Func<Uri, CancellationToken, Task<WebSocket>> _connectionFactory;
        private readonly Subject<ResponseMessage> _messageReceivedSubject = new Subject<ResponseMessage>();
        private IDisposable _messageReceivedSubscription = Disposable.Empty;
        private WebSocket _client;
        public IObservable<ResponseMessage> MessageReceived => _messageReceivedSubject.AsObservable();

        /// <inheritdoc />
        public BinanceConnector(Func<ClientWebSocket> clientFactory = null)
                : this(BindClientFactory(clientFactory))
        {
        }

        public BinanceConnector(Func<Uri, CancellationToken, Task<WebSocket>> connectionFactory)
        {
            _connectionFactory = connectionFactory ?? (async (uri, token) =>
            {
                var client = new ClientWebSocket();
                await client.ConnectAsync(uri, token).ConfigureAwait(false);
                return client;
            });
        }


        public async Task StartAsync(Uri uri, CancellationToken token)
        {
            //https://github.com/Marfusios/websocket-client/blob/master/src/Websocket.Client/WebsocketClient.cs
            _client = await _connectionFactory(uri, token).ConfigureAwait(false);
            CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            _messageReceivedSubscription = CreateStream(_client, cts).Subscribe(_messageReceivedSubject);
        }

        public IObservable<ResponseMessage> CreateStream(WebSocket socket, CancellationTokenSource cts)
        {
            
            return Observable.Create<ResponseMessage>(async observer =>
            {
                var responseBuffer = new byte[1024];
                var offset = 0;
                var packet = 1024;

                while (!cts.IsCancellationRequested)
                {
                    var byteReceive = new ArraySegment<byte>(responseBuffer, offset, packet);
                    var response = await _client.ReceiveAsync(byteReceive, cts.Token);
                    string msg = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);
                    observer.OnNext(ResponseMessage.TextMessage(msg));
                }
                return () => cts.Cancel();
            });
        }

        private static Func<Uri, CancellationToken, Task<WebSocket>> BindClientFactory(Func<ClientWebSocket> clientFactory)
        {
            if (clientFactory == null)
                return null;

            return (async (uri, token) =>
            {
                var client = clientFactory();
                await client.ConnectAsync(uri, token).ConfigureAwait(false);
                return client;
            });
        }

        public void Dispose()
        {
            _client?.Dispose();
            _messageReceivedSubscription.Dispose();
        }
    }
}


