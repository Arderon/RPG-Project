namespace RPG.Control
{
    public interface IReycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast(PlayerController playerController);
    }
}