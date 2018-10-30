using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour {

	[SerializeField]
	private bool m_startsVisible = false;
	[SerializeField]
	private bool m_fadeOnAwake = false;
	[SerializeField]
	private bool m_continuous = false;
	[SerializeField]
	private float m_fadeSpeed = 1.0f;
	[SerializeField]
	private float m_minAlpha = 0;
	[SerializeField]
	private float m_maxAlpha = 1.0f;
    Image image = null;

	public bool isVisible {
		get { 
			if (image.color.a == m_maxAlpha)
				return true;
			else return false;
		}
	}
	public bool isHidden {
		get { 
			if (image.color.a == m_minAlpha)
				return true;
			else return false;
		}
	}
	public float fadeSpeed {
		get { return m_fadeSpeed; }
		set { m_fadeSpeed = value; }
	}
	public float minAlpha {
		get { return m_minAlpha; }
		set { m_minAlpha = value; }
	}
	public float maxAlpha {
		get { return m_maxAlpha; }
		set { m_maxAlpha = value; }
	}
	public bool continuous {
		get { return m_continuous; }
		set { m_continuous = value; }
	}

	void Start () {
		image = GetComponent<Image>();
		if (image == null)
		{
			Debug.LogError("FadeImage: No Image found!");
			return;
		}
		
		
		if (m_startsVisible)
		{
			Color imageColor = image.color;
			imageColor.a = m_maxAlpha;
			image.color = imageColor;
			if (m_fadeOnAwake)
				StartCoroutine(FadeOut(null));
		}
		else
		{
			Color imageColor = image.color;
			imageColor.a = m_minAlpha;
			image.color = imageColor;
			if (m_fadeOnAwake)
				StartCoroutine(FadeIn());
		}
	}

	public IEnumerator FadeIn()
	{
		Color imageColor = image.color;

		while (imageColor.a < m_maxAlpha)
		{
			yield return null;
			imageColor.a += m_fadeSpeed * Time.deltaTime;
			image.color = imageColor;
		}

		imageColor.a = m_maxAlpha;
		image.color = imageColor;

		if (m_continuous)
			StartCoroutine(FadeOut(null));
	}

	public IEnumerator FadeOut(GameObject objectToDestroy)
	{
		Color imageColor = image.color;

		while (imageColor.a > m_minAlpha)
		{
			yield return null;
			imageColor.a -= m_fadeSpeed * Time.deltaTime;
			image.color = imageColor;
		}
		imageColor.a = m_minAlpha;
		image.color = imageColor;
        if (objectToDestroy)
            Destroy(objectToDestroy);

		if (m_continuous)
			StartCoroutine(FadeIn());
	}

}
