using System;
using NUnit.Framework;
using PokerApp.Core.Enums;

namespace PokerApp.Core.Tests
{
    public class HandTests
    {
        [Test]
        public void IsInValidHandWhenTwoSameCardsAreDealt()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Hand(new Card(FaceValue.King, Suit.Diamonds),
                    new Card(FaceValue.King, Suit.Diamonds));
            }, "Cannot have duplicate cards in a hand");
        }

        [Test]
        public void TwoCardsWithSameSuitInaHandIsFlush()
        {
            var pokerHand = new Hand(
                                new Card(FaceValue.Ace, Suit.Hearts),
                                new Card(FaceValue.Nine, Suit.Hearts));

            Assert.AreEqual(pokerHand.GetHandRanking(), HandRanking.Flush);
        }

        [Test]
        public void TwoCardsOfSequentialRankSameSuitIsStraightFlush()
        {
            var pokerHand = new Hand(new Card(FaceValue.Ten, Suit.Hearts),
                new Card(FaceValue.Nine, Suit.Hearts));

            Assert.AreEqual(pokerHand.GetHandRanking(), HandRanking.StraightFlush);
        }

        [Test]
        public void TwoCardsOfSequentialRankDifferentSuitIsStraight()
        {
            var pokerHand = new Hand(new Card(FaceValue.Queen, Suit.Spades),
                new Card(FaceValue.King, Suit.Hearts));

            Assert.AreEqual(pokerHand.GetHandRanking(), HandRanking.Straight);
        }

        [Test]
        public void TwoCardsWithSameRankDifferentSuitInaHandIsPair()
        {
            var pokerHand = new Hand(
                new Card(FaceValue.Ace, Suit.Hearts),
                new Card(FaceValue.Ace, Suit.Diamonds));

            Assert.AreEqual(pokerHand.GetHandRanking(), HandRanking.Pair);
        }


        [Test]
        public void TwoCardsWithDifferentRankNotInOrderAndDifferentSuitInaHandIsHigh()
        {
            var pokerHand = new Hand(
                new Card(FaceValue.Seven, Suit.Hearts),
                new Card(FaceValue.Ace, Suit.Diamonds));

            Assert.AreEqual(pokerHand.GetHandRanking(), HandRanking.HighCard);
        }
    }
}
