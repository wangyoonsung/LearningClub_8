using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Tower_4_3 :  Tower {

	[SerializeField]
	private float timeBetweenAttacks;
	[SerializeField]
	private float attackRadius;
	[SerializeField]
	private Projectile projectile;
    [SerializeField]
    private bool isAttack = false; 
	private Enemy targetEnemy = null;
	[SerializeField]
	private float attackCounter;
	private AudioSource audioSource;
	[SerializeField]
	private bool isStun = false;	//스턴 기능 있는 타워인지
    [SerializeField]
    private bool isSlow = false;	//속력 느리게 해주는 타워

    [SerializeField]
	private GameObject towerUI;

    void Start() {
		audioSource = GetComponent<AudioSource>();
        MakeTowerDamageUp(2);

    }


    private List<Tower> GetTowersInRange()
    {
        List<Tower> towersInRange = new List<Tower>();
        foreach (Tower tower in GamePlayManager.Instance.TowerList)
        {
            if (Vector2.Distance(transform.localPosition, tower.transform.localPosition) <= attackRadius )
            {
                towersInRange.Add(tower);
            }
        }
        return towersInRange;
    }

	private void MakeTowerDamageUp(int up)
	{
        foreach (Tower tower in GetTowersInRange())
        {
			tower.attackDamage *= up;
        }
    }

}
