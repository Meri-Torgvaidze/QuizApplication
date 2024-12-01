using Models;
using Repository;

namespace ConsoleApp
{
    public static class QuizActions
    {
        public static void CreateQuiz(QuizRepository quizRepository, User currentUser)
        {
            Console.Clear();
            Console.WriteLine("Create a Quiz");
            Console.Write("Enter Quiz Title: ");
            var title = Console.ReadLine();

            var questions = new List<Question>();
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"Enter Question {i}:");
                var questionText = Console.ReadLine();

                var answers = new List<Answer>();
                for (int j = 1; j <= 4; j++)
                {
                    Console.Write($"Answer {j}: ");
                    answers.Add(new Answer {Id = j, Text = Console.ReadLine() });
                }

                Console.Write("Enter the correct answer number (1-4): ");
                var correctAnswerIndex = int.Parse(Console.ReadLine()!) - 1;

                questions.Add(new Question
                {
                    Id = i,
                    Text = questionText!,
                    Answers = answers,
                    CorrectAnswerIndex = correctAnswerIndex
                });
            }

            quizRepository.CreateQuiz(title!, currentUser.Id, questions);
            Console.WriteLine("Quiz created successfully! Press any key to continue.");
            Console.ReadKey();
        }

        public static void SolveQuiz(QuizRepository quizRepository, User currentUser, UserRepository userRepository)
        {
            Console.Clear();
            Console.WriteLine("Available Quizzes");
            var quizzes = quizRepository.GetAllQuizzes(currentUser.Id);

            if (quizzes.Count == 0)
            {
                Console.WriteLine("No quizzes available to solve. Press any key to return.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < quizzes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {quizzes[i].Title}");
            }

            Console.Write("Select a Quiz to Solve: ");
            if (!int.TryParse(Console.ReadLine(), out int quizIndex) || quizIndex < 1 || quizIndex > quizzes.Count)
            {
                Console.WriteLine("Invalid choice. Press any key to return.");
                Console.ReadKey();
                return;
            }

            var selectedQuiz = quizzes[quizIndex - 1];


            Console.Clear();
            Console.WriteLine($"Solving Quiz: {selectedQuiz.Title}");

            int score = 0;
            var stopwatch = new System.Diagnostics.Stopwatch(); 

            stopwatch.Start();

            foreach (var question in selectedQuiz.Questions)
            {
                if (stopwatch.Elapsed.TotalSeconds > 120)
                {
                    Console.WriteLine("\nTime's up! You cannot continue.");
                    break;
                }

                Console.WriteLine(question.Text);
                for (int i = 0; i < question.Answers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {question.Answers[i].Text}");
                }

                Console.Write("Your answer: ");
                if (!int.TryParse(Console.ReadLine(), out int answerIndex) || answerIndex < 1 || answerIndex > question.Answers.Count)
                {
                    Console.WriteLine("Invalid choice. Press any key to return.");
                    continue;
                }

                if (question.IsCorrectAnswer(answerIndex - 1))
                {
                    score += 20;
                }
                else
                {
                    score -= 20;
                }
            }

            stopwatch.Stop();

            if (stopwatch.Elapsed.TotalSeconds > 120)
            {
                Console.WriteLine("Time is out! No score will be updated. Press any key to continue.");
            }
            else
            {
                userRepository.UpdateScore(score, currentUser);
                Console.WriteLine($"Your score: {score}. Press any key to continue.");
            }

            Console.ReadKey();
        }

        public static void UpdateQuiz(QuizRepository quizRepository, User currentUser)
        {
            Console.Clear();
            Console.WriteLine("Update My Quizzes");

            var userQuizzes = quizRepository.FindByAuthorId(currentUser.Id);
            if (!userQuizzes.Any())
            {
                Console.WriteLine("You have no quizzes to update. Press any key to return.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < userQuizzes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userQuizzes[i].Title}");
            }

            Console.Write("Select a quiz to update: ");
            if (!int.TryParse(Console.ReadLine(), out int quizIndex) || quizIndex < 1 || quizIndex > userQuizzes.Count)
            {
                Console.WriteLine("Invalid choice. Press any key to return.");
                Console.ReadKey();
                return;
            }

            var selectedQuiz = userQuizzes[quizIndex - 1];

            Console.WriteLine($"\nUpdating Quiz: {selectedQuiz.Title}");
            Console.Write("Enter new title (or press Enter to keep current): ");
            var newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                selectedQuiz.Title = newTitle;
            }

            foreach (var question in selectedQuiz.Questions)
            {
                Console.WriteLine($"\nQuestion {question.Id}");
                Console.WriteLine($"Current Text: {question.Text}");
                Console.Write("Enter new text (or press Enter to keep current): ");
                var newQuestionText = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newQuestionText))
                {
                    question.Text = newQuestionText;
                }

                for (int i = 0; i < question.Answers.Count; i++)
                {
                    Console.WriteLine($"Answer {i + 1}: {question.Answers[i].Text}");
                    Console.Write("Enter new answer text (or press Enter to keep current): ");
                    var newAnswerText = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newAnswerText))
                    {
                        question.Answers[i].Text = newAnswerText;
                    }
                }

                Console.WriteLine($"Current Correct Answer: {question.CorrectAnswerIndex + 1}");
                Console.Write("Enter new correct answer index (1-4, or press Enter to keep current): ");
                var newCorrectAnswerInput = Console.ReadLine();
                if (int.TryParse(newCorrectAnswerInput, out int newCorrectAnswerIndex) && newCorrectAnswerIndex >= 1 && newCorrectAnswerIndex <= question.Answers.Count)
                {
                    question.CorrectAnswerIndex = newCorrectAnswerIndex - 1;
                }
            }

            quizRepository.Update(selectedQuiz);
            Console.WriteLine("\nQuiz updated successfully! Press any key to continue.");
            Console.ReadKey();
        }


        public static void DeleteQuiz(QuizRepository quizRepository, User currentUser)
        {
            Console.Clear();
            Console.WriteLine("Delete My Quizzes");

            var userQuizzes = quizRepository.FindByAuthorId(currentUser.Id);
            if (!userQuizzes.Any())
            {
                Console.WriteLine("You have no quizzes to delete. Press any key to return.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < userQuizzes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userQuizzes[i].Title}");
            }

            Console.Write("Select a quiz to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int quizIndex) || quizIndex < 1 || quizIndex > userQuizzes.Count)
            {
                Console.WriteLine("Invalid choice. Press any key to return.");
                Console.ReadKey();
                return;
            }

            var selectedQuiz = userQuizzes[quizIndex - 1];

            Console.Write($"Are you sure you want to delete \"{selectedQuiz.Title}\"? (y/n): ");
            var confirmation = Console.ReadLine();
            if (confirmation?.ToLower() == "y")
            {
                quizRepository.Delete(selectedQuiz.Id);
                Console.WriteLine("Quiz deleted successfully! Press any key to continue.");
            }
            else
            {
                Console.WriteLine("Deletion canceled. Press any key to return.");
            }

            Console.ReadKey();
        }

    }
}
