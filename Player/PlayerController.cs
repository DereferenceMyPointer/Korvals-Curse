using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Controller behavior for the player character (user input)
 * Name attacks after respective input axis required
 */
public class PlayerController : MonoBehaviour
{
    public Character c;
    public Animator anim;
    public Slider HPBar;
    public LayerMask interactable;
    public TextMeshProUGUI HPText;
    public UIFade[] deathFaders;
    public enum Mode
    {
        Normal,
        Insanity
    }

    public Mode mode;

    // Handle frame behaviors
    private void Update()
    {
        if (!c)
        {
            HPBar.value = 0;
            HPText.text = "DEAD";
            foreach(UIFade fader in deathFaders)
            {
                fader.Fade();
            }
            StartCoroutine(EndGame());
            SoundtrackManager.Instance.End();
        } else
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            c.target = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
            Vector2 lookDirection = (Vector2)c.transform.position + c.target;
            Vector2 moveDirection = new Vector2(x, y).normalized;
            c.Move(moveDirection);
            anim.SetBool("Left", x != 0);
            anim.SetBool("Up", y > 0);
            anim.SetBool("Down", y < 0);
            foreach (GameObject o in c.attackObjects)
            {
                if (Input.GetButton(o.name))
                {
                    c.Attack(o.name);
                }
            }
            if (Input.GetButtonDown("Interact"))
            {
                RaycastHit2D hit = Physics2D.Raycast(c.transform.position, lookDirection, lookDirection.magnitude, interactable);
                if (hit)
                {
                    hit.rigidbody.GetComponent<IInteractable>().Interact(c);
                }
            }
            HandleUI();
        }
    }

    // Update UI elements
    void HandleUI()
    {
        HPBar.value = c.Health / (c.defaultHealth * c.inventory.GetMultiplier("health"));
        HPText.text = "" + (int)c.Health + "/" + (int)(c.defaultHealth * c.inventory.GetMultiplier("health"));
    }

    private IEnumerator EndGame()
    {
        if(mode == Mode.Normal && !PlayerPrefs.HasKey("MaxNormal"))
            PlayerPrefs.SetString("MaxNormal", "" + ProgressionManager.Instance.progressionLevel);
        else if (mode == Mode.Normal && int.Parse(PlayerPrefs.GetString("MaxNormal")) < ProgressionManager.Instance.progressionLevel)
            PlayerPrefs.SetString("MaxNormal", "" + ProgressionManager.Instance.progressionLevel);
        if (mode == Mode.Insanity && !PlayerPrefs.HasKey("MaxInsanity"))
            PlayerPrefs.SetString("MaxInsanity", "" + ProgressionManager.Instance.progressionLevel);
        else if (mode == Mode.Insanity && int.Parse(PlayerPrefs.GetString("MaxInsanity")) < ProgressionManager.Instance.progressionLevel)
            PlayerPrefs.SetString("MaxInsanity", "" + ProgressionManager.Instance.progressionLevel);
        PlayerPrefs.Save();
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }

}
