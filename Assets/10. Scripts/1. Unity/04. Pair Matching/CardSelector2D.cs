using UnityEngine;
using UnityEngine.InputSystem;
using Study.CardSelector;

namespace Study.PairMatchingGame
{
    public class CardSelector2D : MonoBehaviour
    {
        public enum Direction
        {
            Up, Down, Left, Right
        }

        [Header("Ref Object")]
        public UnityEngine.Transform cursor;

        [Header("Settings")]
        public float cursorYOffset = -0.5f;

        private Card[] cards; 
        private int currentIndex = 2;

        private Card selectCardA;
        private Card selectCardB;

        public bool WasSelectionCompleted { get; private set; }

        private void Update()
        {
            WasSelectionCompleted = false;

            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                MoveCursor(Direction.Left);
            }
            else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                MoveCursor(Direction.Right);
            }
            else if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                MoveCursor(Direction.Up);
            }
            else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                MoveCursor(Direction.Down);
            }


            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                SelectCard();
            }
        }

        #region Public Methods

        public void SetBoard(Card[] cardArray)
        {
            cards = cardArray;
        }

        public Card[] GetSelectedCards()
        {
            return new[] { selectCardA, selectCardB };
        }

        public void Clear()
        {
            selectCardA = null;
            selectCardB = null;
        }

        #endregion

        #region Private Methods

        private void SelectCard()
        {
            Card currentCard = cards[currentIndex];

            if (selectCardA == null)
            {
                selectCardA = currentCard;
            }
            else if (selectCardA == currentCard)
            {
                return;
            }
            else
            {
                selectCardB = currentCard;
                WasSelectionCompleted = true;
            }

            cards[currentIndex].Flip();
        }

        private void MoveCursor(bool isLeft)
        {
            int temp = currentIndex;

            for (int i = 0; i < cards.Length; ++i)
            {
                temp += isLeft ? -1 : +1;

                if (temp < 0) temp = cards.Length - 1;
                if (temp >= cards.Length) temp = 0;

                if (cards[temp] == null)
                {
                    currentIndex++;
                    continue;
                }
                else
                {
                    currentIndex = temp;
                    float cardX = cards[currentIndex].transform.position.x;
                    float cardY = cards[currentIndex].transform.position.y + cursorYOffset;
                    cursor.position = new Vector3(cardX, cardY, cursor.position.z);
                    return;
                }
            }

            currentIndex = -1;
        }

        private void MoveCursor(Direction direction)
        {
            const int COLUMN_COUNT = 5;

            switch (direction)
            {
                case Direction.Up:
                    for (int i = 0; i < COLUMN_COUNT; ++i)
                        MoveCursor(false);
                    break;
                case Direction.Down:
                    for (int i = 0; i < COLUMN_COUNT; ++i)
                        MoveCursor(true);
                    break;
                case Direction.Left:
                    MoveCursor(true);
                    break;
                case Direction.Right:
                    MoveCursor(false);
                    break;
            }

        }

        #endregion
    }
}


