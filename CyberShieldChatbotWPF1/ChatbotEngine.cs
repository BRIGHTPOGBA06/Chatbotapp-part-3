using System;
using System.Collections.Generic;

namespace CyberShieldChatbotWPF1
{
    public delegate string SentimentResponseHandler(string sentiment, string topicTip);

    public class ChatbotEngine
    {
        private Dictionary<string, List<string>> _knowledgeBase = null!;
        private Dictionary<string, List<string>> _sentimentPhrases = null!;
        private Random _random;

        private TaskManager _taskManager;
        private List<string> _activityLog;
        private QuizManager _quizManager;

        public string UserName { get; private set; } = string.Empty;
        public string LastDiscussedTopic { get; private set; } = string.Empty;

        public event SentimentResponseHandler? OnSentimentDetected;

        public ChatbotEngine()
        {
            _random = new Random();
            _taskManager = new TaskManager();
            _activityLog = new List<string>();
            _quizManager = new QuizManager();

            InitializeKnowledgeBase();
            InitializeSentimentPhrases();

            OnSentimentDetected += FormatSentimentResponse;
        }

        private void InitializeKnowledgeBase()
        {
            _knowledgeBase = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "password", new List<string>
                    {
                        "Always use a unique password with at least 12 characters combining letters, numbers, and symbols.",
                        "Consider using a password manager so you don't have to write down or repeat credentials.",
                        "Avoid using personal details like birthdays or pet names in your passwords."
                    }
                },
                {
                    "scam", new List<string>
                    {
                        "Be cautious of urgent requests for money or personal details—scammers create fake urgency.",
                        "Always double-check link URLs and email sender addresses before taking action.",
                        "If an investment or prize offer sounds too good to be true, it is almost certainly a scam."
                    }
                },
                {
                    "privacy", new List<string>
                    {
                        "Review app permissions on your phone to restrict access to your location and contacts.",
                        "Adjust social media privacy settings so only trusted friends can view your posts.",
                        "Avoid using public Wi-Fi networks for banking or sensitive transactions without a VPN."
                    }
                },
                {
                    "phishing", new List<string>
                    {
                        "Phishing emails often mimic official logos. Always inspect the actual sender email domain.",
                        "Never download attachments from unknown or suspicious emails.",
                        "If you receive a suspicious message from a known contact, verify with them through a separate channel."
                    }
                }
            };
        }

        private void InitializeSentimentPhrases()
        {
            _sentimentPhrases = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "worried", new List<string>
                    {
                        "It's completely normal to feel worried about online security. Scammers can be very convincing.",
                        "Don't worry! Taking small proactive steps makes a huge difference in keeping you safe."
                    }
                },
                {
                    "frustrated", new List<string>
                    {
                        "I understand how frustrating tech security can feel. Let's take it step by step.",
                        "Cybersecurity can feel overwhelming at times, but I am here to make it simple for you."
                    }
                },
                {
                    "curious", new List<string>
                    {
                        "That's a great mindset! Being curious is the best way to stay ahead of cyber threats.",
                        "I love your curiosity! Learning about security gives you control over your digital safety."
                    }
                }
            };
        }

        private string FormatSentimentResponse(string sentiment, string topicTip)
        {
            if (!_sentimentPhrases.TryGetValue(sentiment, out var empatheticList) || empatheticList.Count == 0)
            {
                empatheticList = new List<string> { "I completely understand how you feel." };
            }

            string empathyStatement = empatheticList[_random.Next(empatheticList.Count)];
            return $"{empathyStatement}\n\nHere is a tip to help: {topicTip}";
        }

        public string ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please type a message so I can assist you.";

            string cleanInput = input.Trim().ToLowerInvariant();

            if (cleanInput.Contains("security logs") || cleanInput.Contains("review logs"))
            {
                return "Here are your recent security logs...";
            }

            if (_quizManager.IsQuizActive)
            {
                if (cleanInput.Contains("stop quiz") || cleanInput.Contains("quit") || cleanInput.Contains("exit"))
                {
                    return "Quiz canceled. Type 'start quiz' whenever you're ready to try again!";
                }

                string quizResponse = _quizManager.ProcessAnswer(input);
                LogActivity("Participated in cybersecurity quiz");
                return quizResponse;
            }

            if (cleanInput.Contains("start quiz") || cleanInput.Contains("play quiz") || (cleanInput.Contains("quiz") && cleanInput.Contains("start")))
            {
                LogActivity("Started cybersecurity quiz");
                return _quizManager.StartQuiz();
            }

            if (cleanInput.StartsWith("my name is "))
            {
                UserName = input.Substring(11).Trim();
                LogActivity($"User registered name: {UserName}");
                return $"Got it! Nice to meet you, {UserName}. I'll remember your name during our chat.";
            }

            string detectedSentiment = DetectSentiment(cleanInput);
            if (!string.IsNullOrEmpty(detectedSentiment))
            {
                string fallbackTopic = !string.IsNullOrEmpty(LastDiscussedTopic) ? LastDiscussedTopic : "scam";
                string tip = GetRandomTipForTopic(fallbackTopic);
                LogActivity($"Sentiment detected: {detectedSentiment}");
                return OnSentimentDetected?.Invoke(detectedSentiment, tip) ?? string.Empty;
            }

            if (cleanInput.Contains("add task") || cleanInput.Contains("remind me") || (cleanInput.Contains("task") && cleanInput.StartsWith("add")))
            {
                string taskTitle = input;
                taskTitle = taskTitle.Replace("add a task to", "", StringComparison.OrdinalIgnoreCase)
                                     .Replace("add task", "", StringComparison.OrdinalIgnoreCase)
                                     .Replace("remind me to", "", StringComparison.OrdinalIgnoreCase)
                                     .Replace("task", "", StringComparison.OrdinalIgnoreCase)
                                     .Trim();

                if (string.IsNullOrEmpty(taskTitle)) taskTitle = "General Cybersecurity Task";

                bool success = _taskManager.AddTask(taskTitle, "Created via chatbot assistant.", "No reminder set");

                if (success)
                {
                    LogActivity($"Task added: '{taskTitle}'");
                    string greetingPart = !string.IsNullOrEmpty(UserName) ? $"{UserName}, " : "";
                    return $"{greetingPart}Task added with the description \"{taskTitle}\" to ensure your data is protected. Would you like a reminder?";
                }
                else
                {
                    return "I tried to save that task to the database, but encountered a connection error. Please verify your MySQL server is running.";
                }
            }

            if (cleanInput.Contains("show activity log") || cleanInput.Contains("what have you done for me") || cleanInput.Contains("activity log"))
            {
                LogActivity("Viewed activity log");
                if (_activityLog.Count == 0)
                {
                    return "Here’s a summary of recent actions:\nNo actions recorded yet.";
                }

                string summary = "Here’s a summary of recent actions:\n";
                int count = 1;

                int startIndex = Math.Max(0, _activityLog.Count - 8);
                for (int i = startIndex; i < _activityLog.Count; i++)
                {
                    summary += $"{count}. {_activityLog[i]}\n";
                    count++;
                }
                return summary.TrimEnd();
            }

            if (cleanInput.Contains("tell me more") || cleanInput.Contains("explain more") || cleanInput.Contains("another tip"))
            {
                if (!string.IsNullOrEmpty(LastDiscussedTopic))
                {
                    string personalizedPrefix = !string.IsNullOrEmpty(UserName)
                        ? $"{UserName}, as someone interested in {LastDiscussedTopic}, here is another key tip:\n"
                        : $"Here is another tip regarding {LastDiscussedTopic}:\n";

                    LogActivity($"Requested another tip for topic: {LastDiscussedTopic}");
                    return personalizedPrefix + GetRandomTipForTopic(LastDiscussedTopic);
                }
                return "We haven't picked a specific topic yet! Ask me about passwords, scams, privacy, or phishing.";
            }

            foreach (var key in _knowledgeBase.Keys)
            {
                if (cleanInput.Contains(key))
                {
                    LastDiscussedTopic = key;
                    string personalizedIntro = !string.IsNullOrEmpty(UserName)
                        ? $"Thanks for asking, {UserName}! "
                        : "";

                    LogActivity($"Discussed topic: {key}");
                    return $"{personalizedIntro}{GetRandomTipForTopic(key)}";
                }
            }

            if (cleanInput.Contains("what is my name") || cleanInput.Contains("who am i"))
            {
                return !string.IsNullOrEmpty(UserName)
                    ? $"Your name is {UserName}!"
                    : "You haven't told me your name yet! You can say 'My name is [your name]'.";
            }

            return "I'm not sure I quite understood that. You can ask me to add a task, show your activity log, or ask about passwords, scams, privacy, and phishing.";
        }

        private void LogActivity(string action)
        {
            string timestampEntry = $"{action} ({DateTime.Now:yyyy-MM-dd HH:mm})";
            _activityLog.Add(timestampEntry);
        }

        private string GetRandomTipForTopic(string topic)
        {
            if (_knowledgeBase.TryGetValue(topic, out var tips) && tips.Count > 0)
            {
                return tips[_random.Next(tips.Count)];
            }
            return string.Empty;
        }

        private string DetectSentiment(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("nervous") || input.Contains("anxious"))
                return "worried";
            if (input.Contains("frustrated") || input.Contains("confused") || input.Contains("annoyed"))
                return "frustrated";
            if (input.Contains("curious") || input.Contains("interested") || input.Contains("eager"))
                return "curious";

            return string.Empty;
        }
    }
}