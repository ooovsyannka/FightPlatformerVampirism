using UnityEngine;

public class InputReader : MonoBehaviour
{
    private const string Horizontal = nameof(Horizontal);
    private const string Vertical = nameof(Vertical);

    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }
    public bool IsReload { get; private set; }
    public bool IsDash { get; private set; }
    public bool IsShoot {  get; private set; }
    public Vector3 MousePosition { get; private set; }
    public bool UseSkill { get; private set; }

    private void Update()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        HorizontalInput = Input.GetAxisRaw(Horizontal);
        VerticalInput = Input.GetAxisRaw(Vertical);

        IsShoot = Input.GetMouseButton(0);
        IsReload = Input.GetKeyDown(KeyCode.R);
        IsDash = Input.GetKey(KeyCode.LeftControl);
        UseSkill = Input.GetKeyDown(KeyCode.E);
    }
}
