using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

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
    [SerializeField]
    private int randomTowerPrice;		//타워 가격
    [SerializeField]
    public List<Transform> places;
    [SerializeField]
    private List<Transform> availablePlaces;

	[SerializeField]
	private GameObject selectTower;

	[SerializeField]
	private Tower towerchat;

    //Tower List
    private Dictionary<string, List<string>> towerDic = new Dictionary<string, List<string>>();

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

			Debug.Log("hit:"+hit.collider.tag);

			//If something was hit, the RaycastHit2D.collider will not be null.
			/*if(hit.collider.tag == "BuildSite")
			{
                buildTile = hit.collider;
                buildTile.tag = "BuildSiteFull";
                RegisterBuildSite(buildTile);
                placeTower(hit);
            }
            else */if (hit.collider.tag == "Tower")
			{
				if(selectTower != hit.collider.gameObject && towerchat != null)
				{
                    towerchat.unClickedTower();
					towerchat = null;
					selectTower = null;
                }
				//Debug.Log("tower:" + hit.collider.tag);
				selectTower = hit.collider.gameObject;
				//Debug.Log("selectTower:" + selectTower);
				towerchat = selectTower.GetComponent<Tower>();
				towerchat.clickedTower();
			}
			else if (hit.collider.tag == "SellBtn")
			{
				SellTower(towerchat);
            }
			else if(hit.collider.tag == "UpgradeBtn")
			{
				Debug.Log("put upgrade");
                towerchat.unClickedTower();
                towerchat = null;
                //타워 업그레이드 로직 생성
                upgradeTower(selectTower.name.Replace("(Clone)", "").Trim());
                selectTower = null;
            }
			else
			{
				if(towerchat != null)
				{
                    towerchat.unClickedTower();
                    towerchat = null;
                    selectTower = null;
                }
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
        //돈이 되는지 확인
        if (randomTowerPrice <= GamePlayManager.Instance.TotalMoney)
		{
			//자리가 되는지 확인
            if (availablePlaces.Count > 0)
            {
                
				//랜덤 타워 생성
				randomPlaceTower();
				
                //타워 가격만큼 돈 깎음
                GamePlayManager.Instance.subtractMoney(randomTowerPrice);
                //소리
                SoundManager.Instance.PlaySFX(SoundManager.SFXType.towerBuilt);
            }
            else
            {
                UnityEngine.Debug.Log("All places are occupied, no more towers can be placed.");
            }
        }
			  
	}
	public void randomPlaceTower()
	{
        // 먼저 availablePlaces가 비어 있는지 확인합니다.
        if (availablePlaces.Count == 0)
        {
            Debug.LogError("No available places left to place a tower.");
            return;
        }

        int randTowerIdx;

        // 랜덤 자리
        int randPlaceIdx = Random.Range(0, availablePlaces.Count);

        Debug.Log("randPlaceIdx >>>>>>>>>> " + randPlaceIdx);

        // 랜덤 타워
        float randomValue = Random.Range(0.0f, 1.0f);

        if (randomValue < 0.3f) // 30%
        {
            randTowerIdx = 0;
        }
        else if (randomValue < 0.55f) // 25%
        {
            randTowerIdx = 1;
        }
        else if (randomValue < 0.75f) // 20%
        {
            randTowerIdx = 2;
        }
        else // 25%
        {
            randTowerIdx = 3;
        }

        // 인덱스가 randomTowerList.Count를 초과할 경우 조정
        if (randTowerIdx >= randomTowerList.Count)
        {
            randTowerIdx = randomTowerList.Count - 1;
        }

		Vector3 tempPos = availablePlaces[randPlaceIdx].position;
		tempPos.z = 0;

        // 생성
        //Instantiate(randomTowerList[randTowerIdx], tempPos, Quaternion.identity);

		//생성 시 Tower에 현재 포지션 정보 지정
        GameObject towerInstance = Instantiate(randomTowerList[randTowerIdx], tempPos, Quaternion.identity);
        Tower towerScript = towerInstance.GetComponent<Tower>();
		towerScript.setPlace(availablePlaces[randPlaceIdx]);

        //tower Dictionary 추가
        AddData(randomTowerList[randTowerIdx].gameObject.name, availablePlaces[randPlaceIdx].gameObject.name);

        // 중복 처리를 위한 체킹
        availablePlaces.RemoveAt(randPlaceIdx);
    }
	//랜덤 타워 생성을 타워 티어별로도 할 수 있도록 만들어야 할 것 같은데,,,

	private void SellTower(Tower tower)
	{
        towerchat = null;
        Destroy(selectTower);
        selectTower = null;
        //타워 판매 대금
        GamePlayManager.Instance.addMoney(randomTowerPrice);
		//availablePlaces에도 추가 해야한다.
		availablePlaces.Add(tower.getPlace());
    }

	private void AddData(string name, string position)
    {
        // 이름이 Dictionary에 없으면 새로운 리스트를 생성하여 추가
        if (!towerDic.ContainsKey(name))
        {
            towerDic[name] = new List<string>();
        }

        // 위치 문자열을 리스트에 추가
        towerDic[name].Add(position);
    }

    private void upgradeTower(string towerName)
	{
		Debug.Log("do Upgrade"+towerName);
        //towerName으로 dictionary 검색.
		/*
		 * 총 List가 3개 이상 되는지 확인.
		 * 3이상일 경우, 현재 위치 포함 + 2개 더 선택하기.
		 * 셋 다 삭제 후, 현재 위치에 다음 레벨 랜덤 타워 생성
		 */
        List<string> positions;
         if (towerDic.TryGetValue(towerName, out positions))
         {
             foreach (string position in positions)
             {
                 Debug.Log($"{towerName}의 위치: {position}");
             }
         }
         else
         {
             Debug.Log("해당 이름에 대한 위치가 없습니다.");
         }
        // 전체 Dictionary 조회
        /*foreach (KeyValuePair<string, List<string>> entry in towerDic)
        {
            string name = entry.Key;
            List<string> positions = entry.Value;

            Debug.Log($"Name: {name}");
            foreach (string position in positions)
            {
                Debug.Log($" - Position: {position}");
            }
        }*/
    }
}




