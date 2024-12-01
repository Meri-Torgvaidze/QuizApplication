using Repository;
using Models;

namespace ConsoleApp
{
    public class Program
    {
        private static User _currentUser;
        private static UserRepository _userRepository;
        private static QuizRepository _quizRepository;

        static void Main(string[] args)
        {
            _userRepository = new UserRepository(@"../../../../Repository/Data/Users.json");
            _quizRepository = new QuizRepository(@"../../../../Repository/Data/Quizzes.json");

            ShowMainMenu();
        }

        private static void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                UserActions.ViewLeaderboard(_userRepository);
                Console.WriteLine();
                Console.WriteLine("Quiz Application");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        UserActions.RegisterUser(_userRepository);
                        break;
                    case "2":
                        _currentUser = UserActions.LoginUser(_userRepository);
                        if (_currentUser != null)
                        {
                            ShowUserMenu();
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void ShowUserMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Welcome, {_currentUser!.Username}!");
                Console.WriteLine("1. Create a Quiz");
                Console.WriteLine("2. Solve a Quiz");
                Console.WriteLine("3. Update my Quiz");
                Console.WriteLine("4. Delete my Quiz");
                Console.WriteLine("5. Logout");
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        QuizActions.CreateQuiz(_quizRepository, _currentUser);
                        break;
                    case "2":
                        QuizActions.SolveQuiz(_quizRepository, _currentUser, _userRepository);
                        break;
                    case "3":
                        QuizActions.UpdateQuiz(_quizRepository, _currentUser);
                        break;
                    case "4":
                        QuizActions.DeleteQuiz(_quizRepository, _currentUser);
                        break;
                    case "5":
                        _currentUser = null;
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

    }
}
