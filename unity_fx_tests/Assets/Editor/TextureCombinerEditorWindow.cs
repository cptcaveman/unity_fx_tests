using UnityEngine;
using UnityEditor;

public class TextureCombinerWindow : EditorWindow
{
    private Material m_rgbaMat;
    private Texture2D m_prevMadeTexture;
    private const int m_textureSize = 64;
    private const int m_labelHeight = 16;
    private const int m_padding = 10;

    Rect m_textureRect = new Rect();
    Rect m_labelRect = new Rect();

    Texture2D m_texR;
    Texture2D m_texG;
    Texture2D m_texB;
    Texture2D m_texA;

    [MenuItem("Tools/Texture Combiner")]
    public static void CreateWindow()
    {
        TextureCombinerWindow window = GetWindow<TextureCombinerWindow>();
        window.Show();
    }

    private void OnEnable()
    {
        m_rgbaMat = new Material(Shader.Find("Hidden/RGBAChannelCombiner"));
        m_prevMadeTexture = null;
    }

    private void DrawTextureField(string label, ref Texture2D tex, float x, float y)
    {
        m_labelRect.x = x;
        m_labelRect.y = y;
        GUI.Label(m_labelRect, label);

        m_textureRect.x = x;
        m_textureRect.y = y + m_labelHeight;
        tex = (Texture2D)EditorGUI.ObjectField(m_textureRect, tex, typeof(Texture2D), false);
    }

    private void OnGUI()
    {
        float padding = m_padding;
        float y = padding;

        // set up default dimensions and positions of rects
        m_labelRect.y = y;
        m_labelRect.height = m_labelHeight * 2;
        m_labelRect.width = m_textureSize * 5;

        EditorGUI.HelpBox(m_labelRect, "Please ensure the textures are of the same dimensions", MessageType.Info);

        y += m_labelRect.height;

        m_labelRect.height = m_labelHeight;
        m_labelRect.width = m_textureRect.width = m_textureRect.height = m_textureSize;

        DrawTextureField("R Channel:", ref m_texR, padding, y);
        y += m_labelHeight + m_textureSize;

        DrawTextureField("G Channel:", ref m_texG, padding, y);
        y += m_labelHeight + m_textureSize;

        DrawTextureField("B Channel:", ref m_texB, padding, y);
        y += m_labelHeight + m_textureSize;

        DrawTextureField("A Channel:", ref m_texA, padding, y);
        y += m_labelHeight + m_textureSize;

        m_labelRect.y = this.position.height - padding * 2;
        m_labelRect.width *= 2;
        
        if (GUI.Button(m_labelRect, "Combine Textures"))
        {
            // Combine Textures
            RenderTexture rt = new RenderTexture(m_texR.width, m_texR.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);

            m_rgbaMat.SetTexture("_TexG", m_texG);
            m_rgbaMat.SetTexture("_TexB", m_texB);
            m_rgbaMat.SetTexture("_TexA", m_texA);

            Graphics.Blit(m_texR, rt, m_rgbaMat);

            RenderTexture.active = rt;

            m_prevMadeTexture = new Texture2D(m_texR.width, m_texR.height);
            m_prevMadeTexture.ReadPixels(new Rect(0, 0, m_prevMadeTexture.width, m_prevMadeTexture.height), 0, 0);
            m_prevMadeTexture.Apply();
            
            RenderTexture.active = null;
            rt.Release();

            byte[] bytes = m_prevMadeTexture.EncodeToPNG();

            using (System.IO.FileStream fs = new System.IO.FileStream(Application.dataPath + "/New RGBA Channel Texture.png", System.IO.FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }

            AssetDatabase.Refresh();
        }

        if(m_prevMadeTexture)
        {
            // draw texture to the right
            m_textureRect.x += m_textureSize + padding;
            m_textureRect.width = m_textureRect.height = m_textureSize * 5;
            m_textureRect.y = this.position.height / 2 - m_textureRect.height / 2;
            GUI.DrawTexture(m_textureRect, m_prevMadeTexture, ScaleMode.ScaleToFit);
        }
    }

    private void OnDisable()
    {
        Debug.Log("asdfasdf");
    }
}
