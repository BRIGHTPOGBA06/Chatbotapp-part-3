using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CyberShieldChatbotWPF1
{
    public partial class MainWindow : Window
    {
        private readonly ChatbotEngine _engine;
        private MediaPlayer? _mediaPlayer; 

        public MainWindow()
        {
            InitializeComponent();
            _engine = new ChatbotEngine();

            DisplayAsciiArt();
            PlayVoiceGreeting();

            AppendBotMessage("Hello! I am CyberShield Bot. You can tell me your name, or ask me about passwords, scams, privacy, and phishing!");
        }

        private void DisplayAsciiArt()
        {
            AsciiBox.Text =
@"  ___ _| |_ ___ ___ ___ |_|___| | _| |___ ___ | |_ 
 |  _| . |  _| -_|  _| . | | -_| | . | | . | . ||  _|
 |_| |___|___|___|_| |___|_|___|_|___|_|___|___||_|  ";
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");

                if (File.Exists(soundPath))
                {
                    _mediaPlayer = new MediaPlayer();
                    _mediaPlayer.MediaOpened += (_, _) => _mediaPlayer.Play();
                    _mediaPlayer.MediaFailed += (_, e) =>
                        System.Diagnostics.Debug.WriteLine($"Audio error: {e.ErrorException?.Message ?? "The media file could not be opened."}");
                    _mediaPlayer.Open(new Uri(soundPath, UriKind.Absolute));
                    System.Diagnostics.Debug.WriteLine($"Playing audio via MediaPlayer from: {soundPath}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"greeting.wav not found at: {soundPath}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Audio error: {ex.Message}");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _mediaPlayer?.Stop();
            _mediaPlayer?.Close();
            base.OnClosed(e);
        }

        private void HandleSendMessage()
        {
            string userText = UserInputBox.Text;
            if (string.IsNullOrWhiteSpace(userText)) return;

            AppendUserMessage(userText);
            UserInputBox.Clear();

            string botResponse = _engine.ProcessInput(userText);
            AppendBotMessage(botResponse);
        }

        private void AppendUserMessage(string message)
        {
            ChatDisplay?.AppendText($"You: {message}\n\n");
            ChatScrollViewer?.ScrollToEnd();
        }

        private void AppendBotMessage(string message)
        {
            ChatDisplay?.AppendText($"CyberShield Bot: {message}\n");
            ChatDisplay?.AppendText("--------------------------------------------------\n\n");
            ChatScrollViewer?.ScrollToEnd();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            HandleSendMessage();
        }

        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                HandleSendMessage();
                e.Handled = true;
            }
        }
    }
}
