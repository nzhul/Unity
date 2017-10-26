﻿using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{

  HexCell[] cells;

  private int cellCountX;
  private int cellCountZ;

  public int chunkCountX = 4;
  public int chunkCountZ = 3;

  public HexCell cellPrefab;
  public Text cellLabelPrefab;

  public Color defaultColor = Color.white;

  public Texture2D noiseSource;

  public HexGridChunk chunkPrefab;

  private HexGridChunk[] chunks;

  private void Awake()
  {
    HexMetrics.noiseSource = noiseSource;

    cellCountX = chunkCountX * HexMetrics.chunkSizeX;
    cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;

    this.CreateChunks();
    this.CreateCells();
  }

  private void CreateChunks()
  {
    chunks = new HexGridChunk[chunkCountX * chunkCountZ];

    for (int z = 0, i = 0; z < chunkCountZ; z++)
    {
      for (int x = 0; x < chunkCountX; x++)
      {
        HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
        chunk.transform.SetParent(this.transform);
      }
    }
  }

  private void CreateCells()
  {
    cells = new HexCell[cellCountZ * cellCountX];

    for (int z = 0, i = 0; z < cellCountZ; z++)
    {
      for (int x = 0; x < cellCountX; x++)
      {
        CreateCell(x, z, i++);
      }
    }
  }

  private void OnEnable()
  {
    HexMetrics.noiseSource = noiseSource;
  }

  public HexCell GetCell(Vector3 position)
  {
    position = transform.InverseTransformPoint(position);
    HexCoordinates coordinates = HexCoordinates.FromPosition(position);
    int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
    return cells[index];
  }

  private void CreateCell(int x, int z, int i)
  {
    // 1. The distance between adjacent hexagon cells in the X direction is equal to twice the inner radius.
    // 2. The distance to the next row of cells should be 1.5 times the outer radius.

    Vector3 position;
    position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
    position.y = 0f;
    position.z = z * HexMetrics.outerRadius * 1.5f;

    HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
    //cell.transform.SetParent(this.transform, false);
    cell.transform.localPosition = position;
    cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
    cell.Color = defaultColor;

    if (x > 0)
    {
      cell.SetNeighbor(HexDirection.W, cells[i - 1]);
    }

    if (z > 0)
    {
      if ((z & 1) == 0)
      {
        cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
        if (x > 0)
        {
          cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
        }
      }
      else
      {
        cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
        if (x < cellCountX - 1)
        {
          cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
        }
      }
    }

    Text label = Instantiate<Text>(cellLabelPrefab);
    //label.rectTransform.SetParent(gridCanvas.transform, false);
    label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
    label.text = cell.coordinates.ToStringOnSeparateLines();

    cell.uiRect = label.rectTransform;
    cell.Elevation = 0;

    this.AddCellToChunk(x, z, cell);
  }

  private void AddCellToChunk(int x, int z, HexCell cell)
  {
    int chunkX = x / HexMetrics.chunkSizeX;
    int chunkZ = z / HexMetrics.chunkSizeZ;
    HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

    int localX = x - chunkX * HexMetrics.chunkSizeX;
    int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
    chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
  }
}
