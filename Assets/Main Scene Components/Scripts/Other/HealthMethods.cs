using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Main_Scene_Components.Scripts.Other
{
    internal interface HealthMethods
    {
        public void DecreaseHealth(int damage);
        public void IncreaseHealth(int amount);
    }
}
