
using FluentAssertions;
using multiplayercards;
using static System.Formats.Asn1.AsnWriter;

namespace TestMultiplayerCard
{
    public class MultiplayercardTest 
    {
        [Theory]
        [InlineData(new object[] { new string[] { "4H", "KD" } })]
        public void Calculate_Score_Sum(string[] scores)
        {
        
            CalculateScore calculateScore = new CalculateScore(null, null);

            int score = calculateScore.CalculatePlayerScore(scores);

            score.Should().Be(17);

        }


        [Theory]
        [InlineData(new object[] { new string[] { "4H", "KD" } })]
        public void Calculate_Score_Suit_Sum(string[] scores)
        {

            CalculateScore calculateScore = new CalculateScore(null, null);

            int score = calculateScore.CalculatePlayerScoreSuits(scores);

            score.Should().Be(4);

        }


        [Theory]
        [InlineData("H")]
        public void Check_Suit_Value(string suit)
        {
            CalculateScore calculateScore = new CalculateScore(null, null);

            int result = calculateScore.CheckSuitValue(suit);

            result.Should().Be(1);

        }


        [Theory]
        [InlineData("K")]
        public void Check_Card_Calue(string card)
        {
            CalculateScore calculateScore = new CalculateScore(null, null);

            int result = calculateScore.CheckCardValue(card);

            result.Should().Be(13);

        }


        [Theory]
        [InlineData("4H")]
        public void Check_Suit_Value_Incorrect(string suit)
        {
            CalculateScore calculateScore = new CalculateScore(null, null);

            Action act = () => calculateScore.CheckSuitValue(suit);

            act.Should().Throw<Exception>();

        }


        [Theory]
        [InlineData("4H")]
        public void Check_Card_Calue_Incorrect(string scores)
        {
            CalculateScore calculateScore = new CalculateScore(null, null);

            Action act = () => calculateScore.CheckCardValue(scores);

            act.Should().Throw<Exception>();

        }



        [Theory]
        [InlineData(new object[] { new string[] { "4H", "KD" } })]
        public void Calculate_Score_Sum_New_Scoring(string[] scores)
        {
            
            CalculateScore calculateScore = new CalculateScore(CardsNumbersData(),null);

            int score = calculateScore.CalculatePlayerScore(scores);

            score.Should().Be(6);

        }

        [Theory]
        [InlineData(new object[] { new string[] { "4H", "KD" } })]
        public void Calculate_Score_Suit_Sum_New_Scoring(string[] scores)
        {

            CalculateScore calculateScore = new CalculateScore(null, SuitData());

            int score = calculateScore.CalculatePlayerScoreSuits(scores);

            score.Should().Be(5);

        }


        public static List<CardsNumbers> CardsNumbersData()
        {
            List<CardsNumbers> cardsNumbers = new List<CardsNumbers>();
            cardsNumbers.Add(new CardsNumbers() { card = "4", value = 5 });
            cardsNumbers.Add(new CardsNumbers() { card = "5", value = 6 });
            cardsNumbers.Add(new CardsNumbers() { card = "K", value = 1 });


            return cardsNumbers;
           
        }

        public static List<SuitNumber> SuitData()
        {
            List<SuitNumber> suitNumbers = new List<SuitNumber>();
            suitNumbers.Add(new SuitNumber() { suit = "H", value = 5 });
            suitNumbers.Add(new SuitNumber() { suit = "S", value = 6 });
            suitNumbers.Add(new SuitNumber() { suit = "C", value = 1 });
            suitNumbers.Add(new SuitNumber() { suit = "D", value = 1 });


            return suitNumbers;

        }






    }

    
}