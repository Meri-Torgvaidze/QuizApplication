﻿namespace Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}