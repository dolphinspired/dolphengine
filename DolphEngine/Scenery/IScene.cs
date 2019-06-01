namespace DolphEngine.Scenery
{
    public interface IScene
    {
        void Load();

        void Unload();

        void Update();

        void Draw();
    }
}
