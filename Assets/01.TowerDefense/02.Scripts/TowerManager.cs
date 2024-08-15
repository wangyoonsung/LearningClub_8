using UnityEngine;
using UnityEngine.EventSystems;
 using System.Collections.Generic;
using System.Diagnostics;
//using System.Numerics;

public class TowerManager : Singleton<TowerManager>
{

	public TowerButton towerBtnPressed { get; set; }
	public List<GameObject> TowerList = new List<GameObject>();
	public List<Collider2D> BuildList = new List<Collider2D>();

	private SpriteRenderer spriteRenderer;
	private Collider2D buildTile;

	[SerializeField]
    public List<GameObject> randomTowerList = new List<GameObject>();
    private int randomTowerPrice;
    [SerializeField]
    public List<Transform> places;
    private List<Transform> availablePlaces;


    void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		buildTile = GetComponent<Collider2D>();
        availablePlaces = new List<Transform>(places);

    }

	void Update()
	{
		//If the left mouse button is clicked.
		if (Input.GetMouseButtonDown(0))
		{
			//Get the mouse position on the screen and send a raycast into the game world from that position.
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			//If something was hit, the RaycastHit2D.collider will not be null.
			if (hit.collider.tag == "BuildSite")
			{
				// hit.collider.tag = "BuildSiteFull";
				buildTile = hit.collider;
				buildTile.tag = "BuildSiteFull";
				RegisterBuildSite(buildTile);
				placeTower(hit);
			}
		}
		if (spriteRenderer.enabled)
		{
			followMouse();
		}
	}

	public void RegisterBuildSite(Collider2D buildTag)
	{
		// site.collider.tag = "BuildSiteFull";
		BuildList.Add(buildTag);
	}

	public void RenameTagsBuildSites()
	{
		foreach (Collider2D buildTag in BuildList)
		{
			buildTag.tag = "BuildSite";
		}
		BuildList.Clear();
	}

	public void RegisterTower(GameObject tower)
	{
		TowerList.Add(tower);
	}

	public void DestroyAllTowers()
	{
		foreach (GameObject tower in TowerList)
		{
			Destroy(tower.gameObject);
		}
		TowerList.Clear();
	}

	public void placeTower(RaycastHit2D hit)
	{
		if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
		{
			GameObject newTower = Instantiate(towerBtnPressed.TowerObject);
			newTower.transform.position = hit.transform.position;
			RegisterTower(newTower);
			buyTower(towerBtnPressed.TowerPrice);
			disableDragSprite();
		}
	}

	public void selectedTower(TowerButton towerBtn)
	{
		if (towerBtn.TowerPrice <= GamePlayManager.Instance.TotalMoney)
		{
			towerBtnPressed = towerBtn;
			enableDragSprite(towerBtn.DragSprite);
		}
	}

	public void buyTower(int price)
	{
		GamePlayManager.Instance.subtractMoney(price);
		//GamePlayManager.Instance.AudioSource.PlayOneShot(SoundLevelManager.Instance.BuildTower);
		SoundManager.Instance.PlaySFX(SoundManager.SFXType.towerBuilt);
	}

	private void followMouse()
	{
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector2(transform.position.x, transform.position.y);
	}

	public void enableDragSprite(Sprite sprite)
	{
		spriteRenderer.enabled = true;
		spriteRenderer.sprite = sprite;
	}

	public void disableDragSprite()
	{
		spriteRenderer.enabled = false;
		towerBtnPressed = null;
	}

	public void randomSelectedTower()
	{
		if (availablePlaces.Count > 0)
		{
			// Select a random index from the available places
			int randomIndex = Random.Range(0, availablePlaces.Count);

			// Instantiate the tower at the selected position
			Instantiate(randomTowerList[0], availablePlaces[randomIndex].position, Quaternion.identity);

			// Remove the selected place from the available places
			availablePlaces.RemoveAt(randomIndex);
		}
		else
		{
			UnityEngine.Debug.Log("All places are occupied, no more towers can be placed.");
		}
	}
}




