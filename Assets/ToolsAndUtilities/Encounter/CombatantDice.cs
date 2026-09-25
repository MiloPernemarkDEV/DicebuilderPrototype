using DiceTools;
using UnityRandom = UnityEngine.Random;
using UnityEngine;

namespace Encounter
{

    public interface ICombatantDice
    {
        public void DrawUp();
    }
    public class CombatantDice : ICombatantDice
    {

        private const int MAX_IN_HAND = 5;

        private RuntimeDieBag _drawBag;
        private RuntimeDieBag _inHand;
        private RuntimeDieBag _inPlay;
        private RuntimeDieBag _discardBag;

        // Public constructor
        public CombatantDice(RuntimeDieBag drawBag)
        {
            _drawBag = drawBag;
            _inHand = new RuntimeDieBag();
            _inPlay = new RuntimeDieBag();
            _discardBag = new RuntimeDieBag();
        }

        private void RecycleDiscards()
        {
            // ...
        }

        // ICombatantDice implementation
        public void DrawUp()
        {
            int diceNeeded = MAX_IN_HAND - _inHand.Dice.Count;

            if (diceNeeded <= 0)
            {
                // dont draw up -- already at max
                return;
            }
            if (_drawBag.Dice.Count < diceNeeded)
            {
                RecycleDiscards();
            }

            for (int i = 0; i < diceNeeded; i++)
            {
                int randoIdx = UnityRandom.Range(0, _drawBag.Dice.Count);
                _inHand.Dice.Add(_drawBag.Dice[randoIdx]);
                _drawBag.Dice.RemoveAt(randoIdx);
            }
        }

        // ...

    }
}

