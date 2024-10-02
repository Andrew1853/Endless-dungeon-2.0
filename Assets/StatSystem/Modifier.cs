using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ModifierType
{
    Add,
    PercentAdd
}
namespace DemiurgEngine.StatSystem
{
    public class Modifier
    {
        public ModifierType type;
        public float value;
    }
}
