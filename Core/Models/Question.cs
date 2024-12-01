namespace Core.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public int CorrectAnswerIndex { get; set; }

        public bool IsCorrectAnswer(int answerIndex)
        {
            return answerIndex == CorrectAnswerIndex;
        }
    }
}
