using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntacticalPetApp.Sprites
{
    public class Animation
    {
        public string[] ImagePaths { get; set; }
        public int Frames => ImagePaths.Length;
    }
}
