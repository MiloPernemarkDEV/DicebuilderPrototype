using DiceTools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace Encounter
{

    public class CombatantDice
    {

        //private const int MAX_IN_HAND = 5;

        private RuntimeDieBag _drawBag;
        private RuntimeDieBag _inHand;
        private RuntimeDieBag _inPlay;
        private RuntimeDieBag _discardBag;

        public RuntimeDieBag DrawBag => _drawBag;
        public RuntimeDieBag Inhand => _inHand;
        public RuntimeDieBag InPlay => _inPlay;
        public RuntimeDieBag DiscardBag => _discardBag;


        private void RecycleDiscards()
        {
            Debug.Log("Recycling discards...");
            DrawBag.Dice.AddRange(DiscardBag.Dice);
            DiscardBag.Dice.Clear();
        }


        // Public constructor
        public CombatantDice(RuntimeDieBag drawBag)
        {
            _drawBag = drawBag;
            _inHand = new RuntimeDieBag();
            _inPlay = new RuntimeDieBag();
            _discardBag = new RuntimeDieBag();
        }

        public void DrawUp(int maxInHand)
        {
            Debug.Log(maxInHand);

            int diceNeeded = maxInHand - _inHand.Dice.Count;

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

        public void InHandToInPlay(List<string> dieIDs)
        {
            if (dieIDs == null || dieIDs.Count <= 0) return;

            // Convert incoming IDs to a HashSet for O(1) lookups
            HashSet<string> idSet = new HashSet<string>(dieIDs);

            // 1. Find all matching dice in inHand
            List<RuntimeDie> matchingDice = _inHand.Dice
                .Where(die => die != null && idSet.Contains(die.RuntimeID))
                .ToList();

            if (matchingDice.Count == 0) return;

            // 2. Add matching dice to inPlay
            _inPlay.Dice.AddRange(matchingDice);

            // 3. Remove matching dice from inHand
            _inHand.Dice.RemoveAll(die => die != null && idSet.Contains(die.RuntimeID));
        }

        //...
    }
}

