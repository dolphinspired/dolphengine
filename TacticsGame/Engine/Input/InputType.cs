namespace TacticsGame.Engine.Input
{
    public class InputType
    {
        public static KeyTypeInput Key(int key)
        {
            return new KeyTypeInput { KeyId = key };
        }

        public static CursorTypeInput Cursor()
        {
            return new CursorTypeInput();
        }

        public static WheelTypeInput ScrollWheel()
        {
            return new WheelTypeInput();
        }
    }

    public class KeyTypeInput
    {
        public int KeyId { get; set; }
    }

    public class CursorTypeInput
    {
    }

    public class WheelTypeInput
    {
    }
}
