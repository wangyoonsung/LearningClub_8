using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    [System.Serializable]
    public class Effect
    {
        public EffectType type;
        public GameObject prefab;
    }

    public List<Effect> effects;

    private Dictionary<EffectType, GameObject> effectDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeEffectDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeEffectDictionary()
    {
        effectDictionary = new Dictionary<EffectType, GameObject>();
        foreach (var effect in effects)
        {
            if (!effectDictionary.ContainsKey(effect.type))
            {
                effectDictionary.Add(effect.type, effect.prefab);
            }
        }
    }

    public GameObject GetEffectPrefab(EffectType type)
    {
        if (effectDictionary.TryGetValue(type, out GameObject prefab))
        {
            return prefab;
        }
        else
        {
            Debug.Log("이펙트 타입은 >>>> " + type );
            return null;
        }
    }

    public void SpawnEffect(EffectType type, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = GetEffectPrefab(type);
        if (prefab != null)
        {
            Instantiate(prefab, position, rotation);
        }
    }
}
