namespace Playniax.Ignition.Framework
{
    [System.Serializable]
    public class PlayerData
    {
        public static int defaultLives = 3;

        public static int enemyCount;
        public static int progress;

        public int lives = defaultLives;

        public string name;
        public int scoreboard;

        //public List<string> rewards;

        public Config statistics = new Config();

        public static void Reset(int lives)
        {
            if (_playerData == null) return;

            for (int i = 0; i < _playerData.Length; i++)
            {
                _playerData[i].scoreboard = 0;
                _playerData[i].lives = lives;

                //if (_playerData[i].rewards != null) _playerData[i].rewards.Clear();

                _playerData[i].statistics.Clear();
            }
        }

        public static void SetLives(int lives)
        {
            for (int i = 0; i < _playerData.Length; i++)
            {
                _playerData[i].lives = lives;
            }
        }

        public static PlayerData Get(int player)
        {
            if (player < 0) return null;
            if (_playerData == null) System.Array.Resize(ref _playerData, player + 1);
            if (player >= _playerData.Length) System.Array.Resize(ref _playerData, player + 1);
            if (_playerData[player] == null) _playerData[player] = new PlayerData();

            return _playerData[player];
        }

        public static PlayerData Get(string name)
        {
            for (int i = 0; i < _playerData.Length; i++)
            {
                if (_playerData[i].name == name) return Get(i);
            }

            return null;
        }

        static PlayerData[] _playerData;
    }
}