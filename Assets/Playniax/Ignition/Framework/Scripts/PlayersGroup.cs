using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [AddComponentMenu("Playniax/Ignition/Framework/PlayersGroup")]
    public class PlayersGroup : MonoBehaviour
    {
        public static List<PlayersGroup> list = new List<PlayersGroup>();

        public static GameObject selectedPlayer;

        public string id = "Player 1";

        public bool autoSelect = true;

        public static int CountActive()
        {
            int count = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].gameObject.activeInHierarchy) count += 1;
            }

            return count;
        }

        public static GameObject Get(string id)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].id == id) return list[i].gameObject;
            }

            return null;
        }

        public static List<PlayersGroup> GetActive(bool includeSelected = true)
        {
            _activeList.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                if (includeSelected == false && list[i].gameObject == selectedPlayer) continue;

                if (list[i].gameObject.activeInHierarchy) _activeList.Add(list[i]);
            }

            return _activeList;
        }

        public static GameObject GetAny()
        {
            if (selectedPlayer && selectedPlayer.activeInHierarchy) return selectedPlayer;

            return GetRandom();
        }

        public static GameObject GetRandom(bool includeSelected = true)
        {
            GetActive(includeSelected);

            if (_activeList.Count == 0) return null;

            int index = Random.Range(0, _activeList.Count);

            return _activeList[index].gameObject;
        }

        public static GameObject OneUp()
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].gameObject.activeInHierarchy == false) return list[i].gameObject;
            }

            return null;
        }

        void Awake()
        {
            list.Add(this);
        }

        void OnDestroy()
        {
            list.Remove(this);
        }

        void Update()
        {
            _UpdateSelected();
        }

        void _UpdateSelected()
        {
            if (autoSelect && gameObject.activeInHierarchy && selectedPlayer == null) selectedPlayer = gameObject;
        }

        static List<PlayersGroup> _activeList = new List<PlayersGroup>();
    }
}