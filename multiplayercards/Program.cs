using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace multiplayercards
{

    public class Program
    {

        static void Main(string[] args)
        {
            FileInputOutputSystem fileInputOutputSystem = new FileInputOutputSystem(Directory.GetCurrentDirectory());
            string inputfilepath = "";
            string outputfilepath = "";

            try
            {
                ValidateInputParamters validateInput = new ValidateInputParamters();

                var parameters = validateInput.GetInputOutputFiles(args);

                inputfilepath = fileInputOutputSystem.GetFilePath(parameters.input);

                var fileData = fileInputOutputSystem.ReadInputFile(inputfilepath);

                outputfilepath = fileInputOutputSystem.GetOutPutFilePath(parameters.output);

                LoadPlayer loadPlayer = new LoadPlayer(fileData);

                ProcessResults processResults = new ProcessResults();

                List<Player> winners = processResults.CalculateHighestScore(loadPlayer.playersList);

                if (winners.Count > 1)
                {
                    winners = processResults.RecalculateTiedPlayers(winners);
                }

                fileInputOutputSystem.WriteOutputFile(outputfilepath, string.Join(",", winners.Select(x => x.playerName)) + ":" + winners.Max(x => x.playerScore));

            }
            catch(Exception ex)
            {
                if(string.IsNullOrEmpty(outputfilepath))
                {
                    outputfilepath = Path.Combine(Directory.GetCurrentDirectory(), "error.txt");
                    fileInputOutputSystem.WriteOutputFile(outputfilepath,ex.Message);

                }
                else 
                {
                    fileInputOutputSystem.WriteOutputFile(outputfilepath, ex.Message);
                }
            }
           

        }

    }


    public class Player
    {
        public int Id { get; set; }
        public string playerName { get; set; }
        public string[] playerCards { get; set; }
        public double playerScore { get; set; }
    }

    public class CardsNumbers
    {
        public string card { get; set; }
        public int value { get; set; }
    }

    public class SuitNumber
    {
        public string suit { get; set; }
        public int value { get; set; }
    }

    public class InputOutputParameters
    {
        public string input { get; set; }
        public string output { get; set; }
    }

    public class ValidateInputParamters
    {
        public InputOutputParameters GetInputOutputFiles(string[] parameters)
        {
            InputOutputParameters inputOutputFiles = new InputOutputParameters();
            if(parameters.Length == 4)
            {
                var indexOfInput = Array.FindIndex(parameters, row => row == "--in");
                if(indexOfInput >= 0)
                {
                    inputOutputFiles.input = parameters[indexOfInput + 1];
                }
                else
                {
                    throw new Exception($"Exception: Command parameters --in is incorrect.");

                }


                var indexOfOutput = Array.FindIndex(parameters, row => row == "--out");
                if (indexOfOutput >= 0)
                {
                    inputOutputFiles.output = parameters[indexOfOutput + 1];
                }
                else
                {
                    throw new Exception($"Exception: Command parameters --out is incorrect.");

                }

            }
            else
            {
                throw new Exception($"Exception: Command parameters are required to produce an output.");
            }


            return inputOutputFiles;
        }

    }

    public class FileSystem
    {
        public string directoryPath { get; set; }

        public FileSystem(string directory)
        {
            directoryPath = directory;
        }

        public bool FileExistInPath(string path)
        {
            return File.Exists(path);
        }


        public string GetFilePath(string filename)
        {

            if (FileExistInPath(Path.Combine(directoryPath, filename)))
            {
                return Path.Combine(directoryPath, filename);
            }
            else if (FileExistInPath(filename))
            {
                return filename;
            }
            else
            {
                throw new Exception($"Exception: File Not Found.  {filename}");
            }
        }


        public string[] ReadInputFile(string path)
        {
            string[] data = File.ReadAllLines(path);
            if (data.Length == 0)
            {
                throw new Exception($"Exception: File is empty.");
            }
            return data;

        }



    }

    public class FileInputOutputSystem :FileSystem
    {
        public FileInputOutputSystem(string directory) : base(directory)
        {
        }

        public string GetOutPutFilePath(string filename)
        {
         
             return Path.Combine(directoryPath, filename);

        }

        public void WriteOutputFile(string path, string text)
        {
            if (FileExistInPath(path) == true)
            {

                File.Delete(path);

            }
            File.AppendAllText(path, text+ Environment.NewLine);

        }

    }

    public class LoadPlayer
    {

        public List<Player> playersList { get; set; }

        public LoadPlayer(string[] data)
        {
           
            playersList = new List<Player>();
            int id = 0;
            foreach (var playerData in data )
            {
                
                var splitUserCard = playerData.Split(':');
                if(splitUserCard.Length == 2)
                {
                    if(!splitUserCard[1].Contains(","))
                    {
                        throw new Exception($"Exception: Data is incorrect  {playerData} ");

                    }
                    var scores  = splitUserCard[1].Split(',');
                    if(scores.Length > 0)
                    {
                        id++;
                        var player = new  Player();
                        player.playerName = splitUserCard[0];
                        player.playerCards = new string[scores.Length];
                        player.playerCards = scores;
                        player.Id = id;
                        CalculateScore calculateScore = new CalculateScore(null,null);
                        player.playerScore = calculateScore.CalculatePlayerScore(scores);
                        playersList.Add(
                            player);

                    }
                    else
                    {
                        throw new Exception($"Exception: Incorrect format. {scores}");

                    }
                }
                else
                {
                    throw new Exception($"Exception: Incorrect format. {playerData}");

                }
            }
        }

    }

    public class CalculateScore
    {
        public List<CardsNumbers> cardsNumbers = new List<CardsNumbers>()
            {
                new CardsNumbers() { card = "A", value = 11 },
                new CardsNumbers() { card = "2", value = 2 },
                new CardsNumbers() { card = "3", value = 3 },
                new CardsNumbers() { card = "4", value = 4 },
                new CardsNumbers() { card = "5", value = 5 },
                new CardsNumbers() { card = "6", value = 6 },
                new CardsNumbers() { card = "7", value = 7 },
                new CardsNumbers() { card = "8", value = 8 },
                new CardsNumbers() { card = "9", value = 9 },
                new CardsNumbers() { card = "10",value = 10 },
                new CardsNumbers() { card = "J", value = 11 },
                new CardsNumbers() { card = "Q", value = 12 },
                new CardsNumbers() { card = "K", value = 13 },
            };


        public List<SuitNumber> suitsValues = new List<SuitNumber>()
            {
                new SuitNumber() { suit = "H", value = 1 },
                new SuitNumber() { suit = "S", value = 2 },
                new SuitNumber() { suit = "C", value = 3 },
                new SuitNumber() { suit = "D", value = 4 },

            };

        public CalculateScore(List<CardsNumbers> cardsNumber, List<SuitNumber> suitsValue)
        {
            if(cardsNumber != null)
            {
                cardsNumbers = cardsNumber.ToList();
            }

            if (suitsValue != null)
            {
                suitsValues = suitsValue.ToList();
            }


        }

        public int CalculatePlayerScore(string[] playercards)
        {

            if (playercards.Length != 0)
            {

                List<int> cardTotals = new List<int>();

                int score = 0;
                foreach (var cards in playercards)
                {
                    score = 0;
                    string card = cards.Trim();
                    string cardnumber = card.Substring(0, cards.Length - 1).ToUpper();

                    string cardsuit = card.Substring(cards.Length - 1, 1).ToUpper();
                    CheckSuitValue(cardsuit);
                    score = CheckCardValue(cardnumber);

                    cardTotals.Add(score);
                }

                return cardTotals.OrderByDescending(x=>x).Take(3).Sum();

            }
            else
            {
                throw new Exception($"Exception: No cards");
            }


        }


        public int CalculatePlayerScoreSuits(string[] playercards)
        {

            if (playercards.Length != 0)
            {

                List<int> cardTotals = new List<int>();

                int score = 0;
                foreach (var cards in playercards)
                {
                    score = 0;
                    string card = cards.Trim();
                    string cardsuit = card.Substring(cards.Length - 1, 1).ToUpper();
                    score = CheckSuitValue(cardsuit);

                    cardTotals.Add(score);
                }

                return cardTotals.Aggregate((y,x) => y * x) ;

            }
            else
            {
                throw new Exception($"Exception: No cards.");
            }

        }



        public int CheckCardValue(string cards)
        {
            if (!string.IsNullOrEmpty(cards))
            {

                var card = cardsNumbers.Where(x => x.card.Equals(cards)).FirstOrDefault();
                if(card == null )
                {
                    throw new Exception($"Exception: Incorrect card {cards}" );

                }

                return card.value;
               
            }
            else
            {
                throw new Exception($"Exception: No cards provided");
            }
        }

        public int CheckSuitValue(string suits)
        {
            if (!string.IsNullOrEmpty(suits))
            {

                var suit = suitsValues.Where(x => x.suit.Equals(suits)).FirstOrDefault();
                if (suit == null)
                {
                    throw new Exception($"Exception: Incorrect suit {suits}");

                }

                return suit.value;

            }
            else
            {
                throw new Exception($"Exception: No suit provided");
            }
        }


    }

    public class ProcessResults
    {
        public List<Player> CalculateHighestScore(List<Player> players)
        {

            if (players?.Count() != 0)
            {
                var hightestscore = players.Max(x => x.playerScore);
                var result = players.Where(x => x.playerScore == hightestscore).ToList();

                Console.WriteLine(string.Join(",", result.Select(x => x.playerName)) + ":" + hightestscore);

                return result;
            }
            else
            {
                throw new Exception($"Exception: No cards");
            }
        }

        public List<Player> RecalculateTiedPlayers(List<Player> players)
        {
            foreach (var playerData in players)
            {

                CalculateScore calculateScore = new CalculateScore(null,null);
                playerData.playerScore = calculateScore.CalculatePlayerScoreSuits(playerData.playerCards);

            }

            return CalculateHighestScore(players);

        }
    


    }







}
