using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PolarisationChart : MonoBehaviour
{
	[Header("Child Elements")]
	[SerializeField] private Image _gridImage;
	[SerializeField] private Transform _pointContainer;
	[SerializeField] private GameObject _pointPrefab;
	[SerializeField] private GameObject _pointContextMenu;

	[Header("GUI Elements")]
	[SerializeField] private Slider _zoomSlider; // recommanded - min: 0.5, max: 3
	[SerializeField] private Slider _intensitySlider; // recommended - min: 20 max: 100
	[SerializeField] private Slider _tensionSlider; // recommended - min 0, max: 30
	[SerializeField] private Button _addPointButton;

	private Vector2 selectedPoint;
	public Dictionary<Vector2, GameObject> plots = new();

	private void Start()
	{
		_zoomSlider.onValueChanged.AddListener(delegate { OnZoomValueChanged(); });
		_addPointButton.onClick.AddListener(delegate { OnAddPointButtonPressed(); });
		_pointContextMenu.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnContextMenuDeletePresssed(); });
	}

	#region add_point
	private void OnAddPointButtonPressed()
	{
		AddPoint(_intensitySlider.value, _tensionSlider.value);
	}

	public void AddPoint(float x, float y)
	{
		Vector2 position = new(x, y);

		GameObject plot = Instantiate(_pointPrefab, _pointContainer);
		plot.transform.localPosition = position;
		plot.GetComponent<Button>().onClick.AddListener(delegate {
			selectedPoint = position;
			ShowContextMenu();
		});

		plots[position] = plot;
	}
	#endregion add_point

	public void ShowContextMenu()
	{
		_pointContextMenu.transform.localPosition = selectedPoint;
		_pointContextMenu.SetActive(true);
	}

	#region remove_point
	public void OnContextMenuDeletePresssed()
	{
		_pointContextMenu.SetActive(false);
		RemovePoint(selectedPoint);
	}

	public void RemovePoint(Vector2 pointPosition)
	{
		Destroy(plots[pointPosition]);
		plots.Remove(pointPosition);
	}
	#endregion remove_point

	#region zoom
	private void OnZoomValueChanged()
	{
		UpdateZoom(_zoomSlider.value);
	}

	public void UpdateZoom(float zoom)
	{
		float scale = 1 / zoom;
		_gridImage.pixelsPerUnitMultiplier = zoom;
		_pointContainer.localScale = new Vector2(scale, scale);
	}
	#endregion zoom

	public string GetCSVStringFormat()
	{
		StringBuilder csvString = new();

		csvString.AppendLine("X,Y");
		foreach (Vector2 position in plots.Keys)
		csvString.AppendLine($"{position.x},{position.y}");

		return csvString.ToString();
	}
}
