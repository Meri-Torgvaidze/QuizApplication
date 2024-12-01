using Models;
using System.Security.Principal;

namespace Repository
{
    public class QuizRepository : FileRepository<Quiz>
    {
        private List<Quiz> _quizzes;
        public QuizRepository(string filePath) : base(filePath) { }

        public Quiz? FindById(int id)
        {
            return ReadAll().FirstOrDefault(q => q.Id == id);
        }

        public List<Quiz> FindByAuthorId(int authorId)
        {
            return ReadAll().Where(q => q.AuthorId == authorId).ToList();
        }

        public void CreateQuiz(string title, int authorId, List<Question> questions)
        {
            _quizzes = ReadAll();

            var newQuiz = new Quiz
            {
                Id = _quizzes.Any() ? _quizzes.Max(customer => customer.Id) + 1 : 1,
                Title = title,
                AuthorId = authorId,
                Questions = questions
            };

            Save(newQuiz);
        }

        public List<Quiz> GetAllQuizzes(int currentUserId)
        {
            return ReadAll()
                .Where(q => q.AuthorId != currentUserId)
                .ToList();
        }

        public void Delete(int quizId)
        {
            _quizzes = ReadAll();
            var quizToDelete = _quizzes.FirstOrDefault(q => q.Id == quizId);

            if (quizToDelete != null)
            {
                _quizzes.Remove(quizToDelete);
                SaveAll(_quizzes);
            }
        }

        public void Update(Quiz updatedQuiz)
        {
            _quizzes = ReadAll();
            var index = _quizzes.FindIndex(q => q.Id == updatedQuiz.Id);
            if (index >= 0)
            {
                _quizzes[index] = updatedQuiz;
                SaveAll(_quizzes);
            }
        }

    }
}
