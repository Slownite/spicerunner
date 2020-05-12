using UnityEngine;
using Playniax.Ignition.Framework;
using Playniax.Ignition.ParticleSystem;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/CollisionData")]
    public class CollisionData : SpriteBoxCollider
    {
        [System.Serializable]
        public class CargoSettings
        {
            public GameObject[] prefab;

            public enum Mode { allAtOnce, random };

            public Mode mode = Mode.allAtOnce;

            public void Add(GameObject obj)
            {
                var length = prefab.Length;
                System.Array.Resize(ref prefab, length + 1);
                prefab[length] = obj;
            }

            public void Clear()
            {
                var length = prefab.Length;
                System.Array.Resize(ref prefab, 0);
            }

            public void Init()
            {
                for (int i = 0; i < prefab.Length; i++)
                {
                    if (prefab[i]) prefab[i].SetActive(false);
                }
            }

            public void Release(CollisionData collisionData)
            {
                if (mode == Mode.allAtOnce)
                {
                    _AllAtOnce(collisionData);
                }
                else if (mode == Mode.random)
                {
                    _Random(collisionData);
                }
            }

            void _AllAtOnce(CollisionData collisionData)
            {
                for (int i = 0; i < prefab.Length; i++)
                {
                    if (prefab[i])
                    {
                        var o = AdvancedGameObjectPooler.GetAvailableObject(prefab[i]);
                        o.transform.position = collisionData.transform.position;
                        o.SetActive(true);
                    }
                }
            }

            void _Random(CollisionData collisionData)
            {
                var i = Random.Range(0, prefab.Length);
                var o = AdvancedGameObjectPooler.GetAvailableObject(prefab[i]);
                o.transform.position = collisionData.transform.position;
                o.SetActive(true);
            }

            int _index;
        }

        [System.Serializable]
        public class OutroSettings
        {
            [System.Serializable]
            public class LayerSettings
            {
                public enum Mode { target, inherit, hard };

                public Mode mode = Mode.target;
                public int value = 1;
            }

            [System.Serializable]
            public class MessengerSettings
            {
                public float ttl = 3;
                public string text;
                public int fontSize = 22;
                public Font font;
                public Vector3 velocity = new Vector3(0, .25f, 0);
                public bool enabled = false;

                public void Message(CollisionData collisionData)
                {
                    if (text == "")
                    {
                        if (collisionData.points > 0) Message(collisionData, collisionData.points.ToString() + "+", collisionData.spriteRenderer.sortingOrder);
                    }
                    else
                    {
                        Message(collisionData, text, collisionData.spriteRenderer.sortingOrder);
                    }
                }

                public void Message(CollisionData collisionData, string message, int orderInLayer)
                {
                    var obj = new GameObject(message);

                    var meshRenderer = obj.AddComponent<MeshRenderer>();
                    meshRenderer.sortingOrder = orderInLayer;

                    var textMesh = obj.AddComponent<TextMesh>();
                    textMesh.characterSize = .1f;
                    textMesh.anchor = TextAnchor.MiddleCenter;
                    textMesh.fontSize = fontSize;
                    textMesh.fontStyle = FontStyle.Bold;
                    textMesh.font = font;
                    textMesh.text = message;

                    var textMessage = obj.AddComponent<TextMessage>();
                    textMessage.ttl = ttl;
                    textMessage.velocity = velocity;

                    obj.transform.position = collisionData.transform.position;

                    obj.SetActive(true);
                }
            }

            public string emitterGroupId = "Explosion Red";
            public float emitterGroupScale = 1;
            public LayerSettings layerSettings;
            public MessengerSettings messengerSettings;
            public AudioProperties audioSettings;

            public void Play(CollisionData a, CollisionData b)
            {
                if (a == null) return;

                var group = EmitterGroup.Get(emitterGroupId);
                if (group == null) return;

                var scale = emitterGroupScale;
                var layer = layerSettings.value;
                var position = a.transform.position;

                if (group.size > 0) scale *= Mathf.Max(a.spriteRenderer.sprite.rect.size.x, a.spriteRenderer.sprite.rect.size.y) / group.size;

                scale *= Mathf.Max(a.transform.localScale.x, a.transform.localScale.y);

                if (layerSettings.mode == LayerSettings.Mode.inherit)
                {
                    layer += a.spriteRenderer.sortingOrder;
                }
                else if (layerSettings.mode == LayerSettings.Mode.target && b && b.spriteRenderer)
                {
                    layer += b.spriteRenderer.sortingOrder;
                }

                group.Play(position, scale, layer);
            }
        }

        public static int autoPoints = 10;

        public int playerIndex = -1;
        [Tooltip("Number of hits the object can take.")]
        public int structuralIntegrity = 1;
        [Tooltip("Reward.")]
        public int points;
        [Tooltip("Whether the object can be destroyed on collision.")]
        public bool indestructible;
        [Tooltip("Whether to ignore collision detection or not depending on or off-screen.")]
        public bool checkVisibility = true;
        [Tooltip("Whether the object will be destroyed on structural integraty failure.")]
        public bool dontDestroyOnImpact;
        [Tooltip("Descibes the material the object is made from to determine impact sound by the CollisionAudio script.")]
        public string material = "Metal";

        public OutroSettings outroSettings;
        public CargoSettings cargoSettings;


        [Space(8)][Tooltip("Impact shader.")]
        public Material ghostMaterial;

        [HideInInspector]
        public int countLivesEnabled = -1;

        public GameObject linePiece;
        public Transform spawnLocation;

        public GameObject[] pieces = new GameObject[7];

        public override void Awake()
        {
            base.Awake();

            cargoSettings.Init();

            if (spriteRenderer) _defaultMaterial = spriteRenderer.material;

            _structuralIntegrity = structuralIntegrity;

            if (structuralIntegrity == 0) points = autoPoints;

            if (points == 0) points = structuralIntegrity * autoPoints;
        }

        public override bool AllowCollision()
        {
            if (spriteRenderer == null) return false;
            if (checkVisibility == true && spriteRenderer.isVisible == false) return false;

            return base.AllowCollision();
        }

        public override void OnCollision(SpriteColliderBase collider)
        {
            var collisionData = collider as CollisionData;

            if (collisionData == null) return;

            var a = structuralIntegrity;
            var b = collisionData.structuralIntegrity;


            if (indestructible && collisionData.indestructible) print("oops");

            if (indestructible)
            {
                b = 0;
                collisionData.structuralIntegrity = 0;
            }

            if (collisionData.indestructible)
            {
                a = 0;
                structuralIntegrity = 0;
            }

            structuralIntegrity -= b;
            collisionData.structuralIntegrity -= a;



            if (structuralIntegrity < 0) structuralIntegrity = 0;
            if (collisionData.structuralIntegrity < 0) collisionData.structuralIntegrity = 0;

            if (structuralIntegrity == 0) OnStructuralFailure(collisionData);
            if (collisionData.structuralIntegrity == 0) collisionData.OnStructuralFailure(this);

            _Ghost();
            collisionData._Ghost();

            if ((structuralIntegrity > 0 || collisionData.structuralIntegrity > 0) && material != "" && collisionData.material != "") CollisionAudio.Play(material, collisionData.material);

        }

        void _Ghost()
        {
            if (structuralIntegrity > 0 && spriteRenderer && ghostMaterial && spriteRenderer.material != ghostMaterial)
            {
                spriteRenderer.material = ghostMaterial;

                resetList.Add(this);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnDisable()
        {
            base.OnDisable();

            structuralIntegrity = _structuralIntegrity;
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public virtual void OnOutro(CollisionData collisionData)
        {
            outroSettings.audioSettings.Play();

            outroSettings.Play(this, collisionData);

            if (outroSettings.messengerSettings.enabled) outroSettings.messengerSettings.Message(this);

            if (cargoSettings.prefab.Length > 0) cargoSettings.Release(this);
        }

        public override void OnReset()
        {
            if (spriteRenderer && _defaultMaterial && spriteRenderer.material != _defaultMaterial) spriteRenderer.material = _defaultMaterial;
        }

        public virtual void OnStructuralFailure(CollisionData collisionData)
        {
            if (collisionData && collisionData.playerIndex >= 0) PlayerData.Get(collisionData.playerIndex).scoreboard += points;
            if (countLivesEnabled >= 0) PlayerData.Get(countLivesEnabled).lives -= 1;

            OnOutro(collisionData);

            if (dontDestroyOnImpact)
            {
                isCollisionSuspended = true;
            }
            else
            {
                Destroy();
            }
            int pieceSpawn = UnityEngine.Random.Range(0, 7);
            GameObject pickup = Instantiate(pieces[UnityEngine.Random.Range(0, 10)], transform.position, Quaternion.identity) as GameObject;
            pickup.name = pickup.name.Replace("(Clone)", "");
            }

        public int GetInitialStructuralIntegrity()
        {
            if (_structuralIntegrity == 0) _structuralIntegrity = structuralIntegrity;

            return _structuralIntegrity;
        }

        Material _defaultMaterial;
        int _structuralIntegrity;
    }
}