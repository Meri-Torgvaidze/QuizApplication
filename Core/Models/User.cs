﻿namespace Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Score { get; set; } = 0;

        //public void UpdateScore(int points)
        //{
        //    //TODO გაახლდეს ქულა თუ ახლანდელ ქულაზე მაღალია

        //    Score += points;
        //}
    }
}