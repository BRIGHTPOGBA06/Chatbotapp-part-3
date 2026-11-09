using System;
using System.Collections.Generic;

namespace CyberShieldChatbotWPF1
{
    public class QuizQuestion
    {
        public string QuestionText { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
        public string CorrectAnswer { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
    }

    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex = 0;
        private int _score = 0;
        private bool _isQuizActive = false;

        public QuizManager()
        {
            _questions = new List<QuizQuestion>();
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            // 10+ Cybersecurity Questions covering phishing, passwords, safe browsing, social engineering[cite: 5]
            _questions.Add(new QuizQuestion
            {
                QuestionText = "What should you do if you receive an email asking for your password?",
                Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                CorrectAnswer = "C",
                Explanation = "Reporting phishing emails helps prevent scams and protects other users."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "True or False: Using the same password across multiple websites is considered safe practice.",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "If one site is breached, credential stuffing can compromise all your other accounts."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "Which of the following creates a strong, secure password?",
                Options = new List<string> { "A) Your pet's name and birth year", "B) A mix of 12+ random upper/lowercase letters, numbers, and symbols", "C) 'Password123'", "D) Your phone number" },
                CorrectAnswer = "B",
                Explanation = "Complexity and length make brute-force attacks computationally infeasible."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "What does 'HTTPS' in a website URL signify?",
                Options = new List<string> { "A) The site loads faster", "B) The connection between your browser and the server is encrypted", "C) The website is completely immune to malware", "D) It is government-approved" },
                CorrectAnswer = "B",
                Explanation = "HTTPS encrypts data in transit, preventing eavesdroppers from reading sensitive info."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "True or False: Social engineering relies on human psychology rather than software vulnerabilities.",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "True",
                Explanation = "Attackers manipulate human trust rather than breaking technical code."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "What is Two-Factor Authentication (2FA)?",
                Options = new List<string> { "A) Logging in twice", "B) Using two different passwords", "C) Requiring a second form of verification alongside your password", "D) Having two email accounts" },
                CorrectAnswer = "C",
                Explanation = "2FA adds a critical security layer even if your password is stolen."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "True or False: Public Wi-Fi networks are generally secure for online banking.",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "Public networks can be intercepted by malicious actors; a VPN should be used."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "Which of these is a classic sign of a scam message?",
                Options = new List<string> { "A) Calm instructions", "B) Urgent threats or promises of sudden wealth", "C) Normal newsletter updates", "D) Official receipts" },
                CorrectAnswer = "B",
                Explanation = "Scammers create fake urgency or greed to short-circuit your critical thinking."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "True or False: You should regularly review app permissions on your mobile device.",
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "True",
                Explanation = "Restricting unneeded location or contact access prevents unauthorized data harvesting."
            });

            _questions.Add(new QuizQuestion
            {
                QuestionText = "What is 'Spear Phishing'?",
                Options = new List<string> { "A) Fishing with a spear", "B) A targeted phishing attack tailored to a specific individual or organization", "C) Random spam emails sent to millions", "D) A type of firewall error" },
                CorrectAnswer = "B",
                Explanation = "Spear phishing uses personalized details to appear extremely authentic and trustworthy."
            });
        }

        public bool IsQuizActive => _isQuizActive;

        public string StartQuiz()
        {
            _isQuizActive = true;
            _currentIndex = 0;
            _score = 0;
            return "Starting the Cybersecurity Quiz! Test your knowledge.\n\n" + GetCurrentQuestionFormatted();
        }

        public string ProcessAnswer(string playerAnswer)
        {
            if (!_isQuizActive) return "No active quiz. Type 'start quiz' to begin!";

            var currentQ = _questions[_currentIndex];
            string cleanedInput = playerAnswer.Trim().ToUpper();

            // Check correctness (handles letter choices like 'A', 'B', 'C', 'D' or True/False text)
            bool isCorrect = cleanedInput.Contains(currentQ.CorrectAnswer.ToUpper()) || cleanedInput == currentQ.CorrectAnswer.ToUpper();

            string feedback = "";
            if (isCorrect)
            {
                _score++;
                feedback = $"Correct! {currentQ.Explanation}[cite: 5]";
            }
            else
            {
                feedback = $"Incorrect. The correct answer was {currentQ.CorrectAnswer}. {currentQ.Explanation}[cite: 5]";
            }

            _currentIndex++;

            if (_currentIndex >= _questions.Count)
            {
                _isQuizActive = false;
                string finalMsg = feedback + $"\n\nQuiz complete! Your final score: {_score}/{_questions.Count}.\n";

                if (_score >= 8)
                    finalMsg += "Great job! You're a cybersecurity pro![cite: 5]";
                else
                    finalMsg += "Keep learning to stay safe online![cite: 5]";

                return finalMsg;
            }

            return feedback + "\n\n--- Next Question ---\n" + GetCurrentQuestionFormatted();
        }

        private string GetCurrentQuestionFormatted()
        {
            var q = _questions[_currentIndex];
            string formatted = $"Question {_currentIndex + 1} of {_questions.Count}:\n{q.QuestionText}\n";
            foreach (var opt in q.Options)
            {
                formatted += $"{opt}\n";
            }
            return formatted.TrimEnd();
        }
    }
}