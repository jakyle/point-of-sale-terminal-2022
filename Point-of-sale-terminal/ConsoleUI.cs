namespace Point_of_sale_terminal
{
    public class ConsoleUI : IUI
    {

        public void DisplayWelcomeMessage()
        {
            Console.WriteLine("Welcome to the burger joint, please navigate the menu...");
        }

        public void Reset()
        {
            Console.Clear();
        }

        public int GetValidUserNumberInput(string mainMessage, Action printMenuFn, int maxNumber)
        {
            while (true)
            {
                if (printMenuFn != null)
                {
                    printMenuFn();
                }
                Console.WriteLine(mainMessage);
                var userInput = Console.ReadLine();
                if (int.TryParse(userInput, out int menuNumber) && menuNumber >= 1 && menuNumber <= maxNumber)
                {
                    return menuNumber;
                }
                else
                {
                    Console.WriteLine("Invalid input, press any key to continue");
                    Console.ReadKey();
                    Reset();
                }
            }
        }

        public decimal GetValidUserNumberInput(string mainMessage, decimal minNumber, decimal maxNumber)
        {
            while (true)
            {
                Console.WriteLine(mainMessage);
                var userInput = Console.ReadLine();
                if (decimal.TryParse(userInput, out decimal number) && number >= minNumber && number <= maxNumber)
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("Invalid input, press any key to continue");
                    Console.ReadKey();
                    Reset();
                }
            }
        }

        public DateTime GetValidUserDateTimeInput(string mainMessage)
        {
            while (true)
            {
                Console.WriteLine(mainMessage);
                var userInput = Console.ReadLine();
                if (DateTime.TryParse(userInput, out DateTime expirationDate) 
                    && expirationDate.Month > DateTime.Today.Month 
                    && expirationDate.Year >= DateTime.Today.Year)
                    // TODO, actually handle if we have passed the current month to match year
                {
                    return expirationDate;
                }
                else
                {
                    Console.WriteLine("Invalid input, press any key to continue");
                    Console.ReadKey();
                    Reset();
                }
            }
        }

        public decimal GetValidUserNumberInput(string mainMessage, int minNumber, int maxNumber)
        {
            while (true)
            {
                Console.WriteLine(mainMessage);
                var userInput = Console.ReadLine();
                if (decimal.TryParse(userInput, out decimal number) && number >= minNumber && number <= maxNumber)
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("Invalid input, press any key to continue");
                    Console.ReadKey();
                    Reset();
                }
            }
        }

        public int GetValidUserNumberInput(string mainMessage, int maxNumber)
        {
            return GetValidUserNumberInput(mainMessage, null, maxNumber);
        }

        public void PrintLineItem(int colOne, string colTwo)
        {
            Console.WriteLine($"[{colOne}]. {colTwo}");
        }

        public void PrintLineItem(int colOne, int colTwo, string colThree, decimal colFour)
        {
            Console.WriteLine($"[{colOne}]. qty:{colTwo}\tname: {colThree}\ttotal: {colFour,2}");
        }

        public void PrintAndReadKey(string message = "Press any key to return to main menu...")
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Console.ReadKey();
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public bool GetValidYesNoInput(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                var userInput = Console.ReadLine();
                if (userInput.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else if (userInput.Equals("no", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid input, press any key to continue");
                    Console.ReadKey();
                    Reset();
                }
            }
        }
    }
}
