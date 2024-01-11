namespace Assets.Main_Scene_Components.Scripts.Other
{
    // Specifices health methods that other class must implement if using this class.
    internal interface HealthMethods
    {
        public void DecreaseHealth(int damage);
        public void IncreaseHealth(int amount);
    }
}
