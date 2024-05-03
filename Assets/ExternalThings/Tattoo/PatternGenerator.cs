using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PatternGenerator : MonoBehaviour
{
    [SerializeField] private GameObject dotRefab;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Button generateBtn;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float entropy;

    private List<Dot> dots = new List<Dot>();
    private List<Line> lines = new List<Line>();
    private const float PADDING = 50f;
    
    void Start()
    {
        generateBtn.OnClickAsObservable()
            .Subscribe(_ => Generate())
            .AddTo(this);
        
        Generate();
    }

    private void Generate()
    {
        dots.ForEach(dot => Destroy(dot.gameObject));
        lines.ForEach(line => Destroy(line.gameObject));
        dots.Clear();
        lines.Clear();
        
        var spacingX = (Screen.width - 2 * PADDING) / (width - 1);
        var spacingY = (Screen.height - 2 * PADDING) / (height - 1);
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                var dot = Instantiate(dotRefab, transform).GetComponent<Dot>();
                var dotPosition = new Vector2(PADDING + i * spacingX, PADDING + j * spacingY);
                if (j != 0 && i != 0 && j != height - 1 && i != width - 1)
                {
                    dotPosition += new Vector2(
                        Random.Range(-spacingX * entropy, spacingX * entropy),
                        Random.Range(-spacingY * entropy, spacingY * entropy));
                }
                dot.transform.GetComponent<RectTransform>().anchoredPosition = dotPosition;
                dots.Add(dot);

                if (j != 0)
                {
                    var line = Instantiate(linePrefab, transform).GetComponent<Line>();
                    line.SetVertices(dots[^1], dots[^2]);
                    lines.Add(line);
                }
                
                if (i != 0)
                {
                    var line = Instantiate(linePrefab, transform).GetComponent<Line>();
                    line.SetVertices(dots[^1], dots[^(height + 1)]);
                    lines.Add(line);
                }
            }
        }
    }
    
}
