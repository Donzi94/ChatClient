namespace ChatClient
{
    public static class SocketManager
    {
        private static SocketIOClient.SocketIO _client;
        private static readonly string Path = "/sys25d";
        private static readonly string EventName = "Deeznuutz";
        private static readonly string JoinEvent = "userJoined";
        private static readonly string LeaveEvent = "userLeft";
        
        public static async Task Connect(string username)
        {
            _client = new SocketIOClient.SocketIO("wss://api.leetcode.se", new SocketIOClient.SocketIOOptions
            {
                Path = Path
            });

            _client.OnConnected += (_,_) => Console.WriteLine("Connected to chat! ");
            _client.OnDisconnected += (_,_) => Console.WriteLine("Disconnected from chat.");
            
            _client.On(EventName, response =>
            {
                var recievedMessage = response.GetValue<string>();
                var time = DateTime.Now.ToString("HH:mm");
                Console.WriteLine($"[{time}] {recievedMessage}");
            });
            
            _client.On(JoinEvent, response =>
            {
                var user = response.GetValue<string>();
                Console.WriteLine($"➡ {user} has joined the chat.");
            });
            
            _client.On(LeaveEvent, response =>
            {
                var user = response.GetValue<string>();
                Console.WriteLine($"⬅ {user} has left the chat.");
            });

            await _client.ConnectAsync();
            await _client.EmitAsync(JoinEvent, username);
            
            await Task.Delay(500);
        }
        
        public static async Task SendMessage(string username, string message)
        {
            var sendMessage = $"{username}: {message}";
            var time = DateTime.Now.ToString("HH:mm");
            await _client.EmitAsync(EventName, sendMessage);
            Console.WriteLine($"[{time}] {sendMessage}");
        }
        
        public static async Task Disconnect(string username)
        {
            await _client.EmitAsync(LeaveEvent, username);
            await _client.DisconnectAsync();
        }
    }
}
