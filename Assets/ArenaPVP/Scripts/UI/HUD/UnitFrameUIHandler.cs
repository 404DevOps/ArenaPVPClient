using UnityEngine;
using UnityEngine.UI;

public class UnitFrameUIHandler : MonoBehaviour
{
    public bool IconRightSide;

    public Transform FrameParent;
    public Transform BarHolder;
    public Transform IconHolder;
    public AuraContainerUIHandler AuraContainer;

    public Image IconImage;
    private Healthbar _healthbar;
    private Resourcebar _resourcebar;

    public Player Player;

    [SerializeField] private bool _isPlayerFrame;
    private PlayerHealth _playerHealth;
    private PlayerResource _playerResource;

    // Start is called before the first frame update
    void OnEnable()
    {
        _healthbar = GetComponentInChildren<Healthbar>(true);
        _resourcebar = GetComponentInChildren<Resourcebar>(true);

        ClientEvents.OnPlayerInitialized.AddListener(OnPlayerInitialized);
        ClientEvents.OnPlayerHealthChanged.AddListener(OnHealthChanged);
        ClientEvents.OnPlayerResourceChanged.AddListener(OnResourceChanged);
    }
    private void OnDisable()
    {
        if (!_isPlayerFrame)
        {
            UIEvents.OnTargetChanged.RemoveListener(OnTargetChanged);
        }
        ClientEvents.OnPlayerHealthChanged.RemoveListener(OnHealthChanged);
        ClientEvents.OnPlayerInitialized.RemoveListener(OnPlayerInitialized);
        ClientEvents.OnPlayerResourceChanged.RemoveListener(OnResourceChanged);
    }

    private void OnPlayerInitialized(Player player)
    {
        if (IconRightSide)
        {
            IconHolder.SetAsLastSibling();
            var verticalLayoutGrp = BarHolder.GetComponent<VerticalLayoutGroup>();
            verticalLayoutGrp.padding.left = 5;
            verticalLayoutGrp.padding.right = 0;
        }
        if (_isPlayerFrame)
        {
            if (player.IsOwnedByMe)
            {
                Player = player;
                SetUnitFrameIcon();
                _playerHealth = Player.GetComponent<PlayerHealth>();
                _playerResource = Player.GetComponent<PlayerResource>();
                _healthbar.InitializeBar(Player.ClassType, _playerHealth.CurrentHealth.Value, _playerHealth.MaxHealth.Value);
                _resourcebar.InitializeBar(Player.ClassType, _playerResource.CurrentResource.Value, _playerResource.MaxResource.Value);
                ActivateAuraGrid();
            }
        }
        else
        {
            UIEvents.OnTargetChanged.AddListener(OnTargetChanged);
            FrameParent.gameObject.SetActive(false);
            AuraContainer.gameObject.SetActive(false);
        }
    }
    private void OnHealthChanged(HealthChangedEventArgs args)
    {
        if (Player == null || Player.Id != args.Player.Id)
            return;

        _healthbar.SetNewHealth(_playerHealth.CurrentHealth.Value, _playerHealth.MaxHealth.Value);
    }
    private void OnResourceChanged(ResourceChangedEventArgs args)
    {
        if (Player == null || Player.Id != args.Player.Id)
            return;

        _resourcebar.SetNewValue(_playerResource.CurrentResource.Value, _playerResource.MaxResource.Value);
    }
    private void OnTargetChanged(Player player)
    {
        if (player != null)
        {
            Player = player;
            _playerHealth = Player.GetComponent<PlayerHealth>();
            _playerResource = Player.GetComponent<PlayerResource>();
            SetUnitFrameIcon();
            _healthbar.InitializeBar(Player.ClassType, _playerHealth.CurrentHealth.Value, _playerHealth.MaxHealth.Value);
            _resourcebar.InitializeBar(Player.ClassType, _playerResource.CurrentResource.Value, _playerResource.MaxResource.Value);
            FrameParent.gameObject.SetActive(true);
            ActivateAuraGrid();

        }
        else
        {
            Player = null;
            AuraContainer.gameObject.SetActive(false);
            FrameParent.gameObject.SetActive(false);
        }
    }
    private void ActivateAuraGrid()
    {
        AuraContainer.InitializeGrid(Player.Id, AuraManager.Instance.GetAuraInfosForPlayer(Player.Id));
        AuraContainer.gameObject.SetActive(true);
    }
    private void SetUnitFrameIcon()
    {
        IconImage.sprite = AppearanceData.Instance().GetClassIcon(Player.ClassType);
    }
}
