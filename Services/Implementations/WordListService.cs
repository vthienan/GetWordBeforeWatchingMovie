using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GetWordBeforeWatchingMovie.Core;
using GetWordBeforeWatchingMovie.Services.Interfaces;
using Topdev.OpenSubtitles.Client;

namespace GetWordBeforeWatchingMovie.Services.Implementations
{
    public class WordListService: IWordListServices
    {
        public async Task<List<string>> GetWordLists(string imdbId, Level userEnglishLevel)
        {
            var subtitleList = await GetSubtitlesArray(imdbId);

            switch (subtitleList.Length)
            {
                case 0:
                    Console.WriteLine("Found no movie subtitles");
                    throw new ApplicationException("Cannot find the subtitle for the movie!");
                case >= 1:
                    Console.WriteLine($"Found {subtitleList.Length} movie subtitle(s) ");
                    break;
            }

            var client = new OpenSubtitlesClient();
            string movieName = subtitleList[0].MovieName;
            try
            {
                await client.DownloadSubtitleAsync(subtitleList[0], $"./Core/tempSubFiles/{movieName}.srt");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            var lines = await System.IO.File.ReadAllLinesAsync($"./Core/tempSubFiles/{movieName}.srt");
            var words = WordProcessor.BuildWords(lines);
           
            var wordsToLearn = DictionaryHandler.GetInstance().GetWordsToLearn(userEnglishLevel,words);
            
            // delete the temporary file
            System.IO.File.Delete($"./Core/tempSubFiles/{movieName}.srt");
            return wordsToLearn;
        }
        
        private async Task<Subtitles[]> GetSubtitlesArray(string imdbId)
        {
            try
            {
                OpenSubtitlesClient client = new OpenSubtitlesClient();

                await client.LogInAsync("eng", "GetHardWordFromMovie v0.1", "vitconkute", "Vothien@123an");

                var actualId = imdbId;
                if (imdbId.StartsWith("tt"))
                {
                    actualId = imdbId.Remove(0, 2);
                }

                var foundList = await client.FindSubtitlesAsync(SearchMethod.IMDBId, actualId, "eng");

                return foundList;
            }
            catch (Exception exception)
            {
                return Array.Empty<Subtitles>();
            }
        }
    }
}