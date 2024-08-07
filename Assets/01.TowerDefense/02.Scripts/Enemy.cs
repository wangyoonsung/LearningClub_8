using UnityEngine;

public class Enemy : MonoBehaviour {
	
	[SerializeField]
	private int healthPoints;
	[SerializeField]
	private int rewardAmt;
	[SerializeField]
	private Transform exitPoint;
	[SerializeField]
	private Transform[] wayPoints;
	[SerializeField]
	private float navigationUpdate;
	[SerializeField]
	private Animator anim;
	private int target = 0;
	private Transform enemy;
	private Collider2D enemyCollider;
	private float navigationTime = 0;
	private bool isDead = false; 

	public bool IsDead {
		get {
			return isDead;
		}
	}

	// Use this for initialization
	void Start () {
		enemy = GetComponent<Transform> ();
		anim = GetComponent<Animator>();	
		enemyCollider = GetComponent<Collider2D>();
		GamePlayManager.Instance.RegisterEnemy(this);	
	}

	// Update is called once per frame
	void Update () {
		if (wayPoints != null && !isDead) {
			navigationTime += Time.deltaTime;
			if (navigationTime > navigationUpdate) {
				if (target < wayPoints.Length) {
					enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, 0.8f * navigationTime);
				} else {
					enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, 0.8f * navigationTime);
				}
				navigationTime = 0;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "WayPoint")
			target += 1;
		else if (other.tag == "Finish") {
			GamePlayManager.Instance.TotalEscaped += 1;
			GamePlayManager.Instance.RoundEscaped += 1;
			GamePlayManager.Instance.UnRegister(this);
			GamePlayManager.Instance.isWaveOver();
		} else if (other.tag == "Projectile") {
			Projectile newP = other.gameObject.GetComponent<Projectile>();
			enemyHit(newP.AttackStrength);
			Destroy(other.gameObject);
		}
	}

	public void enemyHit(int hitPoints) {
		if (healthPoints - hitPoints > 0) {
			anim.Play("Hurt");
			//GamePlayManager.Instance.AudioSource.PlayOneShot(SoundLevelManager.Instance.Hit);
            SoundManager.Instance.PlaySFX(SoundManager.SFXType.hit);
            healthPoints -= hitPoints;
		} else {
			die();
		}

		
	}

	public void die() {
		isDead = true;
		anim.SetTrigger("didDie");
		GamePlayManager.Instance.TotalKilled += 1;
		enemyCollider.enabled = false;
		GamePlayManager.Instance.addMoney(rewardAmt);
		//GamePlayManager.Instance.AudioSource.PlayOneShot(SoundLevelManager.Instance.Die);
        SoundManager.Instance.PlaySFX(SoundManager.SFXType.death);
        EffectManager.Instance.SpawnEffect(EffectType.Explosion, transform.position, Quaternion.identity);
        GamePlayManager.Instance.isWaveOver();
		
	}
}
