using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/PlayerControls")]
    public class PlayerControls : MonoBehaviour
    {
        [System.Serializable]
        public class AdditionalSettings
        {
            public Vector2 bounds;
            public Vector2 size;
            public SpriteRenderer spriteRenderer;

            public void Get(GameObject obj)
            {
                if (Camera.main.orthographic && bounds == Vector2.zero) bounds = CameraHelpers.OrthographicBounds(Camera.main).extents;
                if (spriteRenderer == null) spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer && size == Vector2.zero) size = spriteRenderer.bounds.size;
            }
        }

        [System.Serializable]
        public class MouseSettings
        {
            public float speed = 1;

            public bool requireMouseButton = false;
            public bool horizontal = true;
            public bool vertical = true;

            public void Update(PlayerControls playerControls)
            {
                if (Input.GetMouseButton(0) || requireMouseButton == false)
                {
                    var mousePosition = Input.mousePosition;
                    mousePosition.z = playerControls.transform.position.z - Camera.main.transform.position.z;
                    mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

                    if (horizontal == false) mousePosition.x = playerControls.transform.position.x;
                    if (vertical == false) mousePosition.y = playerControls.transform.position.y;

                    playerControls.transform.position = Vector3.MoveTowards(playerControls.transform.position, mousePosition, speed * playerControls.speed * Time.deltaTime);
                }
            }
        }

        [System.Serializable]
        public class SwipeSettings
        {
            public float speed = 250;
            public bool rotation = true;
            public float rotationSpeed = 250;
            public float friction = .95f;
            public Vector3 velocity;
            public bool boundaries = true;

            public bool horizontal = true;
            public bool vertical = true;

            public void Awake(PlayerControls playerControls)
            {
                if (playerControls.gameObject.activeInHierarchy && PlayersGroup.selectedPlayer == null) PlayersGroup.selectedPlayer = playerControls.gameObject;

                velocity = playerControls.gameObject.transform.rotation * Vector3.right * Mathf.Epsilon;
            }

            public void Update(PlayerControls playerControls)
            {
                if (Input.GetMouseButton(0) && PlayersGroup.selectedPlayer == playerControls.gameObject)
                {
                    var mousePosition = CameraHelpers.GetMousePosition();

                    if (_previous == Vector3.zero) _previous = mousePosition;

                    velocity += (mousePosition - _previous) * speed * playerControls.speed * Time.deltaTime;

                    _previous = mousePosition;
                }
                else if (Input.GetMouseButtonDown(0) && PlayersGroup.selectedPlayer != playerControls)
                {
                    var mousePosition = CameraHelpers.GetMousePosition();

                    if (Math2DHelpers.PointInsideRect(mousePosition.x, mousePosition.y, playerControls.gameObject.transform.position.x, playerControls.gameObject.transform.position.y, playerControls.additionalSettings.size.x, playerControls.additionalSettings.size.y))
                    {
                        PlayersGroup.selectedPlayer = playerControls.gameObject;
                    }
                }
                else
                {
                    _previous = Vector3.zero;
                }

                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

                if (rotation)
                {
                    var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    playerControls.gameObject.transform.rotation = Quaternion.RotateTowards(playerControls.gameObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

                if (horizontal == false) velocity.x = 0;
                if (vertical == false) velocity.y = 0;

                playerControls.gameObject.transform.position += velocity * Time.deltaTime;

                if (boundaries && playerControls.additionalSettings.bounds != Vector2.zero) _Bounds(playerControls);

                if (friction != 0) velocity *= 1 / (1 + (Time.deltaTime * friction));
            }

            void _Bounds(PlayerControls playerControls)
            {
                var position = playerControls.gameObject.transform.position;

                if (position.x > playerControls.additionalSettings.bounds.x - playerControls.additionalSettings.size.x / 2)
                {
                    position.x = playerControls.additionalSettings.bounds.x - playerControls.additionalSettings.size.x / 2;
                    velocity.x = 0;
                }
                else if (position.x < -playerControls.additionalSettings.bounds.x + playerControls.additionalSettings.size.x / 2)
                {
                    position.x = -playerControls.additionalSettings.bounds.x + playerControls.additionalSettings.size.x / 2;
                    velocity.x = 0;
                }

                if (position.y > playerControls.additionalSettings.bounds.y - playerControls.additionalSettings.size.y / 2)
                {
                    position.y = playerControls.additionalSettings.bounds.y - playerControls.additionalSettings.size.y / 2;
                    velocity.y = 0;
                }
                else if (position.y < -playerControls.additionalSettings.bounds.y + playerControls.additionalSettings.size.y / 2)
                {
                    position.y = -playerControls.additionalSettings.bounds.y + playerControls.additionalSettings.size.y / 2;
                    velocity.y = 0;
                }

                playerControls.gameObject.transform.position = position;
            }

            Vector3 _previous;
        }

        [System.Serializable]
        public class WASDSettings
        {
            public string horizontal = "Horizontal";
            public string vertical = "Vertical";

            public float speed = 8;

            public bool boundaries = true;

            private bool facingRight;

            void Start()
            {
                facingRight = true;
            }

            public void Update(PlayerControls playerControls)
            {
                var h = Input.GetAxis(horizontal);
                var v = Input.GetAxis(vertical);

                playerControls.gameObject.transform.position += new Vector3(h, v) * Time.deltaTime * speed * playerControls.speed;

                if (boundaries && playerControls.additionalSettings.bounds != default) _Bounds(playerControls);
                if (Input.GetKeyDown(KeyCode.J)) Flip(h, playerControls);
            }

            void _Bounds(PlayerControls playerControls)
            {
                var position = playerControls.gameObject.transform.position;

                if (position.x > playerControls.additionalSettings.bounds.x - playerControls.additionalSettings.size.x / 2)
                {
                    position.x = playerControls.additionalSettings.bounds.x - playerControls.additionalSettings.size.x / 2;
                }
                else if (position.x < -playerControls.additionalSettings.bounds.x + playerControls.additionalSettings.size.x / 2)
                {
                    position.x = -playerControls.additionalSettings.bounds.x + playerControls.additionalSettings.size.x / 2;
                }

                if (position.y > playerControls.additionalSettings.bounds.y - playerControls.additionalSettings.size.y / 2)
                {
                    position.y = playerControls.additionalSettings.bounds.y - playerControls.additionalSettings.size.y / 2;
                }
                else if (position.y < -playerControls.additionalSettings.bounds.y + playerControls.additionalSettings.size.y / 2)
                {
                    position.y = -playerControls.additionalSettings.bounds.y + playerControls.additionalSettings.size.y / 2;
                }

                playerControls.gameObject.transform.position = position;
            }

            private void Flip(float horizontal, PlayerControls playerControls)
            {
                if(facingRight)
                {
                    facingRight = false;
                    Quaternion theRotation = playerControls.gameObject.transform.localRotation;
                    theRotation.y = 180;
                    playerControls.gameObject.transform.localRotation = theRotation;
                } else if(!facingRight){
                    facingRight = true;
                    Quaternion theRotation = playerControls.gameObject.transform.localRotation;
                    theRotation.y = 0;
                    playerControls.gameObject.transform.localRotation = theRotation;
                }

            }
        }

        [System.Serializable]
        public class ZigZagSettings
        {
            public KeyCode controlKey;
            public float rotationSpeed = 100;
            public float speed = 1;

            public void Update(PlayerControls playerControls)
            {
                if (_previousDirection == 0 && _direction != 0) _previousDirection = _direction;

                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(controlKey))
                {
                    _direction = 0;
                }
                else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(controlKey))
                {
                    if (_direction == 0) _direction = _previousDirection;
                    _direction = -_direction;
                    _previousDirection = _direction;
                }

                playerControls.gameObject.transform.Rotate(new Vector3(0, 0, _direction * rotationSpeed) * Time.deltaTime);

                playerControls.gameObject.transform.position += playerControls.gameObject.transform.right * speed * playerControls.speed * Time.deltaTime;
            }

            int _direction = 1;
            int _previousDirection;
        }

        public enum Mode { Swipe, ZigZag, WASD, Mouse };

        public float speed = 1;

        public Mode mode = Mode.Swipe;

        public SwipeSettings swipeSettings;
        public ZigZagSettings zigZagSettings;
        public WASDSettings wasdSettings;
        public MouseSettings mouseSettings;

        [Space(8)]
        public AdditionalSettings additionalSettings;

        void Awake()
        {
            additionalSettings.Get(gameObject);

            if (mode == Mode.Swipe)
            {
                swipeSettings.Awake(this);
            }
        }

        void Update()
        {
            if (mode == Mode.Mouse)
            {
                mouseSettings.Update(this);
            }
            else if (mode == Mode.Swipe)
            {
                swipeSettings.Update(this);
            }
            else if (mode == Mode.WASD)
            {
                wasdSettings.Update(this);
            }
            else if (mode == Mode.ZigZag)
            {
                zigZagSettings.Update(this);
            }
        }
    }
}