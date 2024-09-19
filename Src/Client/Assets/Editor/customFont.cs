using UnityEngine;
using UnityEditor;
using System.IO;

public class CustomFont : MonoBehaviour
{
    //��������ͨ�����е�sprite���������ļ�������ʹ�õ���unity�Դ���sprite editor�����������
    //���⣬����֮��ÿ��sprite�����ֵ����һ���ַ���Ӧ��ascii��ı��룬���磺
    //0�� ����ֻҪ��sprite������������xxx0���Ϳ����ˣ�
    //����ʹ�õ�����sprite���أ���������ͼƬ�����ResourcesĿ¼���棬��������ϣ��ٰ����Ƿŵ�fonts�ļ��л��������ļ����м��ɡ�
    [MenuItem("Assets/CreateMyFontSprite")]
    static void CreateMyFontSprite()
    {

        Debug.LogWarning("abc");

        if (Selection.objects == null)
            return;
        if (Selection.objects.Length == 0)
        {
            Debug.LogWarning("û��ѡ��Sprite�ļ�����Ҫ��Sprite Mode���ó�Multiple���зֺã������������ֵ����һ���ַ�����ascii��");
            return;
        }
        string resoursePath = "Resources";
        UnityEngine.Object o = Selection.objects[0];
        if (o.GetType() != typeof(Texture2D))
        {
            Debug.LogWarning("ѡ�еĲ�����ͼƬ�ļ�");
            return;
        }
        string selectionPath = AssetDatabase.GetAssetPath(o);
        if (selectionPath.Contains(resoursePath))
        {
            string selectionExt = Path.GetExtension(selectionPath);
            if (selectionExt.Length == 0)
            {
                return;
            }
            string loadPath = selectionPath.Remove(selectionPath.Length - selectionExt.Length);
            string fontPathName = loadPath + ".fontsettings";
            string matPathName = loadPath + ".mat";
            float lineSpace = 0.1f;//�����м�࣬����������ߵ�����õ��м�࣬����ǹ̶��߶ȣ��������������е��� 
            loadPath = Path.GetFileNameWithoutExtension(selectionPath);
            Sprite[] sprites = Resources.LoadAll<Sprite>(loadPath);
            if (sprites.Length > 0)
            {
                //��textrue��ʽ��ø���Դ���������õ������Ĳ�����ȥ 
                Texture2D tex = o as Texture2D;
                //����������ʣ����ҽ�ͼƬ���ú� 
                Material mat = new Material(Shader.Find("GUI/Text Shader"));
                AssetDatabase.CreateAsset(mat, matPathName);
                mat.SetTexture("_MainTex", tex);
                //���������ļ������������ļ��Ĳ��� 
                Font m_myFont = new Font();
                m_myFont.material = mat;
                AssetDatabase.CreateAsset(m_myFont, fontPathName);
                //���������е��ַ������� 
                CharacterInfo[] characterInfo = new CharacterInfo[sprites.Length];
                //�õ���ߵĸ߶ȣ������иߺͽ���ƫ�Ƽ��� 
                for (int i = 0; i < sprites.Length; i++)
                {
                    if (sprites[i].rect.height > lineSpace)
                    {
                        lineSpace = sprites[i].rect.height;
                    }
                }
                for (int i = 0; i < sprites.Length; i++)
                {
                    Sprite spr = sprites[i];
                    CharacterInfo info = new CharacterInfo();
                    //����ascii�룬ʹ���з�sprite�����һ����ĸ 
                    info.index = (int)spr.name[spr.name.Length - 1];
                    Rect rect = spr.rect;
                    //����pivot�����ַ���ƫ�ƣ�������Ҫ����ʲô���ģ����Ը����Լ���Ҫ�޸Ĺ�ʽ 
                    float pivot = spr.pivot.y / rect.height - 0.5f;
                    if (pivot > 0)
                    {
                        pivot = -lineSpace / 2 - spr.pivot.y;
                    }
                    else if (pivot < 0)
                    {
                        pivot = -lineSpace / 2 + rect.height - spr.pivot.y;
                    }
                    else
                    {
                        pivot = -lineSpace / 2;
                    }
                    Debug.Log(pivot);
                    int offsetY = (int)(pivot + (lineSpace - rect.height) / 2);
                    //�����ַ�ӳ�䵽�����ϵ����� 
                    info.uvBottomLeft = new Vector2((float)rect.x / tex.width, (float)(rect.y) / tex.height);
                    info.uvBottomRight = new Vector2((float)(rect.x + rect.width) / tex.width, (float)(rect.y) / tex.height);
                    info.uvTopLeft = new Vector2((float)rect.x / tex.width, (float)(rect.y + rect.height) / tex.height);
                    info.uvTopRight = new Vector2((float)(rect.x + rect.width) / tex.width, (float)(rect.y + rect.height) / tex.height);
                    //�����ַ������ƫ��λ�úͿ�� 
                    info.minX = 0;
                    info.minY = -(int)rect.height - offsetY;
                    info.maxX = (int)rect.width;
                    info.maxY = -offsetY;
                    //�����ַ��Ŀ�� 
                    info.advance = (int)rect.width;
                    characterInfo[i] = info;
                }
                // lineSpace += 2; 
                m_myFont.characterInfo = characterInfo;
                EditorUtility.SetDirty(m_myFont);//���ñ��������Դ 
                AssetDatabase.SaveAssets();//����������Դ 
                AssetDatabase.Refresh();//ˢ����Դ��ò����Mac�ϲ������� 

                //��������fresh֮���ڱ༭������Ȼû��ˢ�£�������ʱ�뵽��������� 
                //�Ȱ����ɵ����嵼����һ������Ȼ�������µ�������������Ϳ���ֱ��ˢ���� 
                //������Mac�������ģ���֪��Windows����᲻����֣���������ֿ��԰�������һ��ע�͵� 
                AssetDatabase.ExportPackage(fontPathName, "temp.unitypackage");
                AssetDatabase.DeleteAsset(fontPathName);
                AssetDatabase.ImportPackage("temp.unitypackage", true);
                AssetDatabase.Refresh();

                //��Ѹ߶ȣ����¸���һ�����صļ�࣬�������Ҫ����ע�͵�������������� 
                //��ӡ��Ϊ��ʹʹ���߷�����д�иߣ���Ϊfont��֧�������иߡ� 
                Debug.Log("��������ɹ�, ���߶ȣ�" + lineSpace + ", ��Ѹ߶ȣ�" + (lineSpace + 2));
            }
            else
            {
                Debug.LogWarning("û��ѡ��Sprite�ļ�����Ҫ��Sprite�ŵ�Resources�ļ������棬���Բο������Ϸ���˵������");
            }
        }
    }
}