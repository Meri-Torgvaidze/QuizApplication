using Models;
using Repository;

namespace ConsoleApp
{
    public static class UserActions
    {
        public static void RegisterUser(UserRepository userRepository)
        {
            Console.Clear();
            Console.WriteLine("Register");
            Console.Write("Enter Username: ");
            var username = Console.ReadLine();
            Console.Write("Enter Password: ");
            var password = Console.ReadLine();

            try
            {
                userRepository.RegisterUser(username!, password!);
                Console.WriteLine("Registration successful! Press any key to continue.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadKey();
        }

        public static User LoginUser(UserRepository userRepository)
        {
            Console.Clear();
            Console.WriteLine("Login");
            Console.Write("Enter Username: ");
            var username = Console.ReadLine();
            Console.Write("Enter Password: ");
            var password = Console.ReadLine();

            try
            {
                var user = userRepository.LoginUser(username!, password!);
                Console.WriteLine($"Welcome, {user.Username}! Press any key to continue.");
                Console.ReadKey();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}. Press any key to try again.");
                Console.ReadKey();
                return null;
            }
        }

        public static void ViewLeaderboard(UserRepository userRepository)
        {
            Console.WriteLine("Leaderboard");
            var topUsers = userRepository.GetTopUsers(10);

            foreach (var user in topUsers)
            {
                Console.WriteLine($"{user.Username}: {user.Score} points");
            }
        }
    }

}
