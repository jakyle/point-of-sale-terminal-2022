namespace Point_of_sale_terminal
{
    public interface IUI
    {
        void DisplayWelcomeMessage();
        int GetValidUserNumberInput(string mainMessage, int maxNumber);
        bool GetValidYesNoInput(string message);
        decimal GetValidUserNumberInput(string mainMessage, decimal minNumber, decimal maxNumber);
        public DateTime GetValidUserDateTimeInput(string mainMessage);
        int GetValidUserNumberInput(string mainMessage, Action printMenuFn, int maxNumber);
        void PrintAndReadKey(string message = "Press any key to return to main menu...");
        void Reset();
        void PrintLineItem(int number, string description);
        void PrintLineItem(int number, int qty, string description, decimal price);
        void DisplayMessage(string message);
    }
}
