using UnityEngine;

public class Thrower
{
    public Vector3 CalculateVelocityByHeight(Vector3 start, Vector3 end, float height)
    {
        float timeToRise = CalculateTimeByHeight(height);
        float timeToFall = CalculateTimeByHeight(height + (start - end).y);
        
        Vector3 horizontalVelocity = end - start;
        horizontalVelocity.y = 0;
        horizontalVelocity /= (timeToRise + timeToFall);
        
        Vector3 verticalVelocity = -Physics.gravity * timeToRise;
        
        return horizontalVelocity + verticalVelocity;
    }

    private float CalculateTimeByHeight(float height)
    {
        var g = Mathf.Abs(Physics.gravity.y);
        return Mathf.Sqrt(height * 2f / g);
    }
}