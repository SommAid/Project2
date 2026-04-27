using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VRWhiteboard : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IPointerUpHandler
{
    [Header("Whiteboard Settings")]
    public RawImage whiteboardSurface;
    public int textureWidth = 1024;
    public int textureHeight = 1024;
    public Color brushColor = Color.white;
    [Range(2, 50)] public int brushSize = 5;

    private Texture2D drawTexture;
    private Color[] clearPixels;

    private Vector2 lastPixelPos;
    private bool isDrawing = false;

    void Start()
    {
        drawTexture = new Texture2D(textureWidth, textureHeight);
        whiteboardSurface.texture = drawTexture;
        ClearBoard();
    }

    public void ClearBoard()
    {
        clearPixels = new Color[textureWidth * textureHeight];
        for (int i = 0; i < clearPixels.Length; i++)
        {
            clearPixels[i] = new Color(0, 0, 0, 0);
        }
        drawTexture.SetPixels(clearPixels);
        drawTexture.Apply();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDrawing = true;
        if (GetPixelPosition(eventData, out Vector2 pixelPos))
        {
            lastPixelPos = pixelPos;
            PaintCircle((int)pixelPos.x, (int)pixelPos.y);
            drawTexture.Apply();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrawing = false;
    }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDrawing) return;

        if (GetPixelPosition(eventData, out Vector2 currentPixelPos))
        {
            float distance = Vector2.Distance(lastPixelPos, currentPixelPos);

            float step = Mathf.Max(1f, brushSize / 2f);
            int numSteps = Mathf.CeilToInt(distance / step);

            for (int i = 0; i <= numSteps; i++)
            {
                float t = numSteps == 0 ? 0 : (float)i / numSteps;
                Vector2 lerpedPos = Vector2.Lerp(lastPixelPos, currentPixelPos, t);

                PaintCircle((int)lerpedPos.x, (int)lerpedPos.y);
            }

            lastPixelPos = currentPixelPos;

            drawTexture.Apply();
        }
    }

    private bool GetPixelPosition(PointerEventData eventData, out Vector2 pixelPos)
    {
        pixelPos = Vector2.zero;
        Camera cam = eventData.pressEventCamera ?? eventData.enterEventCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            whiteboardSurface.rectTransform,
            eventData.position,
            cam,
            out Vector2 localPos))
        {
            Rect rect = whiteboardSurface.rectTransform.rect;
            float px = Mathf.Clamp01((localPos.x - rect.x) / rect.width);
            float py = Mathf.Clamp01((localPos.y - rect.y) / rect.height);

            pixelPos.x = Mathf.RoundToInt(px * textureWidth);
            pixelPos.y = Mathf.RoundToInt(py * textureHeight);
            return true;
        }
        return false;
    }

    private void PaintCircle(int x, int y)
    {
        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                if (i * i + j * j <= brushSize * brushSize)
                {
                    int drawX = x + i;
                    int drawY = y + j;

                    if (drawX >= 0 && drawX < textureWidth && drawY >= 0 && drawY < textureHeight)
                    {
                        drawTexture.SetPixel(drawX, drawY, brushColor);
                    }
                }
            }
        }
    }
}