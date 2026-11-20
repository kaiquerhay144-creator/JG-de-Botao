using UnityEngine;

public class JGGameBootstrap : MonoBehaviour
{
    [Header("Field")]
    public float fieldLength = 18f;
    public float fieldWidth = 10f;
    public Material grassMaterial;
    public Material lineMaterial;

    private void Start()
    {
        BuildLighting();
        BuildField();
        BuildGoals();
        BuildTeams();
        BuildBall();
        BuildCamera();
        BuildHud();
    }

    private void BuildLighting()
    {
        RenderSettings.ambientLight = new Color(0.62f, 0.66f, 0.72f);
        var sun = new GameObject("Sun").AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.25f;
        sun.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
    }

    private void BuildField()
    {
        var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Gramado RHAY";
        ground.transform.position = new Vector3(0, -0.18f, 0);
        ground.transform.localScale = new Vector3(fieldLength, 0.35f, fieldWidth);
        ground.GetComponent<Renderer>().material = MakeMaterial(new Color(0.08f, 0.38f, 0.16f));

        float y = 0.005f;
        MakeLine("Linha lateral norte", new Vector3(0, y, fieldWidth / 2), new Vector3(fieldLength, 0.035f, 0.035f));
        MakeLine("Linha lateral sul", new Vector3(0, y, -fieldWidth / 2), new Vector3(fieldLength, 0.035f, 0.035f));
        MakeLine("Linha de fundo esquerda", new Vector3(-fieldLength / 2, y, 0), new Vector3(0.035f, 0.035f, fieldWidth));
        MakeLine("Linha de fundo direita", new Vector3(fieldLength / 2, y, 0), new Vector3(0.035f, 0.035f, fieldWidth));
        MakeLine("Meio campo", Vector3.zero + Vector3.up * y, new Vector3(0.035f, 0.035f, fieldWidth));
        var circle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        circle.name = "Circulo central";
        circle.transform.position = new Vector3(0, 0.012f, 0);
        circle.transform.localScale = new Vector3(2.2f, 0.008f, 2.2f);
        circle.GetComponent<Renderer>().material = MakeMaterial(new Color(0.86f, 0.92f, 0.86f));
        Object.Destroy(circle.GetComponent<Collider>());
        var inner = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        inner.name = "Centro do gramado";
        inner.transform.position = new Vector3(0, 0.02f, 0);
        inner.transform.localScale = new Vector3(2.12f, 0.009f, 2.12f);
        inner.GetComponent<Renderer>().material = MakeMaterial(new Color(0.08f, 0.38f, 0.16f));
        Object.Destroy(inner.GetComponent<Collider>());

        for (int i = -4; i <= 4; i++)
        {
            var stripe = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stripe.name = "Faixa do gramado";
            stripe.transform.position = new Vector3(i * 2f, 0.002f, 0);
            stripe.transform.localScale = new Vector3(0.95f, 0.008f, fieldWidth - 0.08f);
            stripe.GetComponent<Renderer>().material = MakeMaterial(i % 2 == 0 ? new Color(0.1f, 0.43f, 0.18f) : new Color(0.07f, 0.34f, 0.14f));
            Object.Destroy(stripe.GetComponent<Collider>());
        }

        for (int i = -1; i <= 1; i++)
        {
            var board = GameObject.CreatePrimitive(PrimitiveType.Cube);
            board.name = "Placa LED RHAY";
            board.transform.position = new Vector3(i * 5.8f, 0.55f, fieldWidth / 2 + 0.28f);
            board.transform.localScale = new Vector3(4.6f, 0.7f, 0.12f);
            board.GetComponent<Renderer>().material = MakeMaterial(new Color(0.02f, 0.08f, 0.16f), 0.25f);
            var label = new GameObject("RHAY");
            label.transform.SetParent(board.transform, false);
            label.transform.localPosition = new Vector3(0, 0, -0.55f);
            var text = label.AddComponent<TextMesh>();
            text.text = "RHAY";
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.fontSize = 72;
            text.characterSize = 0.12f;
            text.color = new Color(0.1f, 0.85f, 1f);
        }
    }

    private void MakeLine(string objectName, Vector3 position, Vector3 scale)
    {
        var line = GameObject.CreatePrimitive(PrimitiveType.Cube);
        line.name = objectName;
        line.transform.position = position;
        line.transform.localScale = scale;
        line.GetComponent<Renderer>().material = MakeMaterial(Color.white);
        Object.Destroy(line.GetComponent<Collider>());
    }

    private void BuildGoals()
    {
        for (int side = -1; side <= 1; side += 2)
        {
            float x = side * (fieldLength / 2 + 0.42f);
            MakePost(new Vector3(x, 0.55f, -1.25f));
            MakePost(new Vector3(x, 0.55f, 1.25f));
            MakePost(new Vector3(x, 1.55f, 0), true);
            var net = GameObject.CreatePrimitive(PrimitiveType.Cube);
            net.name = "Rede do gol";
            net.transform.position = new Vector3(x, 0.52f, 0);
            net.transform.localScale = new Vector3(0.08f, 1.05f, 2.5f);
            net.GetComponent<Renderer>().material = MakeMaterial(new Color(0.8f, 0.85f, 0.9f, 0.35f));
        }
    }

    private void MakePost(Vector3 position, bool crossbar = false)
    {
        var post = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        post.name = "Trave";
        post.transform.position = position;
        post.transform.localScale = crossbar ? new Vector3(0.07f, 1.25f, 0.07f) : new Vector3(0.07f, 0.55f, 0.07f);
        if (crossbar) post.transform.rotation = Quaternion.Euler(0, 0, 90);
        post.GetComponent<Renderer>().material = MakeMaterial(Color.white);
    }

    private void BuildTeams()
    {
        for (int team = 0; team < 2; team++)
        {
            Color color = team == 0 ? new Color(0.82f, 0.06f, 0.08f) : new Color(0.04f, 0.12f, 0.72f);
            for (int i = 0; i < 11; i++)
            {
                int row = i == 0 ? 0 : (i - 1) / 4 + 1;
                int col = i == 0 ? 0 : (i - 1) % 4;
                float x = team == 0 ? -7.1f + row * 2.0f : 7.1f - row * 2.0f;
                float z = i == 0 ? 0 : (col - 1.5f) * 1.65f;
                var puck = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                puck.name = (team == 0 ? "Time Vermelho " : "Time Azul ") + (i + 1);
                puck.transform.position = new Vector3(x, 0.22f, z);
                puck.transform.localScale = new Vector3(0.72f, 0.2f, 0.72f);
                puck.GetComponent<Renderer>().material = MakeMaterial(color);
                var body = puck.AddComponent<Rigidbody>();
                body.mass = 0.45f;
                body.drag = 1.2f;
                body.angularDrag = 2.5f;
                body.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                var number = new GameObject("Numero");
                number.transform.SetParent(puck.transform, false);
                number.transform.localPosition = new Vector3(0, 0.56f, 0);
                var tm = number.AddComponent<TextMesh>();
                tm.text = (i + 1).ToString();
                tm.anchor = TextAnchor.MiddleCenter;
                tm.alignment = TextAlignment.Center;
                tm.fontSize = 64;
                tm.characterSize = 0.12f;
                tm.color = Color.white;
            }
        }
    }

    private void BuildBall()
    {
        var ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = "Bola";
        ball.transform.position = new Vector3(0, 0.18f, 0);
        ball.transform.localScale = Vector3.one * 0.32f;
        ball.GetComponent<Renderer>().material = MakeMaterial(Color.white);
        var rb = ball.AddComponent<Rigidbody>();
        rb.mass = 0.18f;
        rb.drag = 0.8f;
        rb.angularDrag = 0.6f;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }

    private void BuildCamera()
    {
        var cameraObject = new GameObject("Camera JG");
        var camera = cameraObject.AddComponent<Camera>();
        cameraObject.tag = "MainCamera";
        cameraObject.transform.position = new Vector3(0, 16, -12);
        cameraObject.transform.rotation = Quaternion.Euler(55, 0, 0);
        camera.fieldOfView = 48;
        cameraObject.AddComponent<JGTouchController>();
    }

    private void BuildHud()
    {
        var canvasObject = new GameObject("HUD");
        var canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        var label = new GameObject("Instrucoes");
        label.transform.SetParent(canvas.transform, false);
        var rect = label.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.05f, 0.88f);
        rect.anchorMax = new Vector2(0.95f, 0.98f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        var text = label.AddComponent<UnityEngine.UI.Text>();
        text.text = "JG DE BOTÃO  •  RHAY     |     Toque e arraste um botão para jogar";
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 22;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
    }

    private Material MakeMaterial(Color color, float metallic = 0f)
    {
        var material = new Material(Shader.Find("Standard"));
        material.color = color;
        material.SetFloat("_Metallic", metallic);
        material.SetFloat("_Glossiness", 0.35f);
        return material;
    }
}
