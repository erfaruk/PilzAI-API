using System.Text.RegularExpressions;

namespace PilzGPTArge_V0.Helper
{
    public class CreateTtitle
    {
        public static string GenerateSummaryTitle(string text, int numberOfWords = 4)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Text cannot be null or empty", nameof(text));
            }

            // Remove punctuation and split text into words
            var words = Regex.Replace(text.ToLower(), @"[^\w\s]", "").Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0)
            {
                return string.Empty;
            }

            // Count the frequency of each word
            var wordFrequency = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (wordFrequency.ContainsKey(word))
                {
                    wordFrequency[word]++;
                }
                else
                {
                    wordFrequency[word] = 1;
                }
            }

            // Sort words by frequency and take the top 'numberOfWords' words
            var mostFrequentWords = wordFrequency
                .OrderByDescending(w => w.Value)
                .Take(Math.Min(numberOfWords, wordFrequency.Count))
                .Select(w => w.Key);

            // Join the top words to form a title
            var title = string.Join(" ", mostFrequentWords);

            return title;
        }
    }
}
