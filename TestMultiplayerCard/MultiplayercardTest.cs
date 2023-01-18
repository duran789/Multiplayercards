
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
        
            CalculateScore calculateScore = new CalculateScore();

            int score = calculateScore.CalculatePlayerScore(scores);

            score.Should().Be(17);

        }


        [Theory]
        [InlineData(new object[] { new string[] { "4H", "KD" } })]
        public void Calculate_Score_Suit_Sum(string[] scores)
        {

            CalculateScore calculateScore = new CalculateScore();

            int score = calculateScore.CalculatePlayerScoreSuits(scores);

            score.Should().Be(4);

        }


        [Theory]
        [InlineData("H")]
        public void Check_Suit_Value(string suit)
        {
            CalculateScore calculateScore = new CalculateScore();

            int result = calculateScore.CheckSuitValue(suit);

            result.Should().Be(1);

        }


        [Theory]
        [InlineData("K")]
        public void Check_Card_Calue(string card)
        {
            CalculateScore calculateScore = new CalculateScore();

            int result = calculateScore.CheckCardValue(card);

            result.Should().Be(13);

        }


        [Theory]
        [InlineData("4H")]
        public void Check_Suit_Value_Incorrect(string suit)
        {
            CalculateScore calculateScore = new CalculateScore();

            Action act = () => calculateScore.CheckSuitValue(suit);

            act.Should().Throw<Exception>();

        }


        [Theory]
        [InlineData("4H")]
        public void Check_Card_Calue_Incorrect(string scores)
        {
            CalculateScore calculateScore = new CalculateScore();

            Action act = () => calculateScore.CheckCardValue(scores);

            act.Should().Throw<Exception>();

        }


        
    }

    
}