using UnityEngine;

public class MarkdownNoteTaker : MonoBehaviour
{
    [Header("Window Settings")]
    [Tooltip("The physical distance (in meters) between windows")]
    public float windowSpacing = 1.0f;

    [Tooltip("Check this ONLY on the first window connected to the Laptop")]
    public bool isOriginalWindow = false;
       
    public void SpawnLeft()
    {
        SpawnNewWindow(-transform.right);
    }

    public void SpawnRight()
    {
        SpawnNewWindow(transform.right);
    }

    private void SpawnNewWindow(Vector3 direction)
    {
        Vector3 newPosition = transform.position + (direction * windowSpacing);

        GameObject newWindow = Instantiate(gameObject, newPosition, transform.rotation);

        MarkdownNoteTaker newManager = newWindow.GetComponent<MarkdownNoteTaker>();
        if (newManager != null)
        {
            newManager.isOriginalWindow = false;
        }
    }

    public void CloseWindow()
    {
        if (isOriginalWindow)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}