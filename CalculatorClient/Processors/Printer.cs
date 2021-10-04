using System;

namespace ClientApp.Processors
{
    public class Printer
    {
        public void StartMessage()
        {
            Console.WriteLine("Введите выражение для подсчета:\n");
        }

        public void ErrorSymbolMessage()
        {
            Console.WriteLine("Обнаружен недопустимый символ! Проверьте введенную строку.\n");
        }

        public void ErrorBracketsMessage()
        {
            Console.WriteLine("Проверьте парность скобок в выражении!\n");
        }

        public void ErrorServerMessage()
        {
            Console.WriteLine("Ошибка подключения к серверу! Проверьте запущен ли сервис CalculatorService.\n");
        }

        public void ExceptionMessage(string message)
        {
            Console.WriteLine(message + "\n");
        }

        public void ResultMessage(string result)
        {
            Console.WriteLine(result + "\n");
        }
    }
}
