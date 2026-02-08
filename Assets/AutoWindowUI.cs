using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoWindowUI : MonoBehaviour
{
    [Header("Opcional: asigna 4 sprites para los iconos")]
    public Sprite[] iconSprites = new Sprite[4];

    [Header("URLs para enlaces (3)")]
    public string[] urls = new string[3]
    {
        "https://blog.febucci.com/",
        "https://docs.unity3d.com/",
        "https://github.com/"
    };

    [Header("Textos")]
    public string title = "Ventana UI Responsiva";
    public string[] buttonTexts = new string[3] { "Botón 1", "Botón 2", "Botón 3" };
    public string[] linkTexts = new string[3] { "Enlace 1", "Enlace 2", "Enlace 3" };

    void Start()
    {
        EnsureEventSystem();
        Canvas canvas = EnsureCanvas();

        // ===== WindowRoot =====
        GameObject windowRoot = CreateUIObject("WindowRoot", canvas.transform);
        Image windowImage = windowRoot.AddComponent<Image>();
        windowImage.color = new Color(0.12f, 0.12f, 0.12f, 0.9f);

        RectTransform windowRT = windowRoot.GetComponent<RectTransform>();
        StretchWithMargin(windowRT, 24);

        VerticalLayoutGroup rootLayout = windowRoot.AddComponent<VerticalLayoutGroup>();
        rootLayout.padding = new RectOffset(16, 16, 16, 16);
        rootLayout.spacing = 16;
        rootLayout.childControlWidth = true;
        rootLayout.childControlHeight = true;
        rootLayout.childForceExpandWidth = true;
        rootLayout.childForceExpandHeight = true;

        // ===== TopBar =====
        GameObject topBar = CreateUIObject("TopBar", windowRoot.transform);
        LayoutElement topLE = topBar.AddComponent<LayoutElement>();
        topLE.preferredHeight = 90;

        HorizontalLayoutGroup topLayout = topBar.AddComponent<HorizontalLayoutGroup>();
        topLayout.padding = new RectOffset(12, 12, 12, 12);
        topLayout.childAlignment = TextAnchor.MiddleLeft;
        topLayout.childControlWidth = true;
        topLayout.childControlHeight = true;
        topLayout.childForceExpandWidth = true;
        topLayout.childForceExpandHeight = true;

        GameObject titleGO = CreateUIObject("Title", topBar.transform);
        Text titleText = titleGO.AddComponent<Text>();
        titleText.text = title;
        titleText.font = GetBuiltinFont();
        titleText.fontSize = 42;
        titleText.color = Color.white;
        titleText.alignment = TextAnchor.MiddleLeft;

        // ===== ContentRow =====
        GameObject contentRow = CreateUIObject("ContentRow", windowRoot.transform);
        HorizontalLayoutGroup contentLayout = contentRow.AddComponent<HorizontalLayoutGroup>();
        contentLayout.spacing = 16;
        contentLayout.childControlWidth = true;
        contentLayout.childControlHeight = true;
        contentLayout.childForceExpandWidth = true;
        contentLayout.childForceExpandHeight = true;

        // ===== MainColumn =====
        GameObject mainColumn = CreateUIObject("MainColumn", contentRow.transform);
        VerticalLayoutGroup mainLayout = mainColumn.AddComponent<VerticalLayoutGroup>();
        mainLayout.spacing = 16;
        mainLayout.childControlWidth = true;
        mainLayout.childControlHeight = true;
        mainLayout.childForceExpandWidth = true;
        mainLayout.childForceExpandHeight = true;

        // ===== LinksColumn =====
        GameObject linksColumn = CreateUIObject("LinksColumn", contentRow.transform);
        LayoutElement linksLE = linksColumn.AddComponent<LayoutElement>();
        linksLE.preferredWidth = 280;

        VerticalLayoutGroup linksLayout = linksColumn.AddComponent<VerticalLayoutGroup>();
        linksLayout.spacing = 10;
        linksLayout.childControlWidth = true;
        linksLayout.childControlHeight = true;
        linksLayout.childForceExpandWidth = true;
        linksLayout.childForceExpandHeight = false;

        // ===== IconsRow =====
        GameObject iconsRow = CreateUIObject("IconsRow", mainColumn.transform);
        HorizontalLayoutGroup iconsLayout = iconsRow.AddComponent<HorizontalLayoutGroup>();
        iconsLayout.spacing = 12;

        for (int i = 0; i < 4; i++)
        {
            GameObject icon = CreateUIObject($"Icon{i + 1}", iconsRow.transform);
            Image img = icon.AddComponent<Image>();
            img.color = Color.white;

            if (iconSprites != null && i < iconSprites.Length && iconSprites[i] != null)
            {
                img.sprite = iconSprites[i];
                img.preserveAspect = true;
            }

            LayoutElement le = icon.AddComponent<LayoutElement>();
            le.preferredWidth = 64;
            le.preferredHeight = 64;
        }

        // ===== ButtonsColumn =====
        GameObject buttonsColumn = CreateUIObject("ButtonsColumn", mainColumn.transform);
        VerticalLayoutGroup buttonsLayout = buttonsColumn.AddComponent<VerticalLayoutGroup>();
        buttonsLayout.spacing = 10;
        buttonsLayout.childForceExpandHeight = false;

        for (int i = 0; i < 3; i++)
        {
            GameObject btn = CreateButton(buttonsColumn.transform, buttonTexts[i]);
            btn.GetComponent<LayoutElement>().preferredHeight = 52;

            int idx = i;
            btn.GetComponent<Button>().onClick.AddListener(() =>
                Debug.Log($"Click en Botón {idx + 1}")
            );
        }

        // ===== Links =====
        for (int i = 0; i < 3; i++)
        {
            GameObject link = CreateButton(linksColumn.transform, linkTexts[i]);
            link.GetComponent<LayoutElement>().preferredHeight = 44;

            Image img = link.GetComponent<Image>();
            img.color = new Color(0.15f, 0.25f, 0.45f);

            string url = urls[i];
            link.GetComponent<Button>().onClick.AddListener(() =>
                Application.OpenURL(url)
            );
        }
    }

    // ================= HELPERS =================

    private Font GetBuiltinFont()
    {
        return Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }

    private void EnsureEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() != null) return;
        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
    }

    private Canvas EnsureCanvas()
    {
        Canvas existing = FindFirstObjectByType<Canvas>();
        if (existing != null) return existing;

        GameObject go = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = go.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        return canvas;
    }

    private GameObject CreateUIObject(string name, Transform parent)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go;
    }

    private void StretchWithMargin(RectTransform rt, float margin)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(margin, margin);
        rt.offsetMax = new Vector2(-margin, -margin);
    }

    private GameObject CreateButton(Transform parent, string label)
    {
        GameObject btnGO = CreateUIObject(label, parent);

        Image img = btnGO.AddComponent<Image>();
        img.color = new Color(0.2f, 0.2f, 0.2f);

        Button btn = btnGO.AddComponent<Button>();
        LayoutElement le = btnGO.AddComponent<LayoutElement>();

        GameObject textGO = CreateUIObject("Text", btnGO.transform);
        Text text = textGO.AddComponent<Text>();
        text.text = label;
        text.font = GetBuiltinFont();
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;

        RectTransform tr = textGO.GetComponent<RectTransform>();
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = Vector2.zero;
        tr.offsetMax = Vector2.zero;

        le.preferredHeight = 48;

        return btnGO;
    }
}
