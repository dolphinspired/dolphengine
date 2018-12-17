using System;

namespace DolphEngine.Input.State
{
    public struct InputKey
    {
        public InputKey(int player, string genericKey)
        {
            this.Player = player;
            this.GenericKey = genericKey;
        }

        public readonly int Player;

        public readonly string GenericKey;

        #region Static methods

        public static InputKey Parse(string key)
        {
            var split = key.Split('_');

            if (split.Length == 2)
            {
                return new InputKey(1, key);
            }
            else if (split.Length == 3)
            {
                return new InputKey(ToPlayer(split[0]), $"{split[1]}_{split[2]}");
            }
            else
            {
                throw new InvalidOperationException($"Unable to parse {nameof(InputKey)} ({key})!");
            }
        }

        public static bool TryParse(string key, out InputKey parsed)
        {
            try
            {
                parsed = Parse(key);
                return true;
            }
            catch (InvalidOperationException)
            {
                parsed = default(InputKey);
                return false;
            }
        }

        #endregion

        #region Object overrides

        public override string ToString()
        {
            if (Player > 0)
            {
                return $"P{Player}_{GenericKey}";
            }

            return GenericKey;
        }

        #endregion

        #region Private methods

        private static int ToPlayer(string code)
        {
            if (!code.StartsWith('P'))
            {
                throw new InvalidOperationException($"Invalid player code on {nameof(InputKey)} ({code})!");
            }

            var playerNumberStr = code.Replace("P", "");

            if (!int.TryParse(playerNumberStr, out var playerNumber))
            {
                throw new InvalidOperationException($"Invalid player code on {nameof(InputKey)} ({code})!");
            }

            if (playerNumber <= 0)
            {
                throw new InvalidOperationException($"Invalid player code on {nameof(InputKey)} ({code})! Player number must be > 0.");
            }

            return playerNumber;
        }

        #endregion
    }
}
