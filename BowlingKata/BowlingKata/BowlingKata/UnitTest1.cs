using System;
using System.Collections.Generic;
using System.Xml.Schema;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BowlingKata
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod] // 1
        public void AllGutterBallsEqualZero()
        {
            var game = new Game();
            SimulateRoll(game, 20, 0);

            Assert.AreEqual(0, game.Score());
        }

        [TestMethod] // 2
        public void ScoreWithOutStrikesOrSpares()
        {
            var game = new Game();
            SimulateRoll(game, 20, 1);

            Assert.AreEqual(20, game.Score());
        }

        [TestMethod] // 3
        public void SpareInFirstFollowedByGutterBallsInRest()
        {
            var game = new Game();
            game.Roll(4);
            game.Roll(6);
            SimulateRoll(game, 18, 0);

            Assert.AreEqual(10, game.Score());
        }

        [TestMethod] // 4
        public void SpareInFirstPlusBallBonusGuttersForRest()
        {
            var game = new Game();
            game.Roll(5);
            game.Roll(5);
            game.Roll(1);
            SimulateRoll(game, 17, 0);

            Assert.AreEqual(12, game.Score());
        }


        [TestMethod] // 5 
        public void ConsecutiveSparesMultipleBonuses()
        {
            var game = new Game();
            game.Roll(5);
            game.Roll(5);
            game.Roll(5);
            game.Roll(5);

            SimulateRoll(game, 16, 4);

            Assert.AreEqual(93, game.Score());
        }

        [TestMethod] // 6 
        public void LastFrameSpareBonusBall()
        {
            var game = new Game();

            SimulateRoll(game, 18, 3);
            game.Roll(10);
            game.Roll(10);
            game.Roll(10);
            Assert.AreEqual(84, game.Score());
        }

        [TestMethod] // 7
        public void StrikeFirstFrameGuttersRest()
        {
            var game = new Game();
            game.Roll(10);
            SimulateRoll(game, 19, 0);
            Assert.AreEqual(10, game.Score());
        }

        [TestMethod] // 8
        public void StrikeFirstFramePlusTwoBallBonusGuttersRest()
        {
            var game = new Game();
            game.Roll(10);
            game.Roll(5);
            game.Roll(0);
            game.Roll(5);
            game.Roll(0);
            SimulateRoll(game, 15, 0);
            Assert.AreEqual(25, game.Score());
        }

        [TestMethod] // 9
        public void ThreeConsecutiveStrikesPlusTwoBallBonusThenGutters()
        {
            var game = new Game();
            game.Roll(10);
            game.Roll(10);
            game.Roll(10);
            game.Roll(5);
            game.Roll(0);
            game.Roll(5);
            game.Roll(0);
            SimulateRoll(game, 10, 0);
        }

        [TestMethod] // 10
        public void StrikeInLastFrameBonusBall()
        {
            var game = new Game();
            SimulateRoll(game, 18, 4);
            game.Roll(10);
            game.Roll(10);
            game.Roll(10);

            Assert.AreEqual(102, game.Score());
        }

        [TestMethod] // 11
        public void GuttersThreeStrikesLastFrame()
        {
            var game = new Game();
            SimulateRoll(game, 18, 0);
            game.Roll(10);
            game.Roll(10);
            game.Roll(10);

            Assert.AreEqual(30, game.Score());

        }

        [TestMethod] // 12
        public void PerfectGame()
        {
            var game = new Game();
            SimulateRoll(game, 12, 10);
            Assert.AreEqual(300, game.Score());
        }

        [TestMethod] // 13
        public void AllSpares()
        {
            var game = new Game();

            for (int i = 0; i < 21; i++)
            {
                game.Roll(5);
            }

            Assert.AreEqual(150, game.Score());
        }

        [TestMethod] // 14
        public void SparesAndStrikesPlusBonus()
        {
            var game = new Game();

            game.Roll(5);
            game.Roll(5);
            game.Roll(10);
            game.Roll(10);
            
            SimulateRoll(game, 13, 3);
        }

        [TestMethod] // 15
        public void MaxTenPinsForFrame()
        {
            var game = new Game();
            game.Roll(11);
            Assert.AreEqual(false, game.ValidPinCount);
        }

        [TestMethod] // 16
        public void MustHaveTenFrames()
        {
            var game = new Game();

            Assert.AreEqual(10, game.numberOfFrames);
        }

        public void SimulateRoll(Game game, int loop, int pins)
        {
            for (int i = 0; i < loop; i++)
            {
                game.Roll(pins);
            }
        }
    }

    public class Game
    {
        public List<int> _pintally = new List<int>();
        public int numberOfFrames = 10;
        public bool ValidPinCount;

        public void Roll(int pins)
        {
            if (pins <= 10)
            {
                ValidPinCount = true;
                _pintally.Add(pins);
            }
            else
            {
                ValidPinCount = false;
            }
           
        }

        public int Score()
        {
            int total = 0;
            int roll = 0;

            for (int i = 0; i < numberOfFrames; i++)
            {
                if (IsSpare(roll))
                {
                    total += Spare(roll);
                    roll = roll + 2;
                }
                else if (10 == _pintally[roll])
                {
                    total += Strike(roll);
                    roll = roll + 1;
                }
                else
                {
                    total += _pintally[roll] + _pintally[roll + 1];
                    roll = roll + 2;
                }
            }

            return total;
        }

        public int Strike(int roll)
        {
            return 10 + _pintally[roll + 1] + _pintally[roll + 2];
        }

        public int Spare(int roll)
        {
            return 10 + _pintally[roll + 2];
        }

        public bool IsSpare(int roll)
        {
            return 10 == _pintally[roll] + _pintally[roll + 1];
        }
    }
}
