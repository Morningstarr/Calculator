using Grpc.Net.Client;
using System;
using ClientApp.Processors;
using CalctorClient = CalculatorClient.Calctor.CalctorClient;

namespace ClientApp
{
    public class Program
    {
        private static CalctorClient client;

        private static Parser parser;

        private static Printer printer = new Printer();

        static void Main(string[] args)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:5001");
                client = new CalctorClient(channel);
                parser = new Parser(client);

                StartProcessingExpressions();
            }
            catch (Exception exc)
            {
                if (exc.Message.Contains("StatusCode=\"Unavailable\""))
                {
                    printer.ErrorServerMessage();
                }
                else
                {
                    printer.ExceptionMessage(exc.Message);
                }
            }
        }

        private static void StartProcessingExpressions()
        {
            string expression = string.Empty;
            while (true)
            {
                printer.StartMessage();

                expression = Console.ReadLine();

                if (!parser.SymbolsCheck(expression))
                {
                    printer.ErrorSymbolMessage();
                    continue;
                }

                if (!parser.BracketsCheck(expression))
                {
                    printer.ErrorBracketsMessage();
                    continue;
                }

                try
                {
                    var rep = parser.Parse(expression);
                    printer.ResultMessage(rep.ToString());
                }
                catch (Exception exc)
                {
                    printer.ExceptionMessage(exc.Message);
                    continue;
                }
            }
        }
    }
}
