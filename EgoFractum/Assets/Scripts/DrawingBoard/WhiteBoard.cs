using UnityEngine;
using UnityEngine.Rendering;

public class WhiteBoard : MonoBehaviour
{
   public Texture2D texture;
   public Vector2 textureSize = new Vector2(2048, 2028);

   void Start()
   {
      var r = GetComponent<Renderer>();
      texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
      r.material.mainTexture = texture;
   }
}
