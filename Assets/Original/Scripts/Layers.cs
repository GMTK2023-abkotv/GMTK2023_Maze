public static class Layers
{
    public const int Player = 6;
    public const int Enemies = 7;
    public const int Holes = 8;
    public const int Death = 9;

    public const int PlayerMask = 1 << 6;
    public const int EnemiesMask = 1 << 7;
    public const int HolesMask = 1 << 8;
    public const int DeathMask = 1 << 9;
}