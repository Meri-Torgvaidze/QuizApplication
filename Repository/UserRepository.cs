using Models;

namespace Repository
{
    public class UserRepository : FileRepository<User>
    {
        private List<User> _users;

        public UserRepository(string filePath) : base(filePath) { }

        public User? FindByUsername(string username)
        {
            return ReadAll().FirstOrDefault(u => u.Username == username);
        }

        public User? FindById(int id)
        {
            return ReadAll().FirstOrDefault(u => u.Id == id);
        }

        public User RegisterUser(string username, string password)
        {
            if (FindByUsername(username) != null)
            {
                throw new Exception("Username already exists.");
            }

            _users = ReadAll();

            var newUser = new User
            {
                Id = _users.Any() ? _users.Max(customer => customer.Id) + 1 : 1,
                Username = username,
                Password = password
            };

            Save(newUser);
            return newUser;
        }

        public User LoginUser(string username, string password)
        {
            var user = FindByUsername(username);

            if (user == null || user.Password != password)
            {
                throw new Exception("Invalid username or password.");
            }

            return user;
        }

        public List<User> GetTopUsers(int count)
        {
            return ReadAll()
                .OrderByDescending(u => u.Score)
                .Take(count)
                .ToList();
        }

        public void UpdateScore(int points, User user)
        {
            if (points > user.Score)
            {
                _users = ReadAll();
                var index = _users.FindIndex(c => c.Id == user.Id);
                user.Score = points;
                if (index >= 0)
                {
                    _users[index] = user;
                    SaveAll(_users);
                }
            }
        }
    }
}
