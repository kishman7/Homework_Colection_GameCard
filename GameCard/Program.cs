using System;
using System.Collections.Generic;

//Програма «Гра в карти»
//Створити модель карткової гри.
//Клас Game формує і забезпечує:
//Список гравців(мінімум 2);
//Колоду карт(36 карт);
//Перетасування карт(випадковим чином);
//Роздачу карт гравцям (рівними частинами кожного гравця);
//Ігровий процес.Принцип: Гравці кладуть по одній карті.У кого карта більше,
//той забирає всі карти і кладе їх в кінець своєї колоди.Спрощення: при збігу карт
//забирає перший гравець, шістка не забирає туза. Виграє гравець, який забрав всі карти.
//Клас Player (набір наявних карт, вивід наявних карт).
//Клас(або структура)  Card(масть і тип карти(6 - 10, валет, дама, король, туз).
//Використати колекції.

namespace GameCard
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(2);
            while (game.playersTurn())
            {

            }
        }

        public class Game
        {
            public List<Card> cardDeck;  // список карт
            public List<Player> players;  // список гравців

            private Random _random;
            private int _cardsAmount = 36; // кількість карт
            public Game(int playersCount = 2)  // конструктор приймає кількість гравців, по замовчуванню 2 гравці
            {
                _random = new Random();

                players = new List<Player>(); // створюємо гравця
                for (int i = 0; i < playersCount; i++)
                {
                    players.Add(new Player()); // додаємо гравця в список
                }

                cardDeck = createCardDeck(); // роздаємо карти
                shuffleCards(cardDeck); // перемішуємо карти

                dealCardsToPlayers(players, cardDeck); // граємо
            }

            public List<Card> createCardDeck() // метод по створенню карт
            {
                cardDeck = new List<Card>();  // створюємо крти
                int suitCount = _cardsAmount / 4;  // кількість типу карт для одної масті

                for (int i = 0; i < suitCount; i++) // для кожної масті додаємо свої типи
                {
                    // додаємо в список карт через перелічувач CardSuit(масть) відповідні типи через перелічувач CardValue(тип)
                    cardDeck.Add(new Card((CardValue)i, (CardSuit)0)); 
                    cardDeck.Add(new Card((CardValue)i, (CardSuit)1));
                    cardDeck.Add(new Card((CardValue)i, (CardSuit)2));
                    cardDeck.Add(new Card((CardValue)i, (CardSuit)3));
                }

                return cardDeck;
            }

            public void shuffleCards(List<Card> cards) // метод перемішування карт
            {
                cards.Sort((a, b) => _random.Next(-2, 2));  //ТУТ НЕ ЗРОЗУМІЛО ЯК ПРАЦЮЄ SORT І ЧОМУ В РАНДОМІ -2 ТА 2 ???
            }

            public void dealCardsToPlayers(List<Player> players, List<Card> cards)  // роздача карт гравцям
            {
                int currentPlayer = 0;

                for (int i = 0; i < cards.Count; i++)
                {
                    players[currentPlayer].cards.Add(cards[i]);

                    currentPlayer++;
                    currentPlayer %= players.Count;  //???
                }
            }

            public bool playersTurn() // метод самого ходу картами
            {
                Console.WriteLine("Ход игроков:");
                Console.WriteLine("игрок\tкол-во карт\tход картой");

                int maxValue = -1;  // для тго, щоб цикл запустився з першого разу
                Player playerWithMaxValue = null;
                Stack<Card> cardStack = new Stack<Card>();

                for (int i = 0; i < players.Count; i++)
                {
                    Player player = players[i];

                    if (player.cards.Count > 0)
                    {
                        Card card = player.cards[_random.Next(player.cards.Count)]; //кожного гравця вибирається карта

                        Console.WriteLine($"{i}\t{player.cards.Count}\t\t{card}");
                        player.cards.Remove(card);

                        if ((int)card.value > maxValue) // якщо карта гравця більша, то він забирає карту
                        {
                            maxValue = (int)card.value; // якщо значення карти білбше, то воно присвоюється це значення maxValue
                            playerWithMaxValue = player; // позначаємо гравця в якого більша карта
                        }

                        cardStack.Push(card); // карта, яка кладеться на стіл

                    }
                }

                playerWithMaxValue.cards.AddRange(cardStack); // додаються карти суперників, гравцю, в якого більша карта
                Console.WriteLine($"Забрал игрок {players.IndexOf(playerWithMaxValue)}.");
                Console.WriteLine("------------------------------------------------");

                if (playerWithMaxValue.cards.Count == _cardsAmount) // якщо в гравця опиняються всі карти, то гравець переміг і гра завершується
                {
                    Console.WriteLine($"Победил игрок номер {players.IndexOf(playerWithMaxValue)}");
                    return false;
                }

                return true;
            }
        }

        public class Player // клас для створення гравця
        {
            public List<Card> cards = new List<Card>();
        }

        public enum CardValue // перелічувач для типу карт
        {
            SIX = 0, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING, ACE
        }

        public enum CardSuit // перелічувач для мастей
        {
            HEARTS = 0, DIAMONDS, CLUBS, SPADES
        }
        public class Card // клас для створення карт
        {

            public readonly CardValue value;
            public readonly CardSuit suit;

            public Card(CardValue value, CardSuit suit)
            {
                this.value = value;
                this.suit = suit;
            }

            public override string ToString()
            {
                return $"{value} {suit}";
            }
        }
    }
}

