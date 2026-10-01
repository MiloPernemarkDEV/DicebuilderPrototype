using UnityEngine;
using System.Collections.Generic;
using Encounter;
using DiceTools;

namespace DiceRolling
{
    public interface IDieSpriteHolder
    {
        public GameObject GetGameObject();
        public void RollDice();
        public void SetDiceData(List<RuntimeDie> RuntimeDieList);
        public event System.Action AllDiceFinishedRolling;
        public List<GameObject> GetDieObjects();
        public void SetDiceVisible(bool _visible);
    }

    public sealed class DieSpriteHolder : MonoBehaviour, IDieSpriteHolder
    {
        [SerializeField] private List<GameObject> _dieObjects = new List<GameObject>();

        private bool _areRolling = false;

        private void OnValidate()
        {
            foreach (GameObject obj in _dieObjects)
            {
                if(obj.GetComponent<IDie2D>() == null)
                {
                    Debug.LogError("Die object is missing IDie2D component: " + obj.name);
                }
            }
        }

        private void OnEnable()
        {
            // Connect to the RollingFinished event of each die object
            foreach (GameObject dieObj in _dieObjects)
            {
                IDie2D die2DComponent = dieObj.GetComponent<IDie2D>();
                if (die2DComponent != null)
                {
                    die2DComponent.RollingFinished += HandleRollingFinished;
                }
            }
        }
        private void OnDisable()
        {
            // Disconnect from the RollingFinished event of each die object
            foreach (GameObject dieObj in _dieObjects)
            {
                IDie2D die2DComponent = dieObj.GetComponent<IDie2D>();
                if (die2DComponent != null)
                {
                    die2DComponent.RollingFinished -= HandleRollingFinished;
                }
            }
        }

        private void Start()
        {
            SetDiceVisible(false);
        }

        private void HandleRollingFinished()
        {
            if (!_areRolling) return;
            foreach (GameObject dieObj in _dieObjects)
            {
                IDie2D die2DComponent = dieObj.GetComponent<IDie2D>();
                if (die2DComponent != null && die2DComponent.GetIsRolling())
                {
                    return; // At least one die is still rolling
                }
            }
            _areRolling = false;
            AllDiceFinishedRolling?.Invoke();
        }

        // IDieSpriteHolder implementation

        public event System.Action AllDiceFinishedRolling;
        public GameObject GetGameObject()
        {
            return gameObject;
        }
        public void SetDiceData(List<RuntimeDie> RuntimeDieList)
        {
            if (RuntimeDieList.Count != _dieObjects.Count)
            {
                Debug.LogError("Mismatch between RuntimeDieList count and _dieObjects count.");
                return;
            }
            for (int i = 0; i < RuntimeDieList.Count; i++)
            {
                IDie2D die2DComponent = _dieObjects[i].GetComponent<IDie2D>();
                if (die2DComponent == null)
                {
                    Debug.LogError("Die object is missing IDie2D component: " + _dieObjects[i].name);
                    return;
                }
                die2DComponent.SetRuntimeDie(RuntimeDieList[i]);
            }
        }
        public void RollDice()
        {
            if (_areRolling) return;
            _areRolling = true;
            //... Start rolling all dice

            foreach (GameObject dieObj in _dieObjects)
            {
                IVisibleThrowBehavior visibleThrowBehavior = dieObj.GetComponent<IVisibleThrowBehavior>();
                visibleThrowBehavior.DoThrowBehavior();

                IDie2D die2DComponent = dieObj.GetComponent<IDie2D>();
                if (die2DComponent != null)
                {
                    die2DComponent.Roll();
                }
            }


        }
        public List<GameObject> GetDieObjects()
        {
            return _dieObjects;
        }
        public void SetDiceVisible(bool _visible)
        {
            foreach (GameObject dieObj in _dieObjects)
            {
                dieObj.GetComponent<SpriteRenderer>().enabled = _visible;
            }
        }
    }
}

