using System;
using System.Collections.Generic;
using Code.Gameplay.AI.Behaviours;
using Code.Gameplay.PlayerLogic.Behaviours;
using UnityEngine;

namespace Code.Gameplay.Coloring
{
    [DefaultExecutionOrder(-500)]
    public class ColorField : MonoBehaviour
    {
        public const int UnpaintedOwnerId = 0;
        public const int PlayerOwnerId = 1;

        private const float DEFAULT_SCAN_INTERVAL = 0.25f;
        private const float TILE_BELOW_TOLERANCE = 0.75f;
        private const float TILE_CONTACT_HEIGHT = 0.25f;

        private static readonly Color PlayerPaintColor = Color.yellow;

        public static ColorField Instance { get; private set; }

        private const string BaseColorName = "_BaseColor";
        private const string LegacyColorName = "_Color";
        private const string ArnoldBaseColorName = "_BASE_COLOR";
        private const string BaseMapName = "_BaseMap";
        private const string ArnoldBaseMapName = "_BASE_COLOR_MAP";
        private const string MainTexName = "_MainTex";

        [SerializeField] private Color[] _botColors;
        [SerializeField] private Material _paintMaterial;

        private Tile[] _tiles;
        private readonly Dictionary<int, Material> _ownerMaterials = new Dictionary<int, Material>();
        private Material _runtimePaintTemplate;
        private Texture2D _whiteMap;
        private int _playerScore;
        private int _nextBotOwnerId = PlayerOwnerId + 1;
        private float _scanTimer;

        public Color PlayerColor => PlayerPaintColor;
        public int PlayerScore => _playerScore;
        public event Action<int> PlayerScoreChanged;

        private void Awake()
        {
            Instance = this;
            EnsureBotPalette();
            CollectTiles();
        }

        private void OnEnable()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;

            foreach (KeyValuePair<int, Material> pair in _ownerMaterials)
            {
                if (pair.Value != null)
                    Destroy(pair.Value);
            }

            _ownerMaterials.Clear();

            if (_runtimePaintTemplate != null)
                Destroy(_runtimePaintTemplate);

            if (_whiteMap != null)
                Destroy(_whiteMap);
        }

        private void LateUpdate()
        {
            _scanTimer -= Time.deltaTime;
            if (_scanTimer <= 0f)
            {
                _scanTimer = DEFAULT_SCAN_INTERVAL;
                ScanAgents();
            }
        }

        public Color GetBotColor(int botIndex)
        {
            EnsureBotPalette();
            if (_botColors == null || _botColors.Length == 0)
                return Color.red;

            int index = botIndex % _botColors.Length;
            if (index < 0)
                index += _botColors.Length;

            return _botColors[index];
        }

        public ColorPainter CreatePainter(GameObject agent, bool isPlayer)
        {
            if (agent == null)
                return null;

            if (!agent.TryGetComponent(out ColorPainter painter))
                painter = agent.AddComponent<ColorPainter>();

            if (isPlayer)
            {
                painter.Initialize(this, PlayerOwnerId, PlayerPaintColor, true);
                return painter;
            }

            int ownerId = _nextBotOwnerId++;
            int botIndex = ownerId - (PlayerOwnerId + 1);
            painter.Initialize(this, ownerId, GetBotColor(botIndex), false);
            return painter;
        }

        public bool TryPaint([Bridge.Ref] Vector3 position, int ownerId, [Bridge.Ref] Color color)
        {
            if (_tiles == null || ownerId == UnpaintedOwnerId)
                return false;

            int tileIndex = FindTileIndex(position);
            if (tileIndex < 0)
                return false;

            Tile tile = _tiles[tileIndex];
            if (tile.OwnerId == ownerId)
                return false;

            int previousOwner = tile.OwnerId;
            tile.OwnerId = ownerId;
            _tiles[tileIndex] = tile;
            ApplyTileColor(tile.Renderer, ownerId, color);
            UpdatePlayerScore(previousOwner, ownerId);
            return true;
        }

        private void ScanAgents()
        {
            GamePlayerCharacter[] players = FindObjectsOfType<GamePlayerCharacter>();
            for (int i = 0; i < players.Length; i++)
            {
                GamePlayerCharacter player = players[i];
                if (player != null && player.GetComponent<ColorPainter>() == null)
                    CreatePainter(player.gameObject, true);
            }

            AICharacter[] bots = FindObjectsOfType<AICharacter>();
            for (int i = 0; i < bots.Length; i++)
            {
                AICharacter bot = bots[i];
                if (bot != null && bot.GetComponent<ColorPainter>() == null)
                    CreatePainter(bot.gameObject, false);
            }
        }

        private void CollectTiles()
        {
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(false);
            if (renderers == null || renderers.Length == 0)
            {
                _tiles = new Tile[0];
                return;
            }

            var tiles = new List<Tile>(renderers.Length);
            for (int i = 0; i < renderers.Length; i++)
            {
                MeshRenderer renderer = renderers[i];
                if (renderer == null || !renderer.gameObject.activeInHierarchy)
                    continue;

                Bounds bounds = renderer.bounds;
                Vector3 center = bounds.center;
                Vector3 extents = bounds.extents;
                // Playworks fills center and extents, but Bounds.max stays at zero.
                if (extents.x <= 0.0001f && extents.z <= 0.0001f)
                {
                    Vector3 scale = renderer.transform.lossyScale;
                    extents = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z)) * 0.5f;
                    center = renderer.transform.position;
                }

                tiles.Add(new Tile
                {
                    Renderer = renderer,
                    Center = center,
                    Extents = extents,
                    TopY = center.y + extents.y,
                    OwnerId = UnpaintedOwnerId
                });
            }

            _tiles = tiles.ToArray();
        }

        private int FindTileIndex([Bridge.Ref] Vector3 position)
        {
            int bestIndex = -1;
            float bestSqr = float.MaxValue;

            for (int i = 0; i < _tiles.Length; i++)
            {
                Tile tile = _tiles[i];
                float height = position.y - tile.TopY;
                if (height < -TILE_BELOW_TOLERANCE || height > TILE_CONTACT_HEIGHT)
                    continue;

                float dx = Mathf.Abs(position.x - tile.Center.x);
                float dz = Mathf.Abs(position.z - tile.Center.z);
                if (dx > tile.Extents.x || dz > tile.Extents.z)
                    continue;

                float sqr = dx * dx + dz * dz;
                if (sqr >= bestSqr)
                    continue;

                bestSqr = sqr;
                bestIndex = i;
            }

            return bestIndex;
        }

        private void ApplyTileColor(Renderer renderer, int ownerId, [Bridge.Ref] Color color)
        {
            if (renderer == null)
                return;

            Material material = GetOwnerMaterial(ownerId, color);
            if (material == null)
                return;

            // Playworks throws on SetPropertyBlock(null), so the material assign never runs.
            renderer.sharedMaterial = material;
        }

        private Material GetOwnerMaterial(int ownerId, [Bridge.Ref] Color color)
        {
            if (_ownerMaterials.TryGetValue(ownerId, out Material existing) && existing != null)
                return existing;

            Material template = GetPaintTemplate();
            if (template == null)
                return null;

            Material material = new Material(template);
            material.name = "TilePaint_" + ownerId;
            ApplyColorToMaterial(material, color);
            _ownerMaterials[ownerId] = material;
            return material;
        }

        private Material GetPaintTemplate()
        {
            if (_paintMaterial != null)
                return _paintMaterial;

            if (_runtimePaintTemplate != null)
                return _runtimePaintTemplate;

            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
                shader = Shader.Find("Universal Render Pipeline/Simple Lit");
            if (shader == null)
                shader = Shader.Find("Standard");

            if (shader == null)
                return null;

            _runtimePaintTemplate = new Material(shader);
            _runtimePaintTemplate.name = "TilePaintTemplate";
            _runtimePaintTemplate.SetFloat("_Smoothness", 0.05f);
            _runtimePaintTemplate.SetFloat("_Metallic", 0f);
            if (_runtimePaintTemplate.HasProperty("_Glossiness"))
                _runtimePaintTemplate.SetFloat("_Glossiness", 0.05f);

            return _runtimePaintTemplate;
        }

        private void ApplyColorToMaterial(Material material, [Bridge.Ref] Color color)
        {
            if (material.HasProperty(BaseColorName))
                material.SetColor(BaseColorName, color);
            if (material.HasProperty(LegacyColorName))
                material.SetColor(LegacyColorName, color);
            if (material.HasProperty(ArnoldBaseColorName))
                material.SetColor(ArnoldBaseColorName, color);

            // URP Lit multiplies _BaseMap by _BaseColor. A null map is white in the editor
            // and black in Playworks, so the paint color never shows.
            Texture2D white = GetWhiteMap();
            if (material.HasProperty(BaseMapName))
                material.SetTexture(BaseMapName, white);
            if (material.HasProperty(ArnoldBaseMapName))
                material.SetTexture(ArnoldBaseMapName, white);
            if (material.HasProperty(MainTexName))
                material.SetTexture(MainTexName, white);
        }

        private Texture2D GetWhiteMap()
        {
            if (_whiteMap != null)
                return _whiteMap;

            _whiteMap = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            _whiteMap.name = "TilePaintWhite";
            _whiteMap.SetPixel(0, 0, Color.white);
            _whiteMap.Apply();
            return _whiteMap;
        }

        private void UpdatePlayerScore(int previousOwner, int newOwner)
        {
            int score = _playerScore;
            if (previousOwner == PlayerOwnerId)
                score--;
            if (newOwner == PlayerOwnerId)
                score++;

            score = Mathf.Max(0, score);
            if (score == _playerScore)
                return;

            _playerScore = score;
            PlayerScoreChanged?.Invoke(_playerScore);
        }

        private void EnsureBotPalette()
        {
            if (_botColors != null && _botColors.Length > 0)
                return;

            _botColors = new Color[]
            {
                new Color(1f, 0.22f, 0.22f),
                new Color(0.2f, 0.85f, 0.28f),
                new Color(0.12f, 0.55f, 1f),
                new Color(1f, 0.5f, 0.12f),
                new Color(0.72f, 0.28f, 1f),
                new Color(0.12f, 0.88f, 0.88f),
                new Color(1f, 0.35f, 0.72f),
                new Color(0.55f, 1f, 0.2f),
                new Color(0.95f, 0.18f, 0.55f),
                new Color(0.12f, 0.62f, 0.52f),
                new Color(0.72f, 0.42f, 0.18f),
                new Color(0.25f, 0.35f, 0.85f),
                new Color(1f, 0.42f, 0.42f),
                new Color(0.55f, 0.72f, 0.18f),
                new Color(0.48f, 0.28f, 0.82f)
            };
        }

        private struct Tile
        {
            public Renderer Renderer;
            public Vector3 Center;
            public Vector3 Extents;
            public float TopY;
            public int OwnerId;
        }
    }
}
