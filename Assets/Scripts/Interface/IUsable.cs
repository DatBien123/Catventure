using UnityEngine;

public interface IUsable
{
    public abstract void Use(CharacterPlayer character);
    public abstract void Sell(CharacterPlayer character);
    public abstract void Buy(CharacterPlayer character);
}
