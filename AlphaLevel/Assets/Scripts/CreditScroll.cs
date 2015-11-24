using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.IO;  

public class CreditScroll : MonoBehaviour {

	float y;
	RectTransform rTransform;

	// Use this for initialization
	void Start () {
		rTransform = GetComponent<RectTransform> ();

		StreamReader reader = new StreamReader("Assets/credits.txt", Encoding.Default);

		string line;
		string text = "";

		using (reader)
		{
			// While there's lines left in the text file, do this:
			do
			{
				line = reader.ReadLine();
				text += line + "\n";
				
			} while (line != null);   
			reader.Close();
		}

		GetComponentInChildren<Text> ().text = text;
		float height = LayoutUtility.GetPreferredHeight (rTransform);
		rTransform.anchoredPosition = new Vector2 (0, -rTransform.rect.height);
		y = rTransform.anchoredPosition.y;
	}

	// Update is called once per frame
	void Update () {
		y += Time.deltaTime * 40;
		rTransform.anchoredPosition = new Vector2 (0, y);
		if (y > rTransform.rect.height) {
			Destroy(this);
		}
	}
}
