using UnityEngine;

namespace Encounter
{

    public static class EncounterConstants
    {
        public const int MAX_IN_HAND = 5;
        public const int MAX_SELECTED = 3;
    }

    public static class EncounterTools
    {
        public static string DebugCombatant(EncounterManager em, string combatantID, bool isPlayer)
        {
            string debStr = "";
            //string playerID = em.PlayerCombatant.RuntimeID;
            string hiStr = em.CombatantObjects[combatantID].SayHello();

            RuntimeCombatant c = null;
            if (isPlayer)
            {
                c = em.PlayerCombatant;
            }
            else
            {
                foreach (RuntimeCombatant enemyCombatant in em.EnemyCombatants)
                {
                    if (enemyCombatant.RuntimeID == combatantID)
                    {
                        c = enemyCombatant;
                        break;
                    }
                }
            }
            if (c == null)
            {
                Debug.LogError($"### EncounterTools: There is no combatant with ID {combatantID}");
                return debStr;
            }

            debStr += $"Combatant name: {c.DisplayName}\n";

            int drawBagCount = c.CombatantDice.DrawBag.Dice.Count;
            int inHandCount = c.CombatantDice.Inhand.Dice.Count;
            int inPlayCount = c.CombatantDice.InPlay.Dice.Count;
            int discardBagCount = c.CombatantDice.DiscardBag.Dice.Count;

            debStr += $"Draw bag count; {drawBagCount}\n";
            debStr += $"In hand count; {inHandCount}\n";
            debStr += $"In play count; {inPlayCount}\n";
            debStr += $"Discards count; {discardBagCount}\n";




            /*
            playerStr += $"Player Name: {em.PlayerCombatant.DisplayName}\n";
            int drawBagCount = em.PlayerCombatant.CombatantDice.DrawBag.Dice.Count;
            int inHandCount = em.PlayerCombatant.CombatantDice.Inhand.Dice.Count;
            int inPlayCount = em.PlayerCombatant.CombatantDice.InPlay.Dice.Count;
            int discardBagCount = em.PlayerCombatant.CombatantDice.DiscardBag.Dice.Count;
            playerStr += $"{drawBagCount} -- {inHandCount} -- {inPlayCount} -- {discardBagCount} -- {hiStr}";
            */
            return debStr;
        }
    }
}

