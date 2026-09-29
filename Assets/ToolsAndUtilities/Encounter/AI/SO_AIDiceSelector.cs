using UnityEngine;
using System.Collections.Generic;
using DiceTools;

namespace Encounter
{
    public enum DiceSelectionBehavior
    {
        BASIC_00,
    }

    public interface I_AIDiceSelector
    {
        // selects three dice to play from the in-hand collection
        // based on context gathered from the EM
        public List<int> MakeSelection(RuntimeDieBag inHandDice, EncounterManager em);

    }

    public class DiceSelectionBehavior_Basic : I_AIDiceSelector
    {
        public List<int> MakeSelection(RuntimeDieBag inHandDice, EncounterManager em)
        {
            List<int> selection = new List<int>();

            // just pick the first three...basic...
            for (int i = 0; i < EncounterConstants.MAX_SELECTED; i++)
            {
                selection.Add(i);
            }
            return selection;
        }
    }

    [CreateAssetMenu(fileName = "SO_AIDiceSelector", menuName = "Encounter/AI Dice Selector")]
    public class SO_AIDiceSelector : ScriptableObject
    {
        [SerializeField] private DiceSelectionBehavior _selectionBehavior = DiceSelectionBehavior.BASIC_00;

        public I_AIDiceSelector GetDiceSelector()
        {
            switch ( _selectionBehavior)
            {
                case DiceSelectionBehavior.BASIC_00:
                    return new DiceSelectionBehavior_Basic();
                default:
                    return new DiceSelectionBehavior_Basic();
            }

        }

    }
}


