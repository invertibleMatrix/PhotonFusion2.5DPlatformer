namespace _Runtime.Gameplay.Player
{
    public interface ILocalPlayerStatsListener
    {
        public void OnHealthUpdate(int currentHealth);
        public void OnKillsUpdate(int  totalKills);
        public void OnDeathsUpdate(int totalDeaths);
        public void OnWeaponChange(int i);
    }
}