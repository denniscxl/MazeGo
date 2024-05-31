using UnityEngine;
using GKBase;
using UnityEngine.UI;

public class FogOfWar : GKSingleton<FogOfWar>
{
    private int mapWidth;
    private int mapHeight;
    private bool[,] fogGrid;
    private float viewRadius;
    private Texture2D fogTexture;
    //private SpriteRenderer fogRenderer;
    private RawImage fogRenderer;

    public delegate Vector2Int GetPosition();
    public GetPosition GetPlayerGridPosition = null;

    public void Init(int width, int height, int tileSize, float vr, RawImage renderer, GetPosition fun)
    {
        mapWidth = width;
        mapHeight = height;
        viewRadius = vr;

        fogRenderer = renderer;
        fogRenderer.GetComponent<RectTransform>().sizeDelta = new Vector2(mapWidth * tileSize, mapHeight * tileSize);
        fogRenderer.GetComponent<RectTransform>().anchoredPosition = new Vector2((mapWidth * tileSize) * 0.5f, (mapHeight * tileSize) * 0.5f);

        fogGrid = new bool[mapWidth, mapHeight];
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                fogGrid[x, y] = true;
            }
        }

        fogTexture = new Texture2D(mapWidth, mapHeight);

        GetPlayerGridPosition -= fun;
        GetPlayerGridPosition += fun;
    }

    public void Update()
    {
        UpdateFogGrid();
        UpdateFogTextureSimple();
        //UpdateFogTexture();
    }

    private void UpdateFogGrid()
    {
        Vector2Int playerPos = Vector2Int.zero;
        if (GetPlayerGridPosition != null)
            playerPos = GetPlayerGridPosition();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                if (Vector2Int.Distance(playerPos, gridPos) <= viewRadius)
                {
                    fogGrid[x, y] = false;
                }
            }
        }
    }

    private void UpdateFogTextureSimple()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (fogGrid[x, y])
                {
                    fogTexture.SetPixel(x, y, Color.black);
                }
                else
                {
                    fogTexture.SetPixel(x, y, Color.clear);
                }
            }
        }

        fogTexture.Apply();
        fogRenderer.texture = fogTexture;
    }

    private void UpdateFogTexture()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float distance = Vector2Int.Distance(GetPlayerGridPosition(), new Vector2Int(x, y));
                float opacity = Mathf.Clamp01(distance / viewRadius);
                fogTexture.SetPixel(x, y, new Color(0, 0, 0, opacity));
            }
        }

        fogTexture.Apply();

        // 应用高斯模糊
        int blurIterations = 2;
        int blurSize = 2;
        for (int i = 0; i < blurIterations; i++)
        {
            fogTexture = BlurTexture(fogTexture, blurSize);
        }

        fogRenderer.texture = fogTexture;
    }

    private Texture2D BlurTexture(Texture2D texture, int blurSize)
    {
        Texture2D blurredTexture = new Texture2D(texture.width, texture.height);
        int kernelSize = blurSize * 2 + 1;
        float[,] kernel = GaussianKernel(kernelSize);

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Color blurredColor = ConvolveTexture(texture, kernel, x, y);
                blurredTexture.SetPixel(x, y, blurredColor);
            }
        }

        blurredTexture.Apply();
        return blurredTexture;
    }

    private Color ConvolveTexture(Texture2D texture, float[,] kernel, int x, int y)
    {
        int kernelSize = kernel.GetLength(0);
        float rSum = 0, gSum = 0, bSum = 0, aSum = 0;
        int kernelOffset = (kernelSize - 1) / 2;

        for (int i = 0; i < kernelSize; i++)
        {
            for (int j = 0; j < kernelSize; j++)
            {
                int sampleX = Mathf.Clamp(x + i - kernelOffset, 0, texture.width - 1);
                int sampleY = Mathf.Clamp(y + j - kernelOffset, 0, texture.height - 1);
                Color sampleColor = texture.GetPixel(sampleX, sampleY);

                float kernelValue = kernel[i, j];
                rSum += sampleColor.r * kernelValue;
                gSum += sampleColor.g * kernelValue;
                bSum += sampleColor.b * kernelValue;
                aSum += sampleColor.a * kernelValue;
            }
        }

        return new Color(rSum, gSum, bSum, aSum);
    }

    private float[,] GaussianKernel(int size)
    {
        float[,] kernel = new float[size, size];
        float sigma = size / 3f;
        float sum = 0;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i - (size - 1) / 2f;
                float y = j - (size - 1) / 2f;
                float value = Mathf.Exp(-(x * x + y * y) / (2 * sigma * sigma)) / (2 * Mathf.PI * sigma * sigma);
                kernel[i, j] = value;
                sum += value;
            }
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                kernel[i, j] /= sum;
            }
        }

        return kernel;
    }
}