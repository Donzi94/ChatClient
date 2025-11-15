namespace ChatClient
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine();
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            while (string.IsNullOrEmpty(username))
            {
                Console.Write("Username cannot be empty. Enter again: ");
                username = Console.ReadLine();
            }
            
            await SocketManager.Connect(username);

            Console.WriteLine("Type messages (type 'exit' to leave)");
            
            while (true)
            {
                Console.Write("> ");
                string message = Console.ReadLine();
                if (string.IsNullOrEmpty(message)) continue;

                if (message.ToLower() == "exit")
                {
                    await SocketManager.Disconnect(username);
                    Console.WriteLine("Chat is closed. Goodbye!");
                    break;
                }

                await SocketManager.SendMessage(username, message);
            }
        }
    }
}