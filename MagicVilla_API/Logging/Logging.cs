namespace MagicVilla_API.Logging
{
    public class Logging : ILogging
    {
        public void Log(string message, string type)
        {
            if (type =="error")
            {
                Console.BackgroundColor= ConsoleColor.Red;
                Console.WriteLine("Error: "+message);
                Console.ForegroundColor= ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor= ConsoleColor.Green;
                Console.WriteLine(message);
            }
        }
    }
}
