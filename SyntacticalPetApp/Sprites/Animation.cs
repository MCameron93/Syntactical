namespace SyntacticalPetApp.Sprites
{
    public class Animation
    {
        public string[] ImagePaths { get; set; }
        public int Frames => ImagePaths.Length;
    }
}