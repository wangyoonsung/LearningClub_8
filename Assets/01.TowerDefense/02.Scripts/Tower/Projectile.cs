using UnityEngine;

public enum proType {
	rock, arrow, fireball
};

public enum TowerType
{
    NormalTower,
    StunTower,
    SlowTower
}

public class Projectile : MonoBehaviour {

	[SerializeField]
	private int attackStrength;
	[SerializeField]
	private proType projectileType;
	[SerializeField]
	public TowerType towerType;

    public int AttackStrength {
		get {
			return attackStrength;
		}
		set
		{
			attackStrength = value;
		}
	}

	public proType ProjectileType {
		get {
			return projectileType;
		}
	}
}
