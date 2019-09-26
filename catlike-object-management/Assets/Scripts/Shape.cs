using UnityEngine;

public class Shape : PersistableObject
{
    static int colorPropertyId = Shader.PropertyToID("_Color");

    static MaterialPropertyBlock sharedPropertyBlock;

    int shapeId = int.MinValue;

    Color color;

    MeshRenderer meshRenderer;

    public int MaterialId { get; private set; }

    public int ShapeId
    {
        get
        {
            return shapeId;
        }
        set
        {
            if (shapeId == int.MinValue && value != int.MinValue)
            {
                shapeId = value;
            }
            else
            {
                Debug.LogError("Not allowed to change shapeId.");
            }
        }
    }

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(color);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
    }

    public void SetMaterial(Material material, int materialId)
    {
        meshRenderer.material = material;
        this.MaterialId = materialId;
    }

    public void SetColor(Color color)
    {
        this.color = color;

        /// A downside of setting a material's color is that this results in the creation of a new material, 
        /// unique to the shape. This happens each time its color is set. We can avoid this by using a MaterialPropertyBlock instead.
        //meshRenderer.material.color = color;

        if (sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }

        sharedPropertyBlock.SetColor(colorPropertyId, color);
        meshRenderer.SetPropertyBlock(sharedPropertyBlock);
    }
}