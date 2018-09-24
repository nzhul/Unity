using UnityEngine;

public class ShapeGenerator
{

    ShapeSettings settings;

    public ShapeGenerator(ShapeSettings settings)
    {
        this.settings = settings;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointonUnitSphere)
    {
        return pointonUnitSphere * settings.planetRadius;
    }
}
